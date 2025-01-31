using System;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected float _spawnDelay = 1;
    [SerializeField] protected int _poolCapacity = 10;
    [SerializeField] protected int _poolMaxSize = 10;
    [SerializeField] protected T _object;

    protected float _spawnPositionY = 10;
    protected float _minSpawnPositionX = -5;
    protected float _maxSpawnPositionX = 5;
    protected float _minSpawnPositionZ = -5;
    protected float _maxSpawnPositionZ = 5;

    protected ObjectPool<T> _pool;

    public event Action Spawned;
    public event Action ActiveObjectsCountChanged;

    public int ObjectsCount { get; protected set; }

    protected void Awake()
    {
        _pool = new ObjectPool<T>
            (
                 createFunc: () => Instantiate(_object),
                 actionOnGet:OnGet,
                 actionOnRelease: (@object) => ReleaseObject(@object),
                 actionOnDestroy: (@object) => Destroy(@object.gameObject),
                 collectionCheck: true,
                 defaultCapacity: _poolCapacity,
                 maxSize: _poolMaxSize
            );       
    }

    public int GetPoolCountActive()
    {
        return _pool.CountActive;
    }

    public int GetPoolCountAll()
    {
        return _pool.CountAll;
    }

    protected void ReleaseObject(T cube)
    {
        cube.gameObject.SetActive(false);

        ActiveObjectsCountChanged?.Invoke();
    }

    protected void OnGet(T cube)
    {        
        cube.gameObject.SetActive(true);

        ObjectsCount++;
        Spawned?.Invoke();
    } 
}