using UnityEngine;
using UnityEngine.SceneManagement;
public class Firstkat : MonoBehaviour
{
    public void First(int index)
    {
        SceneManager.LoadScene(index);
    }
}
