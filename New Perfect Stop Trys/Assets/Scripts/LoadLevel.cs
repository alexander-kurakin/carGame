using UnityEngine;

public class LoadLevel : MonoBehaviour
{

    public void LoadStage(string name)
    {
        SceneLoader.Instance.LoadNewLevel(name);
    }
}
