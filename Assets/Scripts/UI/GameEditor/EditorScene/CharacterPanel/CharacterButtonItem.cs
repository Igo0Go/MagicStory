using TMPro;
using UnityEngine;

public class CharacterButtonItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text magicanNameText;

    private Magican magican;
    private SpellListWindow spellListWindow;

    public void InitItem(Magican magican, SpellListWindow spellListWindow)
    {
        this.magican = magican;
        this.spellListWindow = spellListWindow;
        magicanNameText.text = magican.Name;
    }

    public void ChoseThisMagican()
    {
        spellListWindow.ChoseMagican(magican);
    }
}
