using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Elie.Tools.Eyetracking_1
{
    public class VideoManager : MonoBehaviour
    {
        [SerializeField] private DataRecorder ETRecorder = default;
        [SerializeField] private VideoPlayer videoPlayer = default;

        [ContextMenu("Play")]
        public void Play()
        {
            StartCoroutine(PlayVideoRoutine());
        }

        private IEnumerator PlayVideoRoutine()
        {
            videoPlayer.Play();

            while (!videoPlayer.isPlaying)
            {
                yield return null;
            }

            Debug.Log("Video starts.");
            ETRecorder.Play();

            while (videoPlayer.isPlaying)
            {
                yield return null;
            }

            
            ETRecorder.Stop();
            ETRecorder.Save();
        }

        public void PlayVideo()
        {
            videoPlayer.Play();
        }
    }
}