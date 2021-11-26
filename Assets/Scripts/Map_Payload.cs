using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class Map_Payload : UnitySubscriber<MessageTypes.Nav.OccupancyGrid>
    {
        //Member Data
        // Header
        public uint fieldHeaderSeq;
        public uint fieldHeaderStamp_sec;           // Header time stamp in sec
        public string fieldHeaderFrameId;

        // Info
        public uint fieldInfoMapLoadTime_sec;      // Map load time in sec
        public float fieldInfoResolution;           // Map resolution [meter/cell]
        public uint fieldInfoWidth;                // Map Width
        public uint fieldInfoHeight;               // Map Height
        public Vector3 fieldInfoOrigin;               // TurtleBot Position
        public Quaternion fieldInfoOriginOrientation;    // TurtleBot Orientation

        // Map Data
        public sbyte[] data;

        private bool isMessageReceived;                 // check if payload received

        // Member Methods
        protected override void Start()
        {
            base.Start();
        }

        private void Update()
        {
            if (isMessageReceived)
                ProcessMessage();
        }

        protected override void ReceiveMessage(MessageTypes.Nav.OccupancyGrid message)
        {
            Debug.Log("New pos");
            Debug.Log(message.info.origin.position.x);
            Debug.Log(message.info.origin.position.y);
            Debug.Log(message.info.origin.position.z);

            //Header
            fieldHeaderSeq = GetHeaderSequence(message);
            fieldHeaderStamp_sec = GetHeaderStamp(message);
            fieldHeaderFrameId = GetHeaderFrameId(message);

            //Info
            fieldInfoMapLoadTime_sec = GetMapLoadTime(message);
            fieldInfoResolution = GetResolution(message);
            fieldInfoWidth = GetMapWidth(message);
            fieldInfoHeight = GetMapHeight(message);

            fieldInfoOrigin = GetOrigin(message).Ros2Unity();
            fieldInfoOriginOrientation = GetOrientation(message).Ros2Unity();

            //Data
            
            data = GetData(message);

            isMessageReceived = true;
        }

        private void ProcessMessage()
        {
            // You can render the map on the scene in this function using the member data

        }

        // Get Map Payload's Sequence ID / Frame number
        private uint GetHeaderSequence(MessageTypes.Nav.OccupancyGrid message)
        {
            return message.header.seq;
        }

        // Get Map Payload's Stamp time in seconds
        private uint GetHeaderStamp(MessageTypes.Nav.OccupancyGrid message)
        {
            return message.header.stamp.secs;
        }

        // Get Map Payload's Frame ID - Usually "map"
        private string GetHeaderFrameId(MessageTypes.Nav.OccupancyGrid message)
        {
            return message.header.frame_id;
        }

        // Get Map Load Time in seconds
        private uint GetMapLoadTime(MessageTypes.Nav.OccupancyGrid message)
        {
            return message.info.map_load_time.secs;
        }

        // Get Map Resolution in m/sec
        private float GetResolution(MessageTypes.Nav.OccupancyGrid message)
        {
            return message.info.resolution;
        }

        // Get Map Width
        private uint GetMapWidth(MessageTypes.Nav.OccupancyGrid message)
        {
            return message.info.width;
        }

        // Get Map Height
        private uint GetMapHeight(MessageTypes.Nav.OccupancyGrid message)
        {
            return message.info.height;
        }

        // Get TurtleBot Position
        private Vector3 GetOrigin(MessageTypes.Nav.OccupancyGrid message)
        {
            return new Vector3(
                (float) message.info.origin.position.x,
                (float) message.info.origin.position.y,
                (float) message.info.origin.position.z
            );
        }

        // Get Turtlebot Orientation
        private Quaternion GetOrientation(MessageTypes.Nav.OccupancyGrid message)
        {
            return new Quaternion(
                (float)message.info.origin.orientation.x,
                (float)message.info.origin.orientation.y,
                (float)message.info.origin.orientation.z,
                (float)message.info.origin.orientation.w);
        }

        private sbyte[] GetData(MessageTypes.Nav.OccupancyGrid message)
        {
            return message.data;
        }

    }
}