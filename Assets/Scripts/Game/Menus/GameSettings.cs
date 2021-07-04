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
    private bool mute;
    private float volumeLevel, musicLevel;
    private const string soundVolume="SE volume"; 
    private const string musicVolume="MU volume";
    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        mute = true;
        if(PlayerPrefs.HasKey(soundVolume)){
            audioMixer.SetFloat(soundVolume, volumeLevel);
            soundSlider.value = volumeLevel = PlayerPrefs.GetFloat(soundVolume);

        }else audioMixer.SetFloat(soundVolume, Mathf.Log10(initialVolumeLevel) * 20);

        if(PlayerPrefs.HasKey(musicVolume)){
            musicSlider.value = musicLevel=PlayerPrefs.GetFloat(musicVolume);
            musicMixer.SetFloat(musicVolume,musicLevel);
            
        }else musicMixer.SetFloat(musicVolume, Mathf.Log10(initialVolumeLevel) * 20);
    }
    #region Sounds volume
   
    public void SetEffectsVolume(float volume)
    {
        mute = false;
        float newVol=Mathf.Log10(volume)*20;
        audioMixer.SetFloat(soundVolume, volumeLevel=newVol);
        if(!mute)audioS.ClipAndPlay(sampleClip);
        mute = true;
    }
    public void SetEffectsVolume(bool mute){
        if(mute)audioMixer.SetFloat(soundVolume,0);
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
        mute = false;
        float newVol = Mathf.Log10(volume)*20;
        musicMixer.SetFloat(musicVolume, musicLevel = newVol);
        if(!mute)audioS.ClipAndPlay(sampleClip);
        mute=true;
    } 
    #endregion
   
    public void SetFullScreen(bool value)
    {
        Screen.fullScreen = value;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
