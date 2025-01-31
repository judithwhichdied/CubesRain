using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Bomb : MonoBehaviour
{
    private Renderer _renderer;

    private float _randomMinValue = 2f;
    private float _randomMaxValue = 5f;
    private float _explosionRadius = 200f;
    private float _explosionForce = 200f;
    private float _step;
    private float _timeCount;

    public event Action<Bomb> Destroyed;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        _renderer.material.color = Color.black;
    }

    public void StartTimer()
    {
        StartCoroutine(ExplodeCountDown());
    }

    private IEnumerator ExplodeCountDown()
    {
        _timeCount = Random.Range(_randomMinValue, _randomMaxValue);

        float currentTime = 0;

        while (currentTime < _timeCount)
        {
            currentTime += Time.deltaTime;

            _step = currentTime / _timeCount;

            _renderer.material.color = new Color
                (0, 0, 0, Mathf.MoveTowards(_renderer.material.color.a, 0, _step * Time.deltaTime));

            yield return null;
        }

        Explode();

        Destroyed?.Invoke(this);

        _renderer.material.color = Color.black;
    }

    private List<Rigidbody> GetExplodableObjects()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        List<Rigidbody> cubes = new List<Rigidbody>();

        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null)
            {
                cubes.Add(hit.attachedRigidbody);
            }
        }

        return cubes;
    }

    private void Explode()
    {
        List<Rigidbody> cubes = GetExplodableObjects();

        foreach (Rigidbody explodableObject in cubes)
        {
            explodableObject.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
        }
    }
}