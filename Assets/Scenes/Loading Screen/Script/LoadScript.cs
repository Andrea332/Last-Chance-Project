using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadScript : MonoBehaviour
{
    public Image progressBar;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(2);
        while (asyncOperation.progress < 1)
        {
            progressBar.fillAmount = asyncOperation.progress;
            yield return new WaitForEndOfFrame();
        }

    }
}
