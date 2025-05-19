using UnityEngine;

public class LevelProgress : MonoBehaviour
{
    public int stage = 1;
    public static LevelProgress instance;
    public float time = 3;
    public bool inProgress = false;
    public int endStage = 11;
    public InGameUI inGameUI;
    public float timeOfStage { get => 21 + 4 * stage;}

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.parent = null;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("Start");
        StartTimer();
        Invoke("StartStage", 3.5f);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToglePause();
        }
        if (inProgress)
        {
            time += Time.deltaTime;
        }
    }

    public void ToglePause()
    {
        GameObject pause = PauseUI.instance.gameObject;
        if (pause.activeInHierarchy)
        {
            Progress.instance.UnPause();
            PauseUI.instance.gameObject.SetActive(false);
        }
        else
        {
            Progress.instance.Pause();
            PauseUI.instance.gameObject.GetComponent<PauseUI>().UpdateStats();
            PauseUI.instance.gameObject.SetActive(true);
        }
    }

    void StartStage()
    {
        time = 0;
        Debug.Log("StartStage");
        Invoke("EndStage", timeOfStage);
        inProgress = true;
        EnemySpawner.instance.StartSpawning();
    }

    void EndStage()
    {
        Debug.Log("EndStage");
        CancelInvoke("StartSpawning");
        stage += 1;
        inProgress = false;
        EnemySpawner.instance.KillAllYourFriends();
        if(stage == endStage)
        {
            EndGame();
            EndScreen.instance.End(false);
            return;
        }
        InGameUI.instance.InvokeEndTimer();
        Invoke("ShowAds", 2);
        Invoke("DistrWeapons", 2.2f);
        Invoke("StartTimer", 3);
        Invoke("StartStage", 6);   
    }

    public void EndGame()
    {
        CancelInvoke("EndStage");
        inProgress = false;
        Debug.Log("EndGame");
    }

    void StartTimer()
    {
        inGameUI.InvokeTimer();
    }

    void ShowAds()
    {
        Progress.instance.ShowAdd(true);
    }

    void DistrWeapons()
    {
        GameObject pl = Progress.instance.player;
        pl.GetComponent<PlayerBehavior>().health = pl.GetComponent<PlayerBehavior>().maxHealth;
        int style = pl.GetComponent<PlayerBehavior>().style;
        pl.transform.GetChild(2).GetComponent<PerkDistributor>().DistributeWeapons(style);
    }

}
