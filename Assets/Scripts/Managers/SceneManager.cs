using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void ChargeScene(string scene)
    {
        print("ok");
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
