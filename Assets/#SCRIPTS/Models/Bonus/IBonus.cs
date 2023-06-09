using UnityEngine;

namespace Game.Bonuses
{
    public interface IBonus
    {
        public Sprite Icon { get; }
        public Color CardColor { get; }
        public int ID { get; }
        public string Name { get; }
        public string Description { get; }
        public uint Price { get; }
    }

    public enum ChangingType
    {
        Minus,
        Plus,
        Multiply,
        Divide
    }
}
