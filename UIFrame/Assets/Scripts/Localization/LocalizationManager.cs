using UnityEditor;
using UnityEngine;
using UIFrame;

namespace Localization
{
    public enum SupportLanguage
    {
        SimpleChinese,
        English
    }
    public class LocalizationManager:Singleton<LocalizationManager>
    {
        private LocalizationManager()
        {

        }

        public void LocalizationInit()
        {
            int id = PlayerPrefs.GetInt("LanguageID");
            ChangeLanguage((SupportLanguage)id);
        }
        /// <summary>
        /// 所有存储方法的委托对象
        /// </summary>
        private SupportLanguage _supportLanguage;
        private System.Action<int> localizationEventHandle;
        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="action"></param>
        public void AddLocaliztionListener(System.Action<int> action)
        {
            localizationEventHandle += action;
        }
        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="action"></param>
        public void RemoveLocaliztionListener(System.Action<int> action)
        {
            localizationEventHandle -= action;
        }
        /// <summary>
        /// 更换语言
        /// </summary>
        /// <param name="_supportLanguage"></param>
        public void ChangeLanguage(SupportLanguage _supportLanguage)
        {
            localizationEventHandle((int)_supportLanguage);
            PlayerPrefs.SetInt("LanguageID",(int)_supportLanguage);  
        }

    }
}