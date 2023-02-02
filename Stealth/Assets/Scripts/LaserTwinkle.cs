using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTwinkle : MonoBehaviour
{
    [Header("��˸���ʱ��")]
    public float interval = 2f;
    private float timer = 0;
    Vector3 originPos;
    private bool isShow;
    private void Start()
    {
        originPos = transform.position;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > interval)
        {
            isShow = !isShow;
            timer = 0;
            
        }
        if (isShow)
        {
            transform.position = originPos;
        }
        else
        {
            transform.position = Vector3.up * 1000;
        }
    }

    //����mesh aud collid light���enableΪfalse
}
