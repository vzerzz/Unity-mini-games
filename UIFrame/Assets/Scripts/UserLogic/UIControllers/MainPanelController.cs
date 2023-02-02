using UnityEditor;
using UnityEngine;
using UIFrame;
public class MainPanelController:UIControllerBase
{
    protected override void ControllerStart()
    {
        base.ControllerStart();
        BindEvent();
    }

    private void BindEvent()
    {
        crtModule.FindCurrentModuleWidget("TaskButton_F").AddOnClickListener(() =>
        {
            UIManager.Instance.PushUI("TaskPanel");
        });
        crtModule.FindCurrentModuleWidget("SystemButton_F").AddOnClickListener(() =>
        {
            UIManager.Instance.PushUI("SystemPanel");
        });
        crtModule.FindCurrentModuleWidget("HeaderMask_S").AddOnClickListener(() =>
        {
            UIManager.Instance.PushUI("HeroMsgPanel");
        });
        crtModule.FindCurrentModuleWidget("BagButton_F").AddOnClickListener(() =>
        {
            UIManager.Instance.PushUI("HeroEquipPanel");
            UIManager.Instance.PushUI("BagPanel");
        });


    }
}
