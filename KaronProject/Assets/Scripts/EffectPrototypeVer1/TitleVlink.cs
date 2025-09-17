using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Karon.EffectPrototypeVer1
{
    public class TitleVlink : MonoBehaviour
    {
        public static HashSet<string> hasPlayedScenes = new HashSet<string>();
        public TextMeshProUGUI stageNumber;
        public TextMeshProUGUI stageName;
        public TextMeshProUGUI stageSubname;

        public void Start()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            stageName.gameObject.SetActive(false);
            stageNumber.gameObject.SetActive(false);
            stageSubname.gameObject.SetActive(false);
            if (hasPlayedScenes.Contains(currentSceneName))
            {
                return;
            }
            else
            {
                hasPlayedScenes.Add(currentSceneName);
                StartCoroutine(DelayAndBlinkText());
            }
        }

        IEnumerator DelayAndBlinkText()
        {
            yield return new WaitForSeconds(2f);
            stageName.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            stageNumber.gameObject.SetActive(true);
            stageSubname.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            stageNumber.gameObject.SetActive(false);
            stageName.gameObject.SetActive(false);
            stageSubname.gameObject.SetActive(false);
        }
    }
}
