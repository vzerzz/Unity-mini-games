using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //[SerializeField]private Rigidbody2D rb;
    private Rigidbody2D rb;//����
    private Animator anim;//����
    public Collider2D coll;//��ײ
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
            transform.localScale = new Vector3(horizontalMove, 1, 1);//������vector3
        }
    }

    void Jump()
    {
        if (isGround)
        {
            jumpCount = 1;//������
        }
        if(jumpPressed && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            SoundMananger.instance.JumpAudio();
            jumpCount--;
            jumpPressed = false;
        }
        else if(jumpPressed && jumpCount > 0 && !isGround)//������
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
        if (isHurt)//�ܵ�����
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 3f)//����ֹͣ
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

    private void OnTriggerEnter2D(Collider2D collision)//��ײ������
    {
        if (collision.tag == "Collection")//�ռ�
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
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);//�Ⱥ�����
                anim.SetBool("jumping", true);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)//����
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
    /*void SwitchAnim()//�������ؿؼ�
    {
        //anim.SetBool("idle", false);
        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))//�ߵ���Ե�������
        {
            anim.SetBool("falling", true);
        }
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)//��ʼ����
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (isHurt)//�ܵ�����
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 3f)//����ֹͣ
            {
                anim.SetBool("hurt", false);
                //anim.SetBool("idle", true);
                isHurt = false;
            }
        }
        else if (coll.IsTouchingLayers(ground))//���
        {
            anim.SetBool("falling", false);
        }
    }*/
    /*void Movement()
    {
        float horizontalmove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");
        //move

        rb.velocity = new Vector2(horizontalmove * speed * Time.fixedDeltaTime, rb.velocity.y);//����
        anim.SetFloat("running", Mathf.Abs(facedirection));//�ƶ�����

        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);//ת������
        }

    }*/

    /*void Jump()
    {
        if (Input.GetButton("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.fixedDeltaTime);//x�����ٶȲ��䣬y��������
            jumpAudio.Play();
            anim.SetBool("jumping", true);//��������

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