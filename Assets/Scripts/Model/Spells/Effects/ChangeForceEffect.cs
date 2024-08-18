using UnityEngine;

public class ChangeForceEffect : SpellEffect
{
    public int forcePoints = 1;

    public ChangeForceEffect()
    {
        if (forcePoints > 0)
        {
            _description = "Повышает запас мощи цели на " + forcePoints;
        }
        else
        {
            _description = "Понижает запас мощи цели на " + Mathf.Abs(forcePoints);
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

        if (forcePoints > 0)
        {
            result = forcePoints * SpellForeceRequreSettings.initiativePointMultiplicator;
        }
        else
        {
            if (effectTargetType == EffectTargetType.Target)
            {
                result = Mathf.Abs(forcePoints) * SpellForeceRequreSettings.initiativePointMultiplicator;
            }
            else
            {
                result = forcePoints * SpellForeceRequreSettings.initiativePointMultiplicator;
            }
        }
        return result;
    }

    public override string GetSaveString()
    {
        string result = string.Empty;

        result += "[" + nameof(ChangeForceEffect) + "|" + forcePoints + ","
            + (effectTargetType == EffectTargetType.Target ? 1 : 0) + "]";

        SpellEffect buferEffect = this;

        if(buferEffect.insideEffect != null)
        {
            buferEffect = buferEffect.insideEffect;
            result += FileAccessUtility.propertyPartSeparator + buferEffect.GetSaveString();
        }
        return result;
    }

    public override void UseEffectToTarget(Magican user, Magican target, int effectPercent)
    {
        if (effectTargetType == EffectTargetType.User)
        {
            user.Force += GetEffectValue(forcePoints, effectPercent);
        }
        else
        {
            target.Force += GetEffectValue(forcePoints, effectPercent);
        }
    }
}
