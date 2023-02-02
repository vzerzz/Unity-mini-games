using System.Collections;
using UnityEngine;
using UIFrame;
using DG.Tweening;

namespace UIModules
{
    public class TaskPanelModule : UIModuleBase
    {
        public override void Awake()
        {
            base.Awake();
            //创建控制器
            var controller = new TaskPanelController();
            //绑定控制
            BindController(controller);

        }
        public override void OnEnter()
        {
            transform.DOLocalMoveX(0, 2);
            //执行父类的方法
            base.OnEnter();
        }

        public override void OnExit()
        {
            transform.DOLocalMoveX(2000,2);
            base.OnExit();
        }
    }
    
}