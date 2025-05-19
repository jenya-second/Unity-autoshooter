using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class PlayerData
{
    public float[] scores;
    public bool[] win;
    public float volume;
    public int localization;

    public PlayerData()
    {
        scores = new float[4] { 0, 0, 0, 0 };
        win = new bool[4] { false, false, false, false };
        volume = 1;
        localization = 0;
    }
}

public class Progress : MonoBehaviour
{
    public GameObject typeOfCharacter = null;
    public GameObject player = null;
    float serverTime = 0;
    float lastTimeCallAds = 0;
    int pauses = 0;
    public PlayerData data = new PlayerData();
    public bool Desktop = true;

    public static bool paused = false; 
    public static Progress instance;

    [DllImport("__Internal")]
    public static extern void SaveExtern(string data);

    [DllImport("__Internal")]
    public static extern void LoadExtern();

    [DllImport("__Internal")]
    public static extern void GetTime();

    [DllImport("__Internal")]
    public static extern void ShowAddExtern();

    [DllImport("__Internal")]
    public static extern void ShowRewardedVideoExtern();

    [DllImport("__Internal")]
    public static extern void GRAReady();

    public void Save()
    {
#if !UNITY_EDITOR
        string jsonString = JsonUtility.ToJson(data);
        SaveExtern(jsonString);
#endif
    }

    public void SetTime(float time)
    {
        serverTime = time;
    }

    public void RewardRecieved()
    {
        ContinueMenu.instance.rewarded();
    }

    public void ShowAdd(bool withPause)
    {
#if !UNITY_EDITOR
        if (!CanShowAds()) return;
        lastTimeCallAds = serverTime;
        Pause();
        AudioListener.volume = 0;
        if (withPause)
        {
            Invoker.InvokeDelayed(ShowAddExtern, 2f);
            InGameUI.instance.transform.GetChild(5).gameObject.SetActive(true);
        }
        else
        {
            ShowAddExtern();
        }
#endif
    }

    public void OnCloseSimpleAdv()
    {
        AudioListener.volume = data.volume;
        UnPause();
        InGameUI inst = InGameUI.instance;
        if (inst == null) return;
        inst.transform.GetChild(5).gameObject.SetActive(false);   
    }

    public void ShowRewardedVideo()
    {
#if !UNITY_EDITOR
        AudioListener.volume = 0;
        ShowRewardedVideoExtern();
#endif
    }

    public void OnCloseRewardVideo()
    {
        AudioListener.volume = data.volume;
        ContinueMenu.instance.onClose();
    }

    public void SetPlayerInfo(string value)
    {
#if !UNITY_EDITOR
        PlayerData test = JsonUtility.FromJson<PlayerData>(value);
        if(test == null)
        {
            Save();
        }
        else
        {
            data = test;
            AudioListener.volume = data.volume;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[data.localization];
        }
#endif
    }

    public void SetSystem(int s)
    {
        if (s == 0)
        {
            Desktop = true;
        }
        else
        {
            Desktop = false;
        }
        
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            transform.parent = null;
#if !UNITY_EDITOR
            LoadExtern();
            GRAReady();
#endif
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static float Func(float x)
    {
        if (x >= 1) return x;
        return -10 / (x - 10);
    }

    public void Pause()
    {
        Debug.Log("Paused");
        pauses++;
        Time.timeScale = 0;
        paused = true;
    }

    public void UnPause()
    {
        Debug.Log("UnPaused");
        pauses--;
        if (pauses <= 0)
        {
            pauses = 0;
            Time.timeScale = 1;
            paused = false;
        }
    }

    public bool CanShowAds()
    {
#if !UNITY_EDITOR
        GetTime();
        float t = serverTime - lastTimeCallAds;
        return t / 60 > 1;
#else
        return false;
#endif
    }
}
