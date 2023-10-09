using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ScoreText : MonoBehaviour {
    private Sprite[] ScoreSprite;
    public int score;
    private Transform[] childs;
    private string intToString;
	void Awake()
    {
        ScoreSprite = Resources.LoadAll<Sprite>("Textures/font_number_sprite");
        childs = this.transform.GetComponentsInChildren<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        intToString = string.Format("{0:D8}", score);
        for(int i=1;i<childs.Length;i++)
        {
            childs[i].GetComponent<SpriteRenderer>().sprite = ScoreSprite[int.Parse(intToString.Substring(i-1,1))];
        }
	}
}
