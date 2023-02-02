using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CabinController : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;


    private void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        //���¿ո�����������һ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, 300));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //����ײ�����ǩΪ�ϰ�ʱֱ�����¼��ص�ǰ����
        if (collision.collider.CompareTag("Obstacle"))
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(index);
        }
    }
}
