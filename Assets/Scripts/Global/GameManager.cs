using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager mgr;

    Scene activeScene;
    int sceneIndex;

    private void Awake()
    {
        if (mgr == null)
        {
            mgr = this;
            DontDestroyOnLoad(this);
        }
        else if (mgr != this) Destroy(gameObject);

        SceneManager.activeSceneChanged += CheckScene;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadSceneAsync(1);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadSceneAsync(0);
        }
    }

    void CheckScene(Scene _, Scene scene2)
    {
        activeScene = scene2;
        sceneIndex = scene2.buildIndex;
    }

    public void ChangeScene(int index)
    {
        //if (SceneManager.GetSceneAt(index) != null);
        if (SceneManager.GetSceneByBuildIndex(index) != null)
        SceneManager.LoadSceneAsync(index);
    }
}
