using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    //singleton ponownie 
    //Tutaj jest to samo co w GameManager 

    private static AudioManager _instance;
    public static AudioManager _Instance
    {
        get 
        {
            if(_instance == null)
            {
                Debug.LogError("Audio Manager is null");
            }
            return _instance;
        }

    }

    public AudioSource voiceOver;//zmienna do przypisania voiceOveru w unity aby mógł pracować z doborem odpowiednich klipoów
    public AudioSource music;//zmienna tego samego typu ale do muzyczki
    private void Awake()
    {
        _instance = this;
    }

    public void PlayVoiceOver(AudioClip clipToPlay)//funckja do przekazania do VoiceOVerTrigger
    {
        voiceOver.clip = clipToPlay;//przypisanie clipów do grania do zmiennej
        voiceOver.Play();//odtwrozenie klipów
    }

    public void PlayMusic() { music.Play(); }//funckja do zaczęcia grania muzyczki

}
