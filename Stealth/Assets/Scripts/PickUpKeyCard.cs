using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpKeyCard : MonoBehaviour
{
    public AudioClip pickUpClip;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConsts.TAG_PLAYER))
        {
            PlayerBag.instance.hasKey = true;
            AudioSource.PlayClipAtPoint(pickUpClip, transform.position);
            Destroy(gameObject);
        }
    }
}
