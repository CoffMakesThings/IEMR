using RosSharp.RosBridgeClient;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public GameObject mapGameObject;
    public TextAsset dataTextFile;
    public GameObject nodePrefab;
    public GameObject turtleBot;
    Vector3 robotPositionOffset = new Vector3(10, 0, 10);
    public float odometryMultiplier = 7.5f;
    readonly int dataWidthHeight = 384;
    Node[] nodes;
    List<Payload> payloads = new List<Payload>();
    int currentPayloadIndex = -1;
    Map_Payload mapPayload;
    OdometrySubscriber odometrySubscriber;
    uint lastTimeStamp = 0;

    void Start()
    {
        mapPayload = mapGameObject.GetComponent<Map_Payload>();
        odometrySubscriber = mapGameObject.GetComponent<OdometrySubscriber>();
        nodes = new Node[dataWidthHeight * dataWidthHeight];
        string[] lines = dataTextFile.text.Split('\n');
        bool headerSkipped = false;

        foreach (string line in lines) {
            if (!headerSkipped) {
                headerSkipped = true;
                continue;
            }

            if (line.Length == 0) {
                continue;
            }

            payloads.Add(new Payload(line));
        }

        //foreach(Payload payload in payloads) {
        //    payload.log();
        //}

        int nodeIndex = 0;

        // Instantiate nodes
        for (int x = 0; x < dataWidthHeight; x++) {
            for (int z = 0; z < dataWidthHeight; z++) {
                int xOffset = dataWidthHeight / 2;
                int zOffset = dataWidthHeight / 2;

                GameObject node = Instantiate(nodePrefab, new Vector3((float)x - xOffset, 0, (float)z - zOffset), Quaternion.identity, gameObject.transform);
                nodes[nodeIndex] = node.GetComponentInChildren<Node>();
                nodeIndex++;
            }
        }

        Debug.Log("Instantiated " + nodeIndex + " nodes");
    }

    void Update()
    {
        updateWithRealData();
        // updateWithMockData();
    }

    void updateWithRealData()
    {
        if (lastTimeStamp != mapPayload.fieldHeaderStamp_sec) {
            Debug.Log("New payload " + mapPayload.fieldHeaderStamp_sec);
            lastTimeStamp = mapPayload.fieldHeaderStamp_sec;

            // Payload payload = payloads[currentPayloadIndex];

            // Update turtlebot
            // turtleBot.transform.position = new Vector3(payload.fieldInfoOriginPositionX, payload.fieldInfoOriginPositionY, payload.fieldInfoOriginPositionZ);
            // turtleBot.transform.position = new Vector3(-mapPayload.fieldInfoOrigin.x, -mapPayload.fieldInfoOrigin.z, -mapPayload.fieldInfoOrigin.y);
            turtleBot.transform.position = new Vector3(-odometrySubscriber.position.x, odometrySubscriber.position.y, odometrySubscriber.position.z) * odometryMultiplier + robotPositionOffset;
            turtleBot.transform.rotation = odometrySubscriber.rotation;

            // Update nodes
            for (int i = 0; i < dataWidthHeight * dataWidthHeight; i++) {
                if (nodes[i] == null) {
                    Debug.Log("Node " + i + " is null");
                }

                switch (mapPayload.data[i]) {
                    case 0:
                        nodes[i].setNodeState(NodeState.Floor);
                        break;
                    case 100:
                        nodes[i].setNodeState(NodeState.Wall);
                        break;
                    default:
                        nodes[i].setNodeState(NodeState.Nothing);
                        break;
                }
            }
        }
    }

    void updateWithMockData()
    {
        if (Input.GetKey(KeyCode.Space)) {
            currentPayloadIndex++;

            if (currentPayloadIndex > payloads.Count) {
                Debug.Log("No more payloads");
                return;
            }

            Payload payload = payloads[currentPayloadIndex];

            // Update turtlebot
            //turtleBot.transform.position = new Vector3(payload.fieldInfoOriginPositionX, payload.fieldInfoOriginPositionY, payload.fieldInfoOriginPositionZ);
            turtleBot.transform.position = new Vector3(-payload.fieldInfoOriginPositionX, -payload.fieldInfoOriginPositionZ, -payload.fieldInfoOriginPositionY);

            // Update nodes
            for (int i = 0; i < dataWidthHeight * dataWidthHeight; i++) {
                if (nodes[i] == null) {
                    Debug.Log("Node " + i + " is null");
                }

                switch (payload.getData(i)) {
                    case 0:
                        nodes[i].setNodeState(NodeState.Floor);
                        break;
                    case 100:
                        nodes[i].setNodeState(NodeState.Wall);
                        break;
                    default:
                        nodes[i].setNodeState(NodeState.Nothing);
                        break;
                }
            }
        }
    }
}
