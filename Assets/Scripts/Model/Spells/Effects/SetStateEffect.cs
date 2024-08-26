/// <summary>
/// Эффект задаёт цели определённый статус на указанное количество ходов
/// </summary>
public class SetStateEffect : SpellEffect
{
    /// <summary>
    /// Статус
    /// </summary>
    public MagicanState state;
    /// <summary>
    /// Длительность статуса в ходах
    /// </summary>
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

    /// <summary>
    /// Применить эффект на цель - задать ей указанный статус на указанное количество ходов
    /// </summary>
    /// <param name="user">Заклинатель</param>
    /// <param name="target">Цель</param>
    /// <param name="effectPercent">Концентрация эффекта</param>
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
        if(state == MagicanState.None)
        {
            result = StatsMultiplicatorPack.statesPrice[state];
        }
        else
        {
            result = StatsMultiplicatorPack.statesPrice[state] * duration;
        }
        return result;
    }

    public override string GetDataString()
    {
        string result = string.Empty;

        result += "[" + nameof(SetStateEffect) + "|" + duration + ","+ state.ToString() + "," 
            + (effectTargetType == EffectTargetType.Target? 1 : 0) + "]";

        SpellEffect buferEffect = this;

        if (buferEffect.insideEffect != null)
        {
            buferEffect = buferEffect.insideEffect;
            result += FileAccessUtility.propertyPartSeparator + buferEffect.GetDataString();
        }
        return result;
    }
}
