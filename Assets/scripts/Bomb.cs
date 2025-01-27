using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Bomb : MonoBehaviour
{
    private Renderer _renderer;

    private float _randomMinValue = 2f;
    private float _randomMaxValue = 5f;
    private float _explosionRadius = 500f;
    private float _explosionForce = 300f;
    private float _step;

    private bool _coroutineEnabled = false;

    public float TimeCount { get; private set; }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        _renderer.material.color = Color.black;
    }

    public void StartTimer()
    {
        StartCoroutine(nameof(ExplodeCountDown));
    }

    private IEnumerator ExplodeCountDown()
    {
        TimeCount = Random.Range(_randomMinValue, _randomMaxValue);

        _step = 1 / TimeCount;

        WaitForSeconds wait = new WaitForSeconds(1);

        while(TimeCount > 0)
        {
            StartCoroutine(AlphaScalling(TimeCount));

            TimeCount--;

            yield return wait;
        }

        Explode();
        Destroy(gameObject);
    }

    private IEnumerator AlphaScalling(float time)
    {
        if (_coroutineEnabled)
            yield break;

        _coroutineEnabled = true;

        while(time > 0)
        {
            _renderer.material.color = new Color(0, 0, 0, Mathf.MoveTowards(_renderer.material.color.a, 0, _step*Time.deltaTime));

            yield return null;
        }

        _coroutineEnabled = false;
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