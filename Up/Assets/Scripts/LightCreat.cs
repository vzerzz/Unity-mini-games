using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCreat : MonoBehaviour
{
    //����Ԥ����
    public GameObject Light1;
    //������Ч
    public AudioClip lightAud;
    //��ʱ
    private float timer = 0;
    GameObject light;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 6f)//6�����һ��
        {
            Creat();
            timer = 0;
        }
    }

    private void Creat()
    {
        float x = Random.Range(-10f, 5f);
        float y = Random.Range(10f, 12f);
        Vector3 pos = new Vector3(x, y, 0);//��һ����Χ���������
        //Ԥ�����ʵ����
        light = Instantiate(Light1, pos, Quaternion.Euler(0, 0, -90));
        //���������Ӷ���ͬ�ƶ�
        light.transform.SetParent(transform, true);
        //������Ч
        AudioSource.PlayClipAtPoint(lightAud,new Vector3(0,0,-10));
    }

}
