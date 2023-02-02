namespace UIFrame
{
    [System.Serializable]
    public class JsonPanelsModel
    {
        public SceneDataModel[] AllData;
    }
    [System.Serializable]
    public class SceneDataModel
    {
        public string SceneName;
        public PanelDataModel[] Data;
    }
    [System.Serializable]
    public class PanelDataModel
    {
        public string PanelName;
        public string PanelPath;
    }

}