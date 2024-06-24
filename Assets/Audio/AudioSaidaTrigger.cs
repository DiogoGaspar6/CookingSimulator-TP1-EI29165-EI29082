using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSaidaTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "npc")
        {
            GetComponent<AudioSource>().Play();
        }
    }
}