using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    [CreateAssetMenu(menuName ="Tools/Eyetracking V1/Recorder settings")]
    public class DataRecorderSettings : ScriptableObject
    {
        public int bufferSize = 3;
    }
}