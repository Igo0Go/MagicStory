using UnityEngine;

/// <summary>
/// Эффект повышает (до максимальной) или понижает (до нуля) ману цели
/// </summary>
public class ChangeForceEffect : SpellEffect
{
    public int forceOffset = 1;

    public ChangeForceEffect()
    {
        if (forceOffset > 0)
        {
            _description = "Повышает запас мощи цели на " + forceOffset;
        }
        else
        {
            _description = "Понижает запас мощи цели на " + Mathf.Abs(forceOffset);
        }
    }

    /// <summary>
    /// Применить эффект на цель - добавить или отнять ману цели
    /// </summary>
    /// <param name="user">Заклинатель</param>
    /// <param name="target">Цель</param>
    /// <param name="effectPercent">Концентрация эффекта</param>
    public override void UseEffectToTarget(Magican user, Magican target, int effectPercent)
    {
        if (effectTargetType == EffectTargetType.User)
        {
            user.Force += GetEffectValue(forceOffset, effectPercent);
        }
        else
        {
            target.Force += GetEffectValue(forceOffset, effectPercent);
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

        if (forceOffset > 0)
        {
            result = forceOffset * StatsMultiplicatorPack.forcePointMultiplicator;
        }
        else
        {
            if (effectTargetType == EffectTargetType.Target)
            {
                result = Mathf.Abs(forceOffset) * StatsMultiplicatorPack.forcePointMultiplicator;
            }
            else
            {
                result = forceOffset * StatsMultiplicatorPack.forcePointMultiplicator;
            }
        }
        return result;
    }

    public override string GetDataString()
    {
        string result = string.Empty;

        result += "[" + nameof(ChangeForceEffect) + "|" + forceOffset + ","
            + (effectTargetType == EffectTargetType.Target ? 1 : 0) + "]";

        SpellEffect buferEffect = this;

        if(buferEffect.insideEffect != null)
        {
            buferEffect = buferEffect.insideEffect;
            result += FileAccessUtility.propertyPartSeparator + buferEffect.GetDataString();
        }
        return result;
    }
}
