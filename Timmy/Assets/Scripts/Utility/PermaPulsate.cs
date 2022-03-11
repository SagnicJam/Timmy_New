using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PermaPulsate : MonoBehaviour {
    public float maxScale;
    Vector3 pingPongerScale;
    bool flipper=false;
    RectTransform thisRect;

    public float maxPulsateRate;

    private void Awake()
    {
        thisRect = GetComponent<RectTransform>();
    }
    private void Start()
    {
        pingPongerScale = Vector3.one * maxScale;
    }

    void Update () {
        if (Vector3.Distance(pingPongerScale, thisRect.localScale) < 0.02f) {
            if (flipper)
            {
                pingPongerScale = Vector3.one;
            }
            else {
             
                pingPongerScale = Vector3.one*maxScale;
            }
            flipper = !flipper;
        }
        thisRect.localScale = Vector3.Lerp(thisRect.localScale,pingPongerScale, maxPulsateRate * Time.unscaledDeltaTime);
	}
}
