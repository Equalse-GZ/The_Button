using Game.Core;
using Game.Services;
using System.Collections;
using UnityEngine;

namespace Game.Bonuses
{
    [CreateAssetMenu(menuName = "Data/Bonuses/Constant Reusable", fileName = "Reusable Constant Bonus")]
    public class ConstantReusableBonus : ConstantBonus
    {
        public void Use()
        {
            GameManager.GlobalTimer.SecondPassedEvent.AddListener(AddTickets);
        }

        public override void Disable()
        {
            GameManager.GlobalTimer.SecondPassedEvent.RemoveListener(AddTickets);
        }

        private void AddTickets()
        {
            GameManager.TicketsBankController.AddTickets((uint)(ChangingValue * Count), this);
        }
    }
}
