using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public void ReloadCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }
}
