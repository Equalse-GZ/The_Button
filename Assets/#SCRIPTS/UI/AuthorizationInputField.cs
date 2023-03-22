using Game.Animations;
using UnityEngine;

namespace Game.UI
{
    [RequireComponent(typeof(SinglePulseAnimation))]
    public class AuthorizationInputField : MonoBehaviour
    {
        public void PlayPulseAnimation() => GetComponent<SinglePulseAnimation>().Play();
    }
}
