using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureDetector : MonoBehaviour
{
    public float swipeThreshold;

    Vector3 downVec;
    Vector3 upVec;
    Vector3 deltaVec;
    Vector3 deltaVec2;
    bool inputgiven;
    private void Update()
    {
        if(PlayerController.instance == null)
        {
            return;
        }

        if (Time.timeScale == 0)
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            upVec = Input.mousePosition;
            deltaVec = upVec - downVec;
            float delta = deltaVec.magnitude;
            if (delta < swipeThreshold)
            {
                PlayerController.instance.WPressed();
            }
            inputgiven = false;
            PlayerController.instance.SUp();
        }

        if (Input.GetMouseButtonDown(0) && !inputgiven)
        {
            downVec = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && !inputgiven)
        {
            deltaVec2 = Input.mousePosition - downVec;
            float delta2 = deltaVec2.magnitude;

            if (delta2 > swipeThreshold)
            {
                if (deltaVec2.x< -swipeThreshold/2)
                {
                    inputgiven = true;
                    PlayerController.instance.APressed();
                    downVec = Input.mousePosition;
                }
                else if (deltaVec2.x > swipeThreshold / 2)
                {
                    inputgiven = true;
                    PlayerController.instance.DPressed();
                    downVec = Input.mousePosition;
                }
                else if (deltaVec2.y > 0)
                {
                    inputgiven = true;
                    PlayerController.instance.WPressed();
                    downVec = Input.mousePosition;
                }
                else if (deltaVec2.y < 0)
                {
                    inputgiven = true;
                    PlayerController.instance.SPressed();
                    downVec = Input.mousePosition;
                }
            }
        }
    }
}
