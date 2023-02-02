using UnityEngine;

namespace UIFrame
{
    public class UIType
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public UIType(string path)
        {
            Path = path;
            //Debug.Log(path);
            Name = path.Substring(path.LastIndexOf('/')+1);
        }
    }
}
