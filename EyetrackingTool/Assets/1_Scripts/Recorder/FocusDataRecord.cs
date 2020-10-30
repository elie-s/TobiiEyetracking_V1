using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    public struct FocusDataRecord
    {
        public FocusData[] data;
        public int screenWidth;
        public int screenHeight;
        public string version;
        public SessionInfo session;
        public float duration;

        public FocusDataRecord(FocusData[] _data, int _screenWidth, int _screenHeight, string _version, SessionInfo _session)
        {
            data = _data;
            screenWidth = _screenWidth;
            screenHeight = _screenHeight;
            version = _version;
            session = _session;
            if (_data != null) duration = _data[_data.Length - 1].time;
            else duration = -1.0f;
        }

        public void ExportRecord(string _path, bool _forceWrite = false)
        {

            string fileContent = "Version: " + version + "\n"
                + "Screen Resolution: " + screenWidth + "x" + screenHeight + "\n"
                + session.ToString() + "\n";

            for (int i = 0; i < data.Length - 1; i++)
            {
                fileContent += data[i].ToString() + "\n";
            }

            fileContent += data[data.Length - 1].ToString();

            string path = _path + (_path[_path.Length - 1] == '/' || _path[_path.Length - 1] == '\\' ? "" : "/");
            string fileName = session.GetSessionPrefix() + "EyetrackingData" + "_";
            int index = 0;

            while (File.Exists(path+fileName+index+".etr"))
            {
                index++;
            }

            path = path + fileName + index + ".etr";

            File.WriteAllText(path, fileContent);

            Debug.Log("Record saved at: " + path);
        }

        public FocusData[] GetData(float _startTimecode, float _endTimecode)
        {
            List<FocusData> result = new List<FocusData>();

            foreach (FocusData focusData in data)
            {
                if(focusData.time >= _startTimecode)
                {
                    if (focusData.time > _endTimecode) break;

                    result.Add(focusData);
                }
            }

            return result.ToArray();
        }

        public FocusData GetDataByTimecode(float _timecode)
        {
            if (_timecode <= 0.0f) return data[0];
            if (_timecode >= duration) return data[data.Length];

            int index = Mathf.FloorToInt((_timecode / duration) * (float)data.Length);

            if (index < 0) index = 0;
            if (index > data.Length - 1) index = data.Length - 1;

            while (data[index].time > _timecode || _timecode > data[index+1].time)
            {
                if (data[index].time > _timecode) index--;
                else if (data[index + 1].time < _timecode) index++;

                if (index < 0)
                {
                    index = 0;
                    break;
                }
                if (index > data.Length - 1)
                {
                    index = data.Length - 1;
                    break;
                }
            }

            return data[index];
        }

        public static FocusDataRecord LoadRecord(string _path)
        {
            if (!File.Exists(_path))
            {
                Debug.LogWarning("File not found. Check the path and the name of the file: " + _path);
                throw new System.Exception();
            }

            string[] fileContent = File.ReadAllLines(_path);
            string version = fileContent[0].Replace("Version: ", "");
            string resolutionText = fileContent[1].Replace("Screen Resolution: ", "");
            Vector2Int resolution = new Vector2Int(int.Parse(resolutionText.Split('x')[0]), int.Parse(resolutionText.Split('x')[1]));
            SessionInfo session = SessionInfo.Decypher(fileContent[2]);
            FocusData[] data = new FocusData[fileContent.Length - 3];

            for (int i = 3; i < fileContent.Length; i++)
            {
                data[i - 3] = FocusData.Decypher(fileContent[i]);
            }

            Debug.Log("Data loaded successfully.");

            return new FocusDataRecord(data, resolution.x, resolution.y, version, session);
        }
    }
}