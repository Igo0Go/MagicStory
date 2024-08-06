using TMPro;
using UnityEngine;

public class ChangeAccuracyEffectUIItem : MonoBehaviour
{
    private SpellEffectEditorPanel spellEffectEditorPanel;
    private ChangeAccuracyEffect changeAccuracyEffect;

    [SerializeField]
    private TMP_InputField AccuracyField;
    [SerializeField]
    private TMP_Dropdown targetDropdown;
    [SerializeField]
    private TMP_Text requareForceText;

    public void CreateItem(ChangeAccuracyEffect changeAccuracy, SpellEffectEditorPanel spellEffectEditorPanel)
    {
        this.spellEffectEditorPanel = spellEffectEditorPanel;
        changeAccuracyEffect = changeAccuracy;
        AccuracyField.onValueChanged.AddListener(OnAccuracyChanged);
        targetDropdown.onValueChanged.AddListener(OnDropdownChanged);
        UpdateItemData();
    }

    public void OnAccuracyChanged(string accuracyString)
    {
        int accuracy = 0;
        if(string.IsNullOrEmpty(accuracyString))
        {
            requareForceText.text = "Нагрузка: --";
            return;
        }

        if (!int.TryParse(accuracyString, out accuracy))
        {
            requareForceText.text = "Нагрузка: --";
            return;
        }
        AccuracyField.text = accuracy.ToString();
        changeAccuracyEffect.accuracyPoints = accuracy;
        UpdateItemData();
        spellEffectEditorPanel.RebuildRunes();
    }

    public void OnDropdownChanged(int index)
    {
        changeAccuracyEffect.effectTargetType = (EffectTargetType)index;
        UpdateItemData();
    }

    public void OnDeleteEffectClick()
    {
        spellEffectEditorPanel.RemoveEffectFromSpell(changeAccuracyEffect);
    }

    public void UpdateItemData()
    {
        requareForceText.text = "Нагрузка: " + changeAccuracyEffect.CalculateReqareForceForThisEffectOnly();
        AccuracyField.text = changeAccuracyEffect.accuracyPoints.ToString();
        targetDropdown.value = (int)changeAccuracyEffect.effectTargetType;
        spellEffectEditorPanel.Center.spellEditor.DrawSpellForceRequared();
    }
}
