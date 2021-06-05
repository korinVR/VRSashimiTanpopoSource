using UnityEngine;

namespace VRSashimiTanpopo.Tanpopo
{
    public class Tanpopo : MonoBehaviour
    {
        TanpopoSupplier tanpopoSupplier;

        public void Initialize(Vector3 position, Quaternion rotation, TanpopoSupplier tanpopoSupplier)
        {
            transform.SetPositionAndRotation(position, rotation);

            this.tanpopoSupplier = tanpopoSupplier;
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer != LayerName.DeadZone) return;

            tanpopoSupplier.DisposeTanpopo(this);
        }
    }
}
