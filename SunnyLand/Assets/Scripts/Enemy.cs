using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected AudioSource deathAudio;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        deathAudio = GetComponent<AudioSource>();
    }
    public void Death()
    {
        GetComponent<Collider2D>().enabled = false;//��������ײ
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        anim.SetTrigger("death");
        deathAudio.Play();
    }
}

