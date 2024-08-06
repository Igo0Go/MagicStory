using TMPro;
using UnityEngine;

public class SpellListLoadButtonItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text buttonText;

    private string fileName;
    private SpellListWindow spellListWindow;

    public void InitItem(string fileName, SpellListWindow spellListWindow)
    {
        this.fileName = fileName;
        this.spellListWindow = spellListWindow;
        string[] strings = fileName.Split(new char[] { '\\' }, System.StringSplitOptions.RemoveEmptyEntries);
        strings = strings[strings.Length - 1].Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries);
        buttonText.text = strings[0];
    }

    public void LoadSpell()
    {
        spellListWindow.AddSpell(Utility_SpellSaver.LoadSpellFromTheFile(fileName, false));
    }
}
