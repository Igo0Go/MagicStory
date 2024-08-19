using TMPro;
using UnityEngine;

public class CharacterButtonItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text magicanNameText;

    private Magican magican;
    private CharacterEditorPanel characterEditorPanel;

    public void InitItem(Magican magican, CharacterEditorPanel characterEditorPanel)
    {
        this.magican = magican;
        this.characterEditorPanel = characterEditorPanel;
        magicanNameText.text = magican.Name;
    }

    public void ChoseThisMagican()
    {
        characterEditorPanel.EditCharacter(magican);
    }
}
