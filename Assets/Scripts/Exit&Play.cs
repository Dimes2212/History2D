using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPlay : MonoBehaviour
{
    private static string previousSceneName; // Статическая переменная для хранения имени предыдущей сцены

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
