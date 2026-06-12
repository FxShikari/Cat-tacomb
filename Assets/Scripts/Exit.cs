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
        AudioManager.Instance.PlayMusic(2);
        _sceneManager.ChargeScene(_sceneToCharge);
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.PlayMusic(2);
        _sceneManager.ChargeScene(_sceneToCharge);
    }
}
