using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }//setΪ˽������ get�������

    public Image mask;
    float originalSize;

    void Awake()
    {
        instance = this;//�������� ֻ��һ������ֵ��
    }
    // Start is called before the first frame update
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;//��ȡ��Ļ�ϵĴ�С
    }

    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);//�Ӵ��������ô�С�����
    }
}
