using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BramaBadura.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject presistentObjectPrefab;

        static bool hasSpawn = false;

        private void Awake()
        {
            if (hasSpawn) return;

            SpawnPersistentObject();

            hasSpawn = true;
             
        }
        private void SpawnPersistentObject()
        {
            GameObject persistentObject = Instantiate(presistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}


