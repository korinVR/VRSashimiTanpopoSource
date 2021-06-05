using System;
using System.Collections.Generic;
using FrameSynthesis;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace VRSashimiTanpopo.Tanpopo
{
    public class TanpopoSupplier
    {
        readonly GameObject tanpopoPrefab;

        public int TanpopoCount => tanpopos.Count;
        
        readonly List<Tanpopo> tanpopos = new List<Tanpopo>(); 
        
        public TanpopoSupplier(GameObject tanpopoPrefab)
        {
            this.tanpopoPrefab = tanpopoPrefab;
        }

        public void Supply(int count)
        {
            var tanpopoSupplyAreas = Object.FindObjectsOfType<TanpopoSupplyArea>();

            foreach (var tanpopoSupplyArea in tanpopoSupplyAreas)
            {
                var bounds = tanpopoSupplyArea.gameObject.GetComponent<BoxCollider>().bounds;

                var tanpopoCount = count / tanpopoSupplyAreas.Length;
                for (var i = 0; i < tanpopoCount; i++)
                {
                    var x = Random.Range(bounds.min.x, bounds.max.x);
                    var y = MathHelper.LinearMap(i, 0, tanpopoCount, bounds.min.y, bounds.max.y);
                    // var y = Random.Range(bounds.min.y, bounds.max.y);
                    var z = Random.Range(bounds.min.z, bounds.max.z);

                    var position = new Vector3(x, y, z);
                    var rotation = Quaternion.identity;

                    var tanpopo = Object.Instantiate(tanpopoPrefab).GetComponent<Tanpopo>();
                    tanpopo.Initialize(position, rotation, this);
                    tanpopos.Add(tanpopo);
                }
            }
        }

        public void DisposeTanpopo(Tanpopo tanpopo)
        {
            Object.Destroy(tanpopo.gameObject);
            tanpopos.Remove(tanpopo);
        }

        public void DisposeAllTanpopos()
        {
            tanpopos.ForEach(tanpopo => Object.Destroy(tanpopo.gameObject));
            tanpopos.Clear();
        }
    }
}