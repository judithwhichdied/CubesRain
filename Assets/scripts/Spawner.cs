using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _spawnDelay = 1;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 10;
    [SerializeField] private Cube _cube;

    private float _spawnPositionY = 10;
    private float _minSpawnPositionX = -5;
    private float _maxSpawnPositionX = 5;
    private float _minSpawnPositionZ = -5;
    private float _maxSpawnPositionZ = 5;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>
            (
                 createFunc: () => Instantiate(_cube),
                 actionOnGet:OnGet,
                 actionOnRelease: (cube) => cube.gameObject.SetActive(false),
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
        _pool.Release(cube);

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
    }

    private void GetCube()
    {
        _pool.Get();
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
}
