using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowerInteraction : MonoBehaviour
{
    [SerializeField] private GameObject interactionWindow;
    [SerializeField] private string endScreenSceneName = "EndScreen";

    private bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactionWindow.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionWindow.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene(endScreenSceneName);
        }
    }
}
