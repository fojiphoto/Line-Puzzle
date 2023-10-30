using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class url : MonoBehaviour
{
    public string urlToOpen = "https://play.google.com/store/apps/developer?id=Orbit+Games+Global"; // Set the URL you want to open

    public void OpenExternalURL()
    {
        // Open the URL in a web browser
        Application.OpenURL(urlToOpen);
    }
    public string urlToopen = "https://orbitgamesglobal-privacy-policy.blogspot.com/"; // Set the URL you want to open

    public void OpenPrivacyURL()
    {
        // Open the URL in a web browser
        Application.OpenURL(urlToopen);
    }
    public string urltoopen = "https://play.google.com/store/apps/details?id="; // Set the URL you want to open

    public void OpenrateusURL()
    {
        // Open the URL in a web browser
        Application.OpenURL(urltoopen);
    }
}

