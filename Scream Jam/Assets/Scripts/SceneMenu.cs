using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("finalScene");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Title()
    {
        SceneManager.LoadScene("Title");
    }
}
