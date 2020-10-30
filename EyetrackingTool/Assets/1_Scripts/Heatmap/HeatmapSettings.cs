using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    [CreateAssetMenu(menuName ="Tools/Eyetracking_V1/Heatmap Settings")]
    public class HeatmapSettings : ScriptableObject
    {
        public Gradient gradient = default;
        public int radius = default;
        public string path = default;
        public string settingsName = "Heatmap";
        public RecordTimespan[] extracts;
    }
}