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
    //Target and the points for the patrol
    private Transform Target;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;

    private float chaseDist = 5f;
    private float attackDist = 1.5f;
    private float distanceToPoint;

    private States currentState; 
    public NavMeshAgent agent;

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
        
    }
    public void Chase()
    {
        enemyColor.material.color = Color.red;
    }
    public void Attack()
    {
        Debug.Log("Pew pew");
        enemyColor.material.color = Color.black;
    }
    public void Search()
    {
        Debug.Log("Where did you go?");
        enemyColor.material.color = Color.yellow;
    }
    public void Retreat()
    {
        Debug.Log("Run away!");
        enemyColor.material.color = Color.green;
    }
}
