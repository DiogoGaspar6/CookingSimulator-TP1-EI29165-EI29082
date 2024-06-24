using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPCManager : MonoBehaviour
{
    public GameObject npcPrefab;
    public Transform spawn;
    public Transform target;
    public Transform exit;


    void Start()
    {
    }

    public void NovaRota(GameObject npc)
    {
        NavMeshAgent agent = npc.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(spawn.position); // Teletransporta o NPC para a nova posição
            npc.GetComponent<MovimentoNPC>().SetNewTarget(target);
        }
        else
        {
            Debug.LogError("NavMeshAgent component is not found on the NPC!");
        }
    }
}

