using Game.Bonuses;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TemporaryBonusCardModal : BonusCardModal
    {
        [SerializeField] private Text _actionTimeText;
        [SerializeField] private Text _profitText;

        public void UpdateInfo(TemporaryBonus bonus)
        {
            UpdateBaseInfo(bonus);
            _actionTimeText.color = new Color(bonus.CardColor.r, bonus.CardColor.g, bonus.CardColor.b, _actionTimeText.color.a);
            _actionTimeText.text = $"Время  действия: {bonus.ActionTime} с";

            _profitText.color = new Color(bonus.CardColor.r, bonus.CardColor.g, bonus.CardColor.b, _profitText.color.a);
            _profitText.text = $"Прибыль: {bonus.Profit}";
        }
    }
}
