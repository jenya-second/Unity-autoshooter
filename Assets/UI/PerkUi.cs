using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkUi : MonoBehaviour
{
    GameObject perk;
    int perkType;

    private void Start()
    {
        Settings.instance.onChangeLang += SetText;
    }

    public void SetPerk(GameObject perk)
    {
        this.perk = perk;
        perkType = 0;
        SetText();
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(delegate { ApplyPerk(); });
        
    }

    public void SetWeapon(GameObject weapon)
    {
        this.perk = weapon;
        perkType = 1;
        SetText();
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(delegate { ApplyWeapon(); });
    }

    void SetText()
    {
        if (perk == null) return;
        if(perkType == 1)
        {
            transform.GetChild(0).GetComponent<Image>().sprite = perk.GetComponent<WeaponBehavior>().image;
            transform.GetChild(1).GetComponent<TMP_Text>().text = perk.GetComponent<WeaponBehavior>().GetText();
            transform.GetChild(2).GetComponent<TMP_Text>().text = perk.GetComponent<WeaponBehavior>().GetName();
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().sprite = perk.GetComponent<Perk>().image;
            transform.GetChild(1).GetComponent<TMP_Text>().text = perk.GetComponent<Perk>().GetText();
            transform.GetChild(2).GetComponent<TMP_Text>().text = perk.GetComponent<Perk>().GetName();
        }
        
    }

    void ApplyPerk()
    {
        Progress.instance.UnPause();
        Progress.instance.player.GetComponent<PlayerBehavior>().AddPerk(perk);
        Progress.instance.player.transform.GetChild(2).gameObject.SetActive(false);
    }

    void ApplyWeapon()
    {
        Progress.instance.UnPause();
        Progress.instance.player.GetComponent<PlayerBehavior>().AddWeapon(perk);
        Progress.instance.player.transform.GetChild(2).gameObject.SetActive(false);
    }
}
