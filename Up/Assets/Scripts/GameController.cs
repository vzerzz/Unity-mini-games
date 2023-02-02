using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    //��Ϸ��ʱ
    public float timer = 0;
    //ͨ�ػ���
    public GameObject canvas;
    private Rigidbody2D rb;
    //�Ϸ���ʾ������
    private TMP_Text text;
    //�ؿ�ʱ��
    private float roundTime = 60f;
    public GameObject playBtn;
    public GameObject pauseBtn;
    public AudioClip winAud;
    public GameObject Bg;

    private void Awake()
    {
        rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        text = transform.GetChild(0).GetComponent<TMP_Text>();
    }
    private void Start()
    {
        OnButtonClickPlay();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        text.text = (roundTime - timer).ToString("F0");
        if (timer > roundTime)//������ʱ������ͨ��ʱ
        {
            //ֹͣ������˶�
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0;
            rb.GetComponentInParent<BoxCollider2D>().enabled = false;
            rb.GetComponentInParent<CircleCollider2D>().enabled = false;
            if (Bg != null)
            {
                //ֹͣ�׵�����
                Bg.GetComponent<LightCreat>().enabled = false;
            }
            canvas.gameObject.SetActive(true);//����ͨ�ػ���
            gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(winAud, new Vector3(0, 0, -10));//����ͨ����Ч
            timer = 0;
        }
    }
    /// <summary>
    /// ���ֿ�ʼ
    /// </summary>
    public void OnButtonClickResume()
    {
        //���¼��ص�ǰ����
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }
    /// <summary>
    /// ֹͣ
    /// </summary>
    public void OnButtonClickPause()
    {
        Time.timeScale = 0;
        playBtn.SetActive(true);
        pauseBtn.SetActive(false);
    }
    /// <summary>
    /// ��ʼ
    /// </summary>
    public void OnButtonClickPlay()
    {
        Time.timeScale = 1f;
        playBtn.SetActive(false);
        pauseBtn.SetActive(true);
    }

    public void OnButtonClickHome()
    {
        SceneManager.LoadScene("Main");
    }

}
