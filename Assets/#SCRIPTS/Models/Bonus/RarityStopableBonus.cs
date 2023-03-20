using Game.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Bonuses
{
    [CreateAssetMenu(menuName = "Data/Bonuses/Rarity Stopable", fileName = "Rarity Stopable Bonus")]
    public class RarityStopableBonus : RarityBonus
    {
        [SerializeField] private List<TemporaryBonus> _stopableBonuses = new List<TemporaryBonus>();

        public void Use()
        {
            foreach (TemporaryBonus bonus in _stopableBonuses)
                GameManager.BonusRepositoryController.RemoveAllBonuses(bonus);

            WorkCompleted.Invoke(this);
        }

        public override void Disable()
        {
            base.Disable();
        }
    }
}
