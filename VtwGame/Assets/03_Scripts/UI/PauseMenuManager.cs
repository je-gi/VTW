using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
  [SerializeField] private GameObject _pauseMenuCanvasGO;
  [SerializeField] private GameObject _settingsMenuCanvasGO;
  [SerializeField] private GameObject _audioSettingsMenuCanvasGO;
  [SerializeField] private GameObject _keyboardSettingsMenuCanvasGO;

  [Header("Input Action Asset to deactivate on Pause")]
  [SerializeField] private PlayerInput playerInput;

  [Header("First selected Options")]
  [SerializeField] private GameObject _pauseMenuFirst;
  [SerializeField] private GameObject _settingsMenuFirst;
  [SerializeField] private GameObject _audioSettingsMenuFirst;
  [SerializeField] private GameObject _keyboardSettingsMenuFirst;
  private bool isPaused;


  private void Start()
  {
    _pauseMenuCanvasGO.SetActive(false);
    _settingsMenuCanvasGO.SetActive(false);
    _audioSettingsMenuCanvasGO.SetActive(false);
    _keyboardSettingsMenuCanvasGO.SetActive(false);
  }


  private void Update()
  {
    if (InputManager.instance.MenuOpenInput)
    {
      if (!isPaused)
      {
        Pause();
      }
    }
    else if (InputManager.instance.MenuCloseInput)
    {
      if (isPaused)
        Unpause();
    }

  }


  #region Pause/unpause Functions


  public void Pause()
  {
    isPaused = true;
    Time.timeScale = 0f;
    OpenPauseMenu();
    InputManager._playerInput.SwitchCurrentActionMap("UI");
  }

  public void Unpause()
  {
    isPaused = false;
    Time.timeScale = 1f;
    CloseAllMenus();
    InputManager._playerInput.SwitchCurrentActionMap("Player");
  }
  #endregion


  #region Canvas Activations
  private void OpenPauseMenu()
  {
    _pauseMenuCanvasGO.SetActive(true);
    _settingsMenuCanvasGO.SetActive(false);
    _audioSettingsMenuCanvasGO.SetActive(false);
    _keyboardSettingsMenuCanvasGO.SetActive(false);

    EventSystem.current.SetSelectedGameObject(_pauseMenuFirst);
  }


  private void OpenSettingsMenuHandle()
  {
    _pauseMenuCanvasGO.SetActive(false);
    _settingsMenuCanvasGO.SetActive(true);
    _audioSettingsMenuCanvasGO.SetActive(false);
    _keyboardSettingsMenuCanvasGO.SetActive(false);

    EventSystem.current.SetSelectedGameObject(_settingsMenuFirst);
  }

  private void OpenAudioSettingsMenuHandle()
  {
    _pauseMenuCanvasGO.SetActive(false);
    _settingsMenuCanvasGO.SetActive(false);
    _audioSettingsMenuCanvasGO.SetActive(true);
    _keyboardSettingsMenuCanvasGO.SetActive(false);

    EventSystem.current.SetSelectedGameObject(_audioSettingsMenuFirst);
  }

  private void OpenKeyboardSettingsMenuHandle()
  {
    _pauseMenuCanvasGO.SetActive(false);
    _settingsMenuCanvasGO.SetActive(false);
    _audioSettingsMenuCanvasGO.SetActive(false);
    _keyboardSettingsMenuCanvasGO.SetActive(true);

    EventSystem.current.SetSelectedGameObject(_keyboardSettingsMenuFirst);
  }
  private void CloseAllMenus()
  {
    _pauseMenuCanvasGO.SetActive(false);
    _settingsMenuCanvasGO.SetActive(false);
    _audioSettingsMenuCanvasGO.SetActive(false);
    _keyboardSettingsMenuCanvasGO.SetActive(false);

    EventSystem.current.SetSelectedGameObject(null);
  }
  #endregion


  #region Pause Menu Button Actions
  public void OnSettingsPress()
  {
    OpenSettingsMenuHandle();
  }

  public void OnAudioSettingsPress()
  {
    OpenAudioSettingsMenuHandle();
  }

  public void OnKeyboardSettingsPress()
  {
    OpenKeyboardSettingsMenuHandle();
  }


  public void OnResumePress()
  {
    Unpause();
  }

  public void OnQuitButton()
  {
    Application.Quit();
  }

      public void OnMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
  #endregion


  #region Settings Menu Button Actions
  public void OnSettingsBackPress()
  {
    OpenPauseMenu();
  }
  #endregion


}