using UnityEngine;

namespace Entity
{
    public class Missile : Entity
    {
        [SerializeField] private LayerMask layerToHit;
        [SerializeField] private float speed, lifeTime;

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        protected override void Update()
        {
            base.Update();
            var hit = Physics2D.BoxCast(Transform.position, Transform.localScale, 0, Vector2.zero, 0, layerToHit);
            if (hit)
            {
                hit.transform.GetComponent<Asteroid>().Split();
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            Transform.position += Transform.up * (speed * Time.fixedDeltaTime);
        }
    }
}