using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class PlayerBehavior : MonoBehaviour
{
    public float maxHealth = 100;
    public float shield = 0;
    public float damageIncrease = 0;
    public float speed = 10;
    public float healing = 1;
    public float attackSpeed = 0;

    public float expByLevel = 10;
    public float health = 100;
    public float level = 1;
    public float experience = 0;
    public float money = 0;
    public List<GameObject> weapons;
    float meleWeaponCount = 0;

    public Sprite image;
    public string nameOfCharacter = "aboba";
    public int characterIndex;

    public bool perk = true;
    public int style = 0;
    bool died = false;

    public LocalizedStringTable _ui;
    public AudioClip ExpRec;
    public AudioClip LevelUp;

    void Start()
    {
        health = maxHealth;
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].transform.GetChild(0).GetComponent<RangeWeapon>() == null)
            {
                meleWeaponCount++;
            }
            weapons[i] = Instantiate(weapons[i], gameObject.transform);
        }
        SetRotationToWeapons();
        AutoHeal();
    }

    public void ApplyDamage(float damage)
    {
        transform.GetChild(3).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.SetFloat("_gr", 0);
        Invoke("ChangeColor", 0.1f);
        if (shield <= 0)
        {
            ChangeHealth(-1 * damage);
        }
        else
        {
            ChangeHealth(-1 * damage * (1 / (1 + shield / 15)));
        }
    }

    void ChangeColor()
    {
        transform.GetChild(3).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.SetFloat("_gr", 1);
    }

    void AutoHeal()
    {
        Statistic.instance.healing++;
        ChangeHealth(1);
        Invoke("AutoHeal", 2 / (Progress.Func(healing) / (1 + Progress.Func(healing) / 15)));
    }

    void ChangeHealth(float value)
    {
        health = Mathf.Clamp(health + value, 0, maxHealth);
        if (health == 0)
        {
            if (Progress.paused) return;
            Progress.instance.Pause();
            if (died)
            {
                Death();
                return;
            }
            ContinueMenu.instance.ui.SetActive(true);
        }
    }

    public void Death()
    {
        Progress.instance.UnPause();
        gameObject.GetComponentInChildren<Camera>().GetComponent<CameraMovement>().enabled = true;
        gameObject.GetComponentInChildren<Camera>().transform.parent = null;
        LevelProgress.instance.EndGame();
        EndScreen.instance.End(true);
        Destroy(gameObject);
    }

    public void ContinueGame()
    {
        died = true;
        ChangeHealth(maxHealth / 2);
    }

    void SetRotationToWeapons()
    {
        float y = 360 / meleWeaponCount;
        float z = 360 / (weapons.Count - meleWeaponCount);
        int q = 0, w = 0;
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].transform.rotation = transform.rotation;
            weapons[i].transform.position = transform.position;
        }
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].transform.GetChild(0).GetComponent<RangeWeapon>() == null)
            {
                weapons[i].transform.Rotate(0, y * q, 0);
                q++;
            }
            if (weapons[i].transform.GetChild(0).GetComponent<RangeWeapon>() != null)
            {
                Quaternion rot = Quaternion.AngleAxis(z * w, Vector3.up);
                Vector3 aboba = (rot * Vector3.forward) + weapons[i].transform.position;
                weapons[i].transform.position = aboba;
                weapons[i].transform.rotation = rot;
                w++;
            }
        }
    }

    public void AddWeapon(GameObject weaponType)
    {
        if (weaponType.transform.GetChild(0).GetComponent<RangeWeapon>() == null)
        {
            meleWeaponCount++;
        }
        GameObject weapon = Instantiate(weaponType, gameObject.transform);
        weapons.Add(weapon);
        SetRotationToWeapons();
    }

    public void AddExperience(float value)
    {
        GetComponent<AudioSource>().clip = ExpRec;
        GetComponent<AudioSource>().Play();
        money += 1;
        experience += value;
        Statistic.instance.experiense += value;
        if (experience >= level * expByLevel)
        {
            experience -= (level * expByLevel);
            level += 1;
            GetComponent<AudioSource>().clip = LevelUp;
            GetComponent<AudioSource>().Play();
            if (perk)
            {
                transform.GetChild(2).GetComponent<PerkDistributor>().DistributePerks();
            }
            else
            {
                transform.GetChild(2).GetComponent<PerkDistributor>().DistributeWeapons(style);
            }
        }
    }

    public void AddPerk(GameObject perkType)
    {
        Instantiate(perkType, transform.GetChild(1).transform);
    }

    public void ShangeStats(float maxHealth, float shield, float damageIncrease, float speed, float healing, float attackSpeed)
    {
        if (this.maxHealth + maxHealth <= 0)
        {
            this.maxHealth = 1;
        }
        else
        {
            this.maxHealth += maxHealth;
        }
        this.shield += shield;
        this.damageIncrease += damageIncrease;
        this.speed += speed;
        this.healing += healing;
        this.attackSpeed += attackSpeed;
    }

    public string GetStats()
    {
        var tableUI = _ui.GetTable();

        string[] names = tableUI["stats"].LocalizedValue.Split(',');
        float[] val = { maxHealth, shield, damageIncrease, speed, healing, attackSpeed };
        string text = "";
        for (int i = 0; i < names.Length; i++)
        {
            if (val[i] < 0)
            {
                text += "<color=\"white\">" + names[i] + ": <color=\"red\">" + val[i] + "\n";
            }
            if (val[i] >= 0)
            {
                text += "<color=\"white\">" + names[i] + ": <color=\"white\">" + val[i] + "\n";
            }
        }
        return text;
    }
}
