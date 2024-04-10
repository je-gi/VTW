using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class KeyboardSettingsManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    private void OnEnable()
    {
        LoadKeybindings();
    }

    private void OnDisable()
    {
        SaveKeybindings();
    }

    private void LoadKeybindings()
    {
        var rebinds = PlayerPrefs.GetString("Keybindings");
        if (!string.IsNullOrEmpty(rebinds))
        {
            inputActions.LoadBindingOverridesFromJson(rebinds);
        }
    }

    private void SaveKeybindings()
    {
        var rebinds = inputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("Keybindings", rebinds);
        PlayerPrefs.Save();
    }

    public void ResetKeybindings()
    {
        PlayerPrefs.DeleteKey("Keybindings");
        PlayerPrefs.Save();
        LoadKeybindings();
    }
}
