using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    [CreateAssetMenu(menuName = "Tools/Eyetracking V1/Save settings")]
    public class SaveManagerSettings : ScriptableObject
    {
        public string savePath = "Records";
        public string extension = "er";

    }
}