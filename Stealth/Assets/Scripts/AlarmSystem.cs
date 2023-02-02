using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSystem : MonoBehaviour
{
    public static AlarmSystem instance;

    [Header("过渡速度")]
    public float turnSpeed = 3f;
    [HideInInspector]
    public Vector3 alarmPos = new Vector3(0,1000,0);
    [HideInInspector]
    public Vector3 safePos = new Vector3(0,1000,0);

    private AlarmLight alarmLight;

    private Light mainLight;

    private AudioSource normalAud;
    private AudioSource panicAud;
    private GameObject[] sirensObj;
    private AudioSource[] sirensAud;

    private void Awake()
    {
        instance = this;
        alarmLight = GameObject.FindWithTag(GameConsts.TAG_ALARMLIGHT).GetComponent<AlarmLight>();  
        mainLight = GameObject.FindWithTag(GameConsts.TAG_MAINLIGHT).GetComponent<Light>();
        normalAud = GetComponent<AudioSource>();
        panicAud = transform.GetChild(0).GetComponent<AudioSource>();
        sirensObj = GameObject.FindGameObjectsWithTag(GameConsts.TAG_SIREN);

    }

    private void Start()
    {
        sirensAud = new AudioSource[sirensObj.Length];
        for (int i = 0; i < sirensAud.Length; i++)
        {
            sirensAud[i] = sirensObj[i].GetComponent<AudioSource>();
        }
    }
    private void Update()
    {
        AlarmSystemOperation(alarmPos != safePos);//简化if else
    }
    /// <summary>
    /// 警报操作
    /// </summary>
    /// <param name="isAlarm"></param>
    private void AlarmSystemOperation(bool isAlarm)
    {
        float value = 0;
        if (isAlarm)
        {
            value = 1;
        }

        alarmLight.isAlarm = isAlarm;
        mainLight.intensity = Mathf.Lerp(mainLight.intensity, 1-value, Time.deltaTime * turnSpeed);
        normalAud.volume = Mathf.Lerp(normalAud.volume, 1-value, Time.deltaTime * turnSpeed);
        panicAud.volume = Mathf.Lerp(panicAud.volume, value, Time.deltaTime * turnSpeed);
        for (int i = 0; i < sirensAud.Length; i++)
        {
            sirensAud[i].volume = Mathf.Lerp(sirensAud[i].volume, value, Time.deltaTime * turnSpeed);
        }
    }
}
