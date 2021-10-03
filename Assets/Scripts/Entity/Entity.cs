using System;
using UnityEngine;

namespace Entity
{
    public class Entity : MonoBehaviour
    {
        protected Transform Transform;

        protected virtual void Awake()
        {
            Transform = transform;
        }

        private void OnBecameInvisible()
        {
            var position = Transform.position;

            if (position.x <= -20 || position.x >= 20)
            {
                position.x = -position.x;
            }

            if (position.y <= -20 || position.y >= 20)
            {
                position.y = -position.y;
            }

            Transform.position = position;

            Invoke(nameof(AutoDestroy), 2);
        }

        private void OnBecameVisible()
        {
            CancelInvoke();
        }

        private void AutoDestroy() => Destroy(gameObject);
    }
}