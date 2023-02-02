using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    private Transform transform;
    //飞行速度
    private float speed = 10f;

    private void Awake()
    {
        transform = GetComponent<Transform>();
    }

    private void Update()
    {
        //匀速向左飞行
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
