using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Ҫ���������
    public Transform target;
    //������ٶ�
    private float moveSpeed = 5f;
    //����ı߽�
    public Vector2 minPosition;
    public Vector2 maxPosition;

    private void Update()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position;
            targetPos.y = Mathf.Clamp(targetPos.y, minPosition.y, maxPosition.y);//��Ŀ��λ�������������С��Χ��
            targetPos.x = 0f;
            targetPos.z = -10f;
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed);//�����ƶ���Ŀ��λ��
        }
    }

}
