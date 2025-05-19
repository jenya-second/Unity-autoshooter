using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public static InGameUI instance = null;
    public LocalizedStringTable _ui;
    string round = "";
    string endWave = "";
    string lvl = "";

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        SetText();
        Settings.instance.onChangeLang += SetText;
    }

    void SetText()
    {
        var ui = _ui.GetTable();
        round = ui["round"].LocalizedValue;
        endWave = ui["endWave"].LocalizedValue;
        lvl = ui["lvl"].LocalizedValue;
    }

    void Update()
    {
        Transform HP = transform.GetChild(0);
        Transform EXP = transform.GetChild(1);
        Transform Timer = transform.GetChild(2);
        HP.GetChild(2).GetComponent<Image>().fillAmount = GetHealthProgressBar();
        HP.GetChild(3).GetComponent<TMP_Text>().text = GetHealth();
        EXP.GetChild(2).GetComponent<Image>().fillAmount = GetExperienceProgressBar();
        EXP.GetChild(3).GetComponent<TMP_Text>().text = GetExp();
        EXP.GetChild(4).GetComponent<TMP_Text>().text = GetExpLevel();
        if (LevelProgress.instance.inProgress)
        {
            Timer.GetComponent<TMP_Text>().text = GetTime();
        }
        else
        {
            Timer.GetComponent<TMP_Text>().text = "";
        }

    }

    string GetHealth()
    {
        string s;
        GameObject player = Progress.instance.player;
        if (player == null) return "-";
        PlayerBehavior script = player.GetComponent<PlayerBehavior>();
        s = Mathf.Round(script.health) + " / " + script.maxHealth;
        return s;
    }

    float GetHealthProgressBar()
    {
        float s;
        GameObject player = Progress.instance.player;
        if (player == null) return 0;
        PlayerBehavior script = player.GetComponent<PlayerBehavior>();
        s = script.health / script.maxHealth;
        return s;
    }
    string GetExp()
    {
        string s;
        GameObject player = Progress.instance.player;
        if (player == null) return "-";
        PlayerBehavior script = player.GetComponent<PlayerBehavior>();
        s = script.experience + " / " + (script.level * script.expByLevel);
        return s;
    }

    float GetExperienceProgressBar()
    {
        float s;
        GameObject player = Progress.instance.player;
        if (player == null) return 0;
        PlayerBehavior script = player.GetComponent<PlayerBehavior>();
        s = script.experience / (script.level * script.expByLevel);
        return s;
    }

    string GetExpLevel()
    {
        string s;
        GameObject player = Progress.instance.player;
        if (player == null) return "-";
        PlayerBehavior script = player.GetComponent<PlayerBehavior>();
        s = lvl + " " + script.level;
        return s;
    }

    string GetTime()
    {
        float time = LevelProgress.instance.timeOfStage - LevelProgress.instance.time;
        string t = round + " " + LevelProgress.instance.stage + "\n" + (int)time;
        return t;
    }

    IEnumerator SetTextToTimer(string text, bool sound, float delayTime, bool onRound)
    {
        yield return new WaitForSeconds(delayTime);
        if (onRound) text = round + " " + LevelProgress.instance.stage;
        Transform ReverseTimer = transform.GetChild(3);
        ReverseTimer.GetComponent<TMP_Text>().text = text;
        if (sound) GetComponent<AudioSource>().Play();
    }

    public void InvokeTimer()
    {
        Transform ReverseTimer = transform.GetChild(3);
        
        ReverseTimer.GetComponent<Animator>().SetBool("Work", true);
        StartCoroutine(SetTextToTimer("3", true, 0,false));
        StartCoroutine(SetTextToTimer("2", true, 0.9f, false));
        StartCoroutine(SetTextToTimer("1", true, 1.9f, false));
        StartCoroutine(SetTextToTimer("", true, 2.9f, true));
        StartCoroutine(SetTextToTimer("", false, 3.7f, false));
    }

    public void InvokeEndTimer()
    {
        Transform EndWaveTimer = transform.GetChild(4);
        EndWaveTimer.gameObject.SetActive(true);
        EndWaveTimer.GetComponent<TMP_Text>().text = endWave;
        Transform ReverseTimer = transform.GetChild(3);
        ReverseTimer.GetComponent<Animator>().SetBool("Work", false);
        Invoke(nameof(SetE), 1.9f);
    }

    private void SetE()
    {
        Transform EndWaveTimer = transform.GetChild(4);
        EndWaveTimer.gameObject.SetActive(false);
    }
}
