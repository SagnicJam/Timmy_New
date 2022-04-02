using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceCinematic : MonoBehaviour
{
    public Animator animer;
    public GameObject vfxer;
    public GameObject normalTimmy;
    public GameObject spaceTimmy;
    public Transform home;
    // Start is called before the first frame update
    void Start()
    {
        StopCoroutine("SpaceMovie");
        StartCoroutine("SpaceMovie"); 
    }
    public CanvasGroup canvasGroup;
    public float fadeSpeed;

    IEnumerator SpaceMovie()
    {
        float temp1 = 0;
        while (temp1 < 1f)
        {
            canvasGroup.alpha = 1 - temp1;
            temp1 += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        permaRotate = true;
        newVFx.SetActive(true);
        yield return new WaitForSeconds(2f);
        isMove = true;
        vfxer.SetActive(true);
        yield return new WaitForSeconds(4f);
        normalTimmy.SetActive(false);
        spaceTimmy.SetActive(true);

        yield return new WaitForSeconds(2f);

        newVFx.SetActive(false);
        float temp;
        temp = 0;
        while (temp < 1f)
        {
            canvasGroup.alpha = temp;
            temp += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);
            Debug.Log("Loading next scne");
        }

        yield break;
    }

    public float upDirection;
    bool isMove;
    bool permaRotate;
    public float rotateDirection;
    public Transform policia;
    public GameObject newVFx;
    private void Update()
    {
        if (isMove)
        {
            policia.transform.position += Vector3.up * upDirection * Time.deltaTime;
        }

        if (permaRotate)
        {
            home.transform.Rotate(Vector3.up,rotateDirection*Time.deltaTime);
        }
    }
}
