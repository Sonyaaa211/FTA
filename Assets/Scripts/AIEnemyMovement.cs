using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemyMovement : MonoBehaviour
{
    
    public NavMeshAgent agent;
    public Transform target;
    public Transform[] moveZone;
    public Animator animator;
    public float speed = 5;
    public LayerMask targetLayer;
    private GameController gameController;

    //Patroling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkingPointRange;

    //Dodging
    public float sightRange;
    public bool targetInSightRange = false;
    public AIEnemyThrowGrenade AInade;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        gameController = GameController.Instance;
        AInade = gameObject.GetComponent<AIEnemyThrowGrenade>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.isGamePlaying) return;
        if (target != null)
        {
            targetInSightRange = Physics.CheckSphere(transform.position, sightRange, targetLayer);
        }

        if (!targetInSightRange)
        {
            Patroling();
        }
        if (AInade.isThrowingGrenade)
        {
            agent.speed = 0;
            
        }
        else
        {
            agent.speed = speed;
            animator.SetFloat("Speed_f", speed);
        }
    }

    public void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        agent.SetDestination(walkPoint);

        Vector3 distanceToWalk = transform.position - walkPoint;

        if(distanceToWalk.magnitude < 1f) { walkPointSet = false; }
    }

    public void SearchWalkPoint()
    {
        float x = UnityEngine.Random.Range(moveZone[0].position.x, moveZone[1].position.x);
        float z = UnityEngine.Random.Range(moveZone[0].position.z, moveZone[1].position.z);

        walkPoint = new Vector3(x, transform.position.y, z);

        walkPointSet = true;
    }
}
