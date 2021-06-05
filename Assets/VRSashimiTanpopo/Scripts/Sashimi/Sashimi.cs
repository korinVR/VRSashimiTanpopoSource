using System;
using UnityEngine;
using VRSashimiTanpopo.Tanpopo;
using Random = UnityEngine.Random;

namespace VRSashimiTanpopo.Sashimi
{
    public class Sashimi : MonoBehaviour
    {
        public bool HasNoTanpopo => tanpopoCounter.IsZero;

        public event Action ScoreIncremented;
        public event Action ScoreDecremented;
        public event Action TwoTanpopoMounted;
        
        [SerializeField] GameObject scoreMessagePrefab;
        [SerializeField] Transform scoreMessageSpawnPoint;

        [SerializeField] TanpopoCounter tanpopoCounter;
        [SerializeField] TanpopoAlertBox tanpopoAlertBox;
        
        SashimiSupplier sashimiSupplier;
        Transform sashimiSpawnPoint;
        bool scoreEnabled;

        public void Construct(SashimiSupplier sashimiSupplier, Transform sashimiSpawnPoint, bool scoreEnabled)
        {
            this.sashimiSupplier = sashimiSupplier;
            this.sashimiSpawnPoint = sashimiSpawnPoint;
            this.scoreEnabled = scoreEnabled;
        }

        void Start()
        {
            if (sashimiSpawnPoint != null)
            {
                var spawnPosition = sashimiSpawnPoint.position + Vector3.forward * Random.Range(-0.08f, 0.08f);

                transform.position = spawnPosition;
                transform.rotation = Quaternion.Euler(0f, Random.Range(-5f, 5f), 0f);
            }

            if (scoreEnabled)
            {
                tanpopoCounter.IncreasedToOne += OnIncreasedToOneTanpopo;
                tanpopoCounter.IncreasedToTwo += OnIncreasedToTwoTanpopos;
                tanpopoCounter.DecreasedToZero += OnDecreasedToZeroTanpopo;
            }
        }

        public void StartAlert() => tanpopoAlertBox.StartAlert();
        public void StopAlert() => tanpopoAlertBox.StopAlert();

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer != LayerName.DeadZone) return;

            sashimiSupplier.DisposeSashimi(this);
        }

        void OnIncreasedToOneTanpopo()
        {
            ScoreIncremented?.Invoke();
            SpawnScoreMessage(1);
        }

        void OnIncreasedToTwoTanpopos()
        {
            TwoTanpopoMounted?.Invoke();
        }

        void OnDecreasedToZeroTanpopo()
        {
            ScoreDecremented?.Invoke();
            SpawnScoreMessage(-1);
        }

        void SpawnScoreMessage(int score)
        {
            var go = Instantiate(scoreMessagePrefab);
            go.GetComponent<ScoreMessage>().Initialize(scoreMessageSpawnPoint.position, score);
        }
    }
}
