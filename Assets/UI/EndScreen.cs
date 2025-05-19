using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public static EndScreen instance = null;
    public LocalizedStringTable _ui;
    public AudioClip win;
    public AudioClip loss;

    void Start()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { GoToMenu(); });
        gameObject.SetActive(false);
    }

    public void End(bool loss)
    {
        gameObject.SetActive(true);
        var tableUI = _ui.GetTable();
        int index = Progress.instance.typeOfCharacter.GetComponent<PlayerBehavior>().characterIndex;
        string endWord = tableUI["end"].LocalizedValue;
        if (loss)
        {
            endWord +=  ": <color=\"red\">" + tableUI["loss"].LocalizedValue;
            GetComponent<AudioSource>().clip = this.loss;
            GetComponent<AudioSource>().Play();
        }
        else
        {
            endWord += " : <color=\"green\">" + tableUI["win"].LocalizedValue;
            GetComponent<AudioSource>().clip = win;
            GetComponent<AudioSource>().Play();
            Progress.instance.data.win[index] = true;
        }
        transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = endWord;
        string[] names = tableUI["statistic"].LocalizedValue.Split(',');
        string text = "";
        text += names[0] + ": " + Statistic.instance.experiense + "\n";
        text += names[1] + ": " + Statistic.instance.kills + "\n";
        text += names[2] + ": " + Statistic.instance.GetTime() + "\n";
        text += names[3] + ": " + Statistic.instance.healing + "\n";
        text += names[4] + ": " + Statistic.instance.damage + "\n";
        int stage = LevelProgress.instance.stage;
        if (stage == 11) stage--;
        text += tableUI["round"].LocalizedValue + ": " + stage + "\n";
        Progress.instance.data.scores[index] = Mathf.Max(Progress.instance.data.scores[index], Statistic.instance.experiense);
        transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = text;
        Progress.instance.Save();
    }

    void GoToMenu()
    {
        SceneManager.LoadScene(1);
        Progress.instance.ShowAdd(false);
    }
}
