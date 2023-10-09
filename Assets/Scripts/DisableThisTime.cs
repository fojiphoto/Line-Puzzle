using UnityEngine;
using System.Collections;

public class DisableThisTime : MonoBehaviour {
    public float DisableTime = 1.0f;
	void OnEnable()
    {
        Invoke("Disable",DisableTime);
    }

    void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
