using System.Collections.Generic;
using MiscTools;
using UnityEngine.UI;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace Dice
{
    [RequireComponent(typeof(Spinner))]
    [RequireComponent(typeof(Image))]

    public class CanvasDie : MonoBehaviour
    {
        private Spinner _spinner;
        private Image _image;
        private RuntimeDie _currentDieData = null;

        private Vector3 _startPos;

        private int _currentDieFaces = -1;
        private int _currentFaceIdx = -1;

        public event System.Action OnResultDecided;

        public bool IsRolling { get { return _spinner.IsRolling; } }

        private void Awake()
        {
            _startPos = GetComponent<RectTransform>().position;
            _spinner = GetComponent<Spinner>();
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {   
            _spinner.OnDieStartedRolling += HandleDieStartRolling;
            _spinner.OnDieStoppedRolling += HandleDieStopRolling;
            _spinner.OnDieUpdated += HandleDieUpdate;
        }

        private void OnDisable()
        {
            _spinner.OnDieStartedRolling -= HandleDieStartRolling;
            _spinner.OnDieStoppedRolling -= HandleDieStopRolling;
            _spinner.OnDieUpdated -= HandleDieUpdate;
        }

        private void Start()
        {
            PresentCurrentFace();
        }


        private void HandleDieStartRolling()
        {
            string debStr = $"### {name}: Die started rolling.";
            Debug.Log(debStr);

            PresentCurrentFace();
        }
        private void HandleDieUpdate()
        {
            IReadOnlyList<int> adjacentFaceIndexes = DiceTools.GetAdjacentFaceIndexes(_currentFaceIdx, _currentDieData.DeeType);
            int rando = UnityRandom.Range(0, adjacentFaceIndexes.Count);
            _currentFaceIdx = adjacentFaceIndexes[rando];

            PresentCurrentFace();


        }
        private void HandleDieStopRolling()
        {
            if (_currentDieData == null) return;
            string debStr = $"### {name}: Die stopped rolling.";

            Debug.Log(debStr);

            PresentCurrentFace();

            OnResultDecided?.Invoke();
        }

        private void PresentCurrentFace()
        {
            if (_currentDieData == null) return;

            RuntimeDieFace faceData = _currentDieData.Faces[_currentFaceIdx];

            int val = faceData.IntegerValue;

            Debug.Log($"### {name}: Current face value {val}, Tags: {string.Join(",", faceData.TagStrings)}");

            if (faceData.ImageSprite != null)
            {
                _image.sprite = faceData.ImageSprite;
            }
            TweakImage();

        }

        private void TweakImage()
        {
            RectTransform _rectTransform = GetComponent<RectTransform>();

            Transform2DTweaksStruct tweaks = MiscTransformTools.GetTransformTweak2D(15.0f, 5.0f);
            Vector3 posAdjust = tweaks.PosTweak;
            Vector3 rotTweak = tweaks.AngleTweak;

            Vector3 newPos = _startPos + posAdjust;
            _rectTransform.position = newPos;
            _rectTransform.localRotation = Quaternion.Euler(0f, 0f, rotTweak.z);
        }

        //=====
        // API
        //=====

        public RuntimeDieFace GetCurrentRuntimeDieFace()
        {
            return _currentDieData.Faces[_currentFaceIdx];
        }

        public void SetRuntimeDieData(RuntimeDie dieData)
        {
            _currentDieData= dieData;
            _currentDieFaces = DiceTools.NumFaces( dieData.DeeType );
            _currentFaceIdx= 0;
        }

        public void Roll()
        {
            if (_currentDieData == null) return;
            if (_currentDieFaces < 2) return;
            if (!_image.gameObject.activeSelf) return;
            if (_spinner.IsRolling) return;

            _spinner.StartRolling();
        }
        public void SetDieVisible(bool visible)
        {
            PresentCurrentFace();
            _image.gameObject.SetActive(visible);
        }


    }
}

