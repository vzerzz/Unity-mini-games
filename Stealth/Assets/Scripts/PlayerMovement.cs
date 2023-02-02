using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�ߵ��ܵ�ʱ��")]
    [Range(0.1f,2f)]
    public float dempTime = 1.5f;
    [Header("ת���ٶ�")]
    public float turnSpeed = 10f;
    [Header("��������Ƭ��")]
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
            //ת��������
            //��������ת��Ϊ��Ԫ��
            targetQua = Quaternion.LookRotation(dir);
            //Lerp��ȥ
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
    /// ��������
    /// </summary>
    private void AudioSetUp()
    {
        bool isLocomotion = ani.GetCurrentAnimatorStateInfo(0).shortNameHash == GameConsts.STATE_LOCOMOTION;
        if (isLocomotion)
        {
            if (!aud.isPlaying)
            {
                aud.Play();//loopѡ��ѡ����ν
            }
        }
        else
        {
            aud.Stop();
        }
    }
    public void PlayShoutAudio()//�����¼��������
    {
        AudioSource.PlayClipAtPoint(shoutClip, transform.position);
    }
}
