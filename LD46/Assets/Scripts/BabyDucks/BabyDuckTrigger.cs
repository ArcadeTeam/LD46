using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDuckTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Human"))
        {
            GetComponentInParent<BabyDuckController>().HumanDetected(other.transform.position);
        }

        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<DuckController>().AddBaby(GetComponentInParent<BabyDuckController>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<DuckController>().RemoveBaby(GetComponentInParent<BabyDuckController>());
        }
    }
}
