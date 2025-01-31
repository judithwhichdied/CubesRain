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
        _bombInfo.text = $"����: ���������� {_bombSpawner.ObjectsCount}\n ������� {_bombSpawner.GetPoolCountActive()}\n ������� {_bombSpawner.ObjectsCount + _bombSpawner.GetPoolCountAll()}";
        _cubeInfo.text = $"�����: ���������� {_cubeSpawner.ObjectsCount}\n ������� {_cubeSpawner.GetPoolCountActive()}\n ������� {_cubeSpawner.ObjectsCount + _cubeSpawner.GetPoolCountAll()}";
    }
}
