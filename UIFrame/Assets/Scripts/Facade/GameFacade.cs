using System;
using UnityEngine;
using UIFrame;

public class GameFacade:MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.PushUI("MainPanel");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.PopUI();
        }
    }
}