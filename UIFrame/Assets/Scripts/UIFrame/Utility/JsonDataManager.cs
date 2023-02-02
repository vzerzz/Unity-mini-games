using UnityEngine;
using System.Collections.Generic;
using UIFrame;

namespace UIFrame
{
    public class JsonDataManager : Singleton<JsonDataManager>
    {
        private JsonDataManager()
        {
            panelDataDic = new Dictionary<int, Dictionary<string, string>>();
            localizationDic = new Dictionary<int, Dictionary<string, string[]>>();
            ParsePanelData();
            ParseLocalizationData();
        }
        #region Saved Structure
        //Panel解析后的数据
        private JsonPanelsModel panelData;
        //Localization解析后的数据
        private JsonLocalizationModel localizationData;
        //解析后的数据字典版
        private Dictionary<int, Dictionary<string, string>> panelDataDic;
        //解析后的语言数据字典版
        private Dictionary<int, Dictionary<string, string[]>> localizationDic;
        #endregion

        /// <summary>
        /// json解析panel配置文件
        /// </summary>
        private void ParsePanelData()
        {
            //获取配置文本资源
            TextAsset panelConfig = AssetsManager.Instance.GetAsset(SystemDefine.PanelConfigPath)as TextAsset;
            //将Panel配置文件进行解析
            panelData = JsonUtility.FromJson<JsonPanelsModel>(panelConfig.text);
            //将panelData转换为方便检索的字典
            for(int i = 0; i < panelData.AllData.Length; i++)
            {
                Dictionary<string, string> crtDic = new Dictionary<string, string>();
                //添加一个场景ID一个字典
                panelDataDic.Add(i,crtDic);
                for(int j = 0; j < panelData.AllData[i].Data.Length; j++)
                {
                    //以PanelName为Key PanelPath为value存储
                    crtDic.Add(panelData.AllData[i].Data[j].PanelName, panelData.AllData[i].Data[j].PanelPath);
                }
            }
        }
        /// <summary>
        /// Json解析本地化配置文件
        /// </summary>
        private void ParseLocalizationData()
        {
            //获取配置文本资源
            TextAsset LocalizationConfig = AssetsManager.Instance.GetAsset(SystemDefine.LocalizationConfigPath) as TextAsset;
            //将Panel配置文件进行解析
            localizationData = JsonUtility.FromJson<JsonLocalizationModel>(LocalizationConfig.text);
            //将panelData转换为方便检索的字典
            for (int i = 0; i < localizationData.AllData.Length; i++)
            {
                Dictionary<string, string[]> crtDic = new Dictionary<string, string[]>();
                //添加一个场景ID一个字典
                localizationDic.Add(i, crtDic);
                for (int j = 0; j < localizationData.AllData[i].Data.Length; j++)
                {
                    //以PanelName为Key PanelPath为value存储
                    crtDic.Add(localizationData.AllData[i].Data[j].TextObjName, localizationData.AllData[i].Data[j].TextLanguageText);
                }
            }
        }
        /// <summary>
        /// 通过Panel名称返回Panel路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string FindPanelPath(string panelName,int sceneID = (int)SystemDefine.SceneID.MainScene)
        {
            if(!panelDataDic.ContainsKey(sceneID))
                return null;
            if (!panelDataDic[sceneID].ContainsKey(panelName))
                return null;
            return panelDataDic[sceneID][panelName];
        }
        /// <summary>
        /// 通过对象名称返回本地化数据的数组
        /// </summary>
        /// <param name="textObjName"></param>
        /// <param name="sceneID"></param>
        /// <returns></returns>
        public string[] FindTextLocalization(string textObjName, int sceneID = (int)SystemDefine.SceneID.MainScene)
        {
            if (!localizationDic.ContainsKey(sceneID))
                return null;
            if (!localizationDic[sceneID].ContainsKey(textObjName))
                return null;
            return localizationDic[sceneID][textObjName];
        }
    }

}
