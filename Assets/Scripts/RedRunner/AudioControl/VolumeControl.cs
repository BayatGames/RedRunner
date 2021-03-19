using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class VolumeControl : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private string audioTargetName = "";
  
    public void SetVolumeLevel(float sliderValue)
    {
        mixer.SetFloat(audioTargetName, Mathf.Log10(sliderValue) * 20);
    }
    
}
