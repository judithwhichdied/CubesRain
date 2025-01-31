using TMPro;
using UnityEngine;

public class View : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bombInfo;
    [SerializeField] private TextMeshProUGUI _cubeInfo;

    [SerializeField] private BombSpawner _bombSpawner;
    [SerializeField] private CubeSpawner _cubeSpawner;

    private void OnEnable()
    {
        _bombSpawner.Spawned += DrawInfo;
        _cubeSpawner.Spawned += DrawInfo;
        _bombSpawner.ActiveObjectsCountChanged += DrawInfo;
        _cubeSpawner.ActiveObjectsCountChanged += DrawInfo;
    }

    private void OnDisable()
    {
        _bombSpawner.Spawned -= DrawInfo;
        _cubeSpawner.Spawned -= DrawInfo;
        _bombSpawner.ActiveObjectsCountChanged -= DrawInfo;
        _cubeSpawner.ActiveObjectsCountChanged-= DrawInfo;
    }

    private void DrawInfo()
    {
        _bombInfo.text = $"бомб: заспавнено {_bombSpawner.ObjectsCount}\n активно {_bombSpawner.GetPoolCountActive()}\n создано {_bombSpawner.ObjectsCount + _bombSpawner.GetPoolCountAll()}";
        _cubeInfo.text = $"кубов: заспавнено {_cubeSpawner.ObjectsCount}\n активно {_cubeSpawner.GetPoolCountActive()}\n создано {_cubeSpawner.ObjectsCount + _cubeSpawner.GetPoolCountAll()}";
    }
}
