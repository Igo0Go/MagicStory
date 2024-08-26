using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterEditorPanel : MonoBehaviour, ISpellEditorUIPart
{
    [SerializeField]
    private Transform charactersListContainer;
    [SerializeField]
    private GameObject magicanButton;
    [SerializeField]
    private GameObject characterEditorPanel;


    [Space(20)]
    [SerializeField]
    private TMP_Text magicanWorkLoadText;
    [SerializeField]
    private TMP_InputField nameInputField;
    [SerializeField]
    private TMP_InputField healthInputField;
    [SerializeField]
    private TMP_InputField forceInputField;
    [SerializeField]
    private Image characterPortrait;

    public SpellEditorUICenter Center { get; private set; }

    public void Init(SpellEditorUICenter center)
    {
        Center = center;
        nameInputField.onEndEdit.AddListener(OnNameInputFieldChanged);
        forceInputField.onEndEdit.AddListener(OnForceInputFieldChanged);
        healthInputField.onEndEdit.AddListener (OnHealthInputFieldChanged);

        characterEditorPanel.SetActive(false);
        LoadAllMagicans();
    }

    public void CreateNewCharacter()
    {
        Magican magican = new Magican(string.Empty, 50, 50);
        ShowCharacterEditorForMagican(magican);
    }

    public void EditCharacter(Magican magican)
    {
        ShowCharacterEditorForMagican(magican);
    }

    public void NextPortrait()
    {
        int index = Center.CurrentMagican.CharacterPortraitIndex+1;

        if(index >= Center.magicanPortraitsHolder.portraits.Count)
        {
            index = 0;
        }

        Center.CurrentMagican.CharacterPortraitIndex = index;
        characterPortrait.sprite = Center.magicanPortraitsHolder.portraits[index];
    }

    public void PreviousPortrait()
    {
        int index = Center.CurrentMagican.CharacterPortraitIndex -1;
        if (index < 0)
        {
            index = Center.magicanPortraitsHolder.portraits.Count - 1;
        }

        Center.CurrentMagican.CharacterPortraitIndex = index;
        characterPortrait.sprite = Center.magicanPortraitsHolder.portraits[index];
    }

    public void SaveMagicanToFile()
    {
        FileAccessUtility.SaveMagicanInTheFile(Center.CurrentMagican, Center.CurrentMagican.Name);
    }

    public void LoadAllMagicans()
    {
        List<string> fileNames = FileAccessUtility.GetAllMagicansFilesNames();

        for (int i = 0; i < charactersListContainer.childCount; i++)
        {
            Destroy(charactersListContainer.GetChild(i).gameObject);
        }

        foreach (string fileName in fileNames)
        {
            Instantiate(magicanButton, charactersListContainer).
                GetComponent<CharacterButtonItem>().InitItem(fileName, this);
        }
    }

    public void UpdateMagicanWorkLoad()
    {
        int magicanWorkLoad = Center.CurrentMagican.CalculateMagicanStatsWorkLoad();
        int magicanSpellBookWorkLoad = Center.CurrentMagican.CalculateMagicanSpellsWorkload();

        magicanWorkLoadText.text = "Нагрузка: " + magicanWorkLoad + " + " + magicanSpellBookWorkLoad + " = "
            + (magicanWorkLoad + magicanSpellBookWorkLoad);
    }

    private void ShowCharacterEditorForMagican(Magican magican)
    {
        Center.CurrentMagican = magican;
        characterEditorPanel.SetActive(true);
        nameInputField.text = magican.Name;
        forceInputField.text = magican.Force.ToString();
        healthInputField.text = magican.Health.ToString();
        characterPortrait.sprite = Center.magicanPortraitsHolder.portraits[magican.CharacterPortraitIndex];
        Center.spellListPanel.DrawCurrentMagicanSpells();
        UpdateMagicanWorkLoad();
    }

    private void OnNameInputFieldChanged(string value)
    {
        Center.CurrentMagican.Name = value;
        UpdateMagicanWorkLoad();
    }
    private void OnForceInputFieldChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }
        
        int force = int.Parse(value);

        if (force < 0)
        {
            force = 0;
            forceInputField.text = "0";
        }
        Center.CurrentMagican.SetMaxForce(force);
        Center.CurrentMagican.Force = force;
        UpdateMagicanWorkLoad();
    }
    private void OnHealthInputFieldChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        int health = int.Parse(value);

        if (health < 0)
        {
            health = 0;
            healthInputField.text = "0";
        }

        Center.CurrentMagican.SetMaxHealth(health);
        Center.CurrentMagican.Health = health;
        UpdateMagicanWorkLoad();
    }
}
