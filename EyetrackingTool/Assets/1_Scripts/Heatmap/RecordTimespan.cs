using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    [System.Serializable]
    public struct RecordTimespan
    {
        public float startTimecode;
        public float endTimecode;

        public RecordTimespan(float _start, float _end)
        {
            startTimecode = _start;
            endTimecode = _end;
        }

        public string EncodeToString()
        {
            return startTimecode + ":" + endTimecode;
        }

        public static RecordTimespan Decypher(string _value)
        {
            string[] values = _value.Split(':');
            return new RecordTimespan(float.Parse(values[0]), float.Parse(values[1]));
        }

        public override string ToString()
        {
            string result = "[M1mS1s-M2mS2s]";
            int startMinutes = Mathf.FloorToInt(startTimecode / 60.0f);
            int startSecondes = Mathf.FloorToInt(startTimecode - startMinutes);
            int endMinutes = Mathf.FloorToInt(endTimecode / 60.0f);
            int endSecondes = Mathf.FloorToInt(endTimecode - endMinutes);

            result = result.Replace("M1", startMinutes > 9 ? startMinutes.ToString() : "0" + startMinutes);
            result = result.Replace("S1", startSecondes > 9 ? startSecondes.ToString() : "0" + startSecondes);
            result = result.Replace("M2", endMinutes > 9 ? endMinutes.ToString() : "0" + endMinutes);
            result = result.Replace("S2", endSecondes > 9 ? endSecondes.ToString() : "0" + endSecondes);

            return result;
        }
    }
}