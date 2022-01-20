using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float maxSize;
    public float scaleIncreaseRate;

    IEnumerator cor;

    bool isScaling;

    public void Initialise()
    {
        gameObject.SetActive(true);
        cor = ScaleUp();
        StartCoroutine(cor);
    }

    public void Close()
    {
        cor = ScaleDown();
        StartCoroutine(cor);
    }

    IEnumerator ScaleUp()
    {
        if(!isScaling)
        {
            isScaling = true;
            transform.localScale = Vector3.zero;
            while (Vector3.Distance(transform.localScale, maxSize * Vector3.one) >= 0.01f)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, maxSize * Vector3.one, Time.deltaTime * scaleIncreaseRate);
                yield return null;
            }
            transform.localScale = maxSize * Vector3.one;
            isScaling = false;
            yield break;
        }
        
    }

    IEnumerator ScaleDown()
    {
        if (!isScaling)
        {
            transform.localScale = maxSize * Vector3.one;
            while (Vector3.Distance(transform.localScale, Vector3.zero) >= 0.01f)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.deltaTime * scaleIncreaseRate);
                yield return null;
            }
            transform.localScale = Vector3.zero;
            isScaling = false;
            gameObject.SetActive(false);
            yield break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Obstacle")
        {
            ObstacleCollider obstacleCollider = other.gameObject.GetComponent<ObstacleCollider>();
            if (!(obstacleCollider is WaterPuddle))
            {
                obstacleCollider.obstacle.Deactivate(other.gameObject);
            }
        }
    }
}
