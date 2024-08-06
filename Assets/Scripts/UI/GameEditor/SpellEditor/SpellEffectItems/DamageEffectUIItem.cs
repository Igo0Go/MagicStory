using TMPro;
using UnityEngine;

public class DamageEffectUIItem : MonoBehaviour
{
    private SpellEffectEditorPanel spellEffectEditorPanel;
    private DamageEffect damageEffect;

    [SerializeField]
    private TMP_InputField damageField;
    [SerializeField]
    private TMP_Dropdown targetDropdown;
    [SerializeField]
    private TMP_Text requareForceText;

    public void CreateItem(DamageEffect damageEffect, SpellEffectEditorPanel spellEffectEditorPanel)
    {
        this.spellEffectEditorPanel = spellEffectEditorPanel;
        this.damageEffect = damageEffect;
        damageField.onValueChanged.AddListener(OnDamageChanged);
        targetDropdown.onValueChanged.AddListener(OnDropdownChanged);
        UpdateItemData();
    }

    public void OnDamageChanged(string damageString)
    {
        int damage;
        if (string.IsNullOrEmpty(damageString))
        {
            requareForceText.text = "Нагрузка: --";
            return;
        }
        if (!int.TryParse(damageString, out damage))
        {
            requareForceText.text = "Нагрузка: --";
            return;
        }

        if (damage < 0)
        {  
            damage = 1;
        }
        damageField.text = damage.ToString();
        damageEffect.damage = damage;
        UpdateItemData();
    }

    public void OnDropdownChanged(int index)
    {
        damageEffect.effectTargetType = (EffectTargetType)index;
        UpdateItemData();
        spellEffectEditorPanel.RebuildRunes();
    }

    public void OnDeleteEffectClick()
    {
        spellEffectEditorPanel.RemoveEffectFromSpell(damageEffect);
    }


    public void UpdateItemData()
    {
        requareForceText.text = "Нагрузка: " + damageEffect.CalculateReqareForceForThisEffectOnly();
        damageField.text = damageEffect.damage.ToString();
        targetDropdown.value = (int)damageEffect.effectTargetType;
        spellEffectEditorPanel.Center.spellEditor.DrawSpellForceRequared();
    }
}
