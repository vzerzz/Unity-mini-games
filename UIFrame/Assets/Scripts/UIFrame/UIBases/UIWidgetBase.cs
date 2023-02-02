using UIFrame;
using UIInterface;
using UnityEngine;

namespace UIFrame
{
    public class UIWidgetBase:UIMono
    {
        //当前元件所处的模块
        private UIModuleBase currentModule;

        public void UIWidgetInit(UIModuleBase uiModuleBase)
        {
            //设置当前元件所属的模块
            currentModule = uiModuleBase;
            //当前元件添加到字典中
            UIManager.Instance.AddUIWidget(currentModule.name, name, this);
        }

        protected virtual void OnDestroy()
        {
            //当前元件移除
            UIManager.Instance.RemoveUIWidget(currentModule.name, name);
        }
    }
}
