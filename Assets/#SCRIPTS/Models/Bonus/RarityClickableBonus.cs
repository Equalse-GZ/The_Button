using Game.Core;
using UnityEngine;

namespace Game.Bonuses
{
    [CreateAssetMenu(menuName = "Data/Bonuses/Rarity Clickable", fileName = "Rarity Clickable Bonus")]
    public class RarityClickableBonus : RarityBonus
    {
        public void Use()
        {
            ChangeAdditionalTickets();
            GameManager.MainButtonController.ClickEvent.AddListener(ResetAdditionalTickets);
        }

        public override void Disable()
        {
            GameManager.MainButtonController.ClickEvent.RemoveListener(ResetAdditionalTickets);
            ResetAdditionalTickets();
            base.Disable();
        }

        private void ChangeAdditionalTickets()
        {
            switch (ChangingType)
            {
                case ChangingType.Plus:
                    GameManager.MainButtonController.IncreaseAdditionalTickets(ChangingType.Plus, ChangingValue);
                    break;

                case ChangingType.Multiply:
                    GameManager.MainButtonController.IncreaseAdditionalTickets(ChangingType.Multiply, ChangingValue);
                    break;

                case ChangingType.Minus:
                    GameManager.MainButtonController.DecreaseAddedTickets(ChangingType.Minus, ChangingValue);
                    break;

                case ChangingType.Divide:
                    GameManager.MainButtonController.DecreaseAddedTickets(ChangingType.Divide, ChangingValue);
                    break;
            }
        }

        private void ResetAdditionalTickets()
        {
            switch (ChangingType)
            {
                case ChangingType.Plus:
                    GameManager.MainButtonController.DecreaseAddedTickets(ChangingType.Minus, ChangingValue);
                    break;

                case ChangingType.Multiply:
                    GameManager.MainButtonController.DecreaseAddedTickets(ChangingType.Divide, ChangingValue);
                    break;

                case ChangingType.Minus:
                    GameManager.MainButtonController.IncreaseAdditionalTickets(ChangingType.Plus, ChangingValue);
                    break;

                case ChangingType.Divide:
                    GameManager.MainButtonController.IncreaseAdditionalTickets(ChangingType.Multiply, ChangingValue);
                    break;
            }

            GameManager.MainButtonController.ClickEvent.RemoveListener(ResetAdditionalTickets);
            WorkCompleted.Invoke(this);
        }
    }
}
