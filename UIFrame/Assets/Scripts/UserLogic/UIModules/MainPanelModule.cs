using System.Collections;
using UnityEngine;
using UIFrame;
using System;

namespace UIModules
{
    public class MainPanelModule : UIModuleBase
    {
        public override void Awake()
        {
            base.Awake();
            //创建控制器
            var controller = new MainPanelController();
            //绑定控制
            BindController(controller);

        }
    }
}