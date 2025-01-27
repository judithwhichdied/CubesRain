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
        _bombInfo.text = $"����: ������� {_spawner.BombsCount}\n ������� {_spawner.BombsActiveCount}";
        _cubeInfo.text = $"�����: ������� {_spawner.CubesCount}\n ������� {_spawner.Pool.CountActive}";
    }
}
