using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndScreenText : MonoBehaviour
{
    public GameObject overlayCanvas;
    public TextMeshProUGUI displayText;
    public float delayBetweenLetters = 0.05f;
    public float delayBeforeTransition = 2f;

    private string fullText =
    "Lux stood amidst the shadows, gazing into the distance. Despite the darkness that veiled Luxpera, a glimmer of hope pierced through.\n\n" +
    "A new chapter lies ahead, Lux. Prepare yourself, for the true adventure is yet to unfold.";

    private bool isTransitioning = false;

    private void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        displayText.text = "";
        foreach (char letter in fullText)
        {
            displayText.text += letter;
            yield return new WaitForSecondsRealtime(delayBetweenLetters);
        }
        yield return new WaitForSecondsRealtime(delayBeforeTransition);

        if (!isTransitioning)
        {
            StartCoroutine(TransitionToMain());
        }
    }

    IEnumerator TransitionToMain()
    {
        isTransitioning = true;

        float duration = 2f;
        float timer = 0f;
        Color originalColor = overlayCanvas.GetComponent<UnityEngine.UI.Image>().color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        while (timer < duration)
        {
            float t = timer / duration;
            overlayCanvas.GetComponent<UnityEngine.UI.Image>().color = Color.Lerp(originalColor, targetColor, t);
            yield return null;
            timer += Time.unscaledDeltaTime;
        }

        SceneManager.LoadScene(0);
    }
}
