using UnityEngine;

namespace Karon
{
    public class FollowingCamera : MonoBehaviour
    {
        public Transform target;
        [SerializeField] private float _smooth = 0.2f;

        private void FixedUpdate()
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, this.transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, _smooth);
        }
    }
}