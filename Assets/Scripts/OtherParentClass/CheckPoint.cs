using System.ComponentModel;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private int index;
    [SerializeField] private Transform spawnPoint;

    public int GetIndex()
    {
        return index;
    }

    public void SetIndex(int val)
    {
        index = val;
    }

    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckpointManager.Instance.UpdateCheckpoint(this);
    }
}
