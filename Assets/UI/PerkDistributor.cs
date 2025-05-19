using System.Collections.Generic;
using UnityEngine;

public class PerkDistributor : MonoBehaviour
{
    public GameObject[] perks;
    public GameObject[] meleWeapons;
    public GameObject[] rangeWeapons;

    public void DistributePerks()
    {
        gameObject.SetActive(true);
        Progress.instance.Pause();
        int count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject perk = perks[Random.Range(0, perks.Length)];
            transform.GetChild(i).GetComponent<PerkUi>().SetPerk(perk);
        }
    }

    public void DistributeWeapons(int style)
    {
        gameObject.SetActive(true);
        Progress.instance.Pause();
        int count = transform.childCount;
        List<GameObject> weapons = new List<GameObject>();
        if(style == 0)
        {
            weapons.AddRange(meleWeapons);
        }
        else if(style == 1)
        {
            weapons.AddRange(rangeWeapons);
        }
        else
        {
            weapons.AddRange(meleWeapons);
            weapons.AddRange(rangeWeapons);
        }
        for (int i = 0; i < count; i++)
        {
            GameObject weapon = weapons[Random.Range(0, weapons.Count)];
            transform.GetChild(i).GetComponent<PerkUi>().SetWeapon(weapon);
        }
    }

    private void OnDestroy()
    {
        if (gameObject.activeSelf)
        {
            Progress.instance.UnPause();
        }
    }
}
