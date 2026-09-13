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
        private SO_Die _currentDieData = null;

        private Vector3 _startPos;

        private bool _isRolling;

        private int _currentDieFaces = -1;
        private int _currentFaceIdx = -1;

        public event System.Action OnResultDecided;

        public bool IsRolling { get { return _isRolling; } }

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

            _isRolling = true;

            PresentCurrentFace();
        }
        private void HandleDieUpdate()
        {
            List<int> adjacentFaceIndexes = DiceTools.GetAdjacentFaceIndexes(_currentFaceIdx, _currentDieData.DeeType);
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

            _isRolling = false;

            OnResultDecided?.Invoke();
        }

        private void PresentCurrentFace()
        {
            if (_currentDieData == null) return;

            SO_DieFace faceData = _currentDieData.FaceSOs[_currentFaceIdx];
            int val = faceData.IntegerValue;

            Debug.Log($"### {name}: Current face value {val}, Tags: {string.Join(",", faceData.TagStrings)}");

            Texture2D faceTexture = faceData.GetSpriteTexture();
            if (faceTexture != null)
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

        public SO_DieFace GetCurrentFaceData()
        {
            return _currentDieData.FaceSOs[_currentFaceIdx];
        }

        public void SetDiceData(SO_Die dieData)
        {
            _currentDieData= dieData;
            _currentDieFaces = DiceTools.NumFaces(_currentDieData.DeeType);
            _currentFaceIdx = 0;

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

