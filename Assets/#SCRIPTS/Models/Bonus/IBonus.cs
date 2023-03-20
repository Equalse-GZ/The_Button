using UnityEngine;

namespace Game.Bonuses
{
    public interface IBonus
    {
        public Sprite Icon { get; }
        public int ID { get; }
        public string Name { get; }
        public string Description { get; }
        public int Price { get; }
    }

    public enum ChangingType
    {
        Minus,
        Plus,
        Multiply,
        Divide
    }
}
