using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
    [Header("平滑过渡时间")]
    [Range(0.1f, 1)]
    public float dampTime = 0.2f;
    [Header("死角度数")]
    public float deadZone = 5f;

    private Animator ani;
    private NavMeshAgent navMeshAgent;
    private Transform player;
    private PlayerHealth playerHealth;
    private EnemySightingAndHearing enemySighting;
    //需要计算的两个动画参数
    private float speed, angularSpeed;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag(GameConsts.TAG_PLAYER).transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        enemySighting = GetComponent<EnemySightingAndHearing>();
    }
    private void Update()
    {
        if(playerHealth.hp > 0)
        {
            ani.SetBool(GameConsts.PARAM_ISPLAYERINSIGHT, enemySighting.isPlayerInsight);
        }
        else
        {
            ani.SetBool(GameConsts.PARAM_ISPLAYERINSIGHT, false);
        }
        Vector3 projection = Vector3.Project(navMeshAgent.desiredVelocity, transform.forward);
        speed = projection.magnitude;

        float angle = Vector3.Angle(transform.forward, navMeshAgent.desiredVelocity);
        Vector3 normal = Vector3.Cross(transform.forward, navMeshAgent.desiredVelocity).normalized;

        if(navMeshAgent.desiredVelocity == Vector3.zero)
        {
            angle = 0;
        }
        if(angle < deadZone && enemySighting.isPlayerInsight)
        {
            angle = 0;
            transform.LookAt(player);
        }
        if(normal.y < 0)
        {
            angle *= -1;
        }
        angle *= Mathf.Deg2Rad;
        angularSpeed = angle;

        ani.SetFloat(GameConsts.PARAM_SPEED, speed, dampTime, Time.deltaTime);
        ani.SetFloat(GameConsts.PARAM_ANGULARSPEED, angularSpeed, dampTime, Time.deltaTime);
    }
    private void OnAnimatorMove()
    {
        navMeshAgent.velocity = ani.deltaPosition / Time.deltaTime;
        transform.rotation = ani.rootRotation;
    }
}
