using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class timelevelConroller : MonoBehaviour
{
    public Button[] buttons; public GameObject[] locks;

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("unlocktimeLevel", 1);
        Debug.Log("unlock level " + unlockedLevel);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
            locks[i].SetActive(false);
        }
    }
}
