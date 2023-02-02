using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCreat : MonoBehaviour
{
    //闪电预制体
    public GameObject Light1;
    //闪电音效
    public AudioClip lightAud;
    //计时
    private float timer = 0;
    GameObject light;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 6f)//6秒产生一次
        {
            Creat();
            timer = 0;
        }
    }

    private void Creat()
    {
        float x = Random.Range(-10f, 5f);
        float y = Random.Range(10f, 12f);
        Vector3 pos = new Vector3(x, y, 0);//在一定范围内随机产生
        //预制体的实例化
        light = Instantiate(Light1, pos, Quaternion.Euler(0, 0, -90));
        //让闪电变成子对象共同移动
        light.transform.SetParent(transform, true);
        //播放音效
        AudioSource.PlayClipAtPoint(lightAud,new Vector3(0,0,-10));
    }

}
