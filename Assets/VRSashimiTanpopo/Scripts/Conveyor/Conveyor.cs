using System;
using UnityEngine;

namespace VRSashimiTanpopo.Conveyor
{
    public class Conveyor : MonoBehaviour
    {
        [SerializeField] GameObject platePrefab;

        const int PlateCount = 44;

        const double Width = 4.4;

        public double Speed { get; set; }

        double globalX;

        readonly ConveyorPlate[] plates = new ConveyorPlate[PlateCount];

        public void Stop()
        {
            Speed = 0;
        }
        
        void Start()
        {
            for (var i = 0; i < PlateCount; i++)
            {
                var go = Instantiate(platePrefab);
                plates[i] = go.GetComponent<ConveyorPlate>();
                plates[i].Initialize(Width, Width * i / PlateCount - Width);
            }
        }

        void FixedUpdate()
        {
            globalX -= Speed * Time.fixedDeltaTime;
            Array.ForEach(plates, plate => plate.UpdatePosition(globalX));
        }
    }
}
