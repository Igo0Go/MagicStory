public class SetStateEffect : SpellEffect
{
    public MagicanState state;
    public int duration;

    public SetStateEffect()
    {
        switch(state)
        {
            case MagicanState.Mirror:
                _description = "Заставить цель отражать атаки. Длительность: " + duration;
                break;
            case MagicanState.Stunned:
                _description = "Заставить цель пропускать ходы. Длительность: " + duration;
                break;
            case MagicanState.MindControl:
                _description = "Взять разум цели под контроль. Длительность: " + duration;
                break;
            case MagicanState.Shield:
                _description = "Заставить цель поглощать атаки. Длительность: " + duration;
                break;
            case MagicanState.None:
                _description = "Снять с цели все эффекты";
                break;
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
        if(state == MagicanState.None)
        {
            result = SpellForeceRequreSettings.statesPrice[state];
        }
        else
        {
            result = SpellForeceRequreSettings.statesPrice[state] * duration;
        }
        return result;
    }

    public override void UseEffectToTarget(Magican user, Magican target, int effectPercent)
    {
        if (effectTargetType == EffectTargetType.User)
        {
            user.SetState(state, duration);
        }
        else
        {
            target.SetState(state, duration);
        }
    }

    public override string GetSaveString()
    {
        string result = string.Empty;

        result += "[" + nameof(SetStateEffect) + "|" + duration + ","+ state.ToString() + "," 
            + (effectTargetType == EffectTargetType.Target? 1 : 0) + "]";

        SpellEffect buferEffect = this;

        if (buferEffect.insideEffect != null)
        {
            buferEffect = buferEffect.insideEffect;
            result += FileAccessUtility.propertyPartSeparator + buferEffect.GetSaveString();
        }
        return result;
    }
}
