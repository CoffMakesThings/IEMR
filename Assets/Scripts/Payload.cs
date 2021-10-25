using System.Reflection;
using UnityEngine;

public class Payload
{
    readonly int dataWidthHeight = 384;
    readonly int dataStartIndex = 15;

    int[] data;

    // These are public so that they are returned by GetFields()
    public string time; // Unknown time format, using string for now
    public int fieldHeaderSeq;
    public string fieldHeaderStamp; // Unknown time format, using string for now
    public string fieldHeaderFrameId;
    public string fieldInfoMapLoadTime; // Unknown time format, using string for now
    public float fieldInfoResolution;
    public int fieldInfoWidth;
    public int fieldInfoHeight;
    public float fieldInfoOriginPositionX;
    public float fieldInfoOriginPositionY;
    public float fieldInfoOriginPositionZ;
    public float fieldInfoOriginOrientationX;
    public float fieldInfoOriginOrientationY;
    public float fieldInfoOriginOrientationZ;
    public float fieldInfoOriginOrientationW;

    public Payload(string stringPayload)
    {
        // Parse string into object
        string[] elements = stringPayload.Split(',');

        time = elements[0];
        fieldHeaderSeq = int.Parse(elements[1]);
        fieldHeaderStamp = elements[2];
        fieldHeaderFrameId = elements[3];
        fieldInfoMapLoadTime = elements[4];
        fieldInfoResolution = float.Parse(elements[5]);
        fieldInfoWidth = int.Parse(elements[6]);
        fieldInfoHeight = int.Parse(elements[7]);

        fieldInfoOriginPositionX = float.Parse(elements[8]);
        fieldInfoOriginPositionY = float.Parse(elements[9]);
        fieldInfoOriginPositionZ = float.Parse(elements[10]);

        fieldInfoOriginOrientationX = float.Parse(elements[11]);
        fieldInfoOriginOrientationY = float.Parse(elements[12]);
        fieldInfoOriginOrientationZ = float.Parse(elements[13]);
        fieldInfoOriginOrientationW = float.Parse(elements[14]);

        data = new int[dataWidthHeight * dataWidthHeight];

        for (int i = 0; i < dataWidthHeight * dataWidthHeight; i++) {
            data[i] = int.Parse(elements[i + dataStartIndex]);
        }
    }

    public int getData(int index)
    {
        return data[index];
    }

    public void log()
    {
        foreach (FieldInfo fieldInfo in typeof(Payload).GetFields()) {
            Debug.Log(fieldInfo.Name + ": " + fieldInfo.GetValue(this));
        }
    }
}
