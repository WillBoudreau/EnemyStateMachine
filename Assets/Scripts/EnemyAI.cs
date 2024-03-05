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
    private float searchTime;
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
            case States.retreat:
                Retreat();
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
            if(Vector3.Distance(transform.position,player.position) > attackDist)
            {
                StartCoroutine(timer());
                currentState = States.chase;
            }
    }
    //Enemy Seach State
    public void Search()
    {
        enemyColor.material.color = Color.yellow;
        MaxTime = 10f;
        agent.SetDestination(LastKnownPOS);
        searchTime = 5;
        Debug.Log(Time.time - searchTime);
        if(Time.time - searchTime  >= MaxTime)
        {
            Debug.Log("Time1 "+searchTime);
            Debug.Log("Time2 "+Time.time);
            searchTime += 5;
            currentState = States.patrol;
        }
        else if(Vector3.Distance(transform.position,player.position) <= chaseDist)
        {
            currentState = States.chase;
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
        else if(Vector3.Distance(transform.position,player.position) <= chaseDist)
        {
            currentState = States.chase;
        }
    }
    IEnumerator timer()
    {
        yield return new WaitForSeconds(MaxTime);
    }
}
