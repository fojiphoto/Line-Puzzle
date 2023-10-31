using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changescene : MonoBehaviour
{
    public GameObject loadingpannel,loadmanager;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(change), 5);
        Invoke(nameof(bannershow), 4.5f);
    }
    public void bannershow()
    {
        AdsManager.instance?.ShowBanner();
    }
    public void change()
    {
        loadmanager.SetActive(true);
        loadingpannel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
