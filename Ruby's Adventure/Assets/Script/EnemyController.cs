using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    bool broken = true;

    private Rigidbody2D rigidbody2d;
    Animator animator;
    float timer;
    int direction = 1;

    public ParticleSystem smokeEffect;

    public AudioClip collectedClip;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;
    }
    private void Update()
    {
        if (!broken)
            return;

        timer -= Time.deltaTime;

        if(timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!broken)
            return;
        Vector2 position = rigidbody2d.position;
        if (vertical)
        {
            position.y += Time.deltaTime * speed*direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed*direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2d.MovePosition(position);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        RubyControler player = collision.gameObject.GetComponent<RubyControler>();

        if (player != null)
            player.ChangeHealth(-1);

        player.PlaySound(collectedClip);
     
    }

    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
    }
}
