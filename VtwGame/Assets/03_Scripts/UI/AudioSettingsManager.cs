using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public AudioMixer MainMixer;
    public Slider MusicVolumeSlider, SFXVolumeSlider, MasterVolumeSlider;

    private void Start()
    {
        MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        MasterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);

        MainMixer.SetFloat("MusicVolume", Mathf.Log10(MusicVolumeSlider.value) * 20);
        MainMixer.SetFloat("SFXVolume", Mathf.Log10(SFXVolumeSlider.value) * 20);
        MainMixer.SetFloat("MasterVolume", Mathf.Log10(MasterVolumeSlider.value) * 20);

        MusicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        SFXVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        MasterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    public void SetMusicVolume(float volume)
    {
        MainMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        MainMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetMasterVolume(float volume)
    {
        MainMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
}
