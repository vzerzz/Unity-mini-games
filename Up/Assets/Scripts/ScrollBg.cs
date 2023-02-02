using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBg : MonoBehaviour
{
    public int speed = 4;
    //两背景位置
    private Vector3 flag1, flag2;
    private GameObject bg1;
    private GameObject bg2;
    private GameObject swap;

    private void Awake()
    {
        //获取两个背景
        bg1 = transform.GetChild(0).gameObject;
        bg2 = transform.GetChild(1).gameObject;
    }
    void Start()
    {
        flag1 = bg1.transform.position;
        flag2 = bg2.transform.position;
    }
    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
        //当后一个背景的X超过前一个背景开始存储的X。 则将前一个背景位置设为后一个背景开始的位置。然后交换，前变后，后变前。进入下一轮循环。
        if (bg2.transform.position.x < flag1.x)
        {
            bg1.transform.position = flag2;
            swap = bg1;
            bg1 = bg2;
            bg2 = swap;
        }
    }
}
