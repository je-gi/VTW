using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class GameplayIntro : MonoBehaviour
{
    public GameObject overlayCanvas;
    public TextMeshProUGUI displayText;
    public float delay = 0.01f;
    public string promptText = "Press 'F' to continue";
    public string promptTextColor = "#FF4D06";
    public int promptTextSize = 28;

    public Image backgroundImage;
    public float fadeDuration = 2f;

    public AudioSource mainMusic;
    public AudioSource randomSoundsSource;

    public float maxVolume = 1f;

    private string fullText = 
        "Luxpera, once a radiant land, now lies cloaked in darkness.\n\n" +
        "Lux, the last lightbearer, emerges to revive the fading world. Guided by faint lights, Lux embarks on a quest to uncover the Radiant Core, a legendary beacon believed to restore balance.\n\n" +
        "Step forth, Lux. Your journey to reclaim the light begins now.";

    private bool introShown = false;

    private void Start()
    {
        int isIntroShown = PlayerPrefs.GetInt("IsIntroShown", 0);
        if (isIntroShown == 1)
        {
            introShown = true;
        }

        if (!introShown)
        {
            StartCoroutine(PlayIntro());
        }
        else
        {
            overlayCanvas.SetActive(false);
        }
    }

    IEnumerator PlayIntro()
    {
        Time.timeScale = 0;

        yield return StartCoroutine(FadeInMusicAndBackground());

        displayText.text = "";
        foreach (char letter in fullText)
        {
            displayText.text += letter;
            if (letter != ' ')
            {
                PlayRandomSound();
            }
            yield return new WaitForSecondsRealtime(delay);
        }

        yield return new WaitForSecondsRealtime(1);

        displayText.text += "\n\n\n\n<align=right><size=" + promptTextSize + "><color=" + promptTextColor + ">" + promptText + "</color></size></align>";
    }

    IEnumerator FadeInMusicAndBackground()
    {
        float startAlpha = backgroundImage.color.a;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, newAlpha);
            mainMusic.volume = Mathf.Lerp(0f, maxVolume, elapsedTime / fadeDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 0f);
        mainMusic.volume = maxVolume;
    }

    void PlayRandomSound()
    {
        float randomPitch = Random.Range(0.5f, 2f);
        float randomVolume = Random.Range(0.2f, 0.8f);

        randomSoundsSource.pitch = randomPitch;
        randomSoundsSource.volume = randomVolume;
        randomSoundsSource.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && displayText.text.Contains(promptText))
        {
            PlayerPrefs.SetInt("IsIntroShown", 1);
            Time.timeScale = 1;

            overlayCanvas.SetActive(false);
        }
    }
}
