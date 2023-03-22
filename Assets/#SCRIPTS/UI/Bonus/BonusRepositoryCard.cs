using Game.Bonuses;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class BonusRepositoryCard : MonoBehaviour
    {
        [Header("Total")]
        [SerializeField] protected Image _icon;
        [SerializeField] protected Image _bgIcon;
        [SerializeField] protected Image _bg;
        [SerializeField] protected Text _nameText;
        [SerializeField] protected Text _typeText;

        public int ID { get; set; }
        public IBonus Owner { get; private set; }
        
        

        public void UpdateTotalInfo(IBonus bonus)
        {
            if(bonus is TemporaryBonus)
            {
                if (((TemporaryBonus)bonus).Type == TemporaryBonusType.Golden)
                    _bgIcon.color = bonus.CardColor;
                else
                    _bgIcon.color = Color.white;
            }


            Owner = bonus;
            string type = "";

            if (bonus is TemporaryBonus)
                type = "Временный";
            else if (bonus is RarityBonus)
                type = "Редкий";
            else
                type = "Постоянный";

            _bg.color = bonus.CardColor;
            _icon.sprite = bonus.Icon;
            _nameText.text = bonus.Name;
            _typeText.text = type;
        }
    }
}
