using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    //��������
    public GameObject helpCanvas;
    //��������ķ��ذ���
    public GameObject buttons;

    /// <summary>
    /// ��ʼ��Ϸ
    /// </summary>
    public void OnClickStart()
    {
        SceneManager.LoadScene("Level1");
    }

    /// <summary>
    /// �򿪰�������
    /// </summary>
    public void OnClickHelp()
    {
        helpCanvas.SetActive(true);
        buttons.SetActive(false);
    }
   /// <summary>
   /// �˳�����
   /// </summary>
    public void OnClickQuit()
    {
        Application.Quit();
    }
    /// <summary>
    /// �Ӱ��������˳�
    /// </summary>
    public void OnClickBack()
    {
        helpCanvas.SetActive(false);
        buttons.SetActive(true);

    }
}
