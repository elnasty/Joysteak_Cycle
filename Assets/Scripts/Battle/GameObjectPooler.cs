using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*      This object pooler resizes based on how many different types of object you want to pool.  I'm trying to make it as
 *      easy as possible to use.  When you want to add a new type of object, just add its name to the SpawnObjEnum
 *      script, then add the prefab to the objectPrefabs List in the editor.*/

namespace ObjectPooler
{
    public enum SpawnObjectEnum { thorn };

    public class GameObjectPooler : MonoBehaviour
    {
        public static GameObjectPooler current;
        public List<List<GameObject>> pools = new List<List<GameObject>>();     //each list is an object pool for one
                                                                                //type of object.

        public List<int> poolSizes;     //the size of each object pool.
        public List<bool> willGrow;     //whether or not the pools will grow to accomodate increasing demand for the object.
        public List<GameObject> objectPrefabs = new List<GameObject>();

        private void Awake()
        {
            current = this;

            /*      Creates one List of GameObjects for each poolable object defined in the SpawnObjEnum.*/
            for (int i = 0; i < Enum.GetNames(typeof(SpawnObjectEnum)).GetLength(0); i++)
            {
                pools.Add(new List<GameObject>());
            }
        }

        /// <summary>
        /// set up all the pools.
        /// </summary>
        private void Start()
        {
            int i = 0;

            foreach (List<GameObject> pool in pools)
            {
                for (int j = 0; j < poolSizes[i]; ++j)
                {
                    GameObject obj = (GameObject)Instantiate(objectPrefabs[i]);
                    obj.SetActive(false);
                    pool.Add(obj);
                }
                i++;
            }
        }

        /// <summary>
        /// Other scripts can tell this one which pooled object they want, this function then gives it to them.
        /// If the pool in question is allowed to grow, then a new object will be added to that list.
        /// </summary>
        /// <returns>The pooled object.</returns>
        /// <param name="objType">Object type.</param>
        public GameObject GetPooledObject(SpawnObjectEnum objType)
        {
            int typeIndex = (int)objType;   //gets the index (in the enum) of the object type.

            List<GameObject> pool = pools[typeIndex];

            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeInHierarchy)
                {
                    return pool[i];
                }
            }

            if (willGrow[typeIndex])
            {
                GameObject obj = (GameObject)Instantiate(objectPrefabs[typeIndex]);
                obj.SetActive(false);
                pool.Add(obj);
            }

            return null;
        }
    }
}