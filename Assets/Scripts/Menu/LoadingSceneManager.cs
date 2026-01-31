using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static LoadingSceneManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public Slider ProgressBar;
    void Start()
    {
        if (ProgressBar != null)
        {
            ProgressBar.value = 0;
        }
        StartCoroutine(SwitchToScene(SceneKey.targetScene));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator SwitchToScene(string SceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneName);
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < 0.9f)
        {
            ProgressBar.value = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            yield return null;
        }
        ProgressBar.value = 1f;
        yield return new WaitForSeconds(0.1f);
        asyncOperation.allowSceneActivation = true;
    }
}
