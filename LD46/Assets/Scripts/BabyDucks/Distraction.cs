using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distraction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("BabyDuck"))
            other.GetComponent<BabyDuckController>().Distract(transform);
    }
}
