using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    private void OnEnable()
    {
        _cubeSpawner.Released += SpawnBomb; 
    }

    private void OnDisable()
    {
        _cubeSpawner.Released -= SpawnBomb;
    }

    private void SpawnBomb(Cube cube)
    {
        Bomb bomb = _pool.Get();

        bomb.transform.position = cube.transform.position;

        bomb.Destroyed += ReleaseBomb;

        bomb.StartTimer();
    }

    private void ReleaseBomb(Bomb bomb)
    {
        bomb.Destroyed -= ReleaseBomb;

        _pool.Release(bomb);
    }
}
