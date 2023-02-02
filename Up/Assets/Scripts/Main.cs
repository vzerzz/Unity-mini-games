using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    //帮助界面
    public GameObject helpCanvas;
    //帮助界面的返回按键
    public GameObject buttons;

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void OnClickStart()
    {
        SceneManager.LoadScene("Level1");
    }

    /// <summary>
    /// 打开帮助画布
    /// </summary>
    public void OnClickHelp()
    {
        helpCanvas.SetActive(true);
        buttons.SetActive(false);
    }
   /// <summary>
   /// 退出程序
   /// </summary>
    public void OnClickQuit()
    {
        Application.Quit();
    }
    /// <summary>
    /// 从帮助界面退出
    /// </summary>
    public void OnClickBack()
    {
        helpCanvas.SetActive(false);
        buttons.SetActive(true);

    }
}
