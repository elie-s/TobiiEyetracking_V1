using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    public struct HeatmapDataGrid
    {

        public float[,] values;
        public float max;
        public float recordDuration;
        public int width;
        public int height;
        public string version;
        public SessionInfo session;
        public RecordTimespan timespan;

        public HeatmapDataGrid(float[,] _values, float _max, float _recordDuration, int _width, int _height, string _version, SessionInfo _session, RecordTimespan _timespan)
        {
            values = _values;
            max = _max;
            recordDuration = _recordDuration;
            width = _width;
            height = _height;
            version = _version;
            session = _session;
            timespan = _timespan;
        }

        public HeatmapDataGrid(float[,] _values,float _max, FocusDataRecord _record, RecordTimespan _timespan)
        {
            values = _values;
            max = _max;
            recordDuration = _record.duration;
            width = _record.screenWidth;
            height = _record.screenHeight;
            version = _record.version;
            session = _record.session;
            timespan = _timespan;
        }

        public void ExportDataGrid(string _path)
        {
            List<string> lines = new List<string>();

            lines.Add("Version: " + version);
            lines.Add("Screen Resolution: " + width + "x" + height);
            lines.Add(session.ToString());
            lines.Add("Max: " + max);
            lines.Add("Record duration: " + recordDuration);
            lines.Add(timespan.ToString() + "|" + timespan.EncodeToString());

            for (int y = 0; y < height; y++)
            {
                string line = "";

                for (int x = 0; x < width ; x++)
                {
                    line += values[x, y] + "|";
                }

                lines.Add(line);
            }

            string[] fileContent = lines.ToArray();
            string path = _path + (_path[_path.Length - 1] == '/' || _path[_path.Length - 1] == '\\' ? "" : "/");
            string fileName = session.GetSessionPrefix() + "EyetrackingDataGrid" + "_";
            int index = 0;

            while (File.Exists(path + fileName + index + ".hdg"))
            {
                index++;
            }

            path = path + fileName + index + ".hdg";

            using (StreamWriter file = new StreamWriter(path))
            {
                foreach (string line in fileContent)
                {
                    file.WriteLine(line);
                }
            }

            Debug.Log("DataGrid saved at: " + path);

        }

        //public void ExportDataGrid(string _path)
        //{
        //    string fileContent = "Version: " + version + "\n"
        //        + "Screen Resolution: " + width + "x" + height + "\n"
        //        + session.ToString() + "\n"
        //        + "Max: " + max + "\n"
        //        + "Record duration: " + recordDuration + "\n"
        //        + timespan.ToString() + "|" + timespan.EncodeToString() + "\n";

        //    for (int y = 0; y < height; y++)
        //    {
        //        for (int x = 0; x < width - 1; x++)
        //        {
        //            fileContent += values[x, y] + "|";
        //        }
        //        fileContent += values[width - 1, y] + "\n";
        //    }

        //    string path = _path + (_path[_path.Length - 1] == '/' || _path[_path.Length - 1] == '\\' ? "" : "/");
        //    string fileName = session.GetSessionPrefix() + "EyetrackingDataGrid" + "_";
        //    int index = 0;

        //    while (File.Exists(path + fileName + index + ".hdg"))
        //    {
        //        index++;
        //    }

        //    path = path + fileName + index + ".hdg";

        //    using (StreamWriter file = new StreamWriter(path))
        //    {

        //    }

        //    File.WriteAllText(path, fileContent);

        //    Debug.Log("DataGrid saved at: " + path);

        //}

        public static HeatmapDataGrid LoadFromFile(string _path)
        {
            if (!File.Exists(_path))
            {
                Debug.LogWarning("File not found. Check the path and the name of the file.");
                throw new System.Exception();
            }

            string[] fileContent = File.ReadAllLines(_path);
            string version = fileContent[0].Replace("Version: ", "");
            string resolutionText = fileContent[1].Replace("Screen Resolution: ", "");
            Vector2Int resolution = new Vector2Int(int.Parse(resolutionText.Split('x')[0]), int.Parse(resolutionText.Split('x')[1]));
            SessionInfo session = SessionInfo.Decypher(fileContent[2]);
            float max = float.Parse(fileContent[3].Replace("Max: ", ""));
            float recordDuration = float.Parse(fileContent[4].Replace("Record duration: ", ""));
            RecordTimespan timespan = RecordTimespan.Decypher(fileContent[5].Split('|')[1]);
            float[,] values = LoadData(fileContent, resolution.x, resolution.y, 6);

            Debug.Log("Data loaded successfully.");

            return new HeatmapDataGrid(values, max, recordDuration, resolution.x, resolution.y, version, session, timespan);
        }

        public static float[,] LoadData(string[] _dataLines, int _width, int _height, int _startingIndex)
        {
            float[,] result = new float[_width, _height];

            for (int y = _startingIndex; y < _dataLines.Length; y++)
            {
                string[] dataLine = _dataLines[y].Split('|');

                for (int x = 0; x < _width; x++)
                {
                    result[x, y - _startingIndex] = float.Parse(dataLine[x]);
                }
            }

            return result;
        }

    }
}