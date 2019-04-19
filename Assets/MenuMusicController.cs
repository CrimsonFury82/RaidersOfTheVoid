using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicController : MonoBehaviour
{
    AudioSource audiosource;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        audiosource = GetComponent<AudioSource>();
    }

    public void StartMusic()
    {
        if (audiosource.isPlaying) return;
        audiosource.Play();
    }

    public void StopMusic()
    {
        audiosource.Stop();
    }
}
