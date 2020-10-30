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
        private List<FocusData> record;
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
            saveManager.SaveRecord(record.ToArray());
            record.Clear();
        }

        private void ResetRecording()
        {
            if (isPlaying) Stop();
            Save();
        }

        private IEnumerator RecordRoutine()
        {
            record = new List<FocusData>();
            buffer = new Buffer<Vector2>(settings.bufferSize);
            isPlaying = true;

            while (!buffer.isFull)
            {
                if (TobiiAPI.GetGazePoint().IsValid) buffer.Add(TobiiAPI.GetGazePoint().Screen);

                yield return null;
            }

            while (true)
            {
                buffer.Add(TobiiAPI.GetGazePoint().Screen);
                record.Add(new FocusData(record.Count, buffer.values));

                yield return null;
            }
        }
    }
}