using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    [SerializeField]
    private string startGameScene; // первая сцена, скорее всего сцена с меню.

    public static SceneLoader Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start () // сюда вставляете загрузку ГПС 
    {
        StartCoroutine(LoadScene(startGameScene));
	}

    private IEnumerator LoadScene(string startGameScene) // загружаем сцену 
    {
        yield return SceneManager.LoadSceneAsync(startGameScene, LoadSceneMode.Additive); // ждем загрузки сцены

        SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1)); // находим загруженную сцену и делаем активной
    }

    private IEnumerator UnloadSceneAndLoadNew(string name) // выгружаем ненужную сцену и загружаем нужную
    {
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));

        StartCoroutine(LoadScene(name));
    }

    public void LoadNewLevel(string name) // метод для кнопок или триггеров 
    {
        StartCoroutine(UnloadSceneAndLoadNew(name));
    }

}
