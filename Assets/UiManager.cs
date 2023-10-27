using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{

    public void PressOkBtn()
    {
        Quest.Instance.TimeCheck = true;
    }

}
