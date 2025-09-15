using UnityEngine;
using TMPro;
using System.Collections;

namespace Karon.EffectPrototypeVer1
{
    public class TitleVlink : MonoBehaviour
    {
        public TextMeshProUGUI stageNumber;
        public TextMeshProUGUI stageName;
        public TextMeshProUGUI stageSubname;

        public void Start()
        {
            stageName.gameObject.SetActive(false);
            stageNumber.gameObject.SetActive(false);
            stageSubname.gameObject.SetActive(false);
            StartCoroutine(DelayAndBlinkText());
        }

        IEnumerator DelayAndBlinkText()
        {
            yield return new WaitForSeconds(2f);
            stageName.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            stageNumber.gameObject.SetActive(true);
            //yield return new WaitForSeconds(1f);
            stageSubname.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            stageNumber.gameObject.SetActive(false);
            stageName.gameObject.SetActive(false);
            stageSubname.gameObject.SetActive(false);
        }
    }
}
