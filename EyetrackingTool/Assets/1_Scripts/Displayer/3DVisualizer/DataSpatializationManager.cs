using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    public class DataSpatializationManager : MonoBehaviour
    {
        [SerializeField] private GameObject dataPrefab = default;
        [SerializeField] private DataLoader dataLoader = default;
        [SerializeField] private Transform parent = default;
        [SerializeField] private float scale = 0.01f;
        [SerializeField] private int indexToGenerate = 0;
        [SerializeField] private Gradient colors = default;

        [ContextMenu("Generate from index")]
        public void GenerateByIndex()
        {
            if (!dataLoader.loaded) dataLoader.LoadData();

            GenerateRecord(dataLoader.GetRecord(indexToGenerate));
        }

        [ContextMenu("Generate all")]
        public void GenerateAll()
        {
            if (!dataLoader.loaded) dataLoader.LoadData();
            FocusDataRecord[] records = dataLoader.GetRecords();

            for (int i = 0; i < records.Length; i++)
            {
                dataPrefab.GetComponent<SpriteRenderer>().color = colors.Evaluate((float)i / (float)records.Length);
                GenerateRecord(records[i]);
            }
        }

        public void GenerateRecord(FocusDataRecord _record)
        {
            foreach (FocusData data in _record.data)
            {
                InstantiateData(data);
            }
        }

        private void InstantiateData(FocusData _data)
        {
            Vector3 pos = new Vector3(_data.averagePosition.x, _data.averagePosition.y, _data.index) * scale;
            Instantiate(dataPrefab, pos, Quaternion.identity, parent);
        }
    }
}