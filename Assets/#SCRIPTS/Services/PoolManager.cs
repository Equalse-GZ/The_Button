using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Services
{
    public class PoolManager : MonoBehaviour
    {
        public class Pool
        {
            private List<GameObject> _inactive = new List<GameObject>();
            private GameObject _prefab;

            public Pool(GameObject prefab)
            {
                _prefab = prefab;
            }

            public GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent)
            {
                GameObject obj;
                if(_inactive.Count == 0)
                {
                    obj = Instantiate(_prefab, position, rotation);
                    obj.name = _prefab.name;
                    obj.transform.SetParent(parent);
                }
                else
                {
                    obj = _inactive[_inactive.Count - 1];
                    _inactive.RemoveAt(_inactive.Count - 1);
                }

                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }

            public void Despawn(GameObject obj)
            {
                obj.SetActive(false);
                _inactive.Add(obj);
            }
        }

        private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

        private void Initialize(GameObject prefab)
        {
            if(prefab != null && _pools.ContainsKey(prefab.name) == false)
                _pools[prefab.name] = new Pool(prefab);
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            Initialize(prefab);
            return _pools[prefab.name].Spawn(position, rotation,parent);
        }

        public void Despawn(GameObject obj)
        {
            if (_pools.ContainsKey(obj.name))
                _pools[obj.name].Despawn(obj);
            else
                Destroy(obj);
        }
    }
}
