using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onenableondisable : MonoBehaviour
{
    private void OnEnable()
    {
        AdsManager.instance?.ShowMRec();
    }
    private void OnDisable()
    {
        AdsManager.instance?.HideMRec();
    }
}
