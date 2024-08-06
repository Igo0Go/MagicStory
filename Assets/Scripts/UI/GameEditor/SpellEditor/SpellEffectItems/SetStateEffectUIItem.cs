using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetStateEffectUIItem : MonoBehaviour
{
    private SpellEffectEditorPanel spellEffectEditorPanel;
    private SetStateEffect setStateEffect;

    [SerializeField]
    private TMP_InputField DurationField;
    [SerializeField]
    private TMP_Dropdown targetDropdown;
    [SerializeField]
    private TMP_Dropdown stateDropdown;
    [SerializeField]
    private TMP_Text requareForceText;

    public void CreateItem(SetStateEffect setStateEffect, SpellEffectEditorPanel spellEffectEditorPanel)
    {
        this.spellEffectEditorPanel = spellEffectEditorPanel;
        this.setStateEffect = setStateEffect;
        DurationField.onValueChanged.AddListener(OnDurationChanged);
        targetDropdown.onValueChanged.AddListener(OnDropdownChanged);
        stateDropdown.onValueChanged.AddListener(OnStateDropdownChanged);
        UpdateItemData();
    }

    public void OnDurationChanged(string durationString)
    {
        int duration;
        if (string.IsNullOrEmpty(durationString))
        {
            requareForceText.text = "Нагрузка: --";
            return;
        }
        if (!int.TryParse(durationString, out duration))
        {
            requareForceText.text = "Нагрузка: --";
            return;
        }

        if(duration <= 0)
        {
            duration = 1;
        }

        DurationField.text = duration.ToString();
        setStateEffect.duration = duration;
        UpdateItemData();
    }

    public void OnDropdownChanged(int index)
    {
        setStateEffect.effectTargetType = (EffectTargetType)index;
        UpdateItemData();
    }
    public void OnStateDropdownChanged(int index)
    {
        setStateEffect.state = (MagicanState)index;
        UpdateItemData();
        spellEffectEditorPanel.RebuildRunes();
    }

    public void OnDeleteEffectClick()
    {
        spellEffectEditorPanel.RemoveEffectFromSpell(setStateEffect);
    }

    public void UpdateItemData()
    {
        requareForceText.text = "Нагрузка: " + setStateEffect.CalculateReqareForceForThisEffectOnly();
        DurationField.text = setStateEffect.duration.ToString();
        stateDropdown.value = (int)setStateEffect.state;
        targetDropdown.value = (int)setStateEffect.effectTargetType;
        spellEffectEditorPanel.Center.spellEditor.DrawSpellForceRequared();
    }
}
