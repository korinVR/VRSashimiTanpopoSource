using UnityEngine;

namespace VRSashimiTanpopo.Conveyor
{
    public class ConveyorPlate : MonoBehaviour
    {
        double conveyorWidth;
        double offsetX;

        Rigidbody rigid;
 
        public void Initialize(double conveyorWidth, double offsetX)
        {
            this.conveyorWidth = conveyorWidth;
            this.offsetX = offsetX;
            
            rigid = GetComponent<Rigidbody>();
        }

        public void UpdatePosition(double globalX)
        {
            var x = (offsetX + globalX) % conveyorWidth + conveyorWidth / 2;
            rigid.MovePosition(new Vector3((float) x, 0.68f, 0.3f));
        }
    }
}