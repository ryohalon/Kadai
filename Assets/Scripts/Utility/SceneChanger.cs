using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public string nextSceneName = null;

    public void ChangeScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
