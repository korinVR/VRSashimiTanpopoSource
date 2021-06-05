using System.Collections.Generic;
using UnityEngine;

namespace VRSashimiTanpopo.Sashimi
{
    public class SashimiSupplier
    {
        readonly GameObject sashimiPrefab;

        readonly List<Sashimi> sashimis = new List<Sashimi>();
        public List<Sashimi> Sashimis => sashimis;

        public SashimiSupplier(GameObject sashimiPrefab)
        {
            this.sashimiPrefab = sashimiPrefab;
        }

        public Sashimi Supply(Transform spawnPoint, bool scoreEnabled)
        {
            var sashimi = Object.Instantiate(sashimiPrefab).GetComponent<Sashimi>();
            sashimi.Construct(this, spawnPoint, scoreEnabled);
            Sashimis.Add(sashimi);

            return sashimi;
        }

        public void DisposeSashimi(Sashimi sashimi)
        {
            Object.Destroy(sashimi.gameObject);
            Sashimis.Remove(sashimi);
        }

        public void DisposeAllSashimis()
        {
            Sashimis.ForEach(sashimi => Object.Destroy(sashimi.gameObject));
            Sashimis.Clear();
        }
    }
}
