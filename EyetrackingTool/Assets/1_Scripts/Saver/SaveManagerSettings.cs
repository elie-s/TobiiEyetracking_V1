using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    [CreateAssetMenu(menuName = "Tools/Eyetracking V1/Save settings")]
    public class SaveManagerSettings : ScriptableObject
    {
        public string savePath = "Records";
        public string version = "0.1";

        public string GetPath()
        {
            string result = savePath;

            if (savePath[1] != ':') result = Application.persistentDataPath + "/" + savePath;
            if (!Directory.Exists(result)) Directory.CreateDirectory(result);

            result += savePath[savePath.Length - 1] == '/' || savePath[savePath.Length - 1] == '\\' ? "" : "/";
            result += System.DateTime.Now.ToString("yyyy-MM-dd") + "/";

            if (!Directory.Exists(result)) Directory.CreateDirectory(result);

            return result;
        }
    }
}