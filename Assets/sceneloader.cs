using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sceneloader : MonoBehaviour
{
    
    public Slider slider;
    public TMP_Text progressText;
    public int scene = 1;
    public float loadingDuration = 5.0f;
    private void Start()
    {
        StartCoroutine(LoadAsynchronously(scene));
    }
    

       
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        yield return new WaitForSeconds(loadingDuration);

        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);

       

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value= progress;
            progressText.text = (progress * 100f).ToString("F0") + "%";

            yield return null;
        }
    }
}
