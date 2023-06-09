using UnityEngine;

namespace Game.Bonuses
{
    public class ConstantBonus : ScriptableObject, IBonus
    {
        [Header("Image")]
        [SerializeField] private Sprite _icon;
        [SerializeField] private Color _cardColor;

        [Space(15)]
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private uint _price;

        [Space(15)]
        [SerializeField] private ChangingType _changingType;
        [SerializeField] private long _changingValue;
        

        [HideInInspector] public int CardID = -1;
        [HideInInspector] public int Count = 1;
        public string Name => _name;
        public uint Price => _price;
        public int ID => _id;
        public string Description => _description;
        public ChangingType ChangingType => _changingType;
        public long ChangingValue => _changingValue;
        public Sprite Icon => _icon;

        public Color CardColor => _cardColor;

        public virtual void Disable() { }
    }
}
