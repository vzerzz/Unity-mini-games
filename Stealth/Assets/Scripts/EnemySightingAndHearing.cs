using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySightingAndHearing : MonoBehaviour
{
    //警报相关
    public bool isPlayerInsight;
    public Vector3 personalAlarmPos;
    private Vector3 previousAlarmPos;
    private AlarmSystem alarmSystem;
    //定义的变量
    [Range(90f,180f)]
    public float viewField = 130;
    private Vector3 dir;
    //组件
    private Transform player;
    private Animator playerAni;
    private RaycastHit hit;
    private NavMeshAgent navMeshAgent;
    private NavMeshPath path;
    private SphereCollider sphereCollider;
    private void Awake()
    {
        player = GameObject.FindWithTag(GameConsts.TAG_PLAYER).transform;
        playerAni = GameObject.FindWithTag(GameConsts.TAG_PLAYER).GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        sphereCollider = GetComponent<SphereCollider>();
        alarmSystem = GameObject.FindWithTag(GameConsts.TAG_GAMECONTROLLER).GetComponent<AlarmSystem>();
        personalAlarmPos = alarmSystem.safePos;
        previousAlarmPos = alarmSystem.safePos;
    }
    private void Update()
    {
        //检查是否变化
        if(previousAlarmPos!= alarmSystem.alarmPos)
        {
            personalAlarmPos = alarmSystem.alarmPos;
        }
        previousAlarmPos = alarmSystem.alarmPos;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(GameConsts.TAG_PLAYER))
            return;
        VisualInspection();
        HearingTest();
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(GameConsts.TAG_PLAYER))
            return;
        isPlayerInsight = false;
    }
    /// <summary>
    /// Visual
    /// </summary>
    private void VisualInspection()
    {
        isPlayerInsight = false ;
        dir = player.position - transform.position;
        float angle = Vector3.Angle(dir, transform.forward);
        if (angle > viewField / 2)
            return;
        Vector3 eyesPos = transform.position + Vector3.up * GameConsts.ENEMY_EYES_HEIGHT;
        if (!Physics.Raycast(eyesPos, dir, out hit))
            return;
        if(!hit.collider.CompareTag(GameConsts.TAG_PLAYER))
            return ;
        alarmSystem.alarmPos = player.position;
        isPlayerInsight=true;
    }
    /// <summary>
    /// Hearing
    /// </summary>
    private void HearingTest()
    {
        bool isLocomotion = playerAni.GetCurrentAnimatorStateInfo(0).shortNameHash == GameConsts.STATE_LOCOMOTION;
        bool isShout = playerAni.GetCurrentAnimatorStateInfo(1).shortNameHash == GameConsts.PARAM_SHOUT;
        if(!isLocomotion && !isShout)
            return ;
        bool canArrive = navMeshAgent.CalculatePath(player.position, path);
        if(!canArrive)
            return ;
        Vector3[] points = new Vector3[path.corners.Length+2];
        points[0] = transform.position;
        points[points.Length-1] = player.position;
        for(int i = 1; i < points.Length - 1; i++)
        {
            points[i] = path.corners[i-1];
        }
        float distance = 0;
        for(int i = 0; i < points.Length-1; i++)
        {
            distance += Vector3.Distance(points[i],points[i+1]);
        }
        if(distance > sphereCollider.radius)
            return ;
        personalAlarmPos = player.position;
    }

}
