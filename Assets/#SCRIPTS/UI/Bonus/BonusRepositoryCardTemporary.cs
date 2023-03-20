using Game.Bonuses;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class BonusRepositoryCardTemporary : BonusRepositoryCard
    {
        [Header("Unique")]
        [SerializeField] private Image _timerImage;
        [SerializeField] private Text _timerText;

        public void UpdateTimer(int actionTime, int remainingTime)
        {
            _timerText.text = "Осталось: " + remainingTime + "с";
            _timerImage.fillAmount = (float)remainingTime / actionTime;
        }
    }
}
