using System.Collections.Generic;
using UnityEngine;

namespace Game.Controllers
{
    public class AvatarsController : MonoBehaviour
    {
        [SerializeField] private List<Game.Avatars.Avatar> _avatars = new List<Game.Avatars.Avatar>();

        public Game.Avatars.Avatar GetAvatar(string avatarName)
        {
            foreach (var avatar in _avatars)
            {
                if (avatar.Name == avatarName)
                    return avatar;
            }

            return null;
        }
    }
}
