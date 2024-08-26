using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Data;
using System.Diagnostics.Eventing.Reader;

public class SpellFormEditorPanel : MonoBehaviour, ISpellEditorUIPart
{
    [Space()]
    [SerializeField]
    private Slider percentSlider;
    [SerializeField]
    private TMP_InputField percentInputField;
    [SerializeField]
    private TMP_Text formDescriptionText;
    [SerializeField]
    public SpriteRenderer formSpriteRenderer;

    public SpellEditorUICenter Center { get; private set; }

    public void Init(SpellEditorUICenter center)
    {
        this.Center = center;
        percentSlider.onValueChanged.AddListener(OnPercentSliderChanged);
        percentInputField.onEndEdit.AddListener(OnPercentInputFieldEndEdit);
    }

    public void ActivatePanel()
    {
        formSpriteRenderer.sprite = null;
        formSpriteRenderer.color = Color.white;
    }

    public void SetArrowForm()
    {
        Center.CurrentSpell.SpellForm = new ArrowSpellForm();

        foreach (var rune in Center.spellRunnesHolder.formSprites)
        {
            if(rune.key == SpellFormType.Arrow)
            {
                formDescriptionText.text = rune.description;
                formSpriteRenderer.sprite = rune.sprite;
                formSpriteRenderer.color = rune.color;
                Center.spellEditor.DrawSpellForceRequared();
                break;
            }
        }
    }

    public void SetRelationForm()
    {
        Center.CurrentSpell.SpellForm = new RelationSpellForm();

        foreach (var rune in Center.spellRunnesHolder.formSprites)
        {
            if (rune.key == SpellFormType.Relation)
            {
                formDescriptionText.text = rune.description;
                formSpriteRenderer.sprite = rune.sprite;
                formSpriteRenderer.color = rune.color;
                Center.spellEditor.DrawSpellForceRequared();
                break;
            }
        }
    }

    public void SetAruraForm()
    {
        Center.CurrentSpell.SpellForm = new AuraSpellForm();

        foreach (var rune in Center.spellRunnesHolder.formSprites)
        {
            if (rune.key == SpellFormType.Aura)
            {
                formDescriptionText.text = rune.description;
                formSpriteRenderer.sprite = rune.sprite;
                formSpriteRenderer.color = rune.color;
                Center.spellEditor.DrawSpellForceRequared();
                break;
            }
        }
    }

    public void SetCloudForm()
    {
        Center.CurrentSpell.SpellForm = new CloudSpellForm();

        foreach (var rune in Center.spellRunnesHolder.formSprites)
        {
            if (rune.key == SpellFormType.Cloud)
            {
                formDescriptionText.text = rune.description;
                formSpriteRenderer.sprite = rune.sprite;
                formSpriteRenderer.color = rune.color;
                Center.spellEditor.DrawSpellForceRequared();
                break;
            }
        }
    }

    public void RebuildRune()
    {
        if(Center.CurrentSpell.SpellForm != null)
        {
            if(Center.CurrentSpell.SpellForm is ArrowSpellForm)
            {
                SetArrowForm();
            }
            else if (Center.CurrentSpell.SpellForm is AuraSpellForm) 
            {
                SetRelationForm();
            }
            else if(Center.CurrentSpell.SpellForm is CloudSpellForm)
            {
                SetCloudForm();
            }
            else if(Center.CurrentSpell.SpellForm is RelationSpellForm)
            {
                SetRelationForm();
            }
        }

        percentInputField.text = Center.CurrentSpell.SuccessPercent.ToString();
        percentSlider.value = Center.CurrentSpell.SuccessPercent;
    }

    public void ClearRune()
    {
        formDescriptionText.text = string.Empty;
        formSpriteRenderer.sprite = null;
    }

    private void OnPercentSliderChanged(float percent)
    {
        int newPercent = Mathf.Clamp((int)percent, 55, 100);
        percentInputField.text = percent.ToString();
        Center.CurrentSpell.SuccessPercent = newPercent;
        Center.spellEditor.DrawSpellForceRequared();
    }
    private void OnPercentInputFieldEndEdit(string value)
    {
        int newPercent = 0;
        if (int.TryParse(value, out newPercent))
        {
            newPercent = Mathf.Clamp(newPercent, 55, 100);
            Center.CurrentSpell.SuccessPercent = newPercent;
            percentSlider.value = Center.CurrentSpell.SuccessPercent;
            Center.spellEditor.DrawSpellForceRequared();
        }
    }
}
