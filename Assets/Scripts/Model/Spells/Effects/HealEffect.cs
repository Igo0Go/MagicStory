using System;
using UnityEngine;

[Serializable]
public class HealEffect : SpellEffect
{
    [Min(1)]
    public int healPoint;

    public HealEffect()
    {
        _description = "Добавляет цели " + healPoint + " здоровья";
    }

    public override void UseEffectToTarget(Magican user, Magican target, int effectPercent)
    {
        target.Health -= GetEffectValue(healPoint, effectPercent);
    }

    public override int CalculateReqareForce()
    {
        int result = CalculateReqareForceForThisEffectOnly();
        if (insideEffect != null)
        {
            result += insideEffect.CalculateReqareForce();
        }
        return result;
    }

    public override int CalculateReqareForceForThisEffectOnly()
    {
        return healPoint * SpellForeceRequreSettings.healPointMultiplicator;
    }

    public override string GetSaveString()
    {
        string result = string.Empty;

        result += "[" + nameof(HealEffect) + "|" + healPoint + ","
            + (effectTargetType == EffectTargetType.Target ? 1 : 0) + "]";

        SpellEffect buferEffect = this;

        if (buferEffect.insideEffect != null)
        {
            buferEffect = buferEffect.insideEffect;
            result += "," + buferEffect.GetSaveString();
        }
        result += Utility_SpellSaver.stringSeparator;
        return result;
    }
}
