using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvasGO;
    [SerializeField] private GameObject settingsMenuCanvasGO;
    [SerializeField] private GameObject audioSettingsMenuCanvasGO;
    [SerializeField] private GameObject keyboardSettingsMenuCanvasGO;

    [SerializeField] private GameObject mainMenuFirst;
    [SerializeField] private GameObject settingsMenuFirst;
    [SerializeField] private GameObject audioSettingsMenuFirst;
    [SerializeField] private GameObject keyboardSettingsMenuFirst;

    [SerializeField] private Image overlayImage;
    [SerializeField] private float fadeDuration = .5f;

    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private float musicFadeDuration = 1f;
    [SerializeField] private float targetMusicVolume = 0.2f;

    private void Start()
    {
        StartCoroutine(FadeInOverlayAndMusic());
        MainMenu();
    }

    private void MainMenu()
    {
        SetMenuVisibility(mainMenuCanvasGO);
        EventSystem.current.SetSelectedGameObject(mainMenuFirst);
    }

    private void SetMenuVisibility(GameObject activeCanvas)
    {
        mainMenuCanvasGO.SetActive(activeCanvas == mainMenuCanvasGO);
        settingsMenuCanvasGO.SetActive(activeCanvas == settingsMenuCanvasGO);
        audioSettingsMenuCanvasGO.SetActive(activeCanvas == audioSettingsMenuCanvasGO);
        keyboardSettingsMenuCanvasGO.SetActive(activeCanvas == keyboardSettingsMenuCanvasGO);
    }

    private void OpenSettingsMenu()
    {
        SetMenuVisibility(settingsMenuCanvasGO);
        EventSystem.current.SetSelectedGameObject(settingsMenuFirst);
    }

    private void OpenAudioSettingsMenu()
    {
        SetMenuVisibility(audioSettingsMenuCanvasGO);
        EventSystem.current.SetSelectedGameObject(audioSettingsMenuFirst);
    }

    private void OpenKeyboardSettingsMenu()
    {
        SetMenuVisibility(keyboardSettingsMenuCanvasGO);
        EventSystem.current.SetSelectedGameObject(keyboardSettingsMenuFirst);
    }

    public void OnSettingsPress()
    {
        OpenSettingsMenu();
    }

    public void OnAudioSettingsPress()
    {
        OpenAudioSettingsMenu();
    }

    public void OnKeyboardSettingsPress()
    {
        OpenKeyboardSettingsMenu();
    }

    public void OnSettingsBackPress()
    {
        MainMenu();
    }

    public void OnAudioSettingsBackPress()
    {
        OpenSettingsMenu();
    }

    public void OnKeyboardSettingsBackPress()
    {
        OpenSettingsMenu();
    }

    public void OnPlayButton()
    {
        PlayerPrefs.SetInt("IsIntroShown", 0);
        StartCoroutine(TransitionToNextScene());
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    private IEnumerator TransitionToNextScene()
    {
        StartCoroutine(FadeOutOverlayAndMusic());
        yield return new WaitForSeconds(Mathf.Max(fadeDuration, musicFadeDuration));
        SceneManager.LoadScene(1);
    }

    private IEnumerator FadeInOverlayAndMusic()
    {
        Color transparentColor = new Color(overlayImage.color.r, overlayImage.color.g, overlayImage.color.b, 0);
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            overlayImage.color = Color.Lerp(overlayImage.color, transparentColor, elapsedTime / fadeDuration);
            backgroundMusic.volume = Mathf.Lerp(0, targetMusicVolume, elapsedTime / musicFadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        overlayImage.color = transparentColor;
        backgroundMusic.volume = targetMusicVolume;
    }

    private IEnumerator FadeOutOverlayAndMusic()
    {
        Color opaqueColor = new Color(overlayImage.color.r, overlayImage.color.g, overlayImage.color.b, 1);
        float elapsedTime = 0f;
        float startVolume = backgroundMusic.volume;
        while (elapsedTime < fadeDuration)
        {
            overlayImage.color = Color.Lerp(overlayImage.color, opaqueColor, elapsedTime / fadeDuration);
            backgroundMusic.volume = Mathf.Lerp(startVolume, 0, elapsedTime / musicFadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        overlayImage.color = opaqueColor;
        backgroundMusic.volume = 0;
    }
}
