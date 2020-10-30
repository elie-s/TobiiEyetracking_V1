using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    public struct FocusData
    {
        public readonly int index;
        public readonly float time;
        public readonly int[][] coordinates;
        public Vector2Int averagePosition;

        public FocusData(int _index, float _time, Vector2[] _coordinates)
        {
            index = _index;
            time = _time;
            coordinates = new int[_coordinates.Length][];

            for (int i = 0; i < _coordinates.Length; i++)
            {
                coordinates[i] = new int[2];
                coordinates[i][0] = Mathf.RoundToInt(_coordinates[i].x);
                coordinates[i][1] = Mathf.RoundToInt(_coordinates[i].y);
            }

            averagePosition = default;
            averagePosition = AverageInt();
        }

        public FocusData(int _index, float _time, Vector2Int _average, int[][] _coordinates)
        {
            index = _index;
            time = _time;
            coordinates = _coordinates;
            averagePosition = _average;
        }

        public Vector2 Average()
        {
            Vector2 result = Vector2.zero;

            for (int i = 0; i < coordinates.Length; i++)
            {
                result += new Vector2(coordinates[i][0], coordinates[i][1]);
            }

            return result / coordinates.Length;
        }

        public Vector2Int AverageInt()
        {
            Vector2 average = Average();

            return new Vector2Int(Mathf.FloorToInt(average.x), Mathf.FloorToInt(average.y));
        }

        public override string ToString()
        {
            string coord = "";

            for (int i = 0; i < coordinates.Length; i++)
            {
                coord += "(" + coordinates[i][0] + "," + coordinates[i][1] + ")";
            }

            return index.ToString() + "|" + time.ToString() + "|("+averagePosition.x.ToString()+","+averagePosition.y.ToString()+")|" + coord;
        }

        public static FocusData Decypher(string _value)
        {
            int index = 0;
            float time = 0.0f;
            Vector2Int average = default;
            int[][] coordinates;
            string[] coordValues = new string[2];

            string[] split = _value.Split('|');

            index = int.Parse(split[0]);
            time = float.Parse(split[1]);

            split[2] = split[2].Replace("(", "");
            split[2] = split[2].Replace(")", "");
            average = new Vector2Int(int.Parse(split[2].Split(',')[0]), int.Parse(split[2].Split(',')[1]));

            split[3] = split[3].Replace(")(", "|");
            split[3] = split[3].Replace(")", "");
            split[3] = split[3].Replace("(", "");
            split = split[3].Split('|');

            coordinates = new int[split.Length][];

            for (int i = 0; i < split.Length; i++)
            {
                coordValues = split[i].Split(',');
                coordinates[i] = new int[] { int.Parse(coordValues[0]), int.Parse(coordValues[1]) };
            }

            return new FocusData(index, time, average, coordinates);
        }
    }
}