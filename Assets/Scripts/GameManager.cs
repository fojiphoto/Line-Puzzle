using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private static GameManager sInstance;
    public static GameManager Instance
    {
        get
        {
            return sInstance;
        }
    }

    public GameObject sound;
    public GameObject Move;
    public Queue<Candy> candyQueue;
    public HashSet<Candy> containList;
    public Candy lastCandyType;
    public int MaxCandyCount;
    private int CurrentCandyCount = 0;
    public Transform SpawnPoint;
    public ObjectPool[] poolArray;
    public GameObject CheckPanel;
    private bool isSpawn = false;//스폰 중이냐?
    public List<GameObject> LineList;
    public ObjectPool[] EffectPool;
    public int MoveCount;
    public GameObject winPannel;
    public TMP_Text movetext;
    


    void Awake()
    {
       
        movetext.text =  MoveCount.ToString();
        candyQueue = new Queue<Candy>();
        StartCoroutine(SpawnCandy());
        candyQueue = new Queue<Candy>();
        containList = new HashSet<Candy>();
        LineList = new List<GameObject>();
        if (sInstance == null)
        {
            sInstance = this;
            //    DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (sInstance != this)
            {
                //    // If an instance already exists and it's not the current one, destroy the duplicate
                //    Destroy(gameObject);
            }
    }

    }

    void Update()
    {
        if(CurrentCandyCount < MaxCandyCount-10)
        {
            if(!isSpawn)
            {
                isSpawn = true;
                StartCoroutine(SpawnCandy());
            }
        }
    }

    IEnumerator SpawnCandy()
    {
        while (CurrentCandyCount < MaxCandyCount)
        {
            yield return null;
            int RandomNumber = Random.Range(0, poolArray.Length);
            GameObject activeObject = poolArray[RandomNumber].ActiveObject(SpawnPoint.position);
            if (activeObject != null)
            {
                activeObject.GetComponent<Candy>().CheckPanel = this.CheckPanel;
                CurrentCandyCount++;
                
            }
        }
        isSpawn = false;
    }

    public void DestroyCandy()
    {
        StartCoroutine(PopCandy());
    }

    IEnumerator PopCandy()
    {
        //MoveCount--;
        //movetext.text = MoveCount.ToString();
        for (int i = 0; i < LineList.Count; i++)
        {
            Destroy(LineList[i].gameObject);
        }
        LineList.Clear();
        if (candyQueue.Count < 3)
        {
           
            while (candyQueue.Count > 0)
            {
                Candy pop = candyQueue.Dequeue();
                pop.DeactiveWhite();
                
            }
            candyQueue.Clear();
            containList.Clear();
            lastCandyType = null;
            yield break;

           
        }
        

        int score = 0;
        int scoreAdd = 10;
        bool candiesPopped = false;

        while (candyQueue.Count > 0)
        {
            score += scoreAdd;
            scoreAdd *= 2;
            Quest.Instance.AddScore(score);
            Candy pop = candyQueue.Dequeue();
            containList.Remove(pop);
            pop.gameObject.SetActive(false);
            Quest.Instance.KillCandy(pop);
            Vector3 position = pop.transform.position;
            position.z = -2;
            int random = Random.Range(0, EffectPool.Length);
            EffectPool[random].ActiveObject(position);
            CurrentCandyCount--;
            yield return new WaitForSeconds(0.1f);
            candiesPopped = true;


        }

        if (candiesPopped)
        {
            // Only decrement the move count if candies were popped
            MoveCount--;
            movetext.text = MoveCount.ToString();
        }
        if (MoveCount ==0)
        {
            PlayerPrefs.SetInt("unlockedmovedLevel", (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1)-12);
            sound.SetActive(false);
            Move.SetActive(false);
            winPannel.SetActive(true);
            AdsManager.instance?.ShowInterstitialWithoutConditions();
        }
        
        
        }
   


}
