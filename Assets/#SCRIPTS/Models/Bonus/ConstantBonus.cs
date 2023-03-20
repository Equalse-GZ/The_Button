using UnityEngine;

namespace Game.Bonuses
{
    public class ConstantBonus : ScriptableObject, IBonus
    {
        [Header("Image")]
        [SerializeField] private Sprite _icon;

        [Space(15)]
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private int _price;

        [Space(15)]
        [SerializeField] private ChangingType _changingType;
        [SerializeField] private int _changingValue;
        

        [HideInInspector] public int CardID = -1;
        [HideInInspector] public int Count = 1;
        public string Name => _name;
        public int Price => _price;
        public int ID => _id;
        public string Description => _description;
        public ChangingType ChangingType => _changingType;
        public int ChangingValue => _changingValue;
        public Sprite Icon => _icon;

        public virtual void Disable() { }
    }
}
