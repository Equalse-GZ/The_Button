using Game.Core;
using Game.Data;
using Game.Services;
using Game.Web;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Controllers
{
    public class SyncController : MonoBehaviour
    {
        public static UnityEvent<GlobalData> DataRecievedEvent = new UnityEvent<GlobalData>();
        [SerializeField] private int _requestDelay = 5;

        private UserData _user;
        private Coroutine _coroutine;
        private GlobalData _globalData;

        public void Initialize(UserData user)
        {
            _user = user;
            _coroutine = StartCoroutine(SendRequest());
        }

        public void Disable()
        {
            if(_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        private void OnDisable()
        {
            Disable();
        }

        private IEnumerator SendRequest()
        {
            while (true)
            {
                yield return new WaitForSeconds(_requestDelay);

                WWWForm form = new WWWForm();
                form.AddField("Type", "Sync");
                form.AddField("UserID", _user.ID);

                GameManager.WebRequestSender.SendData<GlobalDataRaw>(GameManager.Config.DataBaseUrl, form, OnDataRecieved);
            }
        }

        private void OnDataRecieved(GlobalDataRaw data, WebOperationStatus status)
        {
            // Errors

            _globalData = new GlobalData();
            _globalData.User = data.User;

            string json = JsonHelper.FixJson(data.TeamLeaders);
            _globalData.TeamLeaders = JsonHelper.FromJson<TeamData>(json);

            json = JsonHelper.FixJson(data.PersonLeaders);
            _globalData.PersonLeaders = JsonHelper.FromJson<UserData>(json);

            json = JsonHelper.FixJson(data.TeamMembers);
            _globalData.TeamMembers = JsonHelper.FromJson<UserData>(json);

            json = JsonHelper.FixJson(data.Bonuses);
            _globalData.Bonuses = JsonHelper.FromJson<BonusData>(json);

            _globalData.UserPlace = data.UserPlace;
            _globalData.TeamPlace = data.TeamPlace;

            DataRecievedEvent.Invoke(_globalData);
        }
    }
}
