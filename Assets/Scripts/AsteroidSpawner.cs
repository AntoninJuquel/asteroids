using System.Collections.Generic;
using Entity;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private Asteroid asteroid;
    [SerializeField] private Wave[] waves;
    [SerializeField] private float spawnDistance;
    private HashSet<Asteroid> _asteroids;
    private int _waveIndex, _waveNumber;
    private Wave CurrentWave => waves[_waveIndex];

    private void Start()
    {
        NextWave();
    }

    private void NextWave()
    {
        _asteroids = new HashSet<Asteroid>();

        _waveIndex = Random.Range(0, waves.Length);
        _waveNumber++;

        asteroid.Setup(CurrentWave.sizeMinMax, CurrentWave.speedMinMax);

        for (var i = 0; i < CurrentWave.asteroidNumber; i++)
        {
            var direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            var newAsteroid = Instantiate(asteroid, transform.position + direction * spawnDistance, Quaternion.identity);
            OnNewAsteroid(newAsteroid);
        }
    }

    private void OnNewAsteroid(Asteroid newAsteroid)
    {
        _asteroids.Add(newAsteroid);
        newAsteroid.OnNewAsteroid += OnNewAsteroid;
        newAsteroid.OnDestroyAsteroid += OnDestroyAsteroid;
    }

    private void OnDestroyAsteroid(Asteroid toDelete)
    {
        toDelete.OnNewAsteroid -= OnNewAsteroid;
        toDelete.OnDestroyAsteroid -= OnDestroyAsteroid;
        
        _asteroids.Remove(toDelete);
        
        if (_asteroids.Count == 0)
        {
            NextWave();
        }
    }

    [System.Serializable]
    private struct Wave
    {
        public int asteroidNumber;
        public Vector2Int sizeMinMax;
        public Vector2 speedMinMax;
    }
}