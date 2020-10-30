using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    public static class HeatmapGenerator
    {
        public static void Generate(FocusData[] _data, int _screenWidth, int _screenHeight, int radius, Gradient _gradient, string _path, string _fileName)
        {
            float[,] values = new float[_screenWidth,_screenHeight];
            float maxValue = 0.0f;
            Color[] pixels = new Color[_screenWidth * _screenHeight];
            Texture2D texture = new Texture2D(_screenWidth, _screenHeight, TextureFormat.RGB24, false);

            Debug.Log("creating heatmap: " + _screenWidth + "x" + _screenHeight);

            foreach (FocusData data in _data)
            {
                Vector2Int dataPos = data.AverageInt();
                Debug.Log("pos: " + dataPos.x + "," + dataPos.y);
                values[dataPos.x, dataPos.y] += 1.0f;
                
                if (values[dataPos.x, dataPos.y] > maxValue) maxValue = values[dataPos.x, dataPos.y];
            }

            for (int y = 0; y < _screenHeight; y++)
            {
                for (int x = 0; x < _screenWidth; x++)
                {
                    pixels[x + y * _screenHeight] = _gradient.Evaluate(Mathf.InverseLerp(0.0f, maxValue, values[x, y]));
                }
            }

            texture.SetPixels(pixels);

            byte[] bytes = texture.EncodeToPNG();

            System.IO.File.WriteAllBytes(_path + "/" + _fileName + ".png", bytes);
        }
    }
}