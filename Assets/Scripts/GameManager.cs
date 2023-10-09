using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameManager : MonoBehaviour {
    private static GameManager sInstance;
    public static GameManager Instance
    {
        get
        {
            return sInstance;
        }
    }

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
    void Awake()
    {
        if (sInstance == null)
            sInstance = this;
        candyQueue = new Queue<Candy>();
        StartCoroutine(SpawnCandy());
        candyQueue = new Queue<Candy>();
        containList = new HashSet<Candy>();
        LineList = new List<GameObject>();
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
        for (int i = 0; i < LineList.Count; i++)
        {
            Destroy(LineList[i].gameObject);
        }
        LineList.Clear();
        if (candyQueue.Count < 3)
        {
            while(candyQueue.Count > 0)
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
        while(candyQueue.Count > 0)
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
        }
    }
}
