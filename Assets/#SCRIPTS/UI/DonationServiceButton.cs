using Game.Core;
using Game.Data;
using Game.Web;
using UnityEngine;

namespace Game.UI
{
    public class DonationServiceButton : MonoBehaviour
    {
        public void TryDonate()
        {
            if (GameManager.TicketsBankController.Tickets == 1) return;

            WWWForm form = new WWWForm();
            form.AddField("Type", "EventDonate");

            GameManager.WebRequestSender.SendData<UserData>(GameManager.Config.DataBaseUrl, form, OnDataRecieved);
        }

        private void OnDataRecieved(UserData data, WebOperationStatus status)
        {
            GameManager.TicketsBankController.SpendTickets(1, this);
        }
    }
}
