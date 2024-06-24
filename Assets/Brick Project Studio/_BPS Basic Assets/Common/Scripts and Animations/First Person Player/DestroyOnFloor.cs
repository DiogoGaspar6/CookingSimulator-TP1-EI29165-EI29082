using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnFloor : MonoBehaviour
{
    public LayerMask floor;
    public AudioClip audioClip;

    void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & floor) != 0)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null && audioClip != null)
            {
                audioSource.Play();
                Destroy(gameObject, audioClip.length);
            }
        }
    }
}
