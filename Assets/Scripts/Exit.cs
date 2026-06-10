using UnityEngine;

class Exit : Interactable
{
    [SerializeField] private SceneManager _sceneManager;

    [SerializeField] private string _sceneToCharge;

    private void Awake()
    {
        _sceneManager = FindFirstObjectByType<SceneManager>();
    }

    public override void Interation()
    {
        _sceneManager.ChargeScene(_sceneToCharge);
    }
}
