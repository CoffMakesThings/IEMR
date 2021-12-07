using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject mirroredCamera;
    public GameObject headsetData;
    Vector3 directionVector = new Vector3();
    int speed = 10;
    int sensitivity = 10;
    float yaw = 0.0f;
    float pitch = 0.0f;
    float headsetPositionMultiplier = 7.5f;
    Vector3 headsetPositionOffset = new Vector3(-10, 0, -10);

    // Update is called once per frame
    void Update()
    {
        updateWithHeadsetData();
        //updateWithMouse();
        //updateWithKeyboard();
    }

    void updateWithHeadsetData()
    {
        transform.localPosition = headsetData.transform.position * headsetPositionMultiplier + headsetPositionOffset;
        transform.localRotation = headsetData.transform.rotation;
    }

    void updateWithMouse()
    {
        // Mouse looking around
        yaw -= sensitivity * Input.GetAxis("Mouse X");
        pitch -= sensitivity * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    void updateWithKeyboard() {
        // Keyboard movement
        directionVector = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
            directionVector += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            directionVector += Vector3.right;
        if (Input.GetKey(KeyCode.W))
            directionVector += Vector3.forward;
        if (Input.GetKey(KeyCode.S))
            directionVector += Vector3.back;

        directionVector = directionVector.normalized;

        transform.position += transform.rotation * directionVector * speed * Time.deltaTime;
    }
}
