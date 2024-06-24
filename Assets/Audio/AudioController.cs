using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    private bool AudioState = true; 

    public void Volume(float value)
    {
        audioSource.volume = value;
    }
}
