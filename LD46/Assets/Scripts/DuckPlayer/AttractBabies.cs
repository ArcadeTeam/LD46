using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractBabies : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("BabyDuck"))
            other.GetComponent<BabyDuckController>().GoWithMom(transform);
    }
}
