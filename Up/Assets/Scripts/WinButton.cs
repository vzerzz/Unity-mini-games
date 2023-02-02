using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinButton : MonoBehaviour
{
    /// <summary>
    /// ��һ��
    /// </summary>
    public void OnButtonClickNext()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(index);
    }
    /// <summary>
    /// ���´˹�
    /// </summary>
    public void OnButtonClickResume()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }
    /// <summary>
    /// �ص����˵�
    /// </summary>
    public void OnButtonClickHome()
    {
        SceneManager.LoadScene("Main");
    }
}
