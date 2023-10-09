using UnityEngine;
using System.Collections;

public class CheckPanel : MonoBehaviour {
    private SphereCollider col;
    public float CheckDistance = 2.0f;
    public GameObject Line;
    void Awake()
    {
        col = this.GetComponent<SphereCollider>();
        col.enabled = false;
    }

    public void CheckStart()
    {
        col.enabled = true;
    }

    public void CheckEnd()
    {
        col.enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        Candy checkCandy = col.gameObject.GetComponent<Candy>();
        if (checkCandy != null) //충돌한게 캔디라면
        {
            if (GameManager.Instance.containList.Contains(checkCandy)) return;  //이미 체크했다면 넘긴다

            //이까지 왔다면 새로운놈이다
            if (GameManager.Instance.candyQueue.Count == 0) //처음 충돌했다면
            {
                checkCandy.ActiveWhite();
                GameManager.Instance.candyQueue.Enqueue(checkCandy);
                GameManager.Instance.lastCandyType = checkCandy;
                GameManager.Instance.containList.Add(checkCandy);
                print("첫번째 멤버로 " + checkCandy.gameObject.name + "추가했다");
            }
            else //두번째 이상 충돌했다면
            {
                if(col.tag.CompareTo(GameManager.Instance.lastCandyType.tag)==0) //마지막에 들어온거랑 타입이 같으면
                {
                    float distance = Vector3.Distance(col.gameObject.transform.position, GameManager.Instance.lastCandyType.transform.position);
                    if(distance <= CheckDistance)   //이까지 왔다면 타입이 같고 인접했다(거리가 가깝다)
                    {
                        print(GameManager.Instance.candyQueue.Count + "번째 멤버로 " + checkCandy.gameObject.name + "추가했다");
                        checkCandy.ActiveWhite();
                        Vector3 pos1 = GameManager.Instance.lastCandyType.transform.parent.TransformPoint(GameManager.Instance.lastCandyType.transform.localPosition);
                        pos1.z = -1;
                        Vector3 pos2 = checkCandy.transform.parent.TransformPoint(checkCandy.transform.localPosition);
                        pos2.z = -1;
                        GameObject line = Instantiate(Line, pos1, Quaternion.identity) as GameObject;
                        line.GetComponent<ParticleLine>().endTransform = checkCandy.transform;
                        GameManager.Instance.LineList.Add(line);
                        GameManager.Instance.candyQueue.Enqueue(checkCandy);
                        GameManager.Instance.lastCandyType = checkCandy;
                        GameManager.Instance.containList.Add(checkCandy);
                    }
                }
            }
        }
    }
}
