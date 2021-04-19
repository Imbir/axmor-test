using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.ObjectPool
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private List<T> _pool;
        private T _prefab;
        private Transform _parent;

        public int Count => _pool.Count;

        public T[] ToArray => _pool.ToArray();

        public ObjectPool(T prefab, Transform parent = null)
        {
            _pool = new List<T>();
            _prefab = prefab;
            _parent = parent;
        }

        public T GetObject()
        {
            var item = _pool.FirstOrDefault(i => i.IsAvailable);

            if (item == null)
            {
                item = Object.Instantiate(_prefab, _parent);
                _pool.Add(item);
            }

            item.IsAvailable = false;
            item.OnPooled();

            return item;
        }

        public void Recycle(T itemToRecycle)
        {
            var item = _pool.FirstOrDefault(i => i == itemToRecycle);
            
            if (item == null) return;

            item.IsAvailable = true;
            item.OnRecycled();
        }
    }
}