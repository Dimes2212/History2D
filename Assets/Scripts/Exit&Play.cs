using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPlay : MonoBehaviour
{
    public void Play(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void ExitMenu()
    {
        SceneManager.LoadScene(0);
    }

}
