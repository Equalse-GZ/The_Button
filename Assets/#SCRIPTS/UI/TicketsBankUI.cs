using Game.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TicketsBankUI : MonoBehaviour
    {
        [SerializeField] private Text _ticketsAmount;

        public void UpdateInfo(long tickets)
        {
            _ticketsAmount.text = NumberConverter.NumToString(tickets);
        }
    }
}
