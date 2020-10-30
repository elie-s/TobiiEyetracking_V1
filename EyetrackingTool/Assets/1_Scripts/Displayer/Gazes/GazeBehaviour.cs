using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Elie.Tools.Eyetracking_1
{
    public class GazeBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textField = default;
        [SerializeField] private RectTransform rectTransform = default;

        public void Initialize(string _testerName, Color _color)
        {
            gameObject.name = "GazePrefab_" + transform.GetSiblingIndex() + "_" + _testerName;
            SetText(_testerName[0].ToString().ToUpper(), _color);
            SetPosition(0, 0);
        }

        public void SetText(string _text, Color _color = default)
        {
            textField.text = _text;
            textField.color = _color;
        }

        public void SetPosition(int _x, int _y)
        {
            rectTransform.anchoredPosition = new Vector2(_x, _y);
        }

        public void SetPosition(Vector2Int _pos)
        {
            SetPosition(_pos.x, _pos.y);
        }
    }
}