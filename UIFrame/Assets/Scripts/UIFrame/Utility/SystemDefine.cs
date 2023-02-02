namespace UIFrame
{ 
    public static class SystemDefine
    {
        #region Configuration Path
        public const string PanelConfigPath = "Configuration/UIPanelConfig";
        public const string LocalizationConfigPath = "Configuration/UILanguageTextConfig";
        #endregion

        #region Scene ID

        public enum SceneID
        {
            MainScene = 0,
            FightScene = 1
        }

        #endregion

        #region Widget Token

        public static string[] WIDGET_TOKENS = new string[] {"_F","_S","_T"};

        #endregion
    }
}