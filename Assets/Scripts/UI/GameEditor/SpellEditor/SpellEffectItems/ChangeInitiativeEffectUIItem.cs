using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeInitiativeEffectUIItem : MonoBehaviour
{
    private SpellEffectEditorPanel spellEffectEditorPanel;
    private ChangeInitiativeEffect changeInitiativeEffect;

    [SerializeField]
    private TMP_InputField InitiativeField;
    [SerializeField]
    private TMP_Dropdown targetDropdown;
    [SerializeField]
    private TMP_Text requareForceText;

    public void CreateItem(ChangeInitiativeEffect changeInitiative, SpellEffectEditorPanel spellEffectEditorPanel)
    {
        this.spellEffectEditorPanel = spellEffectEditorPanel;
        changeInitiativeEffect = changeInitiative;
        InitiativeField.onValueChanged.AddListener(OnInitiativeChanged);
        targetDropdown.onValueChanged.AddListener(OnDropdownChanged);
        UpdateItemData();
    }

    public void OnInitiativeChanged(string initiativeString)
    {
        int value = 0;
        if (string.IsNullOrEmpty(initiativeString))
        {
            requareForceText.text = "Нагрузка: --";
            return;
        }

        if (!int.TryParse(initiativeString, out value))
        {
            requareForceText.text = "Нагрузка: --";
            return;
        }
        int initiativePoints = value;
        InitiativeField.text = initiativePoints.ToString();
        changeInitiativeEffect.initiativeOffset = initiativePoints;
        UpdateItemData();
        spellEffectEditorPanel.RebuildRunes();
    }

    public void OnDropdownChanged(int index)
    {
        changeInitiativeEffect.effectTargetType = (EffectTargetType)index;
        UpdateItemData();
    }

    public void OnDeleteEffectClick()
    {
        spellEffectEditorPanel.RemoveEffectFromSpell(changeInitiativeEffect);
    }

    public void UpdateItemData()
    {
        requareForceText.text = "Нагрузка: " + changeInitiativeEffect.CalculateReqareForceForThisEffectOnly();
        InitiativeField.text = changeInitiativeEffect.initiativeOffset.ToString();
        targetDropdown.value = (int)changeInitiativeEffect.effectTargetType;
        spellEffectEditorPanel.Center.spellEditor.DrawSpellForceRequared();
    }
}
