using UnityEngine;
using UnityEngine.SceneManagement;

namespace Karon.Prototype1
{
    public class GameManager : MonoBehaviour
    {
        public string TargetScene;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            SceneManager.LoadScene(TargetScene);
        }

    }
}