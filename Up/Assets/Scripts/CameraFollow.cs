using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //要跟随的物体
    public Transform target;
    //跟随的速度
    private float moveSpeed = 5f;
    //跟随的边界
    public Vector2 minPosition;
    public Vector2 maxPosition;

    private void Update()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position;
            targetPos.y = Mathf.Clamp(targetPos.y, minPosition.y, maxPosition.y);//让目标位置限制在最大最小范围内
            targetPos.x = 0f;
            targetPos.z = -10f;
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed);//缓慢移动到目标位置
        }
    }

}
