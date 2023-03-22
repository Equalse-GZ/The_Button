using Game.Services;
using System;
using System.Collections;
using UnityEngine;

namespace Game.Bonuses
{
    [CreateAssetMenu(menuName = "Data/Bonuses/Temporary", fileName = "Temporary Bonus")]
    public class TemporaryBonus : ScriptableObject, IBonus
    {
        [Header("Image")]
        [SerializeField] private Sprite _icon;
        [SerializeField] private Color _cardColor;
        [SerializeField] private TemporaryBonusType _type;

        [Space(15)]
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private int _price;

        [Space(15)]
        [SerializeField] private int _actionTime;
        [SerializeField] private float _profit;

        public int ID => _id;
        public string Name => _name;
        public int Price => _price;
        public string Description => _description;
        public int ActionTime => _actionTime;
        public float Profit => _profit;
        public int Revenue => (int)(ActionTime * _profit);
        public Sprite Icon => _icon;
        public TemporaryBonusType Type => _type;
        public Color CardColor => _cardColor;

        public int CardID = -1;

        private int _remainingTime;

        private Coroutine _coroutine;

        public void Use(int passedTime, Action<TemporaryBonus> onTimeIsUp)
        {
            DetermineRemainingTime(passedTime, onTimeIsUp);
            _coroutine = CoroutinesController.StartRoutine(StartCounter(null, onTimeIsUp));
        }

        public void Use(int passedTime, Action<TemporaryBonus, int> onSecondPassed, Action<TemporaryBonus> onTimeIsUp)
        {
            DetermineRemainingTime(passedTime, onTimeIsUp);
            _coroutine = CoroutinesController.StartRoutine(StartCounter(onSecondPassed, onTimeIsUp));
        }

        public void Disable()
        {
            if(_coroutine != null)
                CoroutinesController.StopRoutine(_coroutine);
        }

        private IEnumerator StartCounter(Action<TemporaryBonus, int> onSecondPassed, Action<TemporaryBonus> onTimeIsUp)
        {
            while (_remainingTime >= 0)
            {
                yield return new WaitForSeconds(1);
                _remainingTime -= 1;
                onSecondPassed?.Invoke(this, _remainingTime);
            }

            onTimeIsUp?.Invoke(this);
        }

        public void DetermineRemainingTime(int passedTime, Action<TemporaryBonus> onTimeIsUp)
        {
            if(passedTime >= ActionTime)
            {
                _remainingTime = 0;
                onTimeIsUp?.Invoke(this);
                return;
            }

            _remainingTime = ActionTime - passedTime;
        }
    }

    public enum TemporaryBonusType
    {
        Casual,
        Golden
    }
}
