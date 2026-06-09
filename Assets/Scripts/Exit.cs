using UnityEditor;
using UnityEngine;

class Exit : Interactable
{
    [SerializeField] private SceneManager _sceneManager;

    [SerializeField] private SceneAsset _sceneToCharge;

    private void Awake()
    {
        _sceneManager = FindFirstObjectByType<SceneManager>();
    }

    public override void Interation()
    {
        _sceneManager.ChargeScene(_sceneToCharge);
    }
}
