using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    [System.Serializable]
    public struct SessionInfo
    {
        public SessionTestType sessionType;
        public string testID;
        public string testerName;

        public SessionInfo(SessionTestType _type, string _id, string _tester)
        {
            sessionType = _type;
            testID = _id;
            testerName = _tester;
        }

        public string GetSessionPrefix()
        {
            return FormatSessionType(sessionType) + testID + "_" + testerName + "_";
        }

        public override string ToString()
        {
            return "Session: " + FormatSessionType(sessionType) + testID + " - " + testerName;
        }

        private static string FormatSessionType(SessionTestType _type)
        {
            switch (_type)
            {
                case SessionTestType.FeatureTest:
                    return "SFT";
                case SessionTestType.MultipleFeaturesTest:
                    return "MFT";
                case SessionTestType.SpectatorTest:
                    return "SPT";
                case SessionTestType.UserExperienceTest:
                    return "UXT";
                case SessionTestType.SmokeTest:
                    return "SMT";
                case SessionTestType.GameplayTest:
                    return "GPT";
                case SessionTestType.Playtest:
                    return "PLT";
                case SessionTestType.ToolTest:
                    return "TTT";
                default:
                    return "TTT";
            }
        }

        private static SessionTestType GetSessionType(string _type)
        {
            switch (_type)
            {
                case "SFT":
                    return SessionTestType.FeatureTest;
                case "MFT":
                    return SessionTestType.MultipleFeaturesTest;
                case "SPT":
                    return SessionTestType.SpectatorTest;
                case "UXT":
                    return SessionTestType.UserExperienceTest;
                case "SMT":
                    return SessionTestType.SmokeTest;
                case "GPT":
                    return SessionTestType.GameplayTest;
                case "PLT":
                    return SessionTestType.Playtest;
                case "TTT":
                    return SessionTestType.ToolTest;        
                default:
                    return SessionTestType.ToolTest;
            }
        }

        public static SessionInfo DecypherPrefix(string _value)
        {
            SessionTestType type = GetSessionType(_value.Remove(3, _value.Length - 3));

            string tmp = _value;
            tmp = tmp.Remove(3, tmp.Length - 3);

            string id = tmp.Split('_')[0];
            string tester = tmp.Split('_')[1];

            return new SessionInfo(type, id, tester);
        }

        public static SessionInfo Decypher(string _value)
        {
            string tmp = _value.Replace("Session: ", "");

            SessionTestType type = GetSessionType(_value.Remove(3, tmp.Length - 3));

            tmp = tmp.Remove(0, 3);

            string id = tmp.Split(' ')[0];
            string tester = tmp.Split(' ')[2];

            return new SessionInfo(type, id, tester);
        }
    }
}