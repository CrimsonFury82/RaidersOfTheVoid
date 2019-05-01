using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSFX : MonoBehaviour
{
    public AudioSource myAudio;
    public AudioClip hoverAudio;
    public AudioClip clickAudio;
    public AudioClip backAudio;
    float lifetime = 3.0f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void HoverSound()
    {
        myAudio.PlayOneShot(hoverAudio);
    }

    public void ClickSound()
    {
        myAudio.PlayOneShot(clickAudio);
    }

    public void BackSound()
    {
        myAudio.PlayOneShot(backAudio);
    }

    private void DestroyAudio()
    {
        Destroy(gameObject, lifetime);
    }
}