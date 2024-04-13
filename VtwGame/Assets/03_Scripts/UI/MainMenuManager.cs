using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("Canvas")]
    [SerializeField] private GameObject mainMenuCanvasGO;
    [SerializeField] private GameObject settingsMenuCanvasGO;
    [SerializeField] private GameObject audioSettingsMenuCanvasGO;
    [SerializeField] private GameObject keyboardSettingsMenuCanvasGO;

    [Header("First selected Options")]
    [SerializeField] private GameObject mainMenuFirst;
    [SerializeField] private GameObject settingsMenuFirst;
    [SerializeField] private GameObject audioSettingsMenuFirst;
    [SerializeField] private GameObject keyboardSettingsMenuFirst;

    [Header("Overlay")]
    [SerializeField] private Image overlayImage;
    [SerializeField] private float fadeDuration = .5f;

    [Header("Background Music")]
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private float musicFadeDuration = 1f;
    [SerializeField] private float targetMusicVolume = 0.2f;
    #endregion

    private void Start()
    {
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

    #region Menu Visibility
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
    #endregion

    #region UI Buttons
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
    #endregion

    #region Transition to Next Scene
    private IEnumerator TransitionToNextScene()
    {
        float elapsedTime = 0f;
        Color originalColor = overlayImage.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        while (elapsedTime < fadeDuration)
        {
            float normalizedTime = elapsedTime / fadeDuration;
            overlayImage.color = Color.Lerp(originalColor, targetColor, normalizedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        overlayImage.color = targetColor;

        elapsedTime = 0f;
        float originalVolume = backgroundMusic.volume;

        while (elapsedTime < musicFadeDuration)
        {
            float normalizedTime = elapsedTime / musicFadeDuration;
            backgroundMusic.volume = Mathf.Lerp(originalVolume, targetMusicVolume, normalizedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        backgroundMusic.volume = targetMusicVolume;

        SceneManager.LoadScene(1);
    }
    #endregion
}



