using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    void Awake()
    {
        int numberScenePersists = FindObjectsByType<ScenePersist>(FindObjectsSortMode.None).Length;
        if (numberScenePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
