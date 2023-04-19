using Game.Core;
using Game.Services;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class MainButtonUI : MonoBehaviour
    {
        public Action ClickEvent;

        [Header("Button Animation")]
        [SerializeField] private AnimationCurve _onButtonClickedCurve;
        [SerializeField] private AnimationCurve _onButtonReleasedCurve;

        [Header("Screen Animation")]
        [SerializeField] private AnimationCurve _screenZoomCurve;
        [SerializeField] private AnimationCurve _screenDistanceCurve;

        [SerializeField] private float duration;

        [Space(10)]
        [SerializeField] private Button _button;

        [Header("Message")]
        [SerializeField] private AnimationCurve _localPositionYCurve;
        [SerializeField] private AnimationCurve _scaleCurve;

        [SerializeField] private MessageUI _popupMessageTemplate;
        [SerializeField] private float _radius;

        private IEnumerator _coroutine;

        private Coroutine _clickedAnimationCoroutine;
        private Coroutine _releasedAnimationCoroutine;

        private Coroutine _zoomAnimationCoroutine;
        private Coroutine _distanceAnimationCoroutine;

        private Coroutine _throwingMessageCoroutine;

        private bool _isInitialized = false;

        public void Initialize()
        {
            if (_isInitialized == true)
                return;

            _button.onClick.AddListener(() => ClickEvent?.Invoke());
            _isInitialized = true;
        }

        public void ThrowMessage(long addedTickets)
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);

            float randPosY = UnityEngine.Random.Range(-_radius, _radius);
            float angle = Mathf.Atan(randPosY / _radius) * Mathf.Rad2Deg;
            float posX = _radius * Mathf.Sqrt(1 - Mathf.Pow(Mathf.Sin(angle * Mathf.Deg2Rad), 2));

            int randSide = UnityEngine.Random.Range(0, 2);
            if (randSide == 1) posX *= -1;

            if (posX > 0)
                angle *= -1;

            var popupMessage = GameManager.PoolManager.Spawn(_popupMessageTemplate.gameObject, Vector3.zero, Quaternion.identity, this.transform).GetComponent<MessageUI>();
            popupMessage.SetPosition(new Vector3(posX, randPosY, 0));
            popupMessage.SetRotation(new Vector3(0, 0, angle));
            popupMessage.Text.text = "+" + NumberConverter.NumToString(addedTickets);

            _throwingMessageCoroutine = StartCoroutine(ThrowMessageRoutine(popupMessage));
        }

        public void PlayOnClickedAnimation()
        {
            StopAnimations();

            _clickedAnimationCoroutine = StartCoroutine(PlayButtonAnimationRoutine(_onButtonClickedCurve));
            _zoomAnimationCoroutine = StartCoroutine(PlayScreenShakeAnimationRoutine(_screenZoomCurve));
        }

        public void PlayOnReleasedAnimation()
        {
            StopAnimations();

            _releasedAnimationCoroutine = StartCoroutine(PlayButtonAnimationRoutine(_onButtonReleasedCurve));
            _distanceAnimationCoroutine = StartCoroutine(PlayScreenShakeAnimationRoutine(_screenDistanceCurve));
        }

        private void StopAnimations()
        {
            if (_releasedAnimationCoroutine != null)
                StopCoroutine(_releasedAnimationCoroutine);

            if (_clickedAnimationCoroutine != null)
                StopCoroutine(_clickedAnimationCoroutine);

            if (_zoomAnimationCoroutine != null)
                StopCoroutine(_zoomAnimationCoroutine);

            if (_distanceAnimationCoroutine != null)
                StopCoroutine(_distanceAnimationCoroutine);
        }

        private IEnumerator PlayScreenShakeAnimationRoutine(AnimationCurve curve)
        {
            float expiredSeconds = 0;
            float progress = 0;

            while (progress < 1)
            {
                expiredSeconds += Time.deltaTime;
                progress = expiredSeconds / duration;

                GameManager.UserInterface.GetScreen<GamingScreen>().GetComponent<RectTransform>().localScale = Vector3.one * curve.Evaluate(progress);
                yield return null;
            }
        }

        private IEnumerator PlayButtonAnimationRoutine(AnimationCurve curve)
        {
            float expiredSeconds = 0;
            float progress = 0;

            while (progress < 1)
            {
                expiredSeconds += Time.deltaTime;
                progress = expiredSeconds / duration;

                _button.GetComponent<RectTransform>().localScale = Vector3.one * curve.Evaluate(progress);
                yield return null;
            }
        }

        private IEnumerator ThrowMessageRoutine(MessageUI message)
        {
            //_popupMessage.SetPosition(Vector3.zero);

            float expiredSeconds = 0;
            float progress = 0;

            Vector3 startPosition = message.GetComponent<RectTransform>().localPosition;

            while (progress < 1)
            {
                expiredSeconds += Time.deltaTime;
                progress = expiredSeconds / duration;

                message.GetComponent<RectTransform>().localPosition = startPosition + new Vector3(0, _localPositionYCurve.Evaluate(progress), 0);
                message.GetComponent<RectTransform>().localScale = Vector3.one * _scaleCurve.Evaluate(progress);

                yield return null;
            }

            //Depawn
            GameManager.PoolManager.Despawn(message.gameObject);
        }
    }
}
