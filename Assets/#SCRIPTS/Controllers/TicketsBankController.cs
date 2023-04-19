using Game.Core;
using Game.Data;
using Game.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Controllers
{
    public class TicketsBankController : MonoBehaviour
    {
        private const string TICKETS_KEY = "TICKETS_KEY";
        public UnityEvent TicketsChangedEvent = new UnityEvent();
        [SerializeField] private TicketsBankUI _ui;

        public long Tickets { get; private set; }

        public void Initialize()
        {
            UpdateTickets(0);
            SyncController.DataRecievedEvent.AddListener(OnSync);
        }

        public void UpdateTickets(long tickets)
        {
            Tickets = tickets;
            _ui.UpdateInfo(Tickets);
            TicketsChangedEvent.Invoke();
        }

        public void OnSync(GlobalData data)
        {
            UpdateTickets(data.User.PlayerData.Tickets);
        }

        public void AddTickets(long value, object sender)
        {
            if (value < 0)
                throw new System.Exception("Value must be positive");

            Tickets += value;
            _ui.UpdateInfo(Tickets);
            TicketsChangedEvent.Invoke();
            Save();
        }

        public void SpendTickets(long value, object spender)
        {
            if (value < 0)
                throw new System.Exception("Value must be positive");

            if (IsEnoughTickets(value) == false)
                throw new System.Exception("Is not enough tickets");

            Tickets -= value;
            _ui.UpdateInfo(Tickets);
            TicketsChangedEvent.Invoke();
            Save();
        }

        public bool IsEnoughTickets(long value)
        {
            return Tickets >= value;
        }

        public void Save()
        {
            WWWForm form = new WWWForm();
            form.AddField("Type", "Save");
            form.AddField("ID", GameManager.UserData.ID);
            form.AddField("Tickets", Tickets.ToString());

            GameManager.WebRequestSender.SendData<UserData>(GameManager.Config.DataBaseUrl, form, null);
        }
    }
}
