using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    public class DataLoader : MonoBehaviour
    {
        [SerializeField] private string[] filesPath = default;
        [SerializeField] private string[] dataLoaded = default;

        private FocusDataRecord[] records;
        private HeatmapDataGrid[] dataGrids;
        
        public bool loaded { get; private set; }

        [ContextMenu("Load")]
        public void LoadData()
        {
            List<FocusDataRecord> recordsList = new List<FocusDataRecord>();
            List<HeatmapDataGrid> dataGrisList = new List<HeatmapDataGrid>();
            int amount = 0;
            
            loaded = true;

            foreach (string filePath in filesPath)
            {
                string path = filePath.Replace("\\", "/");
                string pathEnd = path.Split('/')[path.Split('/').Length - 1];
                if (pathEnd.Contains("."))
                {
                    if(!File.Exists(filePath))
                    {
                        Debug.LogWarning("File does not exists: " + filePath);
                        continue;
                    }

                    if (pathEnd.Split('.')[1] != "etr" && pathEnd.Split('.')[1] != "hdg")
                    {
                        Debug.LogWarning("Invalid file format: " + filePath);
                        continue;
                    }

                    if(pathEnd.Split('.')[1] == "etr")
                        recordsList.Add(FocusDataRecord.LoadRecord(filePath));
                    if (pathEnd.Split('.')[1] == "hdg")
                        dataGrisList.Add(HeatmapDataGrid.LoadFromFile(filePath));

                    amount++;
                }
                else
                {
                    if(!Directory.Exists(filePath))
                    {
                        Debug.LogWarning("Directory does not exists: " + filePath);
                        continue;
                    }

                    string[] subPathsRecords = Directory.GetFiles(filePath, "*.etr");
                    string[] subPathsGrids = Directory.GetFiles(filePath, "*.hdg");

                    if(subPathsRecords.Length == 0 && subPathsGrids.Length == 0)
                    {
                        Debug.LogWarning("No compatible files in the directory: " + filePath);
                        continue;
                    }

                    foreach (string file in subPathsRecords)
                    {
                        recordsList.Add(FocusDataRecord.LoadRecord(file));
                        amount++;
                    }

                    foreach (string file in subPathsGrids)
                    {
                        dataGrisList.Add(HeatmapDataGrid.LoadFromFile(file));
                        amount++;
                    }
                }
            }

            if (amount == 0) Debug.Log("Couldn't load any compatible files.");
            Debug.Log(amount + " file" + (amount > 1 ? "s" : "") + " successfully loaded.");

            records = recordsList.ToArray();
            dataGrids = dataGrisList.ToArray();
            dataLoaded = new string[records.Length];

            for (int i = 0; i < records.Length; i++)
            {
                dataLoaded[i] = records[i].session.ToString().Replace("Session: ", "");
            }
        }

        public void UnloadData()
        {
            records = new FocusDataRecord[0];
            dataGrids = new HeatmapDataGrid[0];
            dataLoaded = new string[0];

            loaded = false;
        }

        public FocusDataRecord[] GetRecords() => records;
        public FocusDataRecord GetRecord(int _index) => records[_index];
        public HeatmapDataGrid[] GetDataGrids() => dataGrids;
        public HeatmapDataGrid GetDatagrid(int _index) => dataGrids[_index];
    }
}