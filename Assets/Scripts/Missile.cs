using System;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private LayerMask layerToHit;
    [SerializeField] private float speed, lifeTime;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        var hit = Physics2D.BoxCast(_transform.position, _transform.localScale, 0, Vector2.zero, 0, layerToHit);
        if (hit)
        {
            //Split asteroid
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        _transform.position += _transform.up * (speed * Time.fixedDeltaTime);
    }

    private void OnBecameInvisible()
    {
        var position = _transform.position;

        if (position.x <= -20 || position.x >= 20)
        {
            position.x = -position.x;
        }

        if (position.y <= -20 || position.y >= 20)
        {
            position.y = -position.y;
        }

        _transform.position = position;
    }
}