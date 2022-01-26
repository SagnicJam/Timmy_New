using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("chala : "+other.gameObject.name);
    }

    public Transform up;
    public Transform down;

    public float scale;

    public void Start()
    {
        ScaleAround(gameObject, up.position,scale*Vector3.one);
    }

    public void ScaleAround(GameObject target, Vector3 pivot, Vector3 newScale)
    {
        Vector3 A = target.transform.localPosition;
        Vector3 B = pivot;

        Vector3 C = A - B; // diff from object pivot to desired pivot/origin

        float RS = newScale.x / target.transform.localScale.x; // relative scale factor

        // calc final position post-scale
        Vector3 FP = B + C * RS;

        // finally, actually perform the scale/translation
        target.transform.localScale = new Vector3(target.transform.localScale.x, newScale.y, target.transform.localScale.z);
        target.transform.localPosition = FP;
    }
}
