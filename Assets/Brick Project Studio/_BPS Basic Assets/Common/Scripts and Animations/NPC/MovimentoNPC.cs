using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MovimentoNPC : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform point;
    public NPCManager npcManager;
    public Transform exit;
    public Animator animator;

    public GameObject triggerSaida;
    private bool isStopped = false;
    private float distance = 0.5f;
    private bool isExiting = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.SetDestination(point.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopped && !agent.pathPending)
        {
            if (agent.remainingDistance <= distance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    isStopped = true;
                    agent.isStopped = true;
                    animator.SetBool("isStopped", isStopped);
                }
            }
        }
        animator.SetBool("isStopped", isStopped);
    }

    public void Exit()
    {
        isStopped = false;
        isExiting = true;
        agent.isStopped = false;
        agent.SetDestination(exit.position);
        animator.SetBool("isStopped", false);
    }

    public void SetNewTarget(Transform newTarget)
    {
        point = newTarget;
        isStopped = false;
        agent.isStopped = false;
        if (point != null)
        {
            agent.SetDestination(point.position);
        }
        animator.SetBool("isStopped", false);
    }
}



