using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("挡位数量")]
    public int gear = 5;
    public float moveSpeed = 1f;
    public float turnSpeed = 1f;
    private Transform followTarget;
    private Vector3 dir;
    private RaycastHit hit;
    private void Awake()
    {
        followTarget = GameObject.FindWithTag(GameConsts.TAG_PLAYER).transform;
        
    }
    private void Start()
    {
        dir = followTarget.position - transform.position;  
        
    }
    private void Update()
    {
        Vector3 bestPos = followTarget.position - dir;
        Vector3 worstPos = followTarget.position + Vector3.up * (dir.magnitude + GameConsts.OVERLOOKHEIGHT);
        Vector3[] watchPoints = new Vector3[gear];
        watchPoints[0] = bestPos;
        watchPoints[watchPoints.Length - 1] = worstPos;
        for(int i = 1; i < watchPoints.Length-1; i++)
        {
            watchPoints[i] = Vector3.Lerp(bestPos, worstPos,(float)i / (gear - 1));

        }
        Vector3 fitPos = bestPos;
        for(int i = 0; i < watchPoints.Length; i++)
        {
            if (CanSeeTarget(watchPoints[i]))
            {
                fitPos = watchPoints[i];
                break;
            }
        }
        transform.position = Vector3.Lerp(transform.position, fitPos, Time.deltaTime * moveSpeed);
        Vector3 lookDir = followTarget.position - transform.position;
        Quaternion targetQua = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetQua, Time.deltaTime * turnSpeed);
        //欧拉角修正
        Vector3 eularAngles = transform.eulerAngles;
        eularAngles.y = 0;
        eularAngles.z = 0;
        transform.eulerAngles = eularAngles;
    }
    private bool CanSeeTarget(Vector3 pos)
    {
        Vector3 currentDir = followTarget.position - pos;
        if(Physics.Raycast(pos, currentDir,out hit))
        {
            if (hit.collider.CompareTag(GameConsts.TAG_PLAYER))
            {
                return true;
            }
        }
        return false;
    }
}//还可对每个followtarget + Vector3.up*PLAYER_BODY_OFFST 之前看的都是脚底
