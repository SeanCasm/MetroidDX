using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace Enemy.Weapon{
    public class Pool : MonoBehaviour
    {
        [Header("General configuration")]
        [SerializeField] AssetReference bulletPrefab;
        [Tooltip("A transform array where the prefab is shooted, can be one or greater.")]
        [SerializeField] Transform[] shootPoint;
        [SerializeField] int poolSize;
        public Transform[] ShootPoint=>shootPoint;

        public int actual{get;set;}=0;
        public List<GameObject> pool{get;private set;} = new List<GameObject>();
        private void Awake() {
            if(shootPoint.Length==0){shootPoint=new Transform[1];shootPoint[0]=transform;}
            bulletPrefab.LoadAssetAsync<GameObject>().Completed+=OnLoadDone;
        }
        public void ActiveNextPoolObject(){
            if(actual==poolSize)actual=0;
            pool[actual].SetActive(true);
            actual++;
        }
        void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj){
            for(int i=0;i<poolSize;i++){
                foreach(Transform item in shootPoint){
                    var gObj=Instantiate(obj.Result, item.position, Quaternion.identity, item);
                    gObj.GetComponent<IPooleable>().parent = item;
                    pool.Add(gObj);
                    gObj.SetActive(false);
                }
            }
        }
    }
}
 
