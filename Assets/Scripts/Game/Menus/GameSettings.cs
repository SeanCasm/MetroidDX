using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class GameSettings : MonoBehaviour
{
    [SerializeField]AudioMixer audioMixer,musicMixer,onPause;
    [SerializeField] float initialVolumeLevel;
    [SerializeField] Slider soundSlider, musicSlider;
    [SerializeField] AudioClip sampleClip;
    private AudioSource audioS;
    private bool setMute;
    private float volumeLevel, musicLevel;
    private const string soundVolume="SE volume"; 
    private const string musicVolume="MU volume";
    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        setMute = true;
        if (SaveSystem.LoadSettings()!=null){
            GlobalGameData data = SaveSystem.LoadSettings();
            soundSlider.value = volumeLevel = data.fXVolume;
            musicSlider.value = musicLevel = data.musicVolume;
            SetEffectsVolume();
            SetMusicVolume();
        }
        else
        {
            audioMixer.SetFloat(soundVolume, initialVolumeLevel);
            onPause.SetFloat(soundVolume,initialVolumeLevel);
            musicMixer.SetFloat(musicVolume, initialVolumeLevel);
        }
    }
    #region Sounds volume
    private void SetEffectsVolume(){
        onPause.SetFloat(soundVolume, volumeLevel);
        audioMixer.SetFloat(soundVolume, volumeLevel);
    }
    public void SetEffectsVolume(float volume)
    {
        if (CheckVolumeLevel(volume)){
            onPause.SetFloat(soundVolume,volumeLevel=volume);
            audioMixer.SetFloat(soundVolume, volumeLevel=volume);
            if(!setMute)audioS.ClipAndPlay(sampleClip);
        }
        SaveSystem.SaveSettings(volumeLevel,musicLevel);
    }
    public void SetEffectsVolume(bool mute){
        if(mute)audioMixer.SetFloat(soundVolume, -80);
        else audioMixer.SetFloat(soundVolume, volumeLevel);
    }
    #endregion
    #region Music volume
    public void SetMusicVolume(bool decrease){
        if(decrease)musicMixer.SetFloat(musicVolume,musicLevel/3);
        else musicMixer.SetFloat(musicVolume,musicLevel);
    }
    public void SetMusicVolume(float volume)
    {
        if (CheckVolumeLevel(volume)){
            musicMixer.SetFloat(musicVolume, musicLevel = volume);
            if(!setMute)audioS.ClipAndPlay(sampleClip);
        }
        setMute=false;
        SaveSystem.SaveSettings(volumeLevel, musicLevel);
    }
    private void SetMusicVolume()
    {
        musicMixer.SetFloat(musicVolume, musicLevel);
    }
    #endregion
    /// <summary>
    /// Checks the actual volume level.
    /// </summary>
    /// <param name="amount">total volume to change</param>
    /// <returns></returns>
    private static bool CheckVolumeLevel(float amount)
    {
        if(amount<=20 && amount >= -80)return true;
        else return false;
    }
    public void SetFullScreen(bool value)
    {
        Screen.fullScreen = value;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
