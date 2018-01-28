using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;
    public AudioClip[] SoundsList;
    public Dictionary<string, AudioClip> SoundsDict;
    public AudioSource efxSource;
    public AudioSource musicSource;   

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        SoundsDict = new Dictionary<string, AudioClip>();
        for (int i = 0; i < SoundsList.Length; i++) {
            SoundsDict.Add(SoundsList[i].name, SoundsList[i]);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void playSound(string soundKey) {
        efxSource.clip = SoundsDict[soundKey];
        efxSource.Play();
    }

    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void PlayRepeatedMusic(string musickey) {
        musicSource.clip = SoundsDict[musickey];
        musicSource.loop = true;
        musicSource.Play();
    }
}
