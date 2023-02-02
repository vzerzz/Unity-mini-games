using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CabinBird : MonoBehaviour
{
    //��ͬ״̬��С�ݵ�ͼƬ
    public Sprite[] sprites;

    private int index = 0;

    private SpriteRenderer spriteRenderer;
    //������ͬ�����Ч
    public AudioClip goodAud;
    public AudioClip badAud;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bad"))//�������ǻ���
        {
            if (++index == sprites.Length - 1)//û�����ؿ�
            {
                int index2 = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(index2);
            }
            else//��������
            {
                AudioSource.PlayClipAtPoint(badAud, new Vector3(0, 0, -10f));
                spriteRenderer.sprite = sprites[index];
            }
        }
        if (other.CompareTag("Good"))//�������Ǻ���
        {
            if (--index == -1)//�����򲻱�
            {
                index = 0;
            }
            else//��������
            {
                AudioSource.PlayClipAtPoint(goodAud, new Vector3(0, 0, -10f));
                spriteRenderer.sprite = sprites[index];
            }
        }
        Destroy(other.gameObject);//������
    }
}

