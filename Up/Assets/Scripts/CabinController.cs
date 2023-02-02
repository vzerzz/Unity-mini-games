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
        //按下空格则向上增加一个力
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, 300));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //当碰撞对象标签为障碍时直接重新加载当前场景
        if (collision.collider.CompareTag("Obstacle"))
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(index);
        }
    }
}
