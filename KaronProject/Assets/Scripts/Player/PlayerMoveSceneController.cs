using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Karon.EffectPrototypeVer1;
using UnityEngine.Rendering.Universal;

namespace Karon.Player
{
    public class PlayerMoveSceneController : MonoBehaviour
    {
        public static PlayerMoveSceneController instance;
        private string nextSpawnPointName;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Portal"))
            {
                Portal portal = collider.GetComponent<Portal>();
                if (portal != null)
                {
                    string sceneName = portal.sceneNameToLoad;
                    nextSpawnPointName = portal.spawnPointName;
                    StartCoroutine(LoadSceneAndPositionPlayer(sceneName));
                }
            }
        }

        private IEnumerator LoadSceneAndPositionPlayer(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            yield return null;
            GameObject spawnPoint = GameObject.Find(nextSpawnPointName);
            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
            }
            else
            {
                Debug.Log("스폰포인트를 찾을 수 없음 : " + nextSpawnPointName);
            }
        }
    }
}