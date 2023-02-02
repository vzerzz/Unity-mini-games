using UnityEditor;
using UnityEngine;
using UIFrame;
public class HeroMsgPanelController : UIControllerBase
    {
    protected override void ControllerStart()
    {
        base.ControllerStart();
        BindEvent();
    }

    private void BindEvent()
    {
        crtModule.FindCurrentModuleWidget("CloseButton_F").AddOnClickListener(() =>
        {
            UIManager.Instance.PopUI();
        });
    }
}
