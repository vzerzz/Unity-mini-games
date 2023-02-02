using System.Collections;
using UnityEngine;
using UIFrame;
using DG.Tweening;

namespace UIModules
{
    public class BagPanelModule : UIModuleBase
    {
        public override void Awake()
        {
            base.Awake();
            //创建控制器
            var controller = new BagPanelController();
            //绑定控制
            BindController(controller);

        }
        public override void OnEnter()
        {
            _canvasGroup.DOFade(1, 1);
            //执行父类的方法
            base.OnEnter();
        }

        public override void OnExit()
        {
            _canvasGroup.DOFade(0, 1);
            base.OnExit();
        }
    }
}