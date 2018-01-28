using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;
    public AudioClip[] SoundsList;
    public Dictionary<string, AudioClip> SoundsDict;
    public AudioSource EfxSource;
    public AudioSource MusicSource;   

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
        EfxSource.clip = SoundsDict[soundKey];
        EfxSource.Play();
    }

    public void PlayRepeatedMusic(string musickey) {
        MusicSource.clip = SoundsDict[musickey];
        MusicSource.loop = true;
        MusicSource.Play();
    }
}
