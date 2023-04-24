using UnityEngine;
using Game.Data;
using Game.Core;
using Game.UI;
using UnityEngine.Events;
using Game.Controllers;

namespace Game.Services
{
    public class GameTimer : MonoBehaviour
    {
        public UnityEvent TimeOutEvent = new UnityEvent();
        [SerializeField] private GameTimerUI _ui;
        private int _seconds;

        public void Initialize(GameUntilTime untilTime)
        {
            GameManager.GlobalTimer.SecondPassedEvent.AddListener(DecreaseTime);
            SyncController.DataRecievedEvent.AddListener(SyncTime);
            _seconds = untilTime.TimeLeft;
            _ui.UpdateTitle(_seconds);
        }

        public void Disable()
        {
            GameManager.GlobalTimer.SecondPassedEvent.RemoveListener(DecreaseTime);
            SyncController.DataRecievedEvent.RemoveListener(SyncTime);
            _seconds = 0;
        }

        private void DecreaseTime()
        {
            _seconds -= 1;
            _ui.UpdateTitle(_seconds);

            if (_seconds <= 0)
            {
                GameManager.ScreensController.ShowScreen<GameOverScreen>();
                Disable();
                TimeOutEvent.Invoke();
            }
        }

        private void SyncTime(GlobalData globalData)
        {
            _seconds = globalData.GameUntilTime.TimeLeft;
            _ui.UpdateTitle(_seconds);
        }
    }
}
