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
    private TMP_InputField nameInputField;
    [SerializeField]
    private TMP_InputField healthInputField;
    [SerializeField]
    private TMP_InputField forceInputField;
    [SerializeField]
    private TMP_InputField accuracyInputField;
    [SerializeField]
    private Slider accuracySlider;
    [SerializeField]
    private TMP_InputField initiativeInputField;
    [SerializeField]
    private Image characterPortrait;

    public SpellEditorUICenter Center { get; private set; }

    public void Init(SpellEditorUICenter center)
    {
        Center = center;
        nameInputField.onEndEdit.AddListener(OnNameInputFieldChanged);
        forceInputField.onEndEdit.AddListener(OnForceInputFieldChanged);
        healthInputField.onEndEdit.AddListener (OnHealthInputFieldChanged);
        accuracyInputField.onEndEdit .AddListener(OnAccuracyInputFieldChanged);
        accuracySlider.onValueChanged.AddListener(OnAccuracySliderChanged);

        characterEditorPanel.SetActive(false);
        LoadAllMagicans();
    }

    public void CreateNewCharacter()
    {
        Magican magican = new Magican(string.Empty, 50, 50, 50, 0);
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

    private void ShowCharacterEditorForMagican(Magican magican)
    {
        Center.CurrentMagican = magican;
        characterEditorPanel.SetActive(true);
        nameInputField.text = magican.Name;
        forceInputField.text = magican.Force.ToString();
        healthInputField.text = magican.Health.ToString();
        accuracyInputField.text = magican.Accuracy.ToString();
        accuracySlider.value = magican.Accuracy;
        characterPortrait.sprite = Center.magicanPortraitsHolder.portraits[magican.CharacterPortraitIndex];
        Center.spellListPanel.DrawCurrentMagicanSpells();
    }

    private void OnNameInputFieldChanged(string value)
    {
        Center.CurrentMagican.Name = value;
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
    }
    private void OnAccuracyInputFieldChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        int accuracy = int.Parse(value);

        if (accuracy < 30 || accuracy > 100)
        {
            accuracy = Mathf.Clamp(accuracy, 30, 100);
            accuracyInputField.text = accuracy.ToString();
        }
        Center.CurrentMagican.SetDefaultAccuracy(accuracy);
        accuracySlider.value = accuracy;
    }
    private void OnAccuracySliderChanged(float value)
    {
        int accuracy = (int)value;
        Center.CurrentMagican.SetDefaultAccuracy(accuracy);
        accuracyInputField.text = accuracy.ToString();
    }
}
