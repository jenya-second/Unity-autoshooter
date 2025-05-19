using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public delegate void MultiDelegate();
    public MultiDelegate onChangeLang;
    public static Settings instance = null;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        transform.GetChild(0).GetComponent<Scrollbar>().onValueChanged.AddListener((float val) => ChangeVolume(val));
        Transform lang = transform.GetChild(1);
        lang.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { ToRu(); });
        lang.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { ToEng(); });
        gameObject.SetActive(false);
    }

    void ChangeVolume(float v)
    {
        AudioListener.volume = v;
        Progress.instance.data.volume = v;
        Progress.instance.Save();
    }

    void ToRu()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        Transform lang = transform.GetChild(1);
        lang.GetChild(0).GetComponent<Image>().pixelsPerUnitMultiplier = 1;
        lang.GetChild(1).GetComponent<Image>().pixelsPerUnitMultiplier = 0.3f;
        Progress.instance.data.localization = 1;
        Progress.instance.Save();
        if (onChangeLang != null)
            onChangeLang();
    }

    void ToEng()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        Transform lang = transform.GetChild(1);
        lang.GetChild(0).GetComponent<Image>().pixelsPerUnitMultiplier = 0.3f;
        lang.GetChild(1).GetComponent<Image>().pixelsPerUnitMultiplier = 1;
        Progress.instance.data.localization = 0;
        Progress.instance.Save();
        if(onChangeLang != null)
            onChangeLang();
    }

    private void OnEnable()
    {
        transform.GetChild(0).GetComponent<Scrollbar>().value = AudioListener.volume;
    }
}
