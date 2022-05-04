using UnityEngine;

public class ExitHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _exitWindow;

    public static event System.Action CloseButtonPressed;

    private void OnEnable()
    {
        CloseButtonPressed += OnCloseButtonPressed;
    }

    private void OnDisable()
    {
        CloseButtonPressed -= OnCloseButtonPressed;
    }

    private void OnCloseButtonPressed()
    {
        _exitWindow.SetActive(true);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    void Update()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MenuScene" && Input.GetKey(KeyCode.Escape))
        {
            CloseButtonPressed?.Invoke();
        }
    }
}
