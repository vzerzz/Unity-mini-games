using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConsts.TAG_PLAYER))
        {
            AlarmSystem.instance.alarmPos = other.transform.position;
        }
    }
}
