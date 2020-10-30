using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Elie.Tools.Eyetracking_1
{
    public static class Heatmap
    {
        public static IEnumerator Generate(FocusDataRecord _record, HeatmapSettings _settings)
        {
            Stopwatch sw = new Stopwatch();

            if (_settings.extracts.Length > 0)
            {
                foreach (RecordTimespan extract in _settings.extracts)
                {
                    sw.Start();
                    yield return Generate(_record, _settings.radius, _settings.gradient, _settings.path, extract, _settings.settingsName);
                    sw.Stop();
                    Debug.Log("Heatmap generated in: " + (sw.ElapsedMilliseconds / 1000.0f));
                    sw.Reset();
                }
            }
            else
            {
                RecordTimespan timespan = new RecordTimespan(0.0f, _record.duration);

                sw.Start();
                yield return Generate(_record, _settings.radius, _settings.gradient, _settings.path, timespan, _settings.settingsName);
                sw.Stop();
                Debug.Log("Heatmap generated in: " + (sw.ElapsedMilliseconds / 1000.0f));
                sw.Reset();
            }
        }

        public static IEnumerator GenerateDataGrid(FocusDataRecord _record, HeatmapSettings _settings)
        {
            Stopwatch sw = new Stopwatch();

            if (_settings.extracts.Length > 0)
            {
                foreach (RecordTimespan extract in _settings.extracts)
                {
                    sw.Start();
                    yield return GenerateDataGrid(_record, _settings.radius, _settings.path, extract);
                    sw.Stop();
                    Debug.Log("Heatmap generated in: " + (sw.ElapsedMilliseconds / 1000.0f));
                    sw.Reset();
                }
            }
            else
            {
                RecordTimespan timespan = new RecordTimespan(0.0f, _record.duration);

                sw.Start();
                yield return GenerateDataGrid(_record, _settings.radius, _settings.path, timespan);
                sw.Stop();
                Debug.Log("Heatmap generated in: " + (sw.ElapsedMilliseconds / 1000.0f));
                sw.Reset();
            }
        }

        public static void Generate(HeatmapDataGrid _grid, HeatmapSettings _settings)
        {
            Generate(_grid, _settings.gradient, _settings.path, _settings.settingsName);
        }

        public static IEnumerator GenerateDataGrid(FocusDataRecord _record, int _radius, string _path, RecordTimespan _timespan)
        {
            float[,] values = new float[_record.screenWidth, _record.screenHeight];
            FocusData[] dataArray = _record.GetData(_timespan.startTimecode, _timespan.endTimecode);
            float maxValue = 0.0f;
            int security = 0;

            Debug.Log("Datagrid Generation : starting.");

            yield return null;

            foreach (FocusData data in dataArray)
            {
                Vector2Int dataPos = data.averagePosition;
                Vector2Int actualPos = default;

                if (dataPos.x + _radius < 0 || dataPos.x - _radius >= _record.screenWidth || dataPos.y + _radius < 0 || dataPos.y - _radius >= _record.screenHeight)
                    continue;

                for (int y = -_radius; y <= _radius; y++)
                {
                    for (int x = -_radius; x <= _radius; x++)
                    {
                        if (x * x + y * y <= _radius * _radius)
                        {
                            actualPos = new Vector2Int(dataPos.x + x, dataPos.y + y);

                            if (actualPos.x >= 0 && actualPos.x < _record.screenWidth && actualPos.y >= 0 && actualPos.y < _record.screenHeight)
                                values[actualPos.x, actualPos.y] += 1.0f - Mathf.InverseLerp(0.0f, _radius, new Vector2(x, y).magnitude);
                        }

                        security++;

                        if (security > 1000000)
                        {
                            security = 0;
                            Debug.Log("break id: " + data.index);

                            yield return null;
                        }
                    }
                }
            }

            Debug.Log("Datagrid Generation: grid values calculated.");

            yield return null;

            Debug.Log("Datagrid Generation: Calculating max value.");

            for (int y = 0; y < _record.screenHeight; y++)
            {
                for (int x = 0; x < _record.screenWidth; x++)
                {
                    if (values[x, y] > maxValue) maxValue = values[x, y];
                }
            }

            yield return null;

            Debug.Log("Datagrid Generation: All values calculated.");

            HeatmapDataGrid result = new HeatmapDataGrid(values, maxValue, _record, _timespan);

            yield return null;

            result.ExportDataGrid(_path);
        }

        public static void Generate(HeatmapDataGrid _dataGrid, Gradient _gradient, string _path, string _settingsName)
        {
            Color[] pixels = new Color[_dataGrid.width * _dataGrid.height];
            Texture2D texture = new Texture2D(_dataGrid.width, _dataGrid.height, TextureFormat.RGBA32, false);
            string filePath;

            for (int y = 0; y < _dataGrid.height; y++)
            {
                for (int x = 0; x < _dataGrid.width; x++)
                {
                    pixels[x + y * _dataGrid.width] = _gradient.Evaluate(Mathf.InverseLerp(0.0f, _dataGrid.max, _dataGrid.values[x, y]));
                }
            }

            texture.SetPixels(pixels);

            byte[] bytes = texture.EncodeToPNG();
            int index = -1;
            string timespan = Mathf.Approximately(_dataGrid.timespan.startTimecode, 0.0f) && Mathf.Approximately(_dataGrid.timespan.endTimecode, _dataGrid.recordDuration) ?
                              "" :
                              "_" + _dataGrid.timespan.ToString();

            if (!System.IO.Directory.Exists(_path)) System.IO.Directory.CreateDirectory(_path);

            do
            {
                index++;
                filePath = _path + "/" + _dataGrid.session.GetSessionPrefix() + "Eyetracking" + _settingsName + timespan + "_" + index.ToString() + ".png";
            } while (System.IO.File.Exists(filePath));

            System.IO.File.WriteAllBytes(filePath, bytes);

            Debug.Log("Heatmap successfully generated. File saved at: " + filePath); ;
        }

        public static IEnumerator Generate(FocusDataRecord _record, int _radius, Gradient _gradient, string _path, RecordTimespan _timespan, string _settingsName)
        {
            float[,] values = new float[_record.screenWidth, _record.screenHeight];
            float averageValue = 0.0f;
            float maxValue = 0.0f;
            int affectedPixelsAmount = 0;
            Color[] pixels = new Color[_record.screenWidth * _record.screenHeight];
            Texture2D texture = new Texture2D(_record.screenWidth, _record.screenHeight, TextureFormat.RGBA32, false);
            string filePath = "";
            int security = 0;
            int littleRadius = Mathf.RoundToInt(_radius / 10.0f);
            FocusData[] dataArray = _record.GetData(_timespan.startTimecode, _timespan.endTimecode);

            if (littleRadius == 0) littleRadius = 1;

            Debug.Log("creating heatmap: " + _record.screenWidth + "x" + _record.screenHeight);

            foreach (FocusData data in dataArray)
            {
                Vector2Int dataPos = data.averagePosition;
                Vector2Int actualPos = default;
                


                if (dataPos.x + _radius < 0 || dataPos.x - _radius >= _record.screenWidth || dataPos.y + _radius < 0 || dataPos.y - _radius >= _record.screenHeight)
                    continue;

                for (int y = -_radius; y <= _radius; y++)
                {
                    for (int x = -_radius; x <= _radius; x++)
                    {
                        if(x*x+y*y <= _radius*_radius)
                        {
                            actualPos = new Vector2Int(dataPos.x + x, dataPos.y + y);

                            if (actualPos.x >= 0 && actualPos.x < _record.screenWidth && actualPos.y >= 0 && actualPos.y < _record.screenHeight)
                                values[actualPos.x, actualPos.y] += 1.0f - Mathf.InverseLerp(0.0f, _radius, new Vector2(x, y).magnitude);
                        }

                        security++;
                    }
                }

                if(security > 1500000)
                {
                    security = 0;
                    yield return null;
                }
                
            }

            yield return null;


            for (int y = 0; y < _record.screenHeight; y++)
            {
                for (int x = 0; x < _record.screenWidth; x++)
                {
                    affectedPixelsAmount += values[x, y] == 0.0f ? 0 : 1;
                    averageValue += values[x, y];
                    if (values[x, y] > maxValue) maxValue = values[x, y];
                }
            }
            averageValue = (averageValue / (float)affectedPixelsAmount) * 3.0f;

            yield return null;

            for (int y = 0; y < _record.screenHeight; y++)
            {
                for (int x = 0; x < _record.screenWidth; x++)
                {
                    pixels[x + y * _record.screenWidth] = _gradient.Evaluate(Mathf.InverseLerp(0.0f, maxValue, values[x, y]));
                }
            }

            texture.SetPixels(pixels);

            byte[] bytes = texture.EncodeToPNG();
            int index = -1;
            string timespan = Mathf.Approximately(_timespan.startTimecode, 0.0f) && Mathf.Approximately(_timespan.endTimecode, _record.duration) ?
                              "" :
                              "_" + _timespan.ToString();

            if (!System.IO.Directory.Exists(_path)) System.IO.Directory.CreateDirectory(_path);

            do
            {
                index++;
                filePath = _path + "/" + _record.session.GetSessionPrefix() + "Eyetracking" + _settingsName + timespan + "_" + index.ToString() + ".png";
            } while (System.IO.File.Exists(filePath));

            System.IO.File.WriteAllBytes(filePath, bytes);

            Debug.Log("Heatmap successfully generated. File saved at: " + filePath); ;
        }
    }
}