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

        [Space(10)]
        [SerializeField] private Image _headerBG;
        [SerializeField] private Image _iconBG;
        [SerializeField] private Image _footerBG;

        protected IBonus bonus;

        protected void UpdateBaseInfo(IBonus bonus)
        {
            if(bonus.Icon != null)
                _icon.sprite = bonus.Icon;

            this.bonus = bonus;

            _headerBG.color = new Color(bonus.CardColor.r, bonus.CardColor.g, bonus.CardColor.b, _headerBG.color.a);
            _iconBG.color = new Color(bonus.CardColor.r, bonus.CardColor.g, bonus.CardColor.b, _iconBG.color.a);
            _footerBG.color = new Color(bonus.CardColor.r, bonus.CardColor.g, bonus.CardColor.b, _footerBG.color.a);

            _nameText.color = new Color(bonus.CardColor.r, bonus.CardColor.g, bonus.CardColor.b, _nameText.color.a);
            _nameText.text = bonus.Name;
            _priceText.color = new Color(bonus.CardColor.r, bonus.CardColor.g, bonus.CardColor.b, _priceText.color.a);
            _priceText.text = $"÷ена: {NumberConverter.NumToString(bonus.Price)}" + " б";
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
