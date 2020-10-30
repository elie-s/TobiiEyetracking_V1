using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

namespace Elie.Tools.Eyetracking_1
{
    public class DataRecorder : MonoBehaviour
    {
        [SerializeField] private DataRecorderSettings settings = default;
        [SerializeField] private SaveManager saveManager = default;

        private Buffer<Vector2> buffer;
        private List<FocusData> dataList;
        private IEnumerator recordRoutine;
        private bool isPlaying = false;

        [ContextMenu("Play")]
        public void Play()
        {
            if (isPlaying) Stop();

            recordRoutine = RecordRoutine();

            StartCoroutine(recordRoutine);
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            isPlaying = false;
            StopCoroutine(recordRoutine);
        }

        [ContextMenu("Save")]
        public void Save()
        {
            saveManager.SaveRecord(dataList.ToArray());
            dataList.Clear();
        }

        private void ResetRecording()
        {
            if (isPlaying) Stop();
            Save();
        }

        private IEnumerator RecordRoutine()
        {
            dataList = new List<FocusData>();
            buffer = new Buffer<Vector2>(settings.bufferSize);
            isPlaying = true;
            float time = 0.0f;

            while (!buffer.isFull)
            {
                if (TobiiAPI.GetGazePoint().IsValid) buffer.Add(TobiiAPI.GetGazePoint().Screen);

                yield return null;
            }

            while (true)
            {
                buffer.Add(TobiiAPI.GetGazePoint().Screen);
                dataList.Add(new FocusData(dataList.Count, time, buffer.values));

                time += Time.deltaTime;

                yield return null;
            }
        }
    }
}