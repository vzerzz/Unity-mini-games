using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    private Transform transform;
    //�����ٶ�
    private float speed = 10f;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    private void Update()
    {
        //�����������
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
