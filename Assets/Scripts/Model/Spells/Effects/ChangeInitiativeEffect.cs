using UnityEngine;

/// <summary>
/// Эффект повышает или понижает добавочную инициативу цели, сдвигая её в порядке ходов
/// </summary>
public class ChangeInitiativeEffect : SpellEffect
{
    /// <summary>
    /// Смещение инициативы
    /// </summary>
    public int initiativeOffset = 1;

    public ChangeInitiativeEffect()
    {
        if (initiativeOffset > 0)
        {
            _description = "Повышает инициативу цели на " + initiativeOffset;
        }
        else
        {
            _description = "Понижает инициативу цели на " + Mathf.Abs(initiativeOffset);
        }
    }

    /// <summary>
    /// Применить эффект на цель - повысить или понизить инициативу цели, чтобы сместить её в порядке ходов
    /// </summary>
    /// <param name="user">Заклинатель</param>
    /// <param name="target">Цель</param>
    /// <param name="effectPercent">Концентрация эффекта</param>
    public override void UseEffectToTarget(Magican user, Magican target, int effectPercent)
    {
        if (effectTargetType == EffectTargetType.User)
        {
            user.ChangeInitiative(GetEffectValue(initiativeOffset, effectPercent));
        }
        else
        {
            target.ChangeInitiative(GetEffectValue(initiativeOffset, effectPercent));
        }
    }

    public override int CalculateWorkLoad()
    {
        int result = CalculateReqareForceForThisEffectOnly();
        if (insideEffect != null)
        {
            result += insideEffect.CalculateWorkLoad();
        }
        return result;
    }

    public override int CalculateReqareForceForThisEffectOnly()
    {
        int result = 0;

        if(initiativeOffset > 0)
        {
            result = initiativeOffset *StatsMultiplicatorPack.initiativePointMultiplicator;
        }
        else
        {
            if(effectTargetType == EffectTargetType.Target)
            {
                result = Mathf.Abs(initiativeOffset) * StatsMultiplicatorPack.initiativePointMultiplicator;
            }
            else
            {
                result = initiativeOffset * StatsMultiplicatorPack.initiativePointMultiplicator;
            }
        }
        return result;
    }

    public override string GetDataString()
    {
        string result = string.Empty;

        result += "[" + nameof(ChangeInitiativeEffect) + "|" + initiativeOffset + ","
            + (effectTargetType == EffectTargetType.Target ? 1 : 0) + "]";

        SpellEffect buferEffect = this;

        if (buferEffect.insideEffect != null)
        {
            buferEffect = buferEffect.insideEffect;
            result += FileAccessUtility.propertyPartSeparator + buferEffect.GetDataString();
        }
        return result;
    }
}
