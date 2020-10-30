using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    public class GazesManager : MonoBehaviour
    {
        [SerializeField] private GameObject gazePrefab = default;
        [SerializeField] private Transform parent = default;
        [SerializeField] private Gradient colors = default;

        private FocusDataRecord[] records;
        private GazeBehaviour[] gazeBehaviours;

        public void SetRecords(FocusDataRecord[] _records)
        {
            records = _records;
            gazeBehaviours = new GazeBehaviour[records.Length];

            for (int i = 0; i < records.Length; i++)
            {
                records[i] = _records[i];
                gazeBehaviours[i] = Instantiate(gazePrefab, parent).GetComponent<GazeBehaviour>();
                gazeBehaviours[i].Initialize(records[i].session.testerName, colors.Evaluate((float)i / (float)records.Length));
            }
        }

        public void SetGazesPositions(float _timecode)
        {
            for (int i = 0; i < records.Length; i++)
            {
                gazeBehaviours[i].SetPosition(records[i].GetDataByTimecode(_timecode).averagePosition);
            }
        }

        public void RemoveRecords()
        {
            records = new FocusDataRecord[0];

            foreach (GazeBehaviour gaze in gazeBehaviours)
            {
                Destroy(gaze.gameObject);
            }

            gazeBehaviours = new GazeBehaviour[0];
        }
    }
}