using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLight : MonoBehaviour
{
    [Header("是否开启警报灯")]
    public bool isAlarm;
    [Header("闪烁速度")]
    public float turnSpeed = 3f;
    private float highIntencity = 2f;
    private float lowIntencity = 0f;
    private float targetIntencity;

    private Light lt;

    private void Awake()
    {
        lt = GetComponent<Light>();
    }
    private void Start()
    {
        targetIntencity = highIntencity;
    }
    private void Update()
    {
        if (isAlarm)
        {
            if(Mathf.Abs(lt.intensity - targetIntencity) < 0.05f)
            {
                if(targetIntencity == highIntencity)
                {
                    targetIntencity = lowIntencity;
                }
                else
                {
                    targetIntencity = highIntencity;
                }
            }
            lt.intensity = Mathf.Lerp(lt.intensity, targetIntencity, Time.deltaTime*turnSpeed);//无限接近
        
        }
        else
        {
            lt.intensity = Mathf.Lerp(lt.intensity, lowIntencity, Time.deltaTime * turnSpeed);
            if (Mathf.Abs(lt.intensity - lowIntencity) < 0.05f)
            {
                lt.intensity = 0;//减少CPU的计算
            }
        }
    }

}
