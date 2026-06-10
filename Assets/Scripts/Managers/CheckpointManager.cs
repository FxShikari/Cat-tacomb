using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }
    [SerializeField] private CheckPoint[] _checkPointList;
    private PlayerCharacter _player;

    [SerializeField] private CheckPoint _currentCheckpoint;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        _player = GameManager.Instance.GetPlayer();

        // Setup la liste des Checkpoint
        for (int i = 0; i < _checkPointList.Length; i++)
        {
            _checkPointList[i].SetIndex(i);
        }
    }

    public void ReturnToLastCheckpoint()
    {
        print(_currentCheckpoint.GetIndex());
        _player.transform.position = _currentCheckpoint.GetSpawnPoint().position;
    }


    public void UpdateCheckpoint(CheckPoint checkPoint)
    {
        _currentCheckpoint = checkPoint;
    }
}
