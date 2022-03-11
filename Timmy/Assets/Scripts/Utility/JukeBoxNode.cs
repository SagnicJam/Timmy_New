using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBoxNode : MonoBehaviour
{
    public static JukeBoxNode instance;
    public AudioSource bgMusic;
    public List<AudioSource> audioSources;

    void Awake(){
        instance = this;
    }

    void Start(){
        bgMusic.Play();
        bgMusic.mute = true;
    }

    public void PlayASS(int _index){
        audioSources[_index].Play();
    }
}
