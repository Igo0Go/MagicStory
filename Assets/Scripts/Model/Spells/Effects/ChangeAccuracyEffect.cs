using UnityEngine;

/// <summary>
/// Эффект смещает занчение точности, которую маг прибавляет к броскам атаки
/// </summary>
public class ChangeAccuracyEffect : SpellEffect
{
    /// <summary>
    /// Смещение точности
    /// </summary>
    public int accuracyOffset = 1;

    public ChangeAccuracyEffect()
    {
        if (accuracyOffset > 0)
        {
            _description = "Повышает точность цели на " + accuracyOffset;
        }
        else
        {
            _description = "Понижает точность цели на " + Mathf.Abs(accuracyOffset);
        }
    }

    /// <summary>
    /// Применить эффект на цель - добавить или понизить добавочную точность мага
    /// </summary>
    /// <param name="user">Заклинатель</param>
    /// <param name="target">Цель</param>
    /// <param name="effectPercent">Концентрация эффекта</param>
    public override void UseEffectToTarget(Magican user, Magican target, int effectPercent)
    {
        if (effectTargetType == EffectTargetType.User)
        {
            user.ChangeAccuracty(GetEffectValue(accuracyOffset, effectPercent));
        }
        else
        {
            target.ChangeAccuracty(GetEffectValue(accuracyOffset, effectPercent));
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

        if (accuracyOffset > 0)
        {
            result = accuracyOffset * StatsMultiplicatorPack.changeAccuracyPointMultiplicator;
        }
        else
        {
            if (effectTargetType == EffectTargetType.Target)
            {
                result = Mathf.Abs(accuracyOffset) * StatsMultiplicatorPack.changeAccuracyPointMultiplicator;
            }
            else
            {
                result = accuracyOffset * StatsMultiplicatorPack.changeAccuracyPointMultiplicator;
            }
        }
        return result;
    }

    public override string GetDataString()
    {
        string result = string.Empty;

        result += "[" + nameof(ChangeAccuracyEffect) + "|" + accuracyOffset + ","
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
