using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int lives;
    [SerializeField] private Missile missile;
    [SerializeField] private float speed, turnSpeed, fireRate, respawnTime;
    private float _lastShot;
    private Vector2 _inputs;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private Collider2D _col;
    private Transform _transform;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _col = GetComponent<Collider2D>();
        _transform = transform;
    }

    private void Update()
    {
        _inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKey(KeyCode.Space) && Time.time - _lastShot >= 1f / fireRate) Shoot();
    }

    private void FixedUpdate()
    {
        _rb.AddForce(_transform.up * (speed * _inputs.y));
        _rb.AddTorque(_inputs.x * -turnSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(RespawnRoutine());
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

    private void Shoot()
    {
        _lastShot = Time.time;
        Instantiate(missile, _transform.position, _transform.rotation);
    }

    private IEnumerator RespawnRoutine()
    {
        lives--;
        _col.enabled = _sr.enabled = _rb.simulated = enabled = false;
        
        if (lives <= 0)
        {
            Debug.Log("Game Over");
            yield break;
        }
        
        yield return new WaitForSeconds(respawnTime);
        _sr.enabled = _rb.simulated = enabled = true;
        _transform.position = Vector3.zero;
        yield return new WaitForSeconds(1f);
        _col.enabled = true;
    }
}