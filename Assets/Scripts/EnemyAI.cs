using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

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
    private float attackDist = 2.5f;
    private int searchTime = 30000;
    private float distanceToPoint;
    //State tracker for the enemy
    private States currentState; 
    public NavMeshAgent agent;
    float MaxTime;
    //Color for the Enemy
    Renderer enemyColor;
    //Enemy Attack signal
    public TextMeshProUGUI AttackString;

    void Start()
    {
        //Set the current Patrol point
        currentPatrolPoint = 0;
        Target = patrolPoints[currentPatrolPoint];
        currentState = States.search;
        Vector3 distance = gameObject.transform.position - Target.transform.position;
        //Get a Renderer for the model
        enemyColor = GetComponent<Renderer>();
        //Distance checks
    }
    // Update is called once per frame
    void Update()
    {
        ChangeState();
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
    public void ChangeState()
    {
        if (Vector3.Distance(transform.position, player.position) <= chaseDist)
        {
            currentState = States.chase;
            if (Vector3.Distance(transform.position, player.position) > chaseDist)
            {
                currentState = States.search;
            }
        }
        if (Vector3.Distance(transform.position, player.position) <= attackDist)
        {
            currentState = States.attack;
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
            currentPatrolPoint++;
            if(currentPatrolPoint == patrolPoints.Length)
            {
                currentPatrolPoint = 0;
            }
            Target = patrolPoints[currentPatrolPoint];
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
            enemyColor.material.color = Color.black;
            agent.SetDestination(transform.position);
            if(Vector3.Distance(transform.position,player.position) > attackDist)
            {
                currentState = States.chase;
            }
    }
    //Enemy Seach State
    public void Search()
    {
        enemyColor.material.color = Color.yellow;
        LastKnownPOS = player.position;
        float searchTime = 5f;
        searchTime -= Time.deltaTime;
        agent.SetDestination(LastKnownPOS);
        if (searchTime <= 0)
        {
            currentState = States.search;
            searchTime = 5f;
        }
    }
    //Enemy Retreat state
    public void Retreat()
    {
        Debug.Log("Run away!");
        enemyColor.material.color = Color.green;
        agent.SetDestination(Target.position);
        distanceToPoint = Vector3.Distance(transform.position, Target.position);
        if(distanceToPoint <= 1f)
        {
           agent.SetDestination(transform.position);
        }
    }
    //IEnumerator timer( int value )
    //{
    //    Debug.Log("Ienumorartor called");
    //    yield return null;
    //    Debug.Log(value);
    //}
}