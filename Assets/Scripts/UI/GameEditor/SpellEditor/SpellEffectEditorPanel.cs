using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SpellEffectEditorPanel : MonoBehaviour, ISpellEditorUIPart
{
    [SerializeField]
    private Transform spellEffectItemsContent;
    [SerializeField]
    private Transform spellRingEffectTransform;
    [SerializeField]
    private GameObject spellRingEffectPrefab;

    [Space]
    [SerializeField]
    private GameObject damageEffectUIItemPrefab;
    [SerializeField]
    private GameObject healEffectUIItemPrefab;
    [SerializeField]
    private GameObject changeAccuracyEffectUIItemPrefab;
    [SerializeField]
    private GameObject changeForceEffectUIItemPrefab;
    [SerializeField]
    private GameObject changeInitiativeEffectUIItemPrefab;
    [SerializeField]
    private GameObject setStateEffectUIItemPrefab;

    [HideInInspector]
    public SpellEditorUICenter Center { get; private set; }

    public void Init(SpellEditorUICenter center)
    {
        this.Center = center;
    }

    public void AddNewDamageEffect()
    {
        DamageEffect effect = new DamageEffect();
        effect.damage = 1;
        effect.effectTargetType = EffectTargetType.Target;
        AddNewEffect(effect);
    }

    public void AddNewHealEffect()
    {
        HealEffect effect = new HealEffect();
        effect.healPoint = 1;
        effect.effectTargetType = EffectTargetType.Target;
        AddNewEffect(effect);

    }

    public void AddNewChangeAccuracyEffect()
    {
        ChangeAccuracyEffect effect = new ChangeAccuracyEffect();
        effect.accuracyOffset = 1;
        effect.effectTargetType = EffectTargetType.Target;
        AddNewEffect(effect);
    }

    public void AddNewChangeForceEffect()
    {
        ChangeForceEffect effect = new ChangeForceEffect();
        effect.forceOffset = 1;
        effect.effectTargetType = EffectTargetType.Target;
        AddNewEffect(effect);
    }

    public void AddNewChangeInitiativeEffect()
    {
        ChangeInitiativeEffect effect = new ChangeInitiativeEffect();
        effect.initiativeOffset = 1;
        effect.effectTargetType = EffectTargetType.Target;
        AddNewEffect(effect);
    }

    public void AddNewStateShieldEffect()
    {
        SetStateEffect effect = new SetStateEffect();
        effect.state = MagicanState.Shield;
        effect.effectTargetType = EffectTargetType.Target;
        AddNewEffect(effect);
    }

    public void AddNewStateMirrorEffect()
    {
        SetStateEffect effect = new SetStateEffect();
        effect.state = MagicanState.Mirror;
        effect.effectTargetType = EffectTargetType.Target;
        AddNewEffect(effect);
    }

    public void AddNewStateStunEffect()
    {
        SetStateEffect effect = new SetStateEffect();
        effect.state = MagicanState.Stunned;
        effect.effectTargetType = EffectTargetType.Target;
        AddNewEffect(effect);
    }

    public void AddNewStateMindControllEffect()
    {
        SetStateEffect effect = new SetStateEffect();
        effect.state = MagicanState.MindControl;
        effect.effectTargetType = EffectTargetType.Target;
        AddNewEffect(effect);
    }

    private void AddNewEffect(SpellEffect newEffect)
    {
        if (Center.CurrentSpell.Effect == null)
        {
            Center.CurrentSpell.Effect = newEffect;
        }
        else
        {
            SpellEffect effect = Center.CurrentSpell.Effect;
            while (effect != null)
            {
                if(effect.insideEffect == null)
                {
                    effect.insideEffect = newEffect;
                    break;
                }
                else
                {
                    effect = effect.insideEffect;
                }
            }
        }
        UpdateEffectUIItems();
    }

    public void RemoveEffectFromSpell(SpellEffect effect)
    {
        if (Center.CurrentSpell.Effect == effect)
        {
            if(Center.CurrentSpell.Effect.insideEffect != null)
            {
                Center.CurrentSpell.Effect = Center.CurrentSpell.Effect.insideEffect;
            }
            else
            {
                Center.CurrentSpell.Effect = null;
            }
        }
        else
        {
            SpellEffect buferEffect = Center.CurrentSpell.Effect;
            while (buferEffect.insideEffect != null)
            {
                if(buferEffect.insideEffect == effect)
                {
                    if (buferEffect.insideEffect.insideEffect == null)
                    {
                        buferEffect.insideEffect = null;
                    }
                    else
                    {
                        buferEffect.insideEffect = buferEffect.insideEffect.insideEffect;
                    }
                    break;
                }
                else
                {
                    buferEffect = buferEffect.insideEffect;
                }
            }
        }
        UpdateEffectUIItems();
    }

    public void UpdateEffectUIItems()
    {
        for(int i = 0; i < spellEffectItemsContent.childCount; i++)
        {
            Destroy(spellEffectItemsContent.GetChild(i).gameObject);
        }

        SpellEffect effect = null;

        if(Center.CurrentSpell.Effect != null)
        {
            effect = Center.CurrentSpell.Effect;
        }
        else
        {
            Center.spellEditor.DrawSpellForceRequared();
            RebuildRunes();
            return;
        }

        do
        {
            if(effect is DamageEffect damageEffect)
            {
                Instantiate(damageEffectUIItemPrefab, spellEffectItemsContent).
                    GetComponent<DamageEffectUIItem>().CreateItem(damageEffect, this);
            }
            else if(effect is HealEffect healEffect)
            {
                Instantiate(healEffectUIItemPrefab, spellEffectItemsContent).
                    GetComponent<HealEffectUIItem>().CreateItem(healEffect, this);
            }
            else if (effect is SetStateEffect setStateEffect)
            {
                Instantiate(setStateEffectUIItemPrefab, spellEffectItemsContent).
                    GetComponent<SetStateEffectUIItem>().CreateItem(setStateEffect, this);
            }
            else if (effect is ChangeAccuracyEffect changeAccuracyEffect)
            {
                Instantiate(changeAccuracyEffectUIItemPrefab, spellEffectItemsContent).
                    GetComponent<ChangeAccuracyEffectUIItem>().CreateItem(changeAccuracyEffect, this);
            }
            else if (effect is ChangeForceEffect changeForceEffect)
            {
                Instantiate(changeForceEffectUIItemPrefab, spellEffectItemsContent).
                    GetComponent<ChangeForceEffectUIItem>().CreateItem(changeForceEffect, this);
            }
            else if (effect is ChangeInitiativeEffect changeInitiativeEffect)
            {
                Instantiate(changeInitiativeEffectUIItemPrefab, spellEffectItemsContent).
                    GetComponent<ChangeInitiativeEffectUIItem>().CreateItem(changeInitiativeEffect, this);
            }

            if(effect.insideEffect != null)
            {
                effect = effect.insideEffect;
            }
            else
            {
                break;
            }
        }
        while (true);

        RebuildRunes();

        Center.spellEditor.DrawSpellForceRequared();
    }

    public void ClearRune()
    {
        StopAllCoroutines();

        int count = spellRingEffectTransform.childCount;

        for (int i = 0; i < count; i++)
        {
            Destroy(spellRingEffectTransform.GetChild(i).gameObject);
        }
    }

    public void RebuildRunes()
    {
        ClearRune();

        SpellEffect effect = null;
        List<EffectSpriteItem> sprites = new List<EffectSpriteItem>();

        if (Center.CurrentSpell.Effect != null)
        {
            effect = Center.CurrentSpell.Effect;
        }
        else
        {
            return;
        }

        do
        {
            if (effect is DamageEffect damageEffect)
            {
                if(damageEffect.effectTargetType == EffectTargetType.Target)
                {
                    sprites.Add(Center.spellRunnesHolder.FindEffectItem(EffectType.Damage));
                }
                else
                {
                    sprites.Add(Center.spellRunnesHolder.FindEffectItem(EffectType.BloodSpell));
                }
            }
            else if (effect is HealEffect healEffect)
            {
                sprites.Add(Center.spellRunnesHolder.FindEffectItem(EffectType.Heal));
            }
            else if (effect is SetStateEffect setStateEffect)
            {
                switch (setStateEffect.state)
                {
                    case MagicanState.Shield:
                        sprites.Add(Center.spellRunnesHolder.FindEffectItem(EffectType.Shield));
                        break;
                    case MagicanState.Mirror:
                        sprites.Add(Center.spellRunnesHolder.FindEffectItem(EffectType.Mirror));
                        break;
                    case MagicanState.Stunned:
                        sprites.Add(Center.spellRunnesHolder.FindEffectItem(EffectType.SkipAction));
                        break;
                    case MagicanState.MindControl:
                        sprites.Add(Center.spellRunnesHolder.FindEffectItem(EffectType.MindControl));
                        break;
                    case MagicanState.None:
                        sprites.Add(Center.spellRunnesHolder.FindEffectItem(EffectType.ClearStates));
                        break;
                }
            }
            else if (effect is ChangeAccuracyEffect changeAccuracyEffect)
            {
                if (changeAccuracyEffect.accuracyOffset > 0)
                {
                    sprites.Add(Center.spellRunnesHolder.FindEffectItem(EffectType.ImproveAccuracy));
                }
                else
                {
                    sprites.Add(Center.spellRunnesHolder.FindEffectItem(EffectType.LowerAccuracy));
                }

            }
            else if (effect is ChangeForceEffect changeForceEffect)
            {
                if (changeForceEffect.forceOffset > 0)
                {
                    sprites.Add(Center.spellRunnesHolder.FindEffectItem(EffectType.ImproveForce));
                }
                else
                {
                    sprites.Add(Center.spellRunnesHolder.FindEffectItem(EffectType.LowerForce));
                }
            }
            else if (effect is ChangeInitiativeEffect changeInitiativeEffect)
            {
                if (changeInitiativeEffect.initiativeOffset > 0)
                {
                    sprites.Add(Center.spellRunnesHolder.FindEffectItem(EffectType.ImproveInitiative));
                }
                else
                {
                    sprites.Add(Center.spellRunnesHolder.FindEffectItem(EffectType.LowerInitiative));
                }
            }

            if (effect.insideEffect != null)
            {
                effect = effect.insideEffect;
            }
            else
            {
                break;
            }
        }
        while (true);

        float angle = 360 / sprites.Count;
        float currentAngle = 0;

        foreach (var sprite in sprites)
        {
            StartCoroutine(AddEffectRune(sprite, currentAngle));
            currentAngle += angle;
        }
    }

    private IEnumerator AddEffectRune(EffectSpriteItem effectSpriteItem, float angle)
    {
        yield return null;
        Transform rune = Instantiate(spellRingEffectPrefab, spellRingEffectTransform).transform;
        SpriteRenderer rend = rune.GetChild(0).GetComponent<SpriteRenderer>();
        rend.GetComponent<RuneRotationHolder>().origin = spellRingEffectTransform;
        rend.color = effectSpriteItem.color;
        rend.sprite = effectSpriteItem.sprite;

        float t = 0;

        Quaternion startRotation = rune.rotation;
        Quaternion endRotation = rune.rotation * Quaternion.Euler(0,0, angle);

        while (t < 1)
        {
            t += Time.deltaTime;
            rune.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }
    }
}
