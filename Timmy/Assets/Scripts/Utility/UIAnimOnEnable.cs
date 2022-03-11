using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimOnEnable : MonoBehaviour
{
    RectTransform thisRect;
    int transitionSpeed=3;
    Vector3 initPos;
    public Vector3 finalPos;

    private void Awake()
    {
        thisRect = GetComponent<RectTransform>();
        initPos = thisRect.anchoredPosition3D;
    }

    private void OnEnable()
    {
        StopCoroutine("EnableTransCor");
        StartCoroutine("EnableTransCor");
    }

    private void OnDisable()
    {
        thisRect.anchoredPosition3D = initPos;
    }

    IEnumerator EnableTransCor()
    {
        thisRect.anchoredPosition3D = initPos;
        float t = 0;
        while (t < 1)
        {
            thisRect.anchoredPosition3D = Vector3.Lerp(initPos, finalPos, t);
            t += Time.unscaledDeltaTime * transitionSpeed;
            yield return null;
        }
        thisRect.anchoredPosition3D = finalPos;
       yield break;
    }
}
