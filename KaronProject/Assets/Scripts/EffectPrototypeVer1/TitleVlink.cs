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
            //StartCoroutine(DelayAndBlinkText2());
        }

        IEnumerator DelayAndBlinkText()
        {
            yield return new WaitForSeconds(2f);
            stageNumber.gameObject.SetActive(true);
            stageName.gameObject.SetActive(true);
            stageSubname.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            stageNumber.gameObject.SetActive(false);
            stageName.gameObject.SetActive(false);
            stageSubname.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);
            //yield return new WaitForSeconds(1f);
            stageName.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            stageNumber.gameObject.SetActive(true);
            stageSubname.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            stageNumber.gameObject.SetActive(false);
            stageName.gameObject.SetActive(false);
            stageSubname.gameObject.SetActive(false);
        }
        
        IEnumerator DelayAndBlinkText2()
        {
            yield return new WaitForSeconds(2f);
            stageNumber.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            stageName.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            stageSubname.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            stageNumber.gameObject.SetActive(false);
            stageName.gameObject.SetActive(false);
            stageSubname.gameObject.SetActive(false);
        }
    }
}
