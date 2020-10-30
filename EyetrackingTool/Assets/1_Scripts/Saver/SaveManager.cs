using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

namespace Elie.Tools.Eyetracking_1
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private SaveManagerSettings settings = default;
        [SerializeField] private SessionInfoAsset session = default;
        [SerializeField] private RectTransform screen = default;

        public void SaveRecord(FocusData[] _data)
        {
            string path = settings.GetPath();
            FocusDataRecord record = new FocusDataRecord(_data, (int)screen.rect.width, (int)screen.rect.height, settings.version, session.sessionInfo);

            record.ExportRecord(path);
        }
    }
}