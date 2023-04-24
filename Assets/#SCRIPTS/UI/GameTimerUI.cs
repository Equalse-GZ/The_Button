using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameTimerUI : MonoBehaviour
    {
        [SerializeField] private Text _title;

        public void UpdateTitle(int seconds)
        {
            int minutes = 0;
            int hours = 0;
            if (seconds / 60 % 60 - 1 > 0) minutes = seconds / 60 % 60 - 1;
            if (seconds / 60 / 60 % 24 - 1 > 0) hours = seconds / 60 / 60 % 24 - 1;

            _title.text = "�� ����� ������� ��������:\n" + (seconds / 24 / 60 / 60).ToString() + " ���� " + hours.ToString() + " ����� " + minutes.ToString() + " ����� " + (seconds % 60).ToString() + " ������";
        }
    }
}
