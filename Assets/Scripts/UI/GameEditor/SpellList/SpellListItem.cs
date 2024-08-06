using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellListItem : MonoBehaviour
{
    [SerializeField]
    private GameObject spellEffectPrefab;

    [SerializeField]
    private SpellRunnesHolder SpellRunnesHolder;

    [SerializeField]
    private TMP_Text spellNameText;
    [SerializeField]
    private TMP_Text targetCountText;
    [SerializeField]
    private TMP_Text accuracyText;
    [SerializeField]
    private TMP_Text reqareForceText;
    [SerializeField]
    private Image formIcon;
    [SerializeField]
    private Transform effectsContainer;

    private Spell spell;
    private SpellListWindow spellListWindow;

    public void Init(Spell spell, SpellListWindow spellListWindow)
    {
        this.spell = spell;
        this.spellListWindow = spellListWindow;

        spellNameText.text = spell.Name;

        reqareForceText.text = spell.CalculateForce().ToString();

        targetCountText.text = spell.TargetsCount.ToString();
        accuracyText.text = spell.SuccessPercent.ToString();

        formIcon.sprite = SpellRunnesHolder.FindFormItem(spell.SpellForm.GetFormType()).sprite;

        if(spell.Effect != null )
        {
            CreateEffectItem(spell.Effect);
            SpellEffect currentEffect = spell.Effect;

            int c = 0;

            while(currentEffect.insideEffect != null)
            {
                CreateEffectItem(currentEffect.insideEffect);
                currentEffect = currentEffect.insideEffect;

                c++;
                if(c >30)
                {
                    break;
                }
            }
        }
    }

    public void DeleteSpell()
    {
        spellListWindow.DeleteSpell(spell);
    }

    public void EditSpell()
    {
        spellListWindow.EditSpell(spell);
    }

    public void SaveSpell()
    {
        Utility_SpellSaver.SaveSpellInTheFile(spell, spell.Name);
    }

    private void CreateEffectItem(SpellEffect spellEffect)
    {
        SpellListEffectItem effectItem = Instantiate(spellEffectPrefab, effectsContainer).GetComponent<SpellListEffectItem>();

        if(spellEffect is ChangeAccuracyEffect changeAccuracy)
        {
            effectItem.effectIcon.sprite = SpellRunnesHolder.FindEffectItem(EffectType.LowerAccuracy).sprite;
            effectItem.effectText.text = changeAccuracy.accuracyPoints.ToString();
        }
        else if(spellEffect is ChangeForceEffect changeForce)
        {
            effectItem.effectIcon.sprite = SpellRunnesHolder.FindEffectItem(EffectType.LowerForce).sprite;
            effectItem.effectText.text = changeForce.forcePoints.ToString();
        }
        else if(spellEffect is ChangeInitiativeEffect initiativeEffect)
        {
            effectItem.effectIcon.sprite = SpellRunnesHolder.FindEffectItem(EffectType.ImproveInitiative).sprite;
            effectItem.effectText.text = initiativeEffect.initiativePoints.ToString();
        }
        else if(spellEffect is DamageEffect damageEffect)
        {
            if(damageEffect.effectTargetType == EffectTargetType.Target)
            {
                effectItem.effectIcon.sprite = SpellRunnesHolder.FindEffectItem(EffectType.Damage).sprite;
            }
            else
            {
                effectItem.effectIcon.sprite = SpellRunnesHolder.FindEffectItem(EffectType.BloodSpell).sprite;
            }
            effectItem.effectText.text = damageEffect.damage.ToString();
        }
        else if(spellEffect is HealEffect healEffect)
        {
            effectItem.effectIcon.sprite = SpellRunnesHolder.FindEffectItem(EffectType.Heal).sprite;
            effectItem.effectText.text = healEffect.healPoint.ToString();

        }
        else if(spellEffect is SetStateEffect setStateEffect)
        {
            switch(setStateEffect.state)
            {
                case MagicanState.None:
                    effectItem.effectIcon.sprite = SpellRunnesHolder.FindEffectItem(EffectType.ClearStates).sprite;
                    effectItem.effectText.text = setStateEffect.duration.ToString();
                    break;
                case MagicanState.Stunned:
                    effectItem.effectIcon.sprite = SpellRunnesHolder.FindEffectItem(EffectType.SkipAction).sprite;
                    effectItem.effectText.text = setStateEffect.duration.ToString();
                    break;
                case MagicanState.MindControl:
                    effectItem.effectIcon.sprite = SpellRunnesHolder.FindEffectItem(EffectType.MindControl).sprite;
                    effectItem.effectText.text = setStateEffect.duration.ToString();
                    break;
                case MagicanState.Shield:
                    effectItem.effectIcon.sprite = SpellRunnesHolder.FindEffectItem(EffectType.Shield).sprite;
                    effectItem.effectText.text = setStateEffect.duration.ToString();
                    break;
                case MagicanState.Mirror:
                    effectItem.effectIcon.sprite = SpellRunnesHolder.FindEffectItem(EffectType.Mirror).sprite;
                    effectItem.effectText.text = setStateEffect.duration.ToString();
                    break;
            }


        }
    }
}
