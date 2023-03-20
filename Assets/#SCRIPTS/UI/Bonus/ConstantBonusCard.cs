using Game.Bonuses;
using UnityEngine;

namespace Game.UI
{
    public class ConstantBonusCard : MonoBehaviour
    {
        [SerializeField] private ConstantBonus _bonus;
        [SerializeField] private ConstantBonusCardModal _modalWindow;

        public void UpdateModalInfo()
        {
            _modalWindow.UpdateInfo(_bonus);
        }
    }
}
