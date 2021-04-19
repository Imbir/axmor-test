using Core.ObjectPool;
using UnityEngine;

namespace Core.MonoBehaviours
{
    public class Torpedo : MonoBehaviour, IPoolable
    { 
        public bool IsAvailable { get; set; }
        
        public void OnPooled() => gameObject.SetActive(true);
        
        public void OnRecycled() => gameObject.SetActive(false);
    }
}