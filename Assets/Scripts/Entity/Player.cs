using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Entity
{
    public class Player : Entity
    {
        [SerializeField] private GameObject destroyParticles;
        [SerializeField] private TextMeshProUGUI livesText;
        [SerializeField] private int lives;
        [SerializeField] private Missile missile;
        [SerializeField] private float speed, turnSpeed, fireRate, respawnTime;
        private float _lastShot;
        private Vector2 _inputs;
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private Collider2D _col;
        private LineRenderer _lr;

        protected override void Awake()
        {
            base.Awake();
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _col = GetComponent<Collider2D>();
            _lr = GetComponent<LineRenderer>();
        }

        protected override void Update()
        {
            base.Update();
            _inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (Input.GetKey(KeyCode.Space) && Time.time - _lastShot >= 1f / fireRate) Shoot();
            _lr.SetPosition(1, Vector3.Lerp(_lr.GetPosition(1), Vector3.down * Mathf.Clamp(_inputs.y, 0.25f, 1f), Time.deltaTime * 7f));
        }

        private void FixedUpdate()
        {
            _rb.AddForce(Transform.up * (speed * _inputs.y));
            Transform.Rotate(0, 0, _inputs.x * -turnSpeed * Time.fixedDeltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Instantiate(destroyParticles, transform.position, Quaternion.identity);
            StartCoroutine(RespawnRoutine());
        }

        private void Shoot()
        {
            _lastShot = Time.time;
            Instantiate(missile, Transform.position, Transform.rotation);
        }

        private IEnumerator RespawnRoutine()
        {
            lives--;
            livesText.text = string.Concat(Enumerable.Repeat("<sprite=0>", lives));
            _col.enabled = _sr.enabled = _rb.simulated = enabled = false;

            if (lives <= 0)
            {
                Debug.Log("Game Over");
                yield break;
            }

            yield return new WaitForSeconds(respawnTime);
            _sr.enabled = _rb.simulated = enabled = true;
            _rb.angularVelocity = 0;
            _rb.velocity = Transform.position = Vector3.zero;
            yield return new WaitForSeconds(1f);
            _col.enabled = true;
        }
    }
}