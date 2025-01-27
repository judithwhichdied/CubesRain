using TMPro;
using UnityEngine;

[RequireComponent(typeof(Spawner))]
public class View : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bombInfo;
    [SerializeField] private TextMeshProUGUI _cubeInfo;

    private Spawner _spawner;

    private void Awake()
    {
        _spawner = GetComponent<Spawner>();
    }

    private void OnEnable()
    {
        _spawner.BombSpawned += DrawInfo;
        _spawner.CubeSpawned += DrawInfo;
    }

    private void OnDisable()
    {
        _spawner.BombSpawned -= DrawInfo;
        _spawner.CubeSpawned -= DrawInfo;
    }

    private void DrawInfo()
    {
        _bombInfo.text = $"бомб: создано {_spawner.BombsCount}\n активно {_spawner.BombsActiveCount}";
        _cubeInfo.text = $"кубов: создано {_spawner.CubesCount}\n активно {_spawner.Pool.CountActive}";
    }
}
