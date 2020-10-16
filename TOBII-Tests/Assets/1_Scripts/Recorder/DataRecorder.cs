using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

namespace Elie.Tools.Eyetracking_1
{
    public class DataRecorder : MonoBehaviour
    {
        [SerializeField] private DataRecorderSettings settings = default;

        private Buffer<Vector2> buffer;
        private List<FocusData> record;
        private IEnumerator recordRoutine;

        [ContextMenu("Play")]
        public void Play()
        {
            if (recordRoutine != null) Stop();

            recordRoutine = RecordRoutine();

            StartCoroutine(recordRoutine);
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            StopCoroutine(recordRoutine);
        }

        private IEnumerator RecordRoutine()
        {
            record = new List<FocusData>();
            buffer = new Buffer<Vector2>(settings.bufferSize);

            while (!buffer.isFull)
            {
                if (TobiiAPI.GetGazePoint().IsValid) buffer.Add(TobiiAPI.GetGazePoint().Screen);

                yield return null;
            }

            while (true)
            {
                buffer.Add(TobiiAPI.GetGazePoint().Screen);
                record.Add(new FocusData(record.Count, buffer.values));
            }
        }
    }
}