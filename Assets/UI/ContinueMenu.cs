using UnityEngine;
using UnityEngine.UI;

public class ContinueMenu : MonoBehaviour
{

    public static ContinueMenu instance = null;
    public GameObject ui;
    public GameObject cb1;
    public GameObject cb2;
    bool update = true;


    private void Start()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        cb1 = transform.GetChild(0).gameObject;
        cb2 = transform.GetChild(1).gameObject;
        cb1.GetComponent<Button>().onClick.AddListener(delegate { clickContinue1(); });
        cb2.GetComponent<Button>().onClick.AddListener(delegate { clickContinue2(); });
        ui.SetActive(false);
    }

    void Update()
    {
        Image image = cb1.transform.GetChild(1).GetComponent<Image>();
        image.fillAmount -= Time.unscaledDeltaTime / 3;
        if (image.fillAmount == 0 && update)
        {
            end();
        }
    }

    void clickContinue1()
    {
        update = false;
        Progress.instance.ShowRewardedVideo();
    }

    public void rewarded()
    {
        cb1.SetActive(false);
        cb2.SetActive(true);
        Progress.instance.player.GetComponent<PlayerBehavior>().ContinueGame();
    }

    public void onClose()
    {
        if (cb1.activeSelf)
        {
            end();
        }
    }

    void clickContinue2()
    {
        ui.SetActive(false);
        Progress.instance.UnPause();
    }

    void end()
    {
        ui.SetActive(false);
        Progress.instance.player.GetComponent<PlayerBehavior>().Death();
    }


}