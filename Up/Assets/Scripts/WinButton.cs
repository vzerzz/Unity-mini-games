using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinButton : MonoBehaviour
{
    /// <summary>
    /// 下一关
    /// </summary>
    public void OnButtonClickNext()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(index);
    }
    /// <summary>
    /// 重新此关
    /// </summary>
    public void OnButtonClickResume()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }
    /// <summary>
    /// 回到主菜单
    /// </summary>
    public void OnButtonClickHome()
    {
        SceneManager.LoadScene("Main");
    }
}
