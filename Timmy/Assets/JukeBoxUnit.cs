using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBoxUnit : MonoBehaviour
{
    public static JukeBoxUnit instance;
    public List<GameObject> vfxes;
    public List<AudioSource> sfxes;

    private void Awake()
    {
        instance = this;
    }

    public void PlayVFX(int id,Vector3 pos)
    {
        GameObject g = Instantiate(vfxes[id]) as GameObject;
        g.transform.position = pos;
    }

    public void PlaySFX(int id)
    {
        sfxes[id].Play();
    }
}
