using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Space]
    [SerializeField]
    private MagicanHolder magicanHolder;
    [SerializeField]
    private Transform magianButtonContainer;
    [SerializeField]
    private GameObject magianButtonPrefab;
    [SerializeField]
    private TMP_Text magicanNameText;
    [SerializeField]
    private Image magicanImage;

    private Magican currentMagican;

    public void Init(SpellEditorUICenter center)
    {
        Center = center;
        DrawMagicanButtons();
        ChoseMagican(magicanHolder.MagicanList[0]);
    }

    public void SaveSpellNamesInTheMagican()
    {
        currentMagican.SaveSpellNames();
    }

    private void OnEnable()
    {
        UpdateSpellListItems();
    }

    public void ChoseMagican(Magican magican)
    {
        currentMagican = magican;
        magicanNameText.text = magican.Name;
        magicanImage.sprite = magican.CharacterPortrait;

        currentMagican.spellsBook.Clear();

        foreach (var item in currentMagican.spellsFilesNames)
        {
            Spell spell = FileAccessUtility.LoadSpellFromTheFile(item, true);

            if(spell != null)
            {
                currentMagican.spellsBook.Add(spell);
            }
        }

        UpdateSpellListItems();
    }

    public void CreateNewSpell()
    {
        currentMagican.spellsBook.Add(new Spell());
        ToSpellEditorWithSpell(currentMagican.spellsBook[currentMagican.spellsBook.Count-1]);
    }

    public void AddSpell(Spell spell)
    {
        LoadSpellPanel.SetActive(false);
        currentMagican.spellsBook.Add(spell);
        UpdateSpellListItems();
    }

    public void EditSpell(Spell spell)
    {
        ToSpellEditorWithSpell(spell);
    }

    public void DeleteSpell(Spell spell)
    {
        currentMagican.spellsBook.Remove(spell);
        UpdateSpellListItems();
    }

    public void ToSpellEditorWithSpell(Spell spell)
    {
        Center.CurrentSpell = spell;
        Center.formPanel.RebuildRune();
        Center.effectPanel.RebuildRunes();
        Center.targetCountPanel.RebuildRunes();
        Center.changer.ToSpellEditorStateFast();
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

    public void SaveMagicanToFile()
    {
        FileAccessUtility.SaveMagicanInTheFile(currentMagican, currentMagican.Name);
    }

    private void UpdateSpellListItems()
    {
        for (int i = 0; i < spellItemsContainer.childCount; i++)
        {
            Destroy(spellItemsContainer.GetChild(i).gameObject);
        }

        foreach (Spell spell in currentMagican.spellsBook)
        {
            CreateSpellItem(spell);
        }
    }

    private void CreateSpellItem(Spell spell)
    {
        Instantiate(spellItemPrefab, spellItemsContainer).GetComponent<SpellListItem>().Init(spell, this);
    }

    private void DrawMagicanButtons()
    {
        foreach(var item in magicanHolder.MagicanList)
        {
            Instantiate(magianButtonPrefab, magianButtonContainer).
                GetComponent<CharacterButtonItem>().InitItem(item, this);
        }
    }
}
