using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CrowdManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] crowdNoises = new AudioClip[8]; // make sure to order them from negative to positive
    public int zeroPoint = 4; // from the list of audiofiles, where is the zero point? This will be used for silence
    float inputValue; // between -100 and 100
    int currentAudioNumber;

    private void ConvertValueToAudio(float inputValue){
        int convertedValue = 0;
        if (inputValue == 0){
            inputValue = zeroPoint;
        }
        if (inputValue < 0){
            convertedValue = (int)(inputValue / 100 * (zeroPoint-1));
        }
        if (inputValue > 0){
            convertedValue = (int)(inputValue / 100 * (crowdNoises.Length-zeroPoint-1));
        }
        Debug.Log("New val: " + convertedValue);
        convertedValue+= zeroPoint;

        if (convertedValue != currentAudioNumber){
            source.Stop();
            source.PlayOneShot(crowdNoises[convertedValue]);
            //Debug.Log("Playing sound " + convertedValue);
            currentAudioNumber = convertedValue;
        }
    }

    void Start(){
        StartCoroutine(GiveValue());
    }

    private IEnumerator GiveValue(){
        while (true){
            yield return new WaitForSeconds(2);
            float newVal = (Random.value-0.5f) * 200;
            ConvertValueToAudio(newVal);
        }
        
    }
}