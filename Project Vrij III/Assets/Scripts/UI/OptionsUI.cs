using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using TMPro;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private FullScreenPassRendererFeature m_CRT;

    [Header("Audio")]
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private Slider m_MasterVolume, m_MusicVolume, m_SoundVolume;

    [Header("Game")]
    [SerializeField] private Slider m_RoundDuration;
    [SerializeField] private TextMeshProUGUI m_DurationValue;
    [SerializeField] private Slider m_WinsNeeded;
    [SerializeField] private TextMeshProUGUI m_WinsValue;

    [Header("Screen")]
    [SerializeField] private Toggle m_FullscreenToggle;
    [SerializeField] private Toggle m_CRTToggle;
    [SerializeField] private TMP_Dropdown m_ResolutionDropdown;
    [SerializeField] private TMP_Dropdown m_ModeDropdown;
    private Resolution[] m_Resolutions;
    private static List<Resolution> m_FilteredResolutions;

    private void Start()
    {
        GetResolutions();
        GetGameModes();
        GetOptionValues();
    }

    public void ExitGame() => Application.Quit();

    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void StartMatch() => RoundManager.Instance.StartMatch();

    public void SetMasterVolume(float masterVolume) => m_AudioMixer.SetFloat("MasterVolume", masterVolume);
    public void SetMusicVolume(float musicVolume) => m_AudioMixer.SetFloat("MusicVolume", musicVolume);
    public void SetSoundVolume(float soundVolume) => m_AudioMixer.SetFloat("SoundVolume", soundVolume);

    public void SetRoundDuration(float duration)
    {
        RoundManager.Instance.SetRoundDuration(Mathf.RoundToInt(duration));
        if (duration > 99) m_DurationValue.text = "∞";
        else m_DurationValue.text = $"{Mathf.RoundToInt(duration)}";
    }

    public void SetWinsNeeded(float wins)
    {
        RoundManager.Instance.SetWinsNeeded(Mathf.RoundToInt(wins));
        m_WinsValue.text = $"{Mathf.RoundToInt(wins)}";
        RoundTracker.Instance.InitializeRoundWinUI();
    }

    public void SetFullscreen(bool isFullscreen) => Screen.fullScreen = isFullscreen;

    public void SetCRT(bool isEnabled) => m_CRT.SetActive(isEnabled);

    public void SetRandomMusic() => MusicManager.Instance.SetRandomMusic();

    public void SetResolution(int resolutionIndex)
    {
        var resolution = m_FilteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void GetResolutions()
    {
        m_Resolutions = Screen.resolutions;
        m_FilteredResolutions = new List<Resolution>();

        m_ResolutionDropdown.ClearOptions();

        var currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;
        for (int i = 0; i < m_Resolutions.Length; i++)
        {
            if (m_Resolutions[i].refreshRateRatio.value == currentRefreshRate)
                m_FilteredResolutions.Add(m_Resolutions[i]);
        }

        var currentResolutionIndex = 0;
        var options = new List<string>();
        for (int i = 0; i < m_FilteredResolutions.Count; i++)
        {
            var resolutionOption = m_FilteredResolutions[i].width + "x" + m_FilteredResolutions[i].height;
            options.Add(resolutionOption);
            if (m_FilteredResolutions[i].width == Screen.currentResolution.width &&
                m_FilteredResolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        m_ResolutionDropdown.AddOptions(options);
        //resolutionDropdown.value = currentResolutionIndex;
        // Does not trigger OnValueChanged on the dropdown
        m_ResolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
        m_ResolutionDropdown.RefreshShownValue();
    }

    public void SetGameMode(int modeIndex) => GameManager.Instance.SetGameMode(modeIndex);

    private void GetGameModes()
    {
        m_ModeDropdown.ClearOptions();

        var modes = Enum.GetValues(typeof(GameMode)).Cast<GameMode>().ToList();
        var options = modes.Select(m => m.ToString()).ToList();

        m_ModeDropdown.AddOptions(options);

        var currentModeIndex = modes.IndexOf(GameManager.Instance.CurrentMode);
        m_ModeDropdown.SetValueWithoutNotify(currentModeIndex);

        m_ModeDropdown.RefreshShownValue();
    }

    private void GetOptionValues()
    {
        m_MasterVolume.value = GetAudioMixerLevel("master");
        m_MusicVolume.value = GetAudioMixerLevel("music");
        m_SoundVolume.value = GetAudioMixerLevel("sound");
        m_RoundDuration.value = RoundManager.Instance.GetRoundDuration();
        m_DurationValue.text = $"{RoundManager.Instance.GetRoundDuration()}";
        m_WinsNeeded.value = RoundManager.Instance.GetWinsNeeded();
        m_WinsValue.text = $"{RoundManager.Instance.GetWinsNeeded()}";
        m_CRTToggle.isOn = m_CRT.isActive;
        m_FullscreenToggle.isOn = Screen.fullScreen;
    }

    private float GetAudioMixerLevel(string mixerName)
    {
        var value = 0f;
        var result = false;

        if (mixerName == "master") result = m_AudioMixer.GetFloat("MasterVolume", out value);
        if (mixerName == "music") result = m_AudioMixer.GetFloat("MusicVolume", out value);
        if (mixerName == "sound") result = m_AudioMixer.GetFloat("SoundVolume", out value);

        if (result) return value;
        else return 0f;
    }
}
