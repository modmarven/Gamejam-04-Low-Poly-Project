using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] wayPoint;
    [SerializeField] private float minDistanceWaypoint = 1f;
    private int currentWaypoint = -1;
    private Animator animator;
    private float timer;

    private bool isWalking;

    void Start()
    {
        if (wayPoint.Length == 0)
        {
            Debug.LogError("No waypointt assign to the enemy");
            enabled = false;
            return;
        }

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetNextWaypoint();
    }

    
    void Update()
    {
        if (agent.remainingDistance <= minDistanceWaypoint && !agent.pathPending)
        {
            SetNextWaypoint();
        }
    }

    private void SetNextWaypoint()
    {
        timer += Time.deltaTime;
        if (timer > 10)
        {
            if (!isWalking)
            {
                animator.SetBool("isWalk", true);
                int randomIndex = Random.Range(0, wayPoint.Length);
                while (randomIndex == currentWaypoint)
                {
                    randomIndex = Random.Range(0, wayPoint.Length);
                }
                currentWaypoint = randomIndex;
                agent.SetDestination(wayPoint[currentWaypoint].position);
                isWalking = true;
                
            }
        }

        if (timer > 20)
        {
            isWalking = false;
            animator.SetBool("isWalk", false);
            timer = 0;
        }
       
    }
}
