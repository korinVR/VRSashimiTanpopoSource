using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VRSashimiTanpopo
{
    public class TransformRecorder
    {
        struct Sample
        {
            public float TimeStamp;
            public Vector3 Position;
            public Quaternion Rotation;
        }

        readonly Queue<Sample> buffer;
        readonly int bufferSize;

        public TransformRecorder(int bufferSize = 5)
        {
            buffer = new Queue<Sample>();

            this.bufferSize = bufferSize;
        }

        public void Record(Vector3 position, Quaternion rotation)
        {
            var record = new Sample
            {
                TimeStamp = Time.time,
                Position = position,
                Rotation = rotation,
            };

            buffer.Enqueue(record);

            if (buffer.Count > bufferSize)
            {
                buffer.Dequeue();
            }
        }

        public Vector3 CalculateVelocity()
        {
            var movement = buffer.Last().Position - buffer.First().Position;
            var dt = buffer.Last().TimeStamp - buffer.First().TimeStamp;

            return movement / dt;
        }
    }
}
