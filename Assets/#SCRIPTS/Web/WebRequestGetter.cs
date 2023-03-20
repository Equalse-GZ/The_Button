using Game.Data;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Web
{
    public class WebRequestGetter : MonoBehaviour
    {
        public Action<IData, WebOperationStatus> DataReceived;

        public void GetData<T>(string uri) where T : IData => StartCoroutine(GetDataRoutine<T>(uri));

        private IEnumerator GetDataRoutine<T>(string uri) where T : IData
        {
            UnityWebRequest www = UnityWebRequest.Get(uri);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                DataReceived?.Invoke(null, WebOperationStatus.Failed);
                yield break;
            }

            Debug.Log(www.downloadHandler.text);
        }
    }
}
