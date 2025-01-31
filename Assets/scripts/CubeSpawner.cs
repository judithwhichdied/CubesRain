using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSpawner : Spawner<Cube>
{
    public event Action<Cube> Released;

    private void Start()
    {
        StartCoroutine(StartSpawning());
    }

    private IEnumerator StartSpawning()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnDelay);

        bool isWork = true;

        while (isWork)
        {
            SpawnCube();

            yield return wait;
        }
    }

    private void ReleaseCube(Cube cube)
    {
        _pool.Release(cube);

        Released?.Invoke(cube);

        cube.Died -= ReleaseCube;

        ReleaseObject(cube);
    }

    private void SpawnCube()
    {
        Cube cube;

        cube = _pool.Get();

        cube.transform.position = new Vector3
            (Random.Range(_minSpawnPositionX, _maxSpawnPositionX),
            _spawnPositionY,
            Random.Range(_minSpawnPositionZ, _maxSpawnPositionZ));

        cube.Died += ReleaseCube;
    }
}