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

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class ImageSubscriber : UnitySubscriber<MessageTypes.Sensor.Image>
    {
        public MeshRenderer meshRenderer;

        private Texture2D texture2D;
        public byte[] imageData;
        private bool isMessageReceived;

        protected override void Start()
        {
			base.Start();
            texture2D = new Texture2D(480, 640);
            meshRenderer.material = new Material(Shader.Find("Standard"));
        }
        private void Update()
        {
            if (isMessageReceived)
            {
                Debug.Log(isMessageReceived);
                ProcessMessage();
            }
        }

        protected override void ReceiveMessage(MessageTypes.Sensor.Image compressedImage)
        {
            imageData = compressedImage.data;
            isMessageReceived = true;
        }

        void SaveImageToDisk(Texture2D texture, string fullPath)
        {
            byte[] imageBytes = texture.EncodeToPNG();
            Debug.Log("Image saved." + texture.ToString());
            System.IO.File.WriteAllBytes(fullPath, imageBytes);
        }

        private void ProcessMessage()
        {
            int k = 0;
            for(int i=0; i<480; i++)
            {
                for(int j=0; j<640; j++)
                {
                    float r = (float)imageData[k] / 255;
                    float g = (float)imageData[k+1] / 255;
                    float b = (float)imageData[k+2] / 255;
                    Color color = new Color(r, g, b);
                    texture2D.SetPixel(i, j, color);
                    k = k + 3;
                }
            }
            SaveImageToDisk(texture2D, "Assets/test.jpg");
        }

    }
}

