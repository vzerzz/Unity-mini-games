using System.Collections.Generic;
using UnityEngine;
using UIFrame;
using System;

namespace UIFrame
{
    public class UIManager:Singleton<UIManager>
    {
        private UIManager()
        {
            uiModules = new Dictionary<UIType, UIModuleBase>();
            _canvas = GameObject.Find("Canvas").transform;
            uiModuleStack = new Stack<UIModuleBase>();
            uiWidgets = new Dictionary<string, Dictionary<string, UIWidgetBase>>();
        }

        //UITyoe.Oath->GameObject里的UIModule组件
        //每一个名字和路径都对应一个UI模块的对象
        //管理当前场景的所有UI模块
        private Dictionary<UIType, UIModuleBase> uiModules;
        //管理当前场景的所有UI元件
        private Dictionary<string, Dictionary<string, UIWidgetBase>> uiWidgets;
        //UI模块的栈存储
        private Stack<UIModuleBase> uiModuleStack;
        //当前场景中画布
        private Transform _canvas;
        #region UI Module GameObject
        /// <summary>
        /// 通过UIType获取该Type所对应的模块游戏对象身上的UIModuleBase
        /// </summary>
        /// <param name="uiType"></param>
        /// <returns></returns>
        private UIModuleBase GetUIModule(UIType uiType)
        {
            UIModuleBase crtModule = null;
            if(!uiModules.TryGetValue(uiType, out crtModule))
            {
                //Instantiate生成该模块
                crtModule = InstantiateUIModule(AssetsManager.Instance.GetAsset(uiType.Path)as GameObject);
                //将该模块添加到字典中
                uiModules.Add(uiType, crtModule);
            }
            else if(crtModule == null)
            {
                //Instantiate生成该模块
                crtModule = InstantiateUIModule(AssetsManager.Instance.GetAsset(uiType.Path) as GameObject);
                //将该模块更新到字典中
                uiModules[uiType]= crtModule;
            }
            return crtModule;
        }

        private UIModuleBase InstantiateUIModule(GameObject prefab)
        {
            //生成当前模块
            GameObject crtModuleObj = GameObject.Instantiate(prefab);
            //设置父物体为画布
            crtModuleObj.transform.SetParent(_canvas, false);
            //返回组件
            return crtModuleObj.GetComponent<UIModuleBase>();
        }
        #endregion

        #region UI Module Stack
        /// <summary>
        /// 通过PanelName获取模块对象并压栈
        /// </summary>
        /// <param name="uiPanelName"></param>
        public void PushUI(string uiPanelName)
        {
            //获取UIType
            UIType _uiType = UITypeManager.Instance.GetUIType(uiPanelName);
            //获取UIModuleBase
            UIModuleBase crtModuleBase = GetUIModule(_uiType);
            //如果栈里有元素
            if(uiModuleStack.Count != 0)
            {
                //此时栈顶的窗口进入暂停状态
                uiModuleStack.Peek().OnPause();
            }
            //进窗口压栈
            uiModuleStack.Push(crtModuleBase);
            //新窗口要执行Enter
            crtModuleBase.OnEnter();
        }
        /// <summary>
        /// 栈顶元素出栈
        /// </summary>
        public void PopUI()
        {
            if (uiModuleStack.Count != 0)
            {
                uiModuleStack.Pop().OnExit();
            }
            else
            {
                Debug.LogWarning("UI栈中无元素");
            }
            //栈顶元素出栈后还有元素
            if(uiModuleStack.Count != 0)
            {
                uiModuleStack.Peek().OnResume();
            }
        }
        #endregion

        #region UI Widgets->Module (Un)Register
        /// <summary>
        /// 注册UI模块
        /// </summary>
        /// <param name="moduleName"></param>
        private void RegisterUIModuleToUIWidgets(string moduleName)
        {
            if (!uiWidgets.ContainsKey(moduleName))
            {
                //向字典中添加元素
                uiWidgets.Add(moduleName,new Dictionary<string, UIWidgetBase>());

            }
            else
            {
                //Debug.LogWarning("模块已存在");
            }
        }
        /// <summary>
        /// 取消注册
        /// </summary>
        /// <param name="moduleName"></param>
        public void UnRegisterUIModuleFromUIWidgets(string moduleName)
        {
            if (uiWidgets.ContainsKey(moduleName))
            {
                uiWidgets.Remove(moduleName);
            }
            else
            {
                //Debug.LogWarning("无该模块");
            }
        }
        #endregion

        #region UI Widgets Add/Remove
        /// <summary>
        /// 添加元件
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="widgetName"></param>
        public void AddUIWidget(string moduleName,string widgetName,UIWidgetBase uiWidget)
        {
            //模块不存在则添加
            RegisterUIModuleToUIWidgets(moduleName);
            //字典中已存在
            if (uiWidgets[moduleName].ContainsKey(widgetName))
            {
                Debug.LogWarning("元件已存在");
            }
            else
            {
                uiWidgets[moduleName].Add(widgetName, uiWidget);
            }
        }
        /// <summary>
        /// 移除元件
        /// </summary>
        public void RemoveUIWidget(string moduleName, string widgetName)
        {
            //如果已存在
            if (uiWidgets[moduleName].ContainsKey(widgetName))
            {
                //移除
                uiWidgets[moduleName].Remove(widgetName);
            }
            else
            {
                Debug.LogWarning("元件不存在");
            }
        }
        #endregion

        #region Find Widget
        /// <summary>
        /// 获取元件
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="widgetName"></param>
        /// <returns></returns>
        public UIWidgetBase FindWidget(string moduleName, string widgetName)
        {
            //如果不存在则注册模块
            RegisterUIModuleToUIWidgets(moduleName);
            //要返回的UIWidgetBase
            UIWidgetBase uiWidget = null;
            //尝试获取 没有则返回null
            uiWidgets[moduleName].TryGetValue(widgetName, out uiWidget);
            //返回结果
            return uiWidget;    
        }
        #endregion
    }
}
