using Localization;
using System;
using System.Collections;
using UIFrame;
using UnityEngine;

namespace UIFrame
{
    /// <summary>
    /// 子类挂载再每一个panel模块上
    /// </summary>
    //当前模块组件依赖于CanvasGroup
    [RequireComponent(typeof(CanvasGroup))]
    public class UIModuleBase : MonoBehaviour
    {
        protected CanvasGroup _canvasGroup;
        //所有子对象
        private Transform[] allChild;
        public virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            //获取当前模块所有子对象
            allChild = GetComponentsInChildren<Transform>();
            //修改当前模块的名称【去Clone】
            gameObject.name = gameObject.name.Remove(gameObject.name.Length - "(Clone)".Length);
            //给所有可用的UI元件添加行为
            AddWidgetBehaviour();
        }
        #region Controller Bind
        protected void BindController(UIControllerBase controllerBase)
        {
            controllerBase.ControllerInit(this);
        }
        #endregion

        #region Set Widgets
        private void AddWidgetBehaviour()
        {
            //遍历所有子对象
            for(int i = 0; i < allChild.Length; i++)
            {
                //遍历所有标记Token
                for(int j = 0; j < SystemDefine.WIDGET_TOKENS.Length; j++)
                {
                    //判断元件是否以该标记为名字结尾
                   if(allChild[i].name.EndsWith(SystemDefine.WIDGET_TOKENS[j]))
                    {
                        //为该元件添加组件
                        //allChild[i].gameObject.AddComponent<UIWidgetBase>();
                        AddComponentForWidget(i);
                    }

                }
            }
        }
        //这样方便更改
        protected virtual void AddComponentForWidget(int index)
        {
            //给该元件添加UIWidgetBase组件
            UIWidgetBase uiWidgetBase = allChild[index].gameObject.AddComponent<UIWidgetBase>();
            //设置该元件的模块
            uiWidgetBase.UIWidgetInit(this);
            
        }
        #endregion

        #region State Call
        /// <summary>
        /// 进入当前模块执行函数
        /// </summary>
        public virtual void OnEnter()
        {
            _canvasGroup.blocksRaycasts = true;
            LocalizationManager.Instance.LocalizationInit();
        }
        /// <summary>
        /// 离开当前模块执行函数
        /// </summary>
        public virtual void OnExit()
        {
            _canvasGroup.blocksRaycasts = false;
        }
        /// <summary>
        /// 暂离当前模块执行函数
        /// </summary>
        public virtual void OnPause()
        {
            _canvasGroup.blocksRaycasts= false;
        }
        /// <summary>
        /// 恢复当前模块执行函数
        /// </summary>
        public virtual void OnResume()
        {
            _canvasGroup.blocksRaycasts = true;
        }
        #endregion

        #region Find Widget
        public UIWidgetBase FindCurrentModuleWidget(string widgetName)
        {
            return UIManager.Instance.FindWidget(name,widgetName);

        }
        #endregion
    }
}