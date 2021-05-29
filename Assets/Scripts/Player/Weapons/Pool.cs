using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Player.Weapon{
    public class Pool : MonoBehaviour
    {
        [Header("General configuration")]
        [SerializeField] Beams beams;
        [Tooltip("A transform array where the prefab is shooted.")]
        [SerializeField] Transform shootPoint;
        [SerializeField] int poolSize;
        public int actual { get; set; } = 0;
        public List<GameObject> pool { get; private set; } = new List<GameObject>();
        /// <summary>
        /// Sets the actual beam selected from player inventory to the pool. The pool is cleared at begin.
        /// </summary>
        /// <param name="beamPrefab"></param>
        public void SetBeamToPool(GameObject beamPrefab){
            if(!beamPrefab.GetComponent<Projectil>().Pooleable)return;
            pool.ForEach(item=>{
                Destroy(item);
            });
            pool.Clear();
            for (int i = 0; i < poolSize; i++)
            {
                var gObj = Instantiate(beamPrefab, shootPoint.position, Quaternion.identity, shootPoint);
                gObj.GetComponent<IPooleable>().parent = shootPoint;
                pool.Add(gObj);
                gObj.SetActive(false);
            }
        }
        public void ActiveNextPoolObject()
        {
            if (actual == poolSize) actual = 0;
            pool[actual].SetActive(true);
            pool[actual].transform.SetParent(null);
            actual++;
        }
    }
}
 
