using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entity
{
    public class Asteroid : Entity
    {
        [SerializeField] private Vector2Int sizeMinMax;
        [SerializeField] private Vector2 speedMinMax;
        [SerializeField] private Sprite[] asteroids;
        private SpriteRenderer _sr;
        private Rigidbody2D _rb;
        public int Size { get; private set; }

        public event Action<Asteroid> OnNewAsteroid, OnDestroyAsteroid;

        protected override void Awake()
        {
            base.Awake();
            _sr = GetComponent<SpriteRenderer>();
            _sr.sprite = asteroids[Random.Range(0, asteroids.Length)];

            _rb = GetComponent<Rigidbody2D>();
            _rb.velocity = Random.insideUnitCircle.normalized * Random.Range(speedMinMax.x, sizeMinMax.y);

            Size = Random.Range(sizeMinMax.x, sizeMinMax.y + 1);
            Transform.localScale = (Size == 1 ? .5f : Size - 1) * Vector3.one;
        }

        private void OnDisable()
        {
            OnDestroyAsteroid?.Invoke(this);
        }

        public void Setup(Vector2Int size, Vector2 speed)
        {
            sizeMinMax = size;
            speedMinMax = speed;
        }

        public void Split()
        {
            if (Size > sizeMinMax.x)
            {
                for (var i = 0; i < 2; i++)
                {
                    var newAsteroid = Instantiate(this, Transform.position, Transform.rotation);
                    newAsteroid.Size = Size - 1;
                    newAsteroid.transform.localScale = (newAsteroid.Size == 1 ? .5f : newAsteroid.Size - 1) * Vector3.one;
                    OnNewAsteroid?.Invoke(newAsteroid);
                }
            }

            Destroy(gameObject);
        }
    }
}