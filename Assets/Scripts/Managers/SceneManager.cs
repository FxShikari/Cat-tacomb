using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void ChargeScene(string scene)
    {
        print("ok");
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    public void StartGame()
    {
        AudioManager.Instance.PlayMusic(1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Chateau");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
}
