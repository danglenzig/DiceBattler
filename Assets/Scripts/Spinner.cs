using UnityRandom = UnityEngine.Random;
using UnityEngine;

namespace Dice
{
    public class Spinner : MonoBehaviour
    {
        private const float SLOWDOWN_TWEAK_MAX = 0.05f;
        private const float START_INTERVAL_TWEAK_FACTOR_MAX = 0.1f;


        [SerializeField, Range(0.02f, 1.0f)] private float _startRollInterval = 0.02f;
        [SerializeField, Range(0.02f, 1.0f)] private float _endRollInterval = 0.33f;
        [SerializeField, Min(1.01f)] private float _slowdownFactor = 1.1f;

        [SerializeField] private bool _randomlyTweakStartInterval = true;
        [SerializeField] private bool _randomlyTweakSlowdown = true;

        //private string _dieIDString = "";
        private bool _isRolling = false;
        private float _actualStartInterval;
        private float _actualSlowdownFactor;

        public event System.Action OnDieStartedRolling;
        public event System.Action OnDieStoppedRolling;
        public event System.Action OnDieUpdated;

        private float _ta = 0.0f;
        private float _currentUpdateInterval = 0.0f;



        [HideInInspector] public bool IsRolling { get { return _isRolling; } }

        private void OnValidate()
        {
            // fix the slowdown factor
            if (_slowdownFactor < 1.01f)
            {
                _slowdownFactor = 1.01f;
            }

            // fix the start interval
            if (_startRollInterval < 0.02f)
            {
                _startRollInterval = 0.02f;
            }
            else if (_startRollInterval > 1.0f)
            {
                _startRollInterval = 1.0f;
            }

            Tweak(_randomlyTweakStartInterval, _randomlyTweakSlowdown);


        }

        void Start()
        {

        }
        void Update()
        {
            if (!_isRolling) return;
            _ta += Time.deltaTime;

            // throttle to the current interval
            if (_ta < _currentUpdateInterval) return;

            _ta = 0.0f;

            // update the current interval
            _currentUpdateInterval *= _actualSlowdownFactor;

            if (_currentUpdateInterval >= _endRollInterval)
            {
                // stop rolling
                _isRolling = false;
                _ta = 0.0f;
                _currentUpdateInterval = _actualStartInterval;

                OnDieStoppedRolling?.Invoke();

                Tweak(_randomlyTweakStartInterval, _randomlyTweakSlowdown);
            }
            else
            {
                OnDieUpdated?.Invoke();
            }
        }

        private void Tweak(bool tweakStartInterval, bool tweakSlowdown)
        {
            if (tweakStartInterval)
            {
                float adjust = UnityRandom.Range(0, START_INTERVAL_TWEAK_FACTOR_MAX);
                _actualStartInterval = _startRollInterval + adjust;
            }
            else
            {
                _actualStartInterval = _startRollInterval;
            }

            if (tweakSlowdown)
            {
                float adjust = UnityRandom.Range(0, SLOWDOWN_TWEAK_MAX);
                _actualSlowdownFactor = _slowdownFactor + adjust;
            }
            else
            {
                _actualSlowdownFactor = _slowdownFactor;
            }

            if (_endRollInterval <= _actualStartInterval)
            {
                _endRollInterval = _actualStartInterval * _actualSlowdownFactor;
            }

        }

        //=====
        // API
        //=====

        public void StartRolling()
        {
            if (IsRolling) return;
            _ta = 0.0f;
            _currentUpdateInterval = _actualStartInterval;
            _isRolling = true;
            OnDieStartedRolling?.Invoke();
        }
        public string SayHello()
        {
            return $"### {name}: Behavior component says hi!";
        }
    }
}


