using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakBubbles : MonoBehaviour
{
    [SerializeField] Image firstImg;
    [SerializeField] Image secondImg;
    [SerializeField] float time;

    float counter = 0.0f;
    void Start() {
        secondImg.enabled = false;        
    }
    void Update() {
        transform.rotation = Camera.main.transform.rotation;

        counter += Time.deltaTime;
        if(counter > time) {
            counter = 0f;
            if (firstImg.enabled) {
                secondImg.enabled = true;
                firstImg.enabled = false;
            } else {
                firstImg.enabled = true;
                secondImg.enabled = false;
            }
        }
    }
}
