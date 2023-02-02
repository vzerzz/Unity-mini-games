using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CabinBird : MonoBehaviour
{
    //不同状态的小屋的图片
    public Sprite[] sprites;

    private int index = 0;

    private SpriteRenderer spriteRenderer;
    //碰到不同鸟的音效
    public AudioClip goodAud;
    public AudioClip badAud;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bad"))//碰到的是坏鸟
        {
            if (++index == sprites.Length - 1)//没命则重开
            {
                int index2 = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(index2);
            }
            else//减少气球
            {
                AudioSource.PlayClipAtPoint(badAud, new Vector3(0, 0, -10f));
                spriteRenderer.sprite = sprites[index];
            }
        }
        if (other.CompareTag("Good"))//碰到的是好鸟
        {
            if (--index == -1)//满命则不变
            {
                index = 0;
            }
            else//增加气球
            {
                AudioSource.PlayClipAtPoint(goodAud, new Vector3(0, 0, -10f));
                spriteRenderer.sprite = sprites[index];
            }
        }
        Destroy(other.gameObject);//销毁鸟
    }
}

