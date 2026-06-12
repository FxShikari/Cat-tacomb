using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip _musicChateau;
    public AudioSource _audioChateau;

    public AudioClip _musiCatacomb;
    public AudioSource _audioCatacomb;
    public static AudioManager Instance { get; private set; }
    [SerializeField] private PlayerCharacter _character;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }



    public void PlayMusic(int num)
    {
        if (num == 1)
        {
            _audioChateau.Play();
        }
        else if (num == 2)
        {
            _audioCatacomb.Play();
        }
    }
}
