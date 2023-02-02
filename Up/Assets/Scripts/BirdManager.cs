using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    //����ͻ����Ԥ��������
    public GameObject[] goodBirds;
    public GameObject[] badBirds;
    //����ͻ���Ĳ���ʱ���ʱ��
    float goodTimer = 0;
    float badTimer = 0;
    //������ɵص�x����
    private float spawnPos = 30f;

    private void Update()
    {
        goodTimer += Time.deltaTime;
        badTimer += Time.deltaTime;
        //��10���������
        if (goodTimer > 10f)
        {
            CreatGoodBird();
            goodTimer = 0;
        }
        //��1���������
        if (badTimer > 1f)
        {
            CreatBadBird();
            badTimer = 0;
        }
    }

    private void CreatGoodBird()
    {
        int goodIndex = (int)Random.Range(0f, goodBirds.Length-2);
        //�����ص��y�������
        float y = Random.Range(-10f, 25f);
        //ʵ����Ԥ����
        Instantiate(goodBirds[goodIndex], new Vector3(spawnPos, y, 0), Quaternion.Euler(0, 180, 0));
    }

    private void CreatBadBird()
    {
        int badIndex = (int)Random.Range(0f, badBirds.Length-2);
        //�����ص��y�������
        float y = Random.Range(-10f, 25f);
        //ʵ����Ԥ����
        Instantiate(badBirds[badIndex], new Vector3(spawnPos, y, 0), Quaternion.Euler(0, 180, 0));
    }


}
