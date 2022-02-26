using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Motivator : MonoBehaviour
{
    public TextMeshProUGUI motivatingText;
    public RectTransform motivateRect;

    public float minTime;
    public float maxTime;

    private void Start()
    {
        StartCoroutine(StartMotivatingCor());
    }

    void MotivatePlayer()
    {
        StartCoroutine(MotivateCor());
    }

    IEnumerator StartMotivatingCor()
    {
        float waitingTime = Random.Range(minTime, maxTime);
        yield return new WaitForSeconds(waitingTime);
        MotivatePlayer();
        StartCoroutine(StartMotivatingCor());
    }

    public float translateSpeed;
    public float goDownAmount;
    public float motivateStayTime;

    IEnumerator MotivateCor()
    {
        motivatingText.text = GameManager.instance.motivationalMessageArr[Random.Range(0, GameManager.instance.motivationalMessageArr.Length)];

        Vector2 initPos= motivateRect.anchoredPosition;
        Vector2 finalPos = motivateRect.anchoredPosition - new Vector2(0,goDownAmount);

        float temp=0;

        while (temp<1f)
        {
            motivateRect.anchoredPosition = Vector2.Lerp(initPos, finalPos,temp);
            temp += Time.deltaTime * translateSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(motivateStayTime);

        temp = 0;
        while (temp < 1f)
        {
            motivateRect.anchoredPosition = Vector2.Lerp(finalPos, initPos, temp);
            temp += Time.deltaTime * translateSpeed;
            yield return null;
        }
        yield break;
    }
}
