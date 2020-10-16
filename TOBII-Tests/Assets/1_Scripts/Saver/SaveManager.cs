using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private SaveManagerSettings settings = default;

        public void SaveRecord(FocusData[] _record)
        {

        }

        private string PathHandler(string _path)
        {
            string result = _path;

            if (_path[1] != ':') result = Application.persistentDataPath + "/" + _path;
            if (!Directory.Exists(result)) Directory.CreateDirectory(result);

            return result;
        }


    }
}