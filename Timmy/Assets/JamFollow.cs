using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JamFollow : MonoBehaviour
{
    public Transform targetirno;

    void Update()
    {
        transform.position = targetirno.position;
    }
}
