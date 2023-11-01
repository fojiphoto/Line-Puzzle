using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneChange : MonoBehaviour {
    public int SceneIdx = 0;
  

    public void ChangeScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneIdx);

    }
}
