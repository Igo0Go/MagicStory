using System.Collections.Generic;
using UnityEngine;

public class SpellListWindow : MonoBehaviour, ISpellEditorUIPart
{
    public SpellEditorUICenter Center { get; private set; }

    [SerializeField]
    private Transform spellItemsContainer;
    [SerializeField]
    private GameObject spellItemPrefab;

    [Space]
    [SerializeField]
    private GameObject LoadSpellPanel;
    [SerializeField]
    private Transform loadSpellContainer;
    [SerializeField]
    private GameObject loadSpellItemPrefab;

    public void Init(SpellEditorUICenter center)
    {
        Center = center;
    }

    public void DrawCurrentMagicanSpells()
    {
        for (int i = 0; i < spellItemsContainer.childCount; i++)
        {
            Destroy(spellItemsContainer.GetChild(i).gameObject);
        }

        foreach (Spell spell in Center.CurrentMagican.spellsBook)
        {
            CreateSpellItem(spell);
        }
    }

    public void CreateNewSpell()
    {
        Center.CurrentMagican.spellsBook.Add(new Spell());
        ToSpellEditorWithSpell(Center.CurrentMagican.spellsBook[Center.CurrentMagican.spellsBook.Count-1]);
    }

    public void AddSpell(Spell spell)
    {
        LoadSpellPanel.SetActive(false);
        Center.CurrentMagican.spellsBook.Add(spell);
        DrawCurrentMagicanSpells();
    }

    public void EditSpell(Spell spell)
    {
        ToSpellEditorWithSpell(spell);
    }

    public void DeleteSpell(Spell spell)
    {
        Center.CurrentMagican.spellsBook.Remove(spell);
        DrawCurrentMagicanSpells();
    }

    public void ToSpellEditorWithSpell(Spell spell)
    {
        Center.CurrentSpell = spell;
        Center.changer.ChangeViewToSpellForm();
        Center.formPanel.RebuildRune();
        Center.effectPanel.UpdateEffectUIItems();
        Center.targetCountPanel.RebuildRunes();
    }

    public void OpenLoadSpellPanel()
    {
        List<string> fileNames = FileAccessUtility.GetAllSpellFilesNames();

        LoadSpellPanel.SetActive(true);

        for(int i = 0; i < loadSpellContainer.childCount; i++)
        { 
            Destroy(loadSpellContainer.GetChild(i).gameObject);
        }

        foreach (string fileName in fileNames)
        {
            Instantiate(loadSpellItemPrefab, loadSpellContainer).
                GetComponent<SpellListLoadButtonItem>().InitItem(fileName, this);
        }
    }

    private void CreateSpellItem(Spell spell)
    {
        Instantiate(spellItemPrefab, spellItemsContainer).GetComponent<SpellListItem>().Init(spell, this);
    }
}
