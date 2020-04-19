using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorteriaTriggerExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            var porteria = GetComponentInParent<PorteriaFireworks>();
            porteria.exit = true;
            porteria.checkGoal();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            GetComponentInParent<PorteriaFireworks>().exit = false;
        }
    }

}
