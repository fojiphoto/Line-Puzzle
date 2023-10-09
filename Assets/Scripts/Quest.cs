using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public enum QuestType
{
    TypeMatch,ScoreMatch
}
public class Quest : MonoBehaviour {
    private static Quest sInstance;
    public static Quest Instance
    {
        get
        {
            return sInstance;
        }
    }

    public QuestType questType;
    public Candy[] Targets;
    public int ClearScore;  //점수내기 일 경우 목표 점수
    private int currentScore;
    private int showScore;
    public int[] ClearKill;   //갯수 없애기 일 경우 목표 갯수
    private int[] currentKill;
    public ScoreText scoreText;
    private bool scoreAdd;
    private float scoreFactor;
    public float clearTime;
    public float leftTime;
    public Image TimeGauge;
    public Text questText;
    public bool TimeCheck;

    public Image[] TargetImage;
    public GameObject GameClearPanel;

    private int scoreDelta;

    void Awake()
    {
        currentScore = 0;
        showScore = 0;
        currentKill = new int[Targets.Length];
        for (int i = 0; i < Targets.Length; i++)
        {
            currentKill[i] = 0;
        }
        sInstance = this;
        scoreFactor = 0.0f;
        leftTime = clearTime;
        switch(questType)
        {
            case QuestType.TypeMatch:
                questText.text = "시간이 끝나기 전에 지정된 갯수만큼 쿠키를 터뜨리세요";
                for (int i = 0; i < TargetImage.Length; i++)
                {
                    TargetImage[i].gameObject.SetActive(true);
                    TargetImage[i].sprite = Targets[i].GetComponent<SpriteRenderer>().sprite;
                }
                break;
            case QuestType.ScoreMatch:
                questText.text = "시간이 끝나기 전에 " + ClearScore + "점을 달성하세요";
                for (int i = 0; i < TargetImage.Length; i++)
                {
                    TargetImage[i].gameObject.SetActive(false);
                }
                break;
        }
    }

    void Update()
    {
        if (TimeCheck)
        {
            leftTime -= Time.deltaTime;
            TimeGauge.fillAmount = Mathf.Clamp01(leftTime / clearTime);
            if (leftTime <= 0)
                GameOver();
        }
        switch (questType)
        {
            case QuestType.TypeMatch:
                bool isClear = true;
                print(Targets.Length);
                for (int i = 0; i < Targets.Length; i++)
                {
                    TargetImage[i].transform.GetChild(0).GetComponent<Text>().text = Mathf.Clamp((ClearKill[i] - currentKill[i]),0,ClearKill[i]).ToString();
                    if (currentKill[i] < ClearKill[i])
                    {
                        isClear = false;
                    }
                }
                if (isClear)
                    GameClear();
                break;
            case QuestType.ScoreMatch:
                if (currentScore >= ClearScore)
                {
                    GameClear();
                }
                break;
        }
        if(scoreAdd)
        {
            int mount = scoreDelta / 5;
            showScore += mount;
            if(showScore >= currentScore)
            {
                scoreAdd = false;
                showScore = currentScore;
            }
        }
        scoreText.score = showScore;
    }

    public void KillCandy(Candy candy)
    {
        if (questType == QuestType.TypeMatch)
        {
            for (int i = 0; i < Targets.Length; i++)
            {
                if (Targets[i].tag.CompareTo(candy.tag) == 0)   //없어지는 캔디가 타겟에 있다.
                {
                    currentKill[i]++;
                    break;
                }
            }
        }
    }

    public void AddScore(int Score)
    {
        currentScore += Score;
        scoreDelta = currentScore - showScore;
        scoreAdd = true;
    }

    void GameClear()
    {
        print("게임끝났다");
        GameClearPanel.transform.Find("Text").GetComponent<Text>().text = "게임클리어!";
        GameClearPanel.SetActive(true);
    }

    void GameOver()
    {
        print("게임오버");
        GameClearPanel.transform.Find("Text").GetComponent<Text>().text = "게임오버...";
        GameClearPanel.SetActive(true);
    }
}
