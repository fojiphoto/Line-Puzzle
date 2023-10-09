using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Candy : MonoBehaviour{
    public GameObject CheckPanel;
    private GameObject white;
    void Awake()
    {
        white = transform.Find("white").gameObject;
    }

    void OnEnable()
    {
        white.gameObject.SetActive(false);
    }

    void OnMouseDown()
    {
        if(GameManager.Instance.candyQueue.Count == 0)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            CheckPanel.transform.position = position;
            CheckPanel.GetComponent<CheckPanel>().CheckStart();
        }
    }

    void OnMouseDrag()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        CheckPanel.transform.position = position;
    }

    void OnMouseUp()
    {
        CheckPanel.GetComponent<CheckPanel>().CheckEnd();
        GameManager.Instance.DestroyCandy();
    }

    public void ActiveWhite()
    {
        white.SetActive(true);
    }

    public void DeactiveWhite()
    {
        white.SetActive(false);
    }
}
