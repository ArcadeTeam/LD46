using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var babyCount = other.GetComponent<DuckController>().nearBabies.Count;
            //Debug.Log("BabyCount: " + babyCount.ToString());
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().CheckWinCondition(babyCount);
        }
    }
}
