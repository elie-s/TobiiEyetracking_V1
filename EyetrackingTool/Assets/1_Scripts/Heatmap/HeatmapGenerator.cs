using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    public class HeatmapGenerator : MonoBehaviour
    {
        [SerializeField] private HeatmapSettings[] hmSettings = default;
        [SerializeField] private HeatmapSettings datagridSettings = default;
        [SerializeField] private DataLoader dataLoader = default;

        [ContextMenu("Generate Heatmaps")]
        public void GenerateAllHeatmaps()
        {
            if (!dataLoader.loaded) dataLoader.LoadData();

            StartCoroutine(GenerateAllHeatmapsRoutine());
        }

        [ContextMenu("Generate DataGrids")]
        public void GenerateDataGrids()
        {
            if (!dataLoader.loaded) dataLoader.LoadData();

            StartCoroutine(GenerateDataGridsRoutine());
        }

        [ContextMenu("Generate First Datagrid")]
        public void GenerateDataGrid()
        {
            if (!dataLoader.loaded) dataLoader.LoadData();

            StartCoroutine(Heatmap.GenerateDataGrid(dataLoader.GetRecord(0), datagridSettings));
        }


        public IEnumerator GenerateAllHeatmapsRoutine()
        {
            FocusDataRecord[] records = dataLoader.GetRecords();

            Debug.Log(records.Length + " Heatmaps to create. Starting...");

            foreach (FocusDataRecord record in records)
            {
                foreach (HeatmapSettings settings in hmSettings)
                {
                    yield return StartCoroutine(Heatmap.Generate(record, settings));
                }
            }
        }

        [ContextMenu("GenerateAllHeatmapsFromDataGrid")]
        public void GenerateAllHeatmapsFromDataGrid()
        {
            if (!dataLoader.loaded) dataLoader.LoadData();

            HeatmapDataGrid[] grids = dataLoader.GetDataGrids();

            Debug.Log(grids.Length + " Heatmaps to create. Starting...");

            foreach (HeatmapDataGrid grid in grids)
            {
                foreach (HeatmapSettings settings in hmSettings)
                {
                    Heatmap.Generate(grid, settings);
                }
            }
        }

        private IEnumerator GenerateDataGridsRoutine()
        {
            FocusDataRecord[] records = dataLoader.GetRecords();

            Debug.Log(records.Length + " Grids to create. Starting...");

            foreach (FocusDataRecord record in records)
            {
                foreach (HeatmapSettings settings in hmSettings)
                {
                    yield return StartCoroutine(Heatmap.GenerateDataGrid(record, settings));
                }
            }
        }
    }
}