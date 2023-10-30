using UnityEngine;


public class SceneChange : MonoBehaviour {
    public int SceneIdx = 0;
    public void ChangeScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneIdx);

    }
}
