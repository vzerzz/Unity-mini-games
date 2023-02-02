using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleDoorController : MonoBehaviour
{
    //�������ڿ����м����� Ҫ��֤��������û���˲Ź� �����Ͳ��ǽ�����뿪��
    private int counter;
    private Animator ani;
    public AudioClip doorAud;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;
        if (other.CompareTag(GameConsts.TAG_PLAYER) || other.CompareTag(GameConsts.TAG_ENEMY))
        {
            if(++counter == 1)
            {
                ani.SetBool(GameConsts.PARAM_DOOROPEN, true);
                AudioSource.PlayClipAtPoint(doorAud, transform.position);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameConsts.TAG_PLAYER) || other.CompareTag(GameConsts.TAG_ENEMY))
        {
            if(--counter == 0)
            {
                ani.SetBool(GameConsts.PARAM_DOOROPEN, false);
                AudioSource.PlayClipAtPoint(doorAud, transform.position);
            }
        }
    }

}
