using UnityEngine;
using UnityEngine.Events;

namespace Game.Bonuses
{
    public class RarityBonus : ScriptableObject, IBonus
    {
        public UnityEvent<RarityBonus> WorkCompleted = new UnityEvent<RarityBonus>();

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

        [Space(15)]
        [SerializeField] private float _occurenceChance;

        [HideInInspector] public int CardID = -1;
        public float OccurrenceChance => _occurenceChance;
        public int ID => _id;
        public string Name => _name;
        public int Price => _price;
        public string Description => _description;
        public ChangingType ChangingType => _changingType;
        public int ChangingValue => _changingValue;
        public Sprite Icon => _icon;

        public virtual void Disable() { }
    }
}
