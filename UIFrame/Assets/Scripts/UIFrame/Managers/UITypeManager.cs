using UIFrame;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrame
{
    public class UITypeManager : Singleton<UITypeManager>
    {
        private UITypeManager()
        {
            _uiTypes = new Dictionary<string, UIType>();
        }
        //UIType缓存池
        private Dictionary<string, UIType> _uiTypes;
        /// <summary>
        /// 通过UIPanelName名称获取UIType
        /// </summary>
        /// <param name="uiPanelName"></param>
        public UIType GetUIType(string uiPanelName)
        {
            UIType uiType = null;
            if(!_uiTypes.TryGetValue(uiPanelName, out uiType))
            {
                uiType = new UIType(JsonDataManager.Instance.FindPanelPath(uiPanelName));
                _uiTypes.Add(uiPanelName, uiType);
            }
            return uiType;
        }
    }
}
