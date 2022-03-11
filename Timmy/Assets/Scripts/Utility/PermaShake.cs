using UnityEngine;
using System.Collections;

public class PermaShake : MonoBehaviour
{
	public float shakeAmount = 0.7f;
	
	Vector3 originalPos;
	
	void OnEnable()
	{
		originalPos = transform.localPosition;
	}

	void Update()
	{
			transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
	}

    void OnDisable(){
        transform.localPosition = originalPos;
    }
}