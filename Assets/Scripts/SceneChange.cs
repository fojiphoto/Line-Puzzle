using UnityEngine;
using System.Collections;

public class SceneChange : MonoBehaviour {
    public int SceneIdx = 0;
    public void ChangeScene()
    {
        SceneManager.Instance.SceneChangeColorFade(SceneIdx, 1.0f, Color.black);
    }
}
