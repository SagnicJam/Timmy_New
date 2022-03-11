using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicToggler : MonoBehaviour
{   
    public AudioSource audioSource;
    public Sprite muteSprite;
    public Sprite unMuteSprite;
    public Image target;
    public void ToggleAudioSource(){
        audioSource.mute = !audioSource.mute;
        if(audioSource.mute){
          target.sprite = muteSprite;  
        }else{
          target.sprite = unMuteSprite;
        }
    }
}
