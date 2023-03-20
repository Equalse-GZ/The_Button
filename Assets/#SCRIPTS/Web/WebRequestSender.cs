using System;
using System.Collections;
using Game.Data;
using Game.Services;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Web
{
    public class WebRequestSender : MonoBehaviour
    {
        public void SendData<T>(string url, WWWForm form, Action<T, WebOperationStatus> callBack) where T : IData => StartCoroutine(SendDataRoutine<T>(url, form, callBack));
        public void GetUsersData<T>(string url, WWWForm form, Action<UsersData, WebOperationStatus> callBack) where T : IData => StartCoroutine(GetUsersDataRoutine<T>(url, form, callBack));
        public void GetTeamsData<T>(string url, WWWForm form, Action<TeamsData, WebOperationStatus> callBack) where T : IData => StartCoroutine(GetTeamsDataRoutine<T>(url, form, callBack));
        public void GetBonusesData(string url, WWWForm form, Action<BonusesData, WebOperationStatus> callBack) => StartCoroutine(GetBonusesDataRoutine(url, form, callBack));

        private IEnumerator SendDataRoutine<T>(string url, WWWForm form, Action<T, WebOperationStatus> callBack) where T : IData
        {
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                yield break;

            T data = JsonUtility.FromJson<T>(www.downloadHandler.text);
            callBack?.Invoke(data, WebOperationStatus.Succesfull);
        }

        private IEnumerator GetUsersDataRoutine<T>(string url, WWWForm form, Action<UsersData, WebOperationStatus> callBack) where T : IData
        {
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                yield break;

            string json = JsonHelper.FixJson(www.downloadHandler.text);
            UserData[] data = JsonHelper.FromJson<UserData>(json);

            UsersData users = new UsersData();
            users.Users = data;
            
            callBack?.Invoke(users, WebOperationStatus.Succesfull);
        }

        private IEnumerator GetTeamsDataRoutine<T>(string url, WWWForm form, Action<TeamsData, WebOperationStatus> callBack) where T : IData
        {
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                yield break;

            string json = JsonHelper.FixJson(www.downloadHandler.text);
            TeamData[] data = JsonHelper.FromJson<TeamData>(json);

            TeamsData teams = new TeamsData();
            teams.Teams = data;

            callBack?.Invoke(teams, WebOperationStatus.Succesfull);
        }

        private IEnumerator GetBonusesDataRoutine(string url, WWWForm form, Action<BonusesData, WebOperationStatus> callBack)
        {
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                yield break;

            string json = JsonHelper.FixJson(www.downloadHandler.text);
            print(json);
            BonusData[] data = JsonHelper.FromJson<BonusData>(json);

            BonusesData bonuses = new BonusesData();
            bonuses.Bonuses = data;

            callBack?.Invoke(bonuses, WebOperationStatus.Succesfull);
        }
    }
}
