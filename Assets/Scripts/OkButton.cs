using UnityEngine;
using System.Collections;

public class OkButton : MonoBehaviour {

    public void pressOkButton()
    {
        this.GetComponent<Animator>().Play("MenuUp");
        Quest.Instance.TimeCheck = true;
    }
}
