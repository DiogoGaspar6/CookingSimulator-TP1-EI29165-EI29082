using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TriggerSaida : MonoBehaviour
{
  public NPCManager npcManager;
  private float timeout = 1.0f;
  void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("npc"))
    {
      StartCoroutine(TimeOut(other.gameObject));
    }
  }

  IEnumerator TimeOut(GameObject npc){
    yield return new WaitForSeconds(timeout);
    npcManager.NovaRota(npc);
  }
}