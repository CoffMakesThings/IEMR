/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
 
 TODO: You have to render the variable texture-2D on quad or any plane

 */

using UnityEngine;
using System;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;

[RequireComponent(typeof(RosSharp.RosBridgeClient.RosConnector))]
public class ImageSubscriber : RosSharp.RosBridgeClient.UnitySubscriber<RosSharp.RosBridgeClient.MessageTypes.Sensor.Image>
{
    public Material material;
    public Material material2;
    public byte[] imageData;
    private bool isMessageReceived;

    int messagesReceived = 0;
    float firstMessageTime = 0;
    bool receivedFirstMessage = false;
    int mockColor = 0;
    int dotIndex = 0;

    private ComputeBuffer buffer;

    protected override void Start()
    {
		base.Start();
    }

    private void Update()
    {
        if (isMessageReceived)
        {
            //Debug.Log(isMessageReceived);
            ProcessMessage();
        }
    }

    protected override void ReceiveMessage(RosSharp.RosBridgeClient.MessageTypes.Sensor.Image compressedImage)
    {
        imageData = compressedImage.data;
        isMessageReceived = true;
    }

    // Would be faster but doesnt work
    void updateThroughShader()
    {
        // Send mock black/white, originally as byte and converted to int
        //dotIndex++;
        //if (dotIndex == 255) {
        //    dotIndex = 0;
        //}

        //int size = 640 * 480 * 3;

        //byte[] imageDataByte = new byte[size];

        //for (int i = 0; i < size; i++) {
        //    imageDataByte[i] = (byte)((i + dotIndex) % 255);
        //}

        //int[] imageDataInt = new int[size];

        //for (int i = 0; i < size; i++) {
        //    imageDataInt[i] = (int)imageDataByte[i];
        //}

        //if (buffer == null || buffer.count != imageDataInt.Length)
        //    buffer = new ComputeBuffer(size, sizeof(int));

        //buffer.SetData(imageDataInt);
        //material.SetBuffer("_Colors", buffer);

        // Send mock black/white
        //int size = 640 * 480 * 3;

        //mockColor++;
        //if (mockColor == 255) {
        //    mockColor = 0;
        //}

        //int[] imageDataInt = new int[size];

        //for (int i = 0; i < size; i++) {
        //    imageDataInt[i] = mockColor;
        //}

        //if (buffer == null || buffer.count != imageDataInt.Length)
        //    buffer = new ComputeBuffer(size, sizeof(int));

        //buffer.SetData(imageDataInt);
        //material.SetBuffer("_Colors", buffer);

        // Convert imageData bytes into ints, 10 lines
        //int[] imageDataInt = new int[640 * 3 * 10];

        //for (int i = 0; i < 640 * 3 * 10; i++) {
        //    imageDataInt[i] = (int)imageData[i];
        //}

        //if (buffer == null || buffer.count != imageData.Length)
        //    buffer = new ComputeBuffer(640 * 3 * 10, sizeof(int));

        //buffer.SetData(imageDataInt);
        //material.SetBuffer("_Colors", buffer);

        // Convert imageData bytes into ints befoe sending to shader, works fine
        int[] imageDataInt = new int[imageData.Length];

        for (int i = 0; i < imageData.Length; i++) {
            imageDataInt[i] = (int)imageData[i];
        }

        if (buffer == null || buffer.count != imageData.Length)
            buffer = new ComputeBuffer(imageDataInt.Length, sizeof(int));

        buffer.SetData(imageDataInt);
        material.SetBuffer("_Colors", buffer);
        material2.SetBuffer("_Colors", buffer);

        //Send bytes to shader directly, has weird artifacts
        //if (buffer == null || buffer.count != imageData.Length)
        //    buffer = new ComputeBuffer(imageData.Length, sizeof(int));

        //buffer.SetData(imageData);
        //material.SetBuffer("_Colors", buffer);

        // Convert imageData bytes into Colors before sending to shader, works fine
        //Color32[] colors = new Color32[480 * 640];

        //int k = 0;
        //for (int i = 0; i < 480 * 640; i++) {
        //    float r = (float)imageData[k] / 255;
        //    float g = (float)imageData[k + 1] / 255;
        //    float b = (float)imageData[k + 2] / 255;
        //    colors[i] = new Color(r, g, b);
        //    k = k + 3;
        //}

        //if (buffer == null || buffer.count != colors.Length)
        //    buffer = new ComputeBuffer(colors.Length, sizeof(float) * 4);

        //buffer.SetData(colors);
        //material.SetBuffer("_Colors", buffer);
    }

    private void ProcessMessage()
    {
        updateThroughShader();

        if (!receivedFirstMessage) {
            receivedFirstMessage = true;
            firstMessageTime = Time.time;
        }

        messagesReceived++;
        Debug.Log(messagesReceived / (Time.time - firstMessageTime) + " messages/second.");
    }
}