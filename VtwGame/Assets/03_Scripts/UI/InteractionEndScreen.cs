using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FlowerInteraction : MonoBehaviour
{
    [SerializeField] private GameObject interactionWindow;
    [SerializeField] private Image imageToFadeIn;
    [SerializeField] private string endScreenSceneName = "EndScreen";
    [SerializeField] private float fadeInDuration = 1.0f;  

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
            StartCoroutine(FadeInAndLoadScene());
        }
    }

    private IEnumerator FadeInAndLoadScene()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            float alpha = elapsedTime / fadeInDuration;
            imageToFadeIn.color = new Color(imageToFadeIn.color.r, imageToFadeIn.color.g, imageToFadeIn.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        imageToFadeIn.color = new Color(imageToFadeIn.color.r, imageToFadeIn.color.g, imageToFadeIn.color.b, 1);
        SceneManager.LoadScene(endScreenSceneName);
    }
}
