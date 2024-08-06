using TMPro;
using UnityEngine;

public class HealEffectUIItem : MonoBehaviour
{
    private SpellEffectEditorPanel spellEffectEditorPanel;
    private HealEffect healEffect;

    [SerializeField]
    private TMP_InputField healField;
    [SerializeField]
    private TMP_Dropdown targetDropdown;
    [SerializeField]
    private TMP_Text requareForceText;

    public void CreateItem(HealEffect healEffect, SpellEffectEditorPanel spellEffectEditorPanel)
    {
        this.spellEffectEditorPanel = spellEffectEditorPanel;
        this.healEffect = healEffect;
        healField.onValueChanged.AddListener(OnHealChanged);
        targetDropdown.onValueChanged.AddListener(OnDropdownChanged);
        UpdateItemData();
    }

    public void OnHealChanged(string healString)
    {
        int heal;
        if (string.IsNullOrEmpty(healString))
        {
            requareForceText.text = "Нагрузка: --";
            return;
        }
        if (!int.TryParse(healString, out heal))
        {
            requareForceText.text = "Нагрузка: --";
            return;
        }
        if (heal < 0)
        {
            heal = 1;
        }
        healField.text = heal.ToString();
        healEffect.healPoint = heal;
        UpdateItemData();
    }

    public void OnDropdownChanged(int index)
    {
        healEffect.effectTargetType = (EffectTargetType)index;
        UpdateItemData();
    }

    public void OnDeleteEffectClick()
    {
        spellEffectEditorPanel.RemoveEffectFromSpell(healEffect);
    }

    public void UpdateItemData()
    {
        requareForceText.text = "Нагрузка: " + healEffect.CalculateReqareForceForThisEffectOnly();
        healField.text = healEffect.healPoint.ToString();
        targetDropdown.value = (int)healEffect.effectTargetType;
        spellEffectEditorPanel.Center.spellEditor.DrawSpellForceRequared();
    }
}
