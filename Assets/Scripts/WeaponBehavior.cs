using UnityEngine;
using UnityEngine.Localization;

public class WeaponBehavior : MonoBehaviour
{
    public float attackSpeed = 10;
    public float damage = 10;
    public bool mele = false;
    public string wName = "Base weapon";
    public Sprite image;

    public LocalizedStringTable _ui;
    public LocalizedStringTable _comments;
    public LocalizedStringTable _names;

    public string GetText()
    {
        var tableUI = _ui.GetTable();
        var tableComments = _comments.GetTable();
        string text = "";
        if (mele)
        {
            text += tableUI["mele"].LocalizedValue + "\n";
        }
        else
        {
            text += tableUI["range"].LocalizedValue + "\n";
        }
        text += tableUI["damage"].LocalizedValue + ": " + damage + "\n";
        text += tableUI["attackSpeed"].LocalizedValue + ": " + attackSpeed + "\n";
        text += tableComments[wName].LocalizedValue;
        return text;
    }

    internal string GetName()
    {
        var names = _names.GetTable();
        return names[wName].LocalizedValue;
    }
}
