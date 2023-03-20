using Game.Bonuses;
using UnityEngine;

namespace Game.UI
{
    public class TemporaryBonusCard : MonoBehaviour
    {
        [SerializeField] private TemporaryBonus _bonus;
        [SerializeField] private TemporaryBonusCardModal _modalWindow;

        public void UpdateModalInfo()
        {
            _modalWindow.UpdateInfo(_bonus);
        }
    }
}
