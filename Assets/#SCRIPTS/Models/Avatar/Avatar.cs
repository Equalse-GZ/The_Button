using UnityEngine;

namespace Game.Avatars
{
    [CreateAssetMenu(menuName = "Data/Avatar", fileName = "Avatar")]
    public class Avatar : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;

        public Sprite Icon => _icon;
        public string Name => _name;
    }
}
