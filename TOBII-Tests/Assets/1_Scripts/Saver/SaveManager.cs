using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private SaveManagerSettings settings = default;
        [SerializeField] private Gradient heatmapGradient = default;

        public void SaveRecord(FocusData[] _record)
        {
            string path = PathHandler(settings.savePath);
            string fileName = FileNameHandler(path, settings.extension);
            string fileContent = "version: "+settings.version+"\n";

            for (int i = 0; i < _record.Length-1; i++)
            {
                fileContent += _record[i].ToString() + "\n";
            }

            fileContent += _record[_record.Length - 1].ToString();

            File.WriteAllText(path + "/" + fileName, fileContent);
            HeatmapGenerator.Generate(_record,Camera.main.pixelWidth, Camera.main.pixelHeight, 1, heatmapGradient, path, fileName.Replace(settings.extension, ""));

            Debug.Log("Record saved at: " + path);
        }

        private string PathHandler(string _path)
        {
            string result = _path;

            if (_path[1] != ':') result = Application.persistentDataPath + "/" + _path;
            if (!Directory.Exists(result)) Directory.CreateDirectory(result);

            result += "_"+DateTime.Now.ToString("yyyy-MM-dd");

            if (!Directory.Exists(result)) Directory.CreateDirectory(result);

            return result;
        }

        private string FileNameHandler(string _path, string _fileExtention)
        {
            int index = Directory.GetFiles(_path, "*." + _fileExtention).Length;

            return "EyetrackingRecord_"+index.ToString()+"."+_fileExtention;
        }


    }
}