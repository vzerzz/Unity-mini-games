using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDoorController : MonoBehaviour
{
    private Animator ani;
    private Animator innerDoorAni;
    public AudioClip doorClip;
    public AudioClip refuseClip;

    private LiftController liftController;
    private Vector3 dir;
    private SphereCollider sphereCollider;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        innerDoorAni = GameObject.FindWithTag(GameConsts.TAG_INNERDOOR).GetComponent<Animator>();
        sphereCollider = GetComponent<SphereCollider>();
        liftController = GameObject.FindWithTag(GameConsts.TAG_LIFT).GetComponent<LiftController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConsts.TAG_PLAYER))
        {
            if (PlayerBag.instance.hasKey) 
            { 
                ani.SetBool(GameConsts.PARAM_DOOROPEN, true);
                innerDoorAni.SetBool(GameConsts.PARAM_DOOROPEN, true);
                AudioSource.PlayClipAtPoint(doorClip, transform.position);
            }
            else
            {
                AudioSource.PlayClipAtPoint(refuseClip, transform.position);
            }
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameConsts.TAG_PLAYER))
        {
            if (ani.GetBool(GameConsts.PARAM_DOOROPEN))
            {
                ani.SetBool(GameConsts.PARAM_DOOROPEN, false);
                innerDoorAni.SetBool(GameConsts.PARAM_DOOROPEN, false);
                AudioSource.PlayClipAtPoint(doorClip, transform.position);

                dir = other.transform.position - transform.position;
                if(dir.z > 0)
                {
                    sphereCollider.enabled = false;

                    liftController.BeginMove();
                }
            }
        }
    }
}
