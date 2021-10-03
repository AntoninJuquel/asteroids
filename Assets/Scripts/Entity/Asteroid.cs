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
        private int _size;

        public event Action<Asteroid> OnNewAsteroid, OnDestroyAsteroid;

        protected override void Awake()
        {
            base.Awake();
            _sr = GetComponent<SpriteRenderer>();
            _sr.sprite = asteroids[Random.Range(0, asteroids.Length)];

            _rb = GetComponent<Rigidbody2D>();
            _rb.velocity = Random.insideUnitCircle.normalized * Random.Range(speedMinMax.x, sizeMinMax.y);

            _size = Random.Range(sizeMinMax.x, sizeMinMax.y + 1);
            Transform.localScale = (_size == 1 ? .5f : _size - 1) * Vector3.one;
        }

        public void Setup(Vector2Int size, Vector2 speed)
        {
            sizeMinMax = size;
            speedMinMax = speed;
        }

        public void Split()
        {
            if (_size > sizeMinMax.x)
            {
                for (var i = 0; i < 2; i++)
                {
                    var newAsteroid = Instantiate(this, Transform.position, Transform.rotation);
                    newAsteroid._size = _size - 1;
                    newAsteroid.transform.localScale = (newAsteroid._size == 1 ? .5f : newAsteroid._size - 1) * Vector3.one;
                    OnNewAsteroid?.Invoke(newAsteroid);
                }
            }

            OnDestroyAsteroid?.Invoke(this);
            Destroy(gameObject);
        }
    }
}