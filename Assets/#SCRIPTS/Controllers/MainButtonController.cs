using Game.Bonuses;
using Game.Core;
using Game.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Controllers
{
    public class MainButtonController : MonoBehaviour
    {
        public UnityEvent ClickEvent = new UnityEvent();
        [SerializeField] private MainButtonUI _ui;
        [SerializeField] private int _addedTickets;

        private Dictionary<ChangingType, int> _additionalTickets = new Dictionary<ChangingType, int>();

        public void Initialize()
        {
            _additionalTickets[ChangingType.Multiply] = 0;
            _additionalTickets[ChangingType.Plus] = 0;

            _addedTickets = 1;
            _ui.Initialize();
            _ui.ClickEvent += OnClick;
        }

        public void IncreaseAdditionalTickets(ChangingType changingType, int value)
        {
            if (_additionalTickets.ContainsKey(changingType) == false)
                return;

            _additionalTickets[changingType] = _additionalTickets.GetValueOrDefault(changingType) + value;
            ChangeAdditionalTickets();
        }

        public void DecreaseAddedTickets(ChangingType changingType, int value)
        {
            if (changingType == ChangingType.Minus)
                _additionalTickets[ChangingType.Plus] = _additionalTickets.GetValueOrDefault(ChangingType.Plus) - value;

            else if (changingType == ChangingType.Divide)
                _additionalTickets[ChangingType.Multiply] = _additionalTickets.GetValueOrDefault(ChangingType.Multiply) / value;

            else return;

            ChangeAdditionalTickets();
        }

        public void Disable()
        {
            _ui.ClickEvent -= OnClick;
            ClickEvent.RemoveAllListeners();
        }

        private void ChangeAdditionalTickets()
        {
            _addedTickets = 1 + _additionalTickets[ChangingType.Plus];
            _addedTickets *= _additionalTickets[ChangingType.Multiply];
        }

        private void OnClick()
        {
            GameManager.TicketsBankController.AddTickets(_addedTickets, this);
            //_ui.ThrowMessage(50000);
            ClickEvent.Invoke();
        }

        private void OnDisable()
        {
            _ui.ClickEvent -= OnClick;
            ClickEvent.RemoveAllListeners();
        }
    }
}