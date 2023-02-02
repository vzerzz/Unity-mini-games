using UnityEditor;
using UnityEngine;
using UIFrame;

    public class BagPanelController : UIControllerBase
    {
        protected override void ControllerStart()
        {
            base.ControllerStart();
            BindEvent();
        }

        private void BindEvent()
        {
        for(int i = 1; i < 5; i++)
        {
            UIWidgetBase widgetBase = crtModule.FindCurrentModuleWidget("Goods"+i.ToString()+"_F");
            crtModule.FindCurrentModuleWidget("Goods"+i.ToString()+"_F").AddOnClickListener(() =>
            {
                UIManager.Instance.PushUI("NormalWindowPanel");
                Sprite spr = widgetBase.GetSprite();
                UIManager.Instance.FindWidget("NormalWindowPanel","Image_S").SetSprite(spr);
            });
        }

        }
    }
