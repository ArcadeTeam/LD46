using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorteriaTriggerEnter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            GetComponentInParent<PorteriaFireworks>().enter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            GetComponentInParent<PorteriaFireworks>().enter = false;
        }
    }

}
