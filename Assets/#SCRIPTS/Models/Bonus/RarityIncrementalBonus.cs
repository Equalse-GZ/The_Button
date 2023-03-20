using Game.Core;
using UnityEngine;

namespace Game.Bonuses
{
    [CreateAssetMenu(menuName = "Data/Bonuses/Rarity Incremental", fileName = "Rarity Incremental Bonus")]
    public class RarityIncrementalBonus : RarityBonus
    {
        public void Use()
        {
            switch (ChangingType)
            {
                case ChangingType.Plus:
                    GameManager.TicketsBankController.AddTickets(ChangingValue, this);
                    break;

                case ChangingType.Minus:
                    GameManager.TicketsBankController.SpendTickets(ChangingValue, this);
                    break;

                case ChangingType.Multiply:
                    GameManager.TicketsBankController.AddTickets(GameManager.TicketsBankController.Tickets * ChangingValue, this);
                    break;

                case ChangingType.Divide:
                    GameManager.TicketsBankController.AddTickets(GameManager.TicketsBankController.Tickets / ChangingValue, this);
                    break;
            }

            WorkCompleted.Invoke(this);
        }

        public override void Disable()
        {
            base.Disable();
        }
    }
}
