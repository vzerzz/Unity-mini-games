using UnityEngine;

namespace UIFrame
{
    /// <summary>
    /// 专注于写UI里的逻辑
    /// </summary>
    public class UIControllerBase
    {
        protected UIModuleBase crtModule;

        public void ControllerInit(UIModuleBase moduleBase)
        {
            crtModule = moduleBase;
            //启动
            ControllerStart();
        }

        protected virtual void ControllerStart()
        {

        }
    }
}