using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public static PauseUI instance = null;

    void Start()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        Transform panel =  transform.GetChild(0);
        panel.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        panel.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { Unpause(); });
        panel.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
        panel.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate { GoToMenu(); });
        gameObject.SetActive(false);
        Settings.instance.onChangeLang += UpdateStats;
    }
    
    void Unpause()
    {
        Progress.instance.UnPause();
        gameObject.SetActive(false);
    }

    public void UpdateStats()
    {
        GameObject player = Progress.instance.player;
        if (player == null) return;
        Transform panel = transform.GetChild(0);
        panel.GetChild(4).GetComponent<TMP_Text>().text = player.GetComponent<PlayerBehavior>().GetStats();
    }

    void GoToMenu()
    {
        Progress.instance.UnPause();
        SceneManager.LoadScene(1);
        Progress.instance.ShowAdd(false);
    }
}
