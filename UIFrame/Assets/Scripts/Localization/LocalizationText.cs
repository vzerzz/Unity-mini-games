using UIFrame;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Localization
{
    [RequireComponent(typeof(Text))]
    public class LocalizationText:MonoBehaviour
    {
        private Text _text;
        [Header("不同语言的文本")]
        private string[] languageTexts;

        private void Awake()
        {
            _text = GetComponent<Text>();
            languageTexts = JsonDataManager.Instance.FindTextLocalization(name);
        }


        private void OnEnable()
        {

            //添加监听
            LocalizationManager.Instance.AddLocaliztionListener(SetLanguageText);
        }
        private void OnDisable()
        {
            //移除监听
            LocalizationManager.Instance.RemoveLocaliztionListener(SetLanguageText);
        }
        /// <summary>
        /// 设置本地化文本
        /// </summary>
        /// <param name="languageID"></param>
        public void SetLanguageText(int languageID)
        {
            _text.text = languageTexts[languageID];
        }
    }
}