using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }
    [SerializeField] private Transform[] _checkPointList;
    private PlayerCharacter _player;

    [SerializeField] private int _currentTransformIndex = -1;


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
        StartCoroutine(UpdateCheckpoint());
    }

    public void ReturnToLastCheckpoint()
    {
        print(_currentTransformIndex);
        _player.transform.position = _checkPointList[_currentTransformIndex].position;
    }

    /// <summary>
    /// Toutes les secondes, on regarde la position du joueur et un mets à jour par rapport à sa position dans les checkpoint.
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateCheckpoint()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < _checkPointList.Length; i++)
            {
                print(i);
                if (_player.transform.position.x < _checkPointList[i].position.x)
                {
                    _currentTransformIndex = i - 1;
                    break;
                }
            }
        }
    }
}
