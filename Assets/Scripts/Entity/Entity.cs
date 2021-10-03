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

        protected virtual void Update()
        {
            var position = Transform.position;
            if (Mathf.Abs(position.x) > 20f)
            {
                Transform.position = new Vector3(-Mathf.Sign(position.x) * 20f, position.y);
            }

            if (Mathf.Abs(position.y) > 20f)
            {
                Transform.position = new Vector3(position.x, -Mathf.Sign(position.y) * 20f);
            }
        }
    }
}