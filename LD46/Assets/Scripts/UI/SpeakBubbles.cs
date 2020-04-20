using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakBubbles : MonoBehaviour
{
    public Image firstImg;
    public Image secondImg;
    public float time;
    public Transform targetPos;
    public Vector3 offsetPos = Vector3.zero;

    float counter = 0.0f;

    void Start() {
        secondImg.enabled = false;
    }
    void Update() {
        transform.rotation = Camera.main.transform.rotation;

        if(targetPos != null) {
            transform.position = targetPos.position + offsetPos;
        }

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
