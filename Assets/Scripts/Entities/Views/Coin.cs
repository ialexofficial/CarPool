using System;
using UnityEngine;

namespace CarPool.Entities.Views
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private int reward = 1;
        [SerializeField] private ParticleSystem collectionVFX;

        private bool _isCollected = false;

        public event Action<int> OnCollect;

        private void Start()
        {
            collectionVFX.transform.parent = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isCollected)
                return;

            _isCollected = true;
            gameObject.SetActive(false);
            collectionVFX.Play();

            OnCollect?.Invoke(reward);
        }
    }
}