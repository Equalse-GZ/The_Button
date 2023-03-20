using Game.Core;
using Game.Bonuses;
using Game.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class BonusCardModal : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _priceText;
        [SerializeField] private Text _descriptionText;

        protected IBonus bonus;

        protected void UpdateBaseInfo(IBonus bonus)
        {
            if(bonus.Icon != null)
                _icon.sprite = bonus.Icon;

            this.bonus = bonus;
            _nameText.text = bonus.Name;
            _priceText.text = $"Цена: <size=110>{NumberConverter.NumToString(bonus.Price)}</size>";
            _descriptionText.text = bonus.Description;
        }

        public void BuyBonus()
        {
            if (GameManager.TicketsBankController.IsEnoughTickets(bonus.Price) == true)
            {
                GameManager.TicketsBankController.SpendTickets(bonus.Price, this);
                GameManager.BonusRepositoryController.AddBonus(bonus);
            }
            else
            {
                // Show UI
            }
        }
    }
}
