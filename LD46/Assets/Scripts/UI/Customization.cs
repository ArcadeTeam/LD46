using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customization : MonoBehaviour
{
    public GameObject[] items;
    int currentIndex = 0;

    void Start() {
        foreach (GameObject go in items)
            go.SetActive(false);

        if (PlayerPrefs.HasKey("Suit")) {
            currentIndex = PlayerPrefs.GetInt("Suit");
            items[currentIndex].SetActive(true);
        }
    }

    public void NextItem() {
        if (currentIndex == 0)
            items[items.Length - 1].SetActive(false);
        else items[currentIndex - 1].SetActive(false);
        items[currentIndex].SetActive(true);
        PlayerPrefs.SetInt("Suit", currentIndex);

        currentIndex++;
        if (currentIndex == items.Length)
            currentIndex = 0;
    }
}
