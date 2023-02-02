using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("走到跑的时间")]
    [Range(0.1f,2f)]
    public float dempTime = 1.5f;
    [Header("转身速度")]
    public float turnSpeed = 10f;
    [Header("喊叫声音片段")]
    public AudioClip shoutClip;

    private float hor, ver;
    private bool sneak, shout;

    private Animator ani;
    private AudioSource aud;
    private Vector3 dir;
    private Quaternion targetQua;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        dir = Vector3.zero;
    }
    private void Update()
    {
        hor = Input.GetAxis(GameConsts.AXIS_HORIZONTAL);
        ver = Input.GetAxis(GameConsts.AXIS_VERTICAL);

        sneak = Input.GetButton(GameConsts.BUTTON_SNEAK);
        shout = Input.GetButtonDown(GameConsts.BUTTON_SHOUT);

        dir.x = hor;
        dir.z = ver;

        if(hor != 0||ver != 0)
        {
            ani.SetFloat(GameConsts.PARAM_SPEED, 5.66f, dempTime, Time.deltaTime);
            //转身两步：
            //方向向量转换为四元数
            targetQua = Quaternion.LookRotation(dir);
            //Lerp过去
            transform.rotation = Quaternion.Lerp(transform.rotation, targetQua, Time.deltaTime * turnSpeed);
            
        }
        else
        {
            ani.SetFloat(GameConsts.PARAM_SPEED, 1.5f);
        }
        ani.SetBool(GameConsts.PARAM_SNEAK, sneak);

        if (shout)
        {
            ani.SetTrigger(GameConsts.PARAM_SHOUT);
        }
        AudioSetUp();
    }
    /// <summary>
    /// 声音配置
    /// </summary>
    private void AudioSetUp()
    {
        bool isLocomotion = ani.GetCurrentAnimatorStateInfo(0).shortNameHash == GameConsts.STATE_LOCOMOTION;
        if (isLocomotion)
        {
            if (!aud.isPlaying)
            {
                aud.Play();//loop选不选无所谓
            }
        }
        else
        {
            aud.Stop();
        }
    }
    public void PlayShoutAudio()//动画事件添加声音
    {
        AudioSource.PlayClipAtPoint(shoutClip, transform.position);
    }
}
