using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //States of the Enemy
    enum States
    {
        patrol,
        chase,
        search,
        attack,
        retreat
    }
    //Vairables
    //Target and the points for the patrol (includes reference to player)
    private Transform Target;
    public Transform player;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;
    private Vector3 LastKnownPOS = Vector3.zero;
    //Distance variables for enemy
    private float chaseDist = 10f;
    private float attackDist = 1.5f;
    private float distanceToPoint;
    //State tracker for the enemy
    private States currentState; 
    public NavMeshAgent agent;
    float MaxTime = 20f;
    float SearchTime;
    //Color for the Enemy
    Renderer enemyColor;

    void Start()
    {
        //Set the current Patrol point
        currentPatrolPoint = 0;
        Target = patrolPoints[currentPatrolPoint];
        currentState = States.patrol;
        Vector3 distance = gameObject.transform.position - Target.transform.position;
        //Get a Renderer for the model
        enemyColor = GetComponent<Renderer>();
    }
    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case States.patrol:
                Patrol();
                break;
            case States.chase:
                Chase();
                break;
            case States.attack:
                Attack();
                break;
            case States.search:
                Search();
                break;
        }
    }
    //Enemy Patrol State
    public void Patrol()
    {
        //Patrol state = blue
        enemyColor.material.color = Color.blue;
        //Agent going to the Target/patrol point
        agent.SetDestination(Target.position);
        distanceToPoint = Vector3.Distance(transform.position, Target.position);
        if(distanceToPoint <= 1f)
        {
            Debug.Log("Going to: "+ Target);
            Debug.Log("At"+currentPatrolPoint);
            Debug.Log("Length: " + patrolPoints.Length);
            currentPatrolPoint++;
            if(currentPatrolPoint == patrolPoints.Length)
            {
                currentPatrolPoint = 0;
            }
            Target = patrolPoints[currentPatrolPoint];
        }
        else if(Vector3.Distance(transform.position,player.position) <= chaseDist)
        {
            currentState = States.chase;
        }
        
    }
    //Enemy Chase State
    public void Chase()
    {
        enemyColor.material.color = Color.red;
        agent.SetDestination(player.position);
        if(Vector3.Distance(transform.position, player.position) > chaseDist )
        {
            currentState = States.search;
        }
    }
    //Enemy Attack State
    public void Attack()
    {
        Debug.Log("Pew pew");
        enemyColor.material.color = Color.black;
    }
    //Enemy Seach State
    public void Search()
    {
        enemyColor.material.color = Color.yellow;
        MaxTime = 20f;
        if(Vector3.Distance(transform.position,player.position) <= chaseDist)
        {
            Debug.Log("Max Time: "+ MaxTime + "Time: " + Time.time + "Search Time: " + SearchTime);
            
            LastKnownPOS = player.position;
            agent.SetDestination(LastKnownPOS);
            currentState = States.chase;
        }
        else if( Time.time - SearchTime >= MaxTime)
        {
            Debug.Log("22Max Time: "+ MaxTime + "Time: " + Time.time + "Search Time: " + SearchTime);
            currentState = States.patrol;
        }
    }
    //Enemy Retreat state
    public void Retreat()
    {
        Debug.Log("Run away!");
        enemyColor.material.color = Color.green;
    }
}
