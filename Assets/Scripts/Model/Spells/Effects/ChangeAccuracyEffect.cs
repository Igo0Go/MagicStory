using UnityEngine;

public class ChangeAccuracyEffect : SpellEffect
{
    public int accuracyPoints = 1;

    public ChangeAccuracyEffect()
    {
        if (accuracyPoints > 0)
        {
            _description = "Повышает точность цели на " + accuracyPoints;
        }
        else
        {
            _description = "Понижает точность цели на " + Mathf.Abs(accuracyPoints);
        }
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
        int result = 0;

        if (accuracyPoints > 0)
        {
            result = accuracyPoints * SpellForeceRequreSettings.changeAccuracyPointMultiplicator;
        }
        else
        {
            if (effectTargetType == EffectTargetType.Target)
            {
                result = Mathf.Abs(accuracyPoints) * SpellForeceRequreSettings.changeAccuracyPointMultiplicator;
            }
            else
            {
                result = accuracyPoints * SpellForeceRequreSettings.changeAccuracyPointMultiplicator;
            }
        }
        return result;
    }

    public override void UseEffectToTarget(Magican user, Magican target, int effectPercent)
    {
        if (effectTargetType == EffectTargetType.User)
        {
            user.ChangeAccuracty(GetEffectValue(accuracyPoints, effectPercent));
        }
        else
        {
            target.ChangeAccuracty(GetEffectValue(accuracyPoints, effectPercent));
        }
    }

    public override string GetSaveString()
    {
        string result = string.Empty;

        result += "[" + nameof(ChangeAccuracyEffect) + "|" + accuracyPoints + ","
            + (effectTargetType == EffectTargetType.Target ? 1 : 0) + "]";

        SpellEffect buferEffect = this;

        if (buferEffect.insideEffect != null)
        {
            buferEffect = buferEffect.insideEffect;
            result += FileAccessUtility.propertyPartSeparator + buferEffect.GetSaveString();
        }
        return result;
    }
}
