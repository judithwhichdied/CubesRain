using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _spawnDelay = 1;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;
    [SerializeField] private Cube _cube;
    [SerializeField] private Bomb _bomb;

    private float _spawnPositionY = 10;
    private float _minSpawnPositionX = -5;
    private float _maxSpawnPositionX = 5;
    private float _minSpawnPositionZ = -5;
    private float _maxSpawnPositionZ = 5;

    public ObjectPool<Cube> Pool { get; private set; }

    public int CubesCount { get; private set; }
    public int BombsCount { get; private set; }
    public int BombsActiveCount { get; private set; }

    public event Action CubeSpawned;
    public event Action BombSpawned;

    private void Awake()
    {
        Pool = new ObjectPool<Cube>
            (
                 createFunc: () => Instantiate(_cube),
                 actionOnGet:OnGet,
                 actionOnRelease: (cube) => SpawnBomb(cube),
                 actionOnDestroy: (cube) => Destroy(cube.gameObject),
                 collectionCheck: true,
                 defaultCapacity: _poolCapacity,
                 maxSize: _poolMaxSize
            );       
    }

    private void Start()
    {
        StartCoroutine(nameof(StartSpawning));
    }

    private void ReleaseCube(Cube cube)
    {
        Pool.Release(cube);

        cube.IsDied -= ReleaseCube;
    }

    private void OnGet(Cube cube)
    {
        cube.transform.position = new Vector3
            (Random.Range(_minSpawnPositionX, _maxSpawnPositionX),
            _spawnPositionY,
            Random.Range(_minSpawnPositionZ, _maxSpawnPositionZ));

        cube.IsDied += ReleaseCube;

        cube.gameObject.SetActive(true);

        CubesCount++;
        CubeSpawned?.Invoke();
    }

    private void GetCube()
    {
        Pool.Get();
    }

    private IEnumerator StartSpawning()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnDelay);

        bool isWork = true;

        while (isWork)
        {
            GetCube();

            yield return wait;
        }
    }

    private void SpawnBomb(Cube cube)
    {
        Bomb bomb;

        cube.gameObject.SetActive(false);

        bomb = Instantiate(_bomb, cube.transform.position, Quaternion.identity);

        BombsCount++;
        BombsActiveCount++;
        BombSpawned?.Invoke();

        bomb.StartTimer();

        BombsActiveCount--;
    }
}