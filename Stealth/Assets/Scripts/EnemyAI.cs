using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float chasingSpeed = 6f;
    public float patrollingSpeed = 2f;
    public float waitingInterval = 3f;
    public Transform[] wayPoints;

    private PlayerHealth playerHealth;
    private EnemySightingAndHearing enemySighting;
    private AlarmSystem alarmSystem;
    private NavMeshAgent navMeshAgent;

    private float timer;
    private int wayPointIndex;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemySighting = GetComponent<EnemySightingAndHearing>();
        playerHealth = GameObject.FindWithTag(GameConsts.TAG_PLAYER).GetComponent<PlayerHealth>();
        alarmSystem = GameObject.FindWithTag(GameConsts.TAG_GAMECONTROLLER).GetComponent<AlarmSystem>();
    }
    private void Update()
    {
        if(playerHealth.hp <= 0)
        {
            Patrolling();
            return;
        }
        if (enemySighting.isPlayerInsight)
        {
            Shooting();
        }
        else if(enemySighting.personalAlarmPos != alarmSystem.safePos)
        {
            Chasing();
        }
        else
        {
            Patrolling();
        }
    }
    private void Shooting()
    {
        navMeshAgent.isStopped = true;
    }
    private void Chasing()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = chasingSpeed;
        navMeshAgent.SetDestination(enemySighting.personalAlarmPos);
        if(navMeshAgent.remainingDistance - navMeshAgent.stoppingDistance < 0.05f)
        {
            timer += Time.deltaTime;
            if(timer > waitingInterval)
            {
                alarmSystem.alarmPos = alarmSystem.safePos;
                enemySighting.personalAlarmPos = alarmSystem.safePos;
                timer = 0;
            }
        }
        else
        {
            timer = 0;
        }
    }
    private void Patrolling()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = patrollingSpeed;
        navMeshAgent.SetDestination(wayPoints[wayPointIndex].position);
        if(navMeshAgent.remainingDistance - navMeshAgent.stoppingDistance < 0.05f)
        {
            timer += Time.deltaTime ;
            if(timer > waitingInterval)
            {
                wayPointIndex = ++wayPointIndex%wayPoints.Length;
                timer = 0;
            }
        }
        else
        {
            timer = 0;
        }
    }
}
