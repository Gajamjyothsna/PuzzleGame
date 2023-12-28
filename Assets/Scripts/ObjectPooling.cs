using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Puzzle2048
{
    public class ObjectPooling : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int totalSize;
        }
        [System.Serializable]
        public enum BlockType
        {
            None,
            ThreeCrossGridBlock
        }
        public Dictionary<string, Queue<GameObject>> poolDictionary;
        public List<Pool> poolList;

        public static ObjectPooling Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        private void Start()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in poolList)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.totalSize; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.tag, objectPool);
            }
        }
        public GameObject SpawnFromPool(string _tag)
        {
            Debug.Log("SpawnFromPool");
            Debug.Log("Tag" + _tag);
            // Debug.Log("_positionToSpawn " + _positionToSpawn);
            //if(!poolDictionary.ContainsKey(_tag))
            //{
            //    return null;
            //}
            GameObject objToSpawn = poolDictionary[_tag].Dequeue();
            objToSpawn.SetActive(true);
            objToSpawn.transform.position = PZGameManager.Instance.sqaureBoardTransform.position;
            // Debug.Log("position " + _positionToSpawn);

            poolDictionary[_tag].Enqueue(objToSpawn);

            return objToSpawn;
        }


    }
}
