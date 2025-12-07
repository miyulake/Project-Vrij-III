using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class Options : MonoBehaviour
{
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private Toggle m_FullscreenToggle;
    [SerializeField] private Slider m_MasterVolume;
    [SerializeField] private TMP_Dropdown m_ResolutionDropdown;
    private Resolution[] m_Resolutions;
    private static List<Resolution> m_FilteredResolutions;

    private void Start()
    {
        GetResolutions();
        GetOptionValues();
    }

    public void ExitGame() => Application.Quit();

    public void SetFullscreen(bool isFullscreen) => Screen.fullScreen = isFullscreen;

    public void SetMasterVolume(float masterVolume) => m_AudioMixer.SetFloat("MasterVolume", masterVolume);

    public void SetResolution(int resolutionIndex)
    {
        var resolution = m_FilteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void GetResolutions()
    {
        m_Resolutions = Screen.resolutions;
        m_FilteredResolutions = new List<Resolution>();

        if (m_ResolutionDropdown != null) m_ResolutionDropdown.ClearOptions();

        var currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;
        for (int i = 0; i < m_Resolutions.Length; i++)
        {
            if (m_Resolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                m_FilteredResolutions.Add(m_Resolutions[i]);
            }
        }

        var currentResolutionIndex = 0;
        var options = new List<string>();
        for (int i = 0; i < m_FilteredResolutions.Count; i++)
        {
            string resolutionOption = m_FilteredResolutions[i].width + "x" + m_FilteredResolutions[i].height;
            options.Add(resolutionOption);
            if (m_FilteredResolutions[i].width == Screen.currentResolution.width &&
                m_FilteredResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        m_ResolutionDropdown.AddOptions(options);
        //resolutionDropdown.value = currentResolutionIndex;
        // Does not trigger OnValueChanged on the dropdown
        m_ResolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
        m_ResolutionDropdown.RefreshShownValue();
    }

    private void GetOptionValues()
    {
        m_FullscreenToggle.isOn = Screen.fullScreen;
        m_MasterVolume.value = GetAudioMixerLevel("master");
    }

    private float GetAudioMixerLevel(string mixerName)
    {
        var value = 0f;
        var result = false;

        if (mixerName == "master") result = m_AudioMixer.GetFloat("MasterVolume", out value);

        if (result) return value;
        else return 0f;
    }
}
