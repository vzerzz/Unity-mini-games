using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }//set为私有属性 get均会调用

    public Image mask;
    float originalSize;

    void Awake()
    {
        instance = this;//单例对象 只有一个生命值条
    }
    // Start is called before the first frame update
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;//获取屏幕上的大小
    }

    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);//从代码中设置大小和瞄点
    }
}
