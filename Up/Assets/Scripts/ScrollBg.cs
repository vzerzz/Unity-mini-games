using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBg : MonoBehaviour
{
    public int speed = 4;
    //������λ��
    private Vector3 flag1, flag2;
    private GameObject bg1;
    private GameObject bg2;
    private GameObject swap;

    private void Awake()
    {
        //��ȡ��������
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
        //����һ��������X����ǰһ��������ʼ�洢��X�� ��ǰһ������λ����Ϊ��һ��������ʼ��λ�á�Ȼ�󽻻���ǰ��󣬺��ǰ��������һ��ѭ����
        if (bg2.transform.position.x < flag1.x)
        {
            bg1.transform.position = flag2;
            swap = bg1;
            bg1 = bg2;
            bg2 = swap;
        }
    }
}
