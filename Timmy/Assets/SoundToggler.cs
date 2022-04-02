using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundToggler : MonoBehaviour
{
    public bool togglapan;

    public void ToggleIt()
    {
        togglapan = !togglapan;
        if (togglapan)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

    public void OpenUTube()
    {
        Application.OpenURL("https://www.youtube.com/user/tommyturnpike");
    }
}
