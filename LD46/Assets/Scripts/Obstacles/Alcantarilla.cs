using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alcantarilla : MonoBehaviour
{
    private void OnCollisionEnter(Collision col) {
        if (col.gameObject.CompareTag("BabyDuck") || col.gameObject.CompareTag("Player")) {
            col.gameObject.GetComponent<Collider>().enabled = false;
            if (col.gameObject.CompareTag("Player")) {
                Camera.main.GetComponent<CameraFollower>().enabled = false;
            }
        }
    }
}
