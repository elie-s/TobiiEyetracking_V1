using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    public class ChronologyManager : MonoBehaviour
    {
        [SerializeField] private GazesManager gazesManager = default;
        [SerializeField] private VideoManager videoManager = default;
        [SerializeField] private DataLoader dataLoader = default;
        [SerializeField] private float maxTime = 180.0f;
        [SerializeField, Range(0.0f, 300.0f)] private float time = 0.0f;

        private bool isPaused;
        private bool isPlaying;
        private IEnumerator mainRoutine;

        [ContextMenu("Play")]
        public void Play()
        {
            if (isPlaying) StopCoroutine(mainRoutine);

            mainRoutine = PlayChronology(0.0f, maxTime);
            StartCoroutine(mainRoutine);
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            if (isPlaying) StopCoroutine(mainRoutine);
            isPlaying = false;
            isPaused = false;
            gazesManager.RemoveRecords();
        }

        [ContextMenu("Pause")]
        public void Pause()
        {
            if (!isPlaying) return;

            isPaused = !isPaused;
        }

        private IEnumerator PlayChronology(float _startAt, float _duration)
        {
            if (!dataLoader.loaded) dataLoader.LoadData();
            gazesManager.SetRecords(dataLoader.GetRecords());
            time = _startAt;
            isPaused = false;
            isPlaying = true;

            //Make video start here
            videoManager.PlayVideo();

            while (time < _duration)
            {
                while (isPaused)
                {
                    gazesManager.SetGazesPositions(time);

                    yield return null;
                }

                gazesManager.SetGazesPositions(time);
                time += Time.deltaTime;

                yield return null;
            }

            isPlaying = false;
            gazesManager.RemoveRecords();
        }
    }
}