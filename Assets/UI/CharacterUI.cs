using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public GameObject character;

    public LocalizedStringTable _ui;
    public LocalizedStringTable _comments;
    public LocalizedStringTable _names;

    void Start()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(delegate { StartGame(); });
        SetText();
        Settings.instance.onChangeLang += SetText;
    }

    void SetText()
    {
        if (character == null) return;
        var tableNames = _names.GetTable();
        var tableUI = _ui.GetTable();
        var tableComments = _comments.GetTable();
        PlayerBehavior script = character.GetComponent<PlayerBehavior>();
        transform.GetChild(0).GetComponent<Image>().sprite = script.image;
        int index = script.characterIndex;
        string description = tableNames[script.nameOfCharacter].LocalizedValue + "\n";
        description += tableComments[script.nameOfCharacter].LocalizedValue + "\n";
        description += tableUI["maxScore"].LocalizedValue + ": " + Progress.instance.data.scores[index];
        if (Progress.instance.data.win[index])
        {
            GetComponent<Image>().color = Color.yellow;
        }
        transform.GetChild(1).GetComponent<TMP_Text>().text = description;
    }

    void StartGame()
    {
        Progress.instance.typeOfCharacter = character;
        SceneManager.LoadScene(2);
        Progress.instance.ShowAdd(false);
    }
}
