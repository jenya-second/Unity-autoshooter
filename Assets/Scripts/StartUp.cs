using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUp : MonoBehaviour
{

    public void OnLoad()
    {
        SceneManager.LoadScene(1);
    }

}
