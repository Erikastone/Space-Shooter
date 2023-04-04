using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void ToNextScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
