using UnityEngine;

[System.Serializable]
public class DamageEffect : SpellEffect
{
    [Min(1)]
    public int damage;

    public DamageEffect()
    {
        _description = "Наносит цели " + damage + " урона";
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
        if (effectTargetType == EffectTargetType.Target)
        {
            result = damage * SpellForeceRequreSettings.damagePointMultiplicator;
        }
        else
        {
            result = -damage * SpellForeceRequreSettings.damagePointMultiplicator;
        }
        return result;
    }

    public override void UseEffectToTarget(Magican user, Magican target, int effectPercent)
    {
        if(effectTargetType == EffectTargetType.User)
        {
            user.Health -= SpellEffect.GetEffectValue(damage, effectPercent);
        }
        else
        {
            if(target == user)
            {
                target.Health -= GetEffectValue(damage, effectPercent);
            }
            else
            {
                user.Health -= GetEffectValue(damage, effectPercent);
            }
        }
    }

    public override string GetSaveString()
    {
        string result = string.Empty;

        result += "[" + nameof(DamageEffect) + "|" + damage + ","
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
