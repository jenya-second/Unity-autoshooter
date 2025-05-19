using UnityEngine;
using UnityEngine.Localization;

public class Perk : MonoBehaviour
{
    public Sprite image;
    public string nameOfPerk;

    public float maxHealth = 0;
    public float shield = 0;
    public float damageIncrease = 0;
    public float speed = 0;
    public float healing = 0;
    public float attackSpeed = 0;

    public LocalizedStringTable _ui;
    public LocalizedStringTable _names;

    private void Start()
    {
        Progress.instance.player.GetComponent<PlayerBehavior>().ShangeStats(maxHealth, shield, damageIncrease, speed, healing, attackSpeed);
    }

    public string GetText()
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
            if (val[i] > 0)
            {
                text += "<color=\"white\">" + names[i] + ": <color=\"green\">+" + val[i] + "\n";
            }
        }
        return text;
    }

    public string GetName()
    {
        var names = _names.GetTable();
        return names[nameOfPerk].LocalizedValue;
    }
}
