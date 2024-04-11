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
        Invoke(nameof(LoadNextScene), 0.6f);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
    #endregion

    #region Delay Load Scene
    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
    #endregion
}



