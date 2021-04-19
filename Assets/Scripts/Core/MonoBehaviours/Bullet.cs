using Core.ObjectPool;
using UnityEngine;

namespace Core.MonoBehaviours
{
    public class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] private GameObject[] _sprites;

        public void SetSprite(int weaponIndex)
        {
            foreach (var sprite in _sprites)
                sprite.SetActive(false);

            if (weaponIndex > _sprites.Length - 1) _sprites[0].SetActive(true);
            else _sprites[weaponIndex].SetActive(true);
        }
        
        public bool IsAvailable { get; set; }
        
        public void OnPooled() => gameObject.SetActive(true);
        
        public void OnRecycled() => gameObject.SetActive(false);
    }
}
