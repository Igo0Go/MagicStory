using UnityEngine;

public class ChangeInitiativeEffect : SpellEffect
{
    public int initiativePoints = 1;

    public ChangeInitiativeEffect()
    {
        if (initiativePoints > 0)
        {
            _description = "Повышает инициативу цели на " + initiativePoints;
        }
        else
        {
            _description = "Понижает инициативу цели на " + Mathf.Abs(initiativePoints);
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

        if(initiativePoints > 0)
        {
            result = initiativePoints *SpellForeceRequreSettings.initiativePointMultiplicator;
        }
        else
        {
            if(effectTargetType == EffectTargetType.Target)
            {
                result = Mathf.Abs(initiativePoints) * SpellForeceRequreSettings.initiativePointMultiplicator;
            }
            else
            {
                result = initiativePoints * SpellForeceRequreSettings.initiativePointMultiplicator;
            }
        }
        return result;
    }

    public override void UseEffectToTarget(Magican user, Magican target, int effectPercent)
    {
        if (effectTargetType == EffectTargetType.User)
        {
            user.ChangeInitiative(GetEffectValue(initiativePoints, effectPercent));
        }
        else
        {
            target.ChangeInitiative(GetEffectValue(initiativePoints, effectPercent));
        }
    }

    public override string GetSaveString()
    {
        string result = string.Empty;

        result += "[" + nameof(ChangeInitiativeEffect) + "|" + initiativePoints + ","
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
