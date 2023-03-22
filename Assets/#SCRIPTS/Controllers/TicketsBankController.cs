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

        public int Tickets { get; private set; }
        private int _userID;

        public void Initialize(int loadedTickets, int userID)
        {
            Tickets = loadedTickets;
            _userID = userID;
            _ui.UpdateInfo(Tickets);
            TicketsChangedEvent.Invoke();
        }

        public void AddTickets(int value, object sender)
        {
            if (value < 0)
                throw new System.Exception("Value must be positive");

            Tickets += value;
            _ui.UpdateInfo(Tickets);
            TicketsChangedEvent.Invoke();
            Save();
        }

        public void SpendTickets(int value, object spender)
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

        public bool IsEnoughTickets(int value)
        {
            return Tickets >= value;
        }

        public void Save()
        {
            WWWForm form = new WWWForm();
            form.AddField("Type", "Save");
            form.AddField("ID", _userID);
            form.AddField("Tickets", Tickets);

            GameManager.WebRequestSender.SendData<UserData>(GameManager.Config.DataBaseUrl, form, null);
        }
    }
}
