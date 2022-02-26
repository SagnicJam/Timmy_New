using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Cinematicor : MonoBehaviour
{
    public float fadeSpeed;
    public float cameraSpeed;

    public CanvasGroup canvasGroup;
    public Camera cam;
    public List<Transform> transformsList;

    Vector3 initPos;
    Quaternion initRot;

    private void Start()
    {
        StartCoroutine(CinematicsCor());
    }

    IEnumerator CinematicsCor()
    {
        cam.transform.position = transformsList[0].position;
        cam.transform.rotation = transformsList[0].rotation;

        float temp = 0;
        while(temp<1f)
        {
            canvasGroup.alpha = 1-temp;
            temp += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        
        for (int i = 1; i < transformsList.Count; i++)
        {
            temp = 0;
            initPos = transformsList[i-1].position;
            initRot = transformsList[i-1].rotation;
            while(Vector3.Distance(cam.transform.position, transformsList[i].position)>=0.02f)
            {
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, transformsList[i].position,cameraSpeed*Time.deltaTime);
                temp = 1-(Vector3.Distance(cam.transform.position, transformsList[i].position) / Vector3.Distance(initPos, transformsList[i].position));
                cam.transform.rotation = Quaternion.Lerp(initRot, transformsList[i].rotation,temp);
                yield return null;
            }
        }

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
}
