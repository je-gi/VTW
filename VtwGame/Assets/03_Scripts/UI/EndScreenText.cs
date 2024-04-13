using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenText : MonoBehaviour
{
    public GameObject overlayCanvas;
    public TextMeshProUGUI displayText;
    public AudioSource mainMusic;
    public AudioSource randomSoundsSource;
    public float delayBetweenLetters = 0.05f;
    public float delayBeforeText = 2f;
    public float fadeDuration = 2f;
    public float textFadeOutDuration = 1f;
    public float musicFadeDuration = 2f;
    public float shortSoundDuration = 0.1f;

    private string fullText =
        "Lux stood amidst the shadows, gazing into the distance. Despite the darkness that veiled Luxpera, a glimmer of hope pierced through.\n\n" +
        "A new chapter lies ahead, Lux. Prepare yourself, for the true adventure is yet to unfold.";

    private void Start()
    {
        StartCoroutine(AnimateOverlay());
    }

    IEnumerator AnimateOverlay()
    {
        StartCoroutine(FadeInMusic(0f, 1f, musicFadeDuration));
        yield return StartCoroutine(FadeOverlay(1f, 0f, fadeDuration));
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        displayText.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            displayText.text += fullText[i];
            if (fullText[i] != ' ')
            {
                PlayRandomSound();
                if (i == fullText.Length - 1)
                {
                    StartCoroutine(StopSoundShort());
                }
            }
            yield return new WaitForSecondsRealtime(delayBetweenLetters);
        }
        yield return new WaitForSecondsRealtime(delayBeforeText);
        StartCoroutine(FadeText(1f, 0f, textFadeOutDuration));
        yield return new WaitForSeconds(textFadeOutDuration);
        StartCoroutine(FadeOverlay(0f, 1f, fadeDuration));
        StartCoroutine(FadeOutMusic(1f, 0f, musicFadeDuration));
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(0);
    }

    void PlayRandomSound()
    {
        float randomPitch = Random.Range(0.5f, 2f);
        float randomVolume = Random.Range(0.2f, 0.8f);
        randomSoundsSource.pitch = randomPitch;
        randomSoundsSource.volume = randomVolume;
        randomSoundsSource.Play();
    }

    IEnumerator StopSoundShort()
    {
        yield return new WaitForSecondsRealtime(shortSoundDuration);
        randomSoundsSource.Stop();
    }

    IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float timer = 0f;
        Color textColor = displayText.color;
        while (timer < duration)
        {
            float t = timer / duration;
            textColor.a = Mathf.Lerp(startAlpha, endAlpha, t);
            displayText.color = textColor;
            yield return null;
            timer += Time.deltaTime;
        }
        textColor.a = endAlpha;
        displayText.color = textColor;
    }

    IEnumerator FadeOverlay(float startAlpha, float endAlpha, float duration)
    {
        float timer = 0f;
        Color color = overlayCanvas.GetComponent<Image>().color;
        while (timer < duration)
        {
            float t = timer / duration;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            overlayCanvas.GetComponent<Image>().color = color;
            yield return null;
            timer += Time.deltaTime;
        }
        color.a = endAlpha;
        overlayCanvas.GetComponent<Image>().color = color;
    }

    IEnumerator FadeInMusic(float startVolume, float endVolume, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            mainMusic.volume = Mathf.Lerp(startVolume, endVolume, timer / duration);
            yield return null;
            timer += Time.deltaTime;
        }
        mainMusic.volume = endVolume;
    }

    IEnumerator FadeOutMusic(float startVolume, float endVolume, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            mainMusic.volume = Mathf.Lerp(startVolume, endVolume, timer / duration);
            yield return null;
            timer += Time.deltaTime;
        }
        mainMusic.volume = endVolume;
    }
}
