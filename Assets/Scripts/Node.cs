using UnityEngine;

public enum NodeState
{
    Nothing,
    Wall,
    Floor
}

public class Node : MonoBehaviour
{
    public GameObject wall;
    public GameObject floor;
    NodeState nodeState = NodeState.Nothing;

    void Start()
    {
        updateDisplay(nodeState);
    }

    public void setNodeState(NodeState _nodeState)
    {
        if (nodeState != _nodeState) {
            updateDisplay(_nodeState);
        }

        nodeState = _nodeState;
    }

    void updateDisplay(NodeState _nodeState)
    {
        wall.SetActive(_nodeState == NodeState.Wall ? true : false);
        floor.SetActive(_nodeState == NodeState.Floor ? true : false);
    }
}
