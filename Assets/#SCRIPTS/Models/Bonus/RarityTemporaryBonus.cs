using Game.Core;
using Game.Services;
using System;
using System.Collections;
using UnityEngine;

namespace Game.Bonuses
{
    [CreateAssetMenu(menuName = "Data/Bonuses/Rarity Temporary", fileName = "Rarity Temporary Bonus")]
    public class RarityTemporaryBonus : RarityBonus
    {
        [SerializeField] private int _actionTime;

        public int ActionTime => _actionTime;
        private Coroutine _coroutine;
        private int _secondsPassed;

        public void Use(Action<RarityTemporaryBonus, int> onSecondPassed, Action<RarityTemporaryBonus> onTimeIsOut)
        {
            GameManager.MainButtonController.IncreaseAdditionalTickets(ChangingType, ChangingValue);
            _coroutine = CoroutinesController.StartRoutine(StartCounter(onSecondPassed, onTimeIsOut));
        }

        public void Use(Action<RarityTemporaryBonus> onTimeIsOut)
        {
            GameManager.MainButtonController.IncreaseAdditionalTickets(ChangingType, ChangingValue);
            _coroutine = CoroutinesController.StartRoutine(StartCounter(null, onTimeIsOut));
        }

        public override void Disable()
        {
            base.Disable();

            if(_coroutine != null)
                ResetAdditionalTickets();

            CoroutinesController.StopRoutine(_coroutine);
            _coroutine = null;
            WorkCompleted.Invoke(this);
        }

        private IEnumerator StartCounter(Action<RarityTemporaryBonus, int> onSecondPassed, Action<RarityTemporaryBonus> onTimeIsOut)
        {
            while(_secondsPassed < ActionTime)
            {
                yield return new WaitForSeconds(1);
                _secondsPassed++;
                onSecondPassed?.Invoke(this, ActionTime - _secondsPassed);
            }

            ResetAdditionalTickets();
            onTimeIsOut?.Invoke(this);
            WorkCompleted.Invoke(this);
            _coroutine = null;
        }

        private void ResetAdditionalTickets()
        {
            switch (ChangingType)
            {
                case ChangingType.Plus:
                    GameManager.MainButtonController.DecreaseAddedTickets(ChangingType.Minus, ChangingValue);
                    break;

                case ChangingType.Multiply:
                    GameManager.MainButtonController.DecreaseAddedTickets(ChangingType.Divide, ChangingValue);
                    break;
            }
        }
    }
}
