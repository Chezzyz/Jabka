using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Pause : MonoBehaviour
{
    public static event System.Action<bool> PauseStateChanged;
    private void OnEnable()
    {
        SceneLoader.SceneLoadStarted += OnSceneLoadStarted;
        SceneLoader.SceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneLoader.SceneLoadStarted -= OnSceneLoadStarted;
        SceneLoader.SceneLoaded -= OnSceneLoaded;
    }

    public void PauseOn()
    {
        PauseStateChanged?.Invoke(true);
        Time.timeScale = 0;
    }

    public void PauseOff()
    {
        PauseStateChanged?.Invoke(false);
        Time.timeScale = 1;
    }

    private void OnSceneLoaded(string _)
    {
        Time.timeScale = 1;
    }

    private void OnSceneLoadStarted(float _)
    {
        GetComponent<Button>().interactable = false;
        PauseOff();
    }

}
