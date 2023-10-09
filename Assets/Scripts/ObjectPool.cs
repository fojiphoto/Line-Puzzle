using UnityEngine;
using System.Collections;

public class ObjectPool : MonoBehaviour {
	public GameObject targetObject;
	public int poolSize = 100;
	private int nextIndex = 0;
	private GameObject[] pool;
	void Awake()
	{
		this.transform.position = Vector3.zero;
		pool = new GameObject[poolSize];
		for(int i=0;i<pool.Length;i++)
		{
			pool[i] = Instantiate(targetObject, this.transform.position, Quaternion.identity)as GameObject;
            pool[i].transform.position = this.transform.position;
            pool[i].transform.SetParent(this.transform);
            pool[i].SetActive(false);
		}
	}

    public GameObject ActiveObject(Vector3 position)
    {
        if (pool.Length > 0)
        {
            if (nextIndex > pool.Length - 1) nextIndex = 0;
            GameObject temp = pool[nextIndex];
            if(temp.activeSelf == true)
            {
                nextIndex++;
                if (nextIndex > pool.Length - 1) nextIndex = 0;
                while(true)
                {
                    temp = pool[nextIndex];
                    if(temp.activeSelf == false)
                    {
                        break;
                    }
                    else
                    {
                        nextIndex++;
                        if (nextIndex > pool.Length - 1) nextIndex = 0;
                    }
                }
            }
            temp.transform.localPosition = position;
            temp.SetActive(true);
            nextIndex++;

            return temp;
        }
        else
        {
            return null;
        }
    }
}
