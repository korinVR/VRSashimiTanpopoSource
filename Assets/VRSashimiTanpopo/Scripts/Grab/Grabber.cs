using System.Linq;
using UnityEngine;

namespace VRSashimiTanpopo
{
    public class Grabber : MonoBehaviour
    {
        Grabbable grabbing;
#if false
        Grabbable grabbableCandidate;
#endif

        TransformRecorder positionRecorder;

        const float Radius = 0.05f;

        const int PullFrame = 4;
        int pullCount;

        void Start()
        {
            positionRecorder = new TransformRecorder();
        }

        public bool Grab()
        {
            if (grabbing != null) return false;

            var collider = Physics.OverlapSphere(transform.position, Radius).ToList()
                .Where(c => c.GetComponentInParent<Tanpopo.Tanpopo>())
                .OrderBy(c => (c.transform.position - transform.position).magnitude)
                .FirstOrDefault();

            if (collider == null) return false;

            // TODO: 完全にタンポポ専用になっているので要修正。
            grabbing = collider.GetComponentInParent<Tanpopo.Tanpopo>().GetComponent<Grabbable>();
            grabbing.transform.localScale = new Vector3(1f, 1f, 0.9f);

            var rigidbody = grabbing.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            rigidbody.detectCollisions = false;
            
            return true;
        }

        public void Release()
        {
            if (grabbing == null) return;

            var rigidbody = grabbing.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.detectCollisions = true;

            rigidbody.position = transform.position;
            rigidbody.rotation = transform.rotation;

            rigidbody.velocity = positionRecorder.CalculateVelocity();

            pullCount = PullFrame;

            grabbing.transform.localScale = Vector3.one;

            grabbing = null;
        }

        void LateUpdate()
        {
            positionRecorder.Record(transform.position, transform.rotation);

            if (grabbing == null) return;

            pullCount -= 1;
            if (pullCount < 1)
            {
                pullCount = 1;
            }

            var t = 1f - (float)(pullCount - 1) / pullCount;

            grabbing.transform.position += (transform.position - grabbing.transform.position) * t;
            grabbing.transform.rotation = Quaternion.Slerp(grabbing.transform.rotation, transform.rotation, t);
        }

#if false
        void OnTriggerEnter(Collider other)
        {
            var grabbable = other.gameObject.GetComponent<Grabbable>();
            if (grabbable)
            {
                grabbableCandidate = grabbable;
            }
        }

        void OnTriggerExit(Collider other)
        {
            var grabbable = other.gameObject.GetComponent<Grabbable>();
            if (grabbableCandidate == grabbable)
            {
                grabbableCandidate = null;
            }
        }
#endif
    }
}
