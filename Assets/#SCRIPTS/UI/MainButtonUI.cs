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

        [SerializeField] private Button _button;
        [SerializeField] private MessageUI _popupMessage;

        private IEnumerator _coroutine;
        private bool _isInitialized = false;

        public void Initialize()
        {
            if (_isInitialized == true)
                return;

            _button.onClick.AddListener(() => ClickEvent?.Invoke());
            _isInitialized = true;
        }

        public void ThrowMessage(int addedTickets)
        {
            if(_coroutine != null)
                StopCoroutine(_coroutine);

            _popupMessage.Text.text = "+" + NumberConverter.NumToString(addedTickets);

            Vector3 position = new Vector3(UnityEngine.Random.Range(-1.7f, 1.7f) * 500, UnityEngine.Random.Range(-1.7f, 1.7f) * 500, _popupMessage.transform.position.z);
            _popupMessage.SetRotation(new Vector3(0, 0, Mathf.Abs(position.x) / position.x * UnityEngine.Random.Range(0, 45)));

            _coroutine = ThrowMessageRoutine(position);
            StartCoroutine(_coroutine);
        }

        private IEnumerator ThrowMessageRoutine(Vector2 position)
        {
            _popupMessage.SetPosition(Vector3.zero);

            while (_popupMessage.transform.GetComponent<RectTransform>().anchoredPosition != position)
            {
                _popupMessage.SetPosition(Vector3.MoveTowards(_popupMessage.transform.GetComponent<RectTransform>().anchoredPosition, position, 15 * 100 * Time.deltaTime));
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
