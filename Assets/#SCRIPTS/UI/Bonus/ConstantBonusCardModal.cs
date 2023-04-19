using Game.Core;
using Game.Bonuses;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ConstantBonusCardModal : BonusCardModal
    {
        [SerializeField] private Text _bonusUsageText;
        [SerializeField] Button _buyButton;
        [SerializeField] private Text _descriptionText;
        public void UpdateInfo(ConstantBonus bonus)
        {
            this.bonus = bonus;
            UpdateBaseInfo(bonus);

            _descriptionText.text = bonus.Description;
            _descriptionText.color = new Color(bonus.CardColor.r, bonus.CardColor.g, bonus.CardColor.b, _descriptionText.color.a);

            _bonusUsageText.color = new Color(bonus.CardColor.r, bonus.CardColor.g, bonus.CardColor.b, _bonusUsageText.color.a);

            if (bonus is ConstantReusableBonus)
                _bonusUsageText.text = "Многоразовые покупки";

            if(bonus is ConstantDisposableBonus)
                _bonusUsageText.text = "Одноразовые покупки";

            CheckBonusUsage();
        }

        public void CheckBonusUsage()
        {
            if (bonus == null) return;
            if (bonus is ConstantReusableBonus) _buyButton.gameObject.SetActive(true);
            else _buyButton.gameObject.SetActive(!GameManager.BonusRepositoryController.ContainsBonus(bonus));
        }

        public override void BuyBonus()
        {
            base.BuyBonus();
            CheckBonusUsage();
        }
    }
}
