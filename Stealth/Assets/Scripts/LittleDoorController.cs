using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleDoorController : MonoBehaviour
{
    //触发器内可能有几个人 要保证触发器内没有人才关 条件就不是进入和离开了
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
