using TMPro;
using UnityEngine;

public class CharacterButtonItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text magicanNameText;

    private string fileName;
    private CharacterEditorPanel characterEditorPanel;

    public void InitItem(string magicanName, CharacterEditorPanel characterEditorPanel)
    {
        fileName = magicanName;
        this.characterEditorPanel = characterEditorPanel;

        string[] strings = fileName.Split(new char[] { '\\' }, System.StringSplitOptions.RemoveEmptyEntries);
        strings = strings[strings.Length - 1].Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries);
        magicanNameText.text = fileName = strings[0];
    }

    public void ChoseThisMagican()
    {
        characterEditorPanel.EditCharacter(FileAccessUtility.LoadMagicanFromFile(fileName, true));
    }
}
