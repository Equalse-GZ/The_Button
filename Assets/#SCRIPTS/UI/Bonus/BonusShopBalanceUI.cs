using Game.Core;
using Game.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class BonusShopBalanceUI : UIElement
    {
        [SerializeField] private Text _text;

        public override void Initialize()
        {
            base.Initialize();
            GameManager.TicketsBankController.TicketsChangedEvent.AddListener(UpdateInfo);
            UpdateInfo();
        }

        public void UpdateInfo()
        {
            _text.text = $"Баланс  <size=85><color=#9864FF>{NumberConverter.NumToString(GameManager.TicketsBankController.Tickets)}</color> <color=#480cc5>б</color></size>";
        }
    }
}
