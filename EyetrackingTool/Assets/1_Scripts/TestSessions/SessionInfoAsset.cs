using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    [CreateAssetMenu(menuName = "Tools/EyetrackingV1/Session Info Asset")]
    public class SessionInfoAsset : ScriptableObject
    {
        public SessionInfo sessionInfo = default;
    }
}