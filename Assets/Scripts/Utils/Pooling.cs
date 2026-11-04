using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.Utils
{
    public class Pooling : MonoBehaviour
    {
        private readonly Dictionary<string, Stack<GameObject>> poolObjects = new();
        private readonly Dictionary<string, Transform> poolParents = new();

        public void PreLoad(GameObject prefab, int amount)
        {
            if (!poolObjects.ContainsKey(prefab.name))
            {
                CreatePool(prefab.name);
            }
            for (int i = 0; i < amount; i++)
            {
                GameObject instance = GetObjectInstance(prefab);
                ReturnObject(prefab.name, instance.gameObject);
            }
        }

        public GameObject GetObject(GameObject prefab, Transform parent)
        {
            return GetObject(prefab, Vector3.zero, Quaternion.identity, parent);
        }

        public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (!poolObjects.ContainsKey(prefab.name))
            {
                CreatePool(prefab.name);
            }

            GameObject obj = GetObjectInstance(prefab);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.transform.SetParent(parent);
            obj.SetActive(true);

            return obj;
        }

        public T GetObjectUI<T>(T prefab, Transform parent) where T : Component
        {
            if (!poolObjects.ContainsKey(prefab.name))
            {
                CreatePool(prefab.name);
            }

            T obj = GetObjectInstance<T>(prefab);
            obj.transform.SetParent(parent, false);
            obj.transform.localScale = Vector3.one;
            obj.gameObject.SetActive(true);
            return obj;
        }

        private void CreatePool(string poolName)
        {
            if (poolObjects.ContainsKey(poolName))
                return;
            GameObject poolParent = new(poolName);
            poolParent.transform.SetParent(gameObject.transform);
            poolParents[poolName] = poolParent.transform;
            poolObjects[poolName] = new Stack<GameObject>();
        }

        private T GetObjectInstance<T>(T prefab) where T : Component
        {
            Stack<GameObject> objectStack = poolObjects[prefab.name];
            T obj;
            if (poolObjects[prefab.name].Count > 0)
            {
                obj = objectStack.Pop().GetComponent<T>();
            }
            else
            {
                obj = Instantiate(prefab).GetComponent<T>();
            }

            return obj;
        }

        private GameObject GetObjectInstance(GameObject prefab)
        {
            Stack<GameObject> objectStack = poolObjects[prefab.name];
            GameObject obj;
            if (poolObjects[prefab.name].Count > 0)
            {
                obj = objectStack.Pop();
            }
            else
            {
                obj = Instantiate(prefab);
            }

            return obj;
        }

        public void ReturnObject(string poolName, GameObject instance)
        {
            if (!poolObjects.TryGetValue(poolName, out Stack<GameObject> objects))
            {
                Debug.Log($"{poolName} not found pool objects");
                return;
            }
            if (!poolParents.TryGetValue(poolName, out Transform parent))
            {
                Debug.Log($"{poolName} not found pool parent");
                return;
            }
            instance.SetActive(false);
            instance.transform.SetParent(parent);
            objects.Push(instance);
        }
    }
}
