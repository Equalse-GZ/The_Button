using Game.Data;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Web
{
    public class WebRequestDeleter : MonoBehaviour
    {
        public Action<IData, WebOperationStatus> DataReceived;

        public void DeleteData(string uri) => StartCoroutine(GetDataRoutine(uri));

        private IEnumerator GetDataRoutine(string uri)
        {
            UnityWebRequest www = UnityWebRequest.Delete(uri);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                DataReceived?.Invoke(null, WebOperationStatus.Failed);
                yield break;
            }
        }
    }
}
