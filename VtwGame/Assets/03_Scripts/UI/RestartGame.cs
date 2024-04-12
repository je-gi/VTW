using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public void ReloadCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        PlayerPrefs.SetInt("IsRestarting", 1);
        
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }
}
