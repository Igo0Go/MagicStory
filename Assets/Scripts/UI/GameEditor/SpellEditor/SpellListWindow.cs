using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellListWindow : MonoBehaviour, ISpellEditorUIPart
{
    public SpellEditorUICenter Center { get; private set; }

    [SerializeField]
    private Transform spellItemsContainer;
    [SerializeField]
    private GameObject spellItemPrefab;
    [SerializeField]
    private TMP_Text spellWorkLoadText;

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

        float spellWorkLoad = 0;

        foreach (Spell spell in Center.CurrentMagican.SpellsBook)
        {
            spellWorkLoad += spell.CalculateWorkLoad();
            CreateSpellItem(spell);
        }

        spellWorkLoadText.text = "Общая нагрузка: " + spellWorkLoad.ToString();
        Center.characterEditorPanel.UpdateMagicanWorkLoad();
    }

    public void CreateNewSpell()
    {
        Center.CurrentMagican.SpellsBook.Add(new Spell());
        ToSpellEditorWithSpell(Center.CurrentMagican.SpellsBook[Center.CurrentMagican.SpellsBook.Count-1]);
    }

    public void AddSpell(Spell spell)
    {
        LoadSpellPanel.SetActive(false);
        Center.CurrentMagican.SpellsBook.Add(spell);
        DrawCurrentMagicanSpells();
    }

    public void EditSpell(Spell spell)
    {
        ToSpellEditorWithSpell(spell);
    }

    public void DeleteSpell(Spell spell)
    {
        Center.CurrentMagican.SpellsBook.Remove(spell);
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
