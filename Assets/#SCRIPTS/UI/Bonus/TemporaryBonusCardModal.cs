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
            _actionTimeText.text = $"�����  ��������: {bonus.ActionTime} �";
            _profitText.text = $"�������: {bonus.Profit}";
        }
    }
}
