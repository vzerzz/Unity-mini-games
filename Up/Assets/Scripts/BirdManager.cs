using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    //好鸟和坏鸟的预制体数组
    public GameObject[] goodBirds;
    public GameObject[] badBirds;
    //好鸟和坏鸟的产生时间计时器
    float goodTimer = 0;
    float badTimer = 0;
    //鸟的生成地点x坐标
    private float spawnPos = 30f;

    private void Update()
    {
        goodTimer += Time.deltaTime;
        badTimer += Time.deltaTime;
        //隔10秒产生好鸟
        if (goodTimer > 10f)
        {
            CreatGoodBird();
            goodTimer = 0;
        }
        //隔1秒产生坏鸟
        if (badTimer > 1f)
        {
            CreatBadBird();
            badTimer = 0;
        }
    }

    private void CreatGoodBird()
    {
        int goodIndex = (int)Random.Range(0f, goodBirds.Length-2);
        //产生地点的y坐标随机
        float y = Random.Range(-10f, 25f);
        //实例化预制体
        Instantiate(goodBirds[goodIndex], new Vector3(spawnPos, y, 0), Quaternion.Euler(0, 180, 0));
    }

    private void CreatBadBird()
    {
        int badIndex = (int)Random.Range(0f, badBirds.Length-2);
        //产生地点的y坐标随机
        float y = Random.Range(-10f, 25f);
        //实例化预制体
        Instantiate(badBirds[badIndex], new Vector3(spawnPos, y, 0), Quaternion.Euler(0, 180, 0));
    }


}
