using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapoTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("BabyDuck"))
            other.transform.parent = transform;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("BabyDuck"))
            other.transform.parent = null;
    }

}
