using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent (typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private float _minLifeDuration = 2;
    private float _maxLifeDuration = 5;

    private Color _color = Color.black;

    private Renderer _renderer;

    private bool _collisionHappened;

    public event Action<Cube> IsDied;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        _renderer.material.color = _color;

        _collisionHappened = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out Platform platform) && _collisionHappened == false)
        {
            _collisionHappened = true;

            _renderer.material.color = new Color(Random.value, Random.value, Random.value);

            StartCoroutine(nameof(LifeCountDown));
        }
    }

    private IEnumerator LifeCountDown()
    {
        yield return new WaitForSeconds(Random.Range(_minLifeDuration, _maxLifeDuration));

        IsDied?.Invoke(this);
    }
}
