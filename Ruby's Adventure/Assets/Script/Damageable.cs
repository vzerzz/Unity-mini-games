using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    //public AudioClip collectedClip;
    private void OnTriggerStay2D(Collider2D other)
    {
        RubyControler controller = other.GetComponent<RubyControler>();

        if (controller != null)
        {
                controller.ChangeHealth(-1);
                //controller.PlaySound(collectedClip);
        }
    }
}
