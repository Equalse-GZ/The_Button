using System.Collections;
using UnityEngine;

namespace Game.UI
{
    public class MenuIndicator : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;

        public void MoveTo(Vector3 position)
        {
            StopAllCoroutines();
            StartCoroutine(MoveRoutine(position));
        }

        private IEnumerator MoveRoutine(Vector3 position)
        {
            while (this.transform.position != position)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, position, _movementSpeed * 100 * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
