using Game.Core;
using UnityEngine;

namespace Game.Bonuses
{
    [CreateAssetMenu(menuName = "Data/Bonuses/Constant Disposable", fileName = "Disposable Constant Bonus")]
    public class ConstantDisposableBonus : ConstantBonus
    {
        public void Use()
        {
            GameManager.MainButtonController.IncreaseAdditionalTickets(ChangingType, ChangingValue);
        }

        public override void Disable()
        {
            switch (ChangingType)
            {
                case ChangingType.Plus:
                    GameManager.MainButtonController.DecreaseAddedTickets(ChangingType.Minus, ChangingValue);
                    break;

                case ChangingType.Multiply:
                    GameManager.MainButtonController.DecreaseAddedTickets(ChangingType.Divide, ChangingValue);
                    break;
            }
        }
    }
}
