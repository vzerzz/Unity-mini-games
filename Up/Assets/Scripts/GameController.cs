using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    //游戏计时
    public float timer = 0;
    //通关画面
    public GameObject canvas;
    private Rigidbody2D rb;
    //上方显示的文字
    private TMP_Text text;
    //关卡时间
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
        if (timer > roundTime)//当倒计时结束即通关时
        {
            //停止物体的运动
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0;
            rb.GetComponentInParent<BoxCollider2D>().enabled = false;
            rb.GetComponentInParent<CircleCollider2D>().enabled = false;
            if (Bg != null)
            {
                //停止雷电生成
                Bg.GetComponent<LightCreat>().enabled = false;
            }
            canvas.gameObject.SetActive(true);//激活通关画面
            gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(winAud, new Vector3(0, 0, -10));//播放通关音效
            timer = 0;
        }
    }
    /// <summary>
    /// 重现开始
    /// </summary>
    public void OnButtonClickResume()
    {
        //重新加载当前场景
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }
    /// <summary>
    /// 停止
    /// </summary>
    public void OnButtonClickPause()
    {
        Time.timeScale = 0;
        playBtn.SetActive(true);
        pauseBtn.SetActive(false);
    }
    /// <summary>
    /// 开始
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
