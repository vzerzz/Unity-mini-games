using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //[SerializeField]private Rigidbody2D rb;
    private Rigidbody2D rb;//刚体
    private Animator anim;//动画
    public Collider2D coll;//碰撞
    public Collider2D DisColl;
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    [SerializeField]
    public int cherry;
    public Text CherryNum;
    private bool isHurt;//stop when get hurt
    // public AudioSource jumpAudio;
                        // public AudioSource itemAudio;
                        //  public AudioSource hurtAudio;
    public Transform CellingCheck, GroundCheck;
    private bool isGround;
    //private int extraJump;

    bool jumpPressed;
    int jumpCount;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!isHurt)
        {
            GroundMovement();
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
        isGround = Physics2D.OverlapCircle(GroundCheck.position, 0.1f, ground);
        Jump();
        SwitchAnim();
        Crouch();
        CherryNum.text = cherry.ToString();
    }

    void GroundMovement()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);//尽量用vector3
        }
    }

    void Jump()
    {
        if (isGround)
        {
            jumpCount = 1;//二段跳
        }
        if(jumpPressed && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            SoundMananger.instance.JumpAudio();
            jumpCount--;
            jumpPressed = false;
        }
        else if(jumpPressed && jumpCount > 0 && !isGround)//二段跳
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            SoundMananger.instance.JumpAudio();
            jumpCount--;
            jumpPressed = false;
        }
    }

    void SwitchAnim()
    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));
        if (isGround)
        {
            anim.SetBool("falling", false);
        }
        else if(!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
        }
        else if(rb.velocity.y < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
        if (isHurt)//受到攻击
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 3f)//弹开停止
            {
                anim.SetBool("hurt", false);
                //anim.SetBool("idle", true);
                isHurt = false;
            }
        }
    }

    void Crouch()
    {
        if (!Physics2D.OverlapCircle(CellingCheck.position, 0.2f, ground))
        {
            if (Input.GetButton("Crouch"))
            {
                anim.SetBool("crouching", true);
                DisColl.enabled = false;
            }
            else if (!Input.GetButton("Crouch"))
            {
                anim.SetBool("crouching", false);
                DisColl.enabled = true;
            }
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CherryCount()
    {
        cherry++;
    }

    private void OnTriggerEnter2D(Collider2D collision)//碰撞触发器
    {
        if (collision.tag == "Collection")//收集
        {
            SoundMananger.instance.CherryAudio();
            collision.GetComponent<Animator>().Play("feedback");
        }
        if (collision.tag == "DeadLine")
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemies")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);//踩后起跳
                anim.SetBool("jumping", true);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)//弹开
            {
                rb.velocity = new Vector2(-5, rb.velocity.y);
                isHurt = true;
                SoundMananger.instance.HurtAudio();
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(5, rb.velocity.y);
                isHurt = true;
                SoundMananger.instance.HurtAudio();
            }
        }
    }
    /*void SwitchAnim()//动画开关控件
    {
        //anim.SetBool("idle", false);
        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))//走到边缘下落情况
        {
            anim.SetBool("falling", true);
        }
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)//开始下落
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (isHurt)//受到攻击
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 3f)//弹开停止
            {
                anim.SetBool("hurt", false);
                //anim.SetBool("idle", true);
                isHurt = false;
            }
        }
        else if (coll.IsTouchingLayers(ground))//落地
        {
            anim.SetBool("falling", false);
        }
    }*/
    /*void Movement()
    {
        float horizontalmove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");
        //move

        rb.velocity = new Vector2(horizontalmove * speed * Time.fixedDeltaTime, rb.velocity.y);//移速
        anim.SetFloat("running", Mathf.Abs(facedirection));//移动动画

        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);//转动方向
        }

    }*/

    /*void Jump()
    {
        if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.fixedDeltaTime);//x方向速度不变，y方向起跳
            jumpAudio.Play();
            anim.SetBool("jumping", true);//起跳动画

        }
    }*/
    /*void newJump()
    {
        if (isGround)
        {
            extraJump = 1;
        }
        if (Input.GetButtonDown("Jump") && extraJump > 0)
        {
            rb.velocity = Vector2.up * jumpforce;//new Vector2(0,1)
            extraJump--;
            // SoundMananger soundMananger = gameObject.GetComponent<SoundMananger>();
            //  soundMananger.jumpAudio();
            SoundMananger.instance.JumpAudio();
            anim.SetBool("jumping", true);
        }
        if (Input.GetButtonDown("Jump") && extraJump == 0 && isGround)
        {
            rb.velocity = Vector2.up * jumpforce;//new Vector2(0,1)
            SoundMananger.instance.JumpAudio();
            anim.SetBool("jumping", true);
        }
    }*/
}