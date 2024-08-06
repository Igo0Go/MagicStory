using TMPro;
using UnityEngine;

public class ChangeForceEffectUIItem : MonoBehaviour
{
    private SpellEffectEditorPanel spellEffectEditorPanel;
    private ChangeForceEffect changeForceEffect;

    [SerializeField]
    private TMP_InputField ForceField;
    [SerializeField]
    private TMP_Dropdown targetDropdown;
    [SerializeField]
    private TMP_Text requareForceText;

    public void CreateItem(ChangeForceEffect changeforce, SpellEffectEditorPanel spellEffectEditorPanel)
    {
        this.spellEffectEditorPanel = spellEffectEditorPanel;
        changeForceEffect = changeforce;
        ForceField.onValueChanged.AddListener(OnForceChanged);
        targetDropdown.onValueChanged.AddListener(OnDropdownChanged);
        UpdateItemData();
    }

    public void OnForceChanged(string forceString)
    {
        int value = 0;
        if (string.IsNullOrEmpty(forceString))
        {
            requareForceText.text = "Нагрузка: --";
            return;
        }

        if (!int.TryParse(forceString, out value))
        {
            requareForceText.text = "Нагрузка: --";
            return;
        }
        int forcePoints = value;
        ForceField.text = forcePoints.ToString();
        changeForceEffect.forcePoints = forcePoints;
        UpdateItemData();
        spellEffectEditorPanel.RebuildRunes();
    }

    public void OnDropdownChanged(int index)
    {
        changeForceEffect.effectTargetType = (EffectTargetType)index;
        UpdateItemData();
    }

    public void OnDeleteEffectClick()
    {
        spellEffectEditorPanel.RemoveEffectFromSpell(changeForceEffect);
    }

    public void UpdateItemData()
    {
        requareForceText.text = "Нагрузка: " + changeForceEffect.CalculateReqareForceForThisEffectOnly();
        ForceField.text = changeForceEffect.forcePoints.ToString();
        targetDropdown.value = (int)changeForceEffect.effectTargetType;
        spellEffectEditorPanel.Center.spellEditor.DrawSpellForceRequared();
    }
}
