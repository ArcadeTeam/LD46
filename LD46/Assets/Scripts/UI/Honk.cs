using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Honk : SpeakBubbles
{
    GameObject player;

    void Start() {
        player = GameObject.Find("DuckPlayer");
    }

    // Update is called once per frame
    void FixedUpdate() {
        firstImg.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, -player.transform.rotation.eulerAngles.y - 90f));
        secondImg.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, -player.transform.rotation.eulerAngles.y - 90f));
    }
}
