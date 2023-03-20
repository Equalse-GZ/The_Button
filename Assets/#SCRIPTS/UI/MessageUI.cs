using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class MessageUI : MonoBehaviour
    {
        [SerializeField] private Text _text;

        public Text Text => _text;

        public void SetPosition(Vector3 newPosition)
        {
            GetComponent<RectTransform>().anchoredPosition = newPosition;
        }

        public void SetRotation(Vector3 newRotation)
        {
            this.transform.localRotation = Quaternion.Euler(newRotation);
        }
    }
}
