using UnityEngine;

namespace Karon
{
    public class FollowingCamera : MonoBehaviour
    {
        public Transform target;
        [SerializeField] private float _smooth = 0.2f;

        private void Start()
        {
            if (target == null)
            {
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    target = player.transform;
                }
                else
                {
                    Debug.LogError("카메라가 따라다닐 오브젝트를 찾지 못 함.");
                }
            }
        }

        private void FixedUpdate()
        {
            if (target != null)
            {
                Vector3 targetPosition = new Vector3(target.position.x, target.position.y, this.transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, _smooth);
            }
        }
    }
}