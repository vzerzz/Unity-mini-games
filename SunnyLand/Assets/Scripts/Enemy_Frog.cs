using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{
    private Rigidbody2D rb;
    public Transform leftpoint, rightpoint;
    private bool Faceleft = true;
    public float speed, JumpForce;
    private Collider2D coll;
    public LayerMask ground;
    private float leftx, rightx;
    //private Animator anim;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        //transform.DetachChildren();
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
        //anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnim();
    }
    void Movement()
    {
        if (Faceleft)
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumpping", true);
                rb.velocity = new Vector2(-speed, JumpForce);
            }
            if (transform.position.x < leftx)
            {
                rb.velocity = new Vector2(0, 0);
                transform.localScale = new Vector3(-1, 1, 1);
                Faceleft = false;
            }
        }
        else
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("jumpping", true);
                rb.velocity = new Vector2(speed, JumpForce);
            }
            if (transform.position.x > rightx)
            {
                rb.velocity = new Vector2(0, 0);
                transform.localScale = new Vector3(1, 1, 1);
                Faceleft = true;
            }
        }
    }
    void SwitchAnim()
    {
        if (anim.GetBool("jumpping"))
        {
            if (rb.velocity.y < 0.1)
            {
                anim.SetBool("jumpping", false);
                anim.SetBool("falling", true);
            }
        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
        }
    }
}


