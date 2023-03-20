using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(menuName = "Configs/Game/Game Config", fileName = "Game Config")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private string _dataBaseUrl;

        public string DataBaseUrl => _dataBaseUrl;
    }
}
