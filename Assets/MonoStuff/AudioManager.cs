using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager inst;
    public AudioSource musicSource;


    public void Awake()
    {
        inst = this;
    }

    public void PlayMusicSfx(string name)
    {
        var audio = Resources.Load<AudioClip>("SFX/" + name);
        AudioSource.PlayClipAtPoint(audio, Camera.main.transform.position);
    
    }

    public void PlayMusicRequests(string name)
    {
        var audio = Resources.Load<AudioClip>("Music/" + name);

        if (musicSource.clip != audio)
        {
            musicSource.clip = audio;
            musicSource.Play();
        
        }
    }
}
