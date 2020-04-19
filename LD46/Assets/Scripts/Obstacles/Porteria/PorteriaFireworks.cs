using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorteriaFireworks : MonoBehaviour
{
    public Transform fireworkSpawner;
    public GameObject fireworks;

    public bool enter = false;
    public bool exit = false;

    public void checkGoal()
    {
        if (enter && exit)
        {
            Debug.Log("GOOOOOOOOOOOOOOOOOOOOOOOOOOOL");
            var obj = Instantiate(fireworks, fireworkSpawner);
            Destroy(obj, 5f);
        }
            
    }
}
