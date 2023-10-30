using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exit : MonoBehaviour
{
    public void ExitToHomeScreen()
    {
        // Use Application.Quit to exit the game and return to the mobile home screen
        Application.Quit();

        // Note: Application.Quit may not work in the Unity Editor, so test it on the target platform (e.g., Android or iOS).
    }
}
