using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyAI : MonoBehaviour
{
    //States of the Enemy
    public enum States
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
    private Vector3 LastKnownPOS;
    //Distance variables for enemy
    private float chaseDist = 10f;
    private float attackDist = 2.5f;
    private float distanceToPoint;
    //State tracker for the enemy
    private States currentState; 
    private float searchTime = 8f;
    private float retreatTime = 8f;
    public NavMeshAgent agent;
    float MaxTime;
    //Color for the Enemy
    Renderer enemyColor;
    //Enemy Bools
    bool isSearching;
    void Start()
    {
        //Set the current Patrol point
        currentPatrolPoint = 0;
        Target = patrolPoints[currentPatrolPoint];
        currentState = States.patrol;
        Vector3 distance = gameObject.transform.position - Target.transform.position;
        //Get a Renderer for the model
        enemyColor = GetComponent<Renderer>();
        //Distance checks
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
            case States.retreat:
                Retreat();
                break;
        }
    }
    public States GetState()
    {
        return currentState;
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
            currentPatrolPoint++;
            if(currentPatrolPoint == patrolPoints.Length)
            {
                currentPatrolPoint = 0;
            }
            Target = patrolPoints[currentPatrolPoint];
        }
        if(Vector3.Distance(transform.position, player.position) <= chaseDist)
        {
            currentState = States.chase;
        }
        
    }
    //Enemy Chase State
    public void Chase()
    {
        enemyColor.material.color = Color.red;
        agent.SetDestination(player.position);
        if(Vector3.Distance(transform.position, player.position) > chaseDist)
        {
            LastKnownPOS = player.position;
            currentState = States.search;
        }
        else if(Vector3.Distance(transform.position, player.position) <= attackDist)
        {
            currentState = States.attack;
        }
    }
    //Enemy Attack State
    public void Attack()
    {
            enemyColor.material.color = Color.black;
            agent.SetDestination(transform.position);
            if(Vector3.Distance(transform.position, player.position) > attackDist)
            {
                if(Vector3.Distance(transform.position, player.position) < chaseDist)
                {
                    currentState = States.chase;
                }
            }
    }
    //Enemy Seach State
    public void Search()
    {
        enemyColor.material.color = Color.yellow;
        if(Vector3.Distance(transform.position, LastKnownPOS) <= 1f)
        {
            agent.SetDestination(transform.position);
            searchTime -= Time.deltaTime;
             if (searchTime <= 0)
            {
                currentState = States.retreat;
                searchTime = 8f;
            }
        }
        if(Vector3.Distance(transform.position, player.position) <= chaseDist)
        {
            currentState = States.chase;
        }
        else
        {
            agent.SetDestination(LastKnownPOS);
        }
    }
    //Enemy Retreat state
    public void Retreat()
    {
        enemyColor.material.color = Color.green;
        agent.SetDestination(Target.position);
        distanceToPoint = Vector3.Distance(transform.position, Target.position);
        if(distanceToPoint <= 1f)
        {
           agent.SetDestination(transform.position);
           retreatTime -= Time.deltaTime;
           if(retreatTime <= 0 )
           {
                currentState = States.patrol;
                retreatTime = 10f;
           }
        }
        if(Vector3.Distance(transform.position, player.position) < chaseDist)
        {
            currentState = States.chase;
        }
    }
}