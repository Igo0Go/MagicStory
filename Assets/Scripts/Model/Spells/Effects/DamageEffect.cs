/// <summary>
/// Эффект наносит цели урон, уменьшая (до нуля) количество очков здоровья
/// </summary>
public class DamageEffect : SpellEffect
{
    /// <summary>
    /// Урон
    /// </summary>
    public int damage;

    public DamageEffect()
    {
        _description = "Наносит цели " + damage + " урона";
    }

    /// <summary>
    /// Применить эффект на цель - нанести ей урон
    /// </summary>
    /// <param name="user">Заклинатель</param>
    /// <param name="target">Цель</param>
    /// <param name="effectPercent">Концентрация эффекта</param>
    public override void UseEffectToTarget(Magican user, Magican target, int effectPercent)
    {
        if (effectTargetType == EffectTargetType.User)
        {
            user.Health -= GetEffectValue(damage, effectPercent);
        }
        else
        {
            if (target == user)
            {
                target.Health -= GetEffectValue(damage, effectPercent);
            }
            else
            {
                user.Health -= GetEffectValue(damage, effectPercent);
            }
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
        if (effectTargetType == EffectTargetType.Target)
        {
            result = damage * StatsMultiplicatorPack.damagePointMultiplicator;
        }
        else
        {
            result = -damage * StatsMultiplicatorPack.damagePointMultiplicator;
        }
        return result;
    }

    public override string GetDataString()
    {
        string result = string.Empty;

        result += "[" + nameof(DamageEffect) + "|" + damage + ","
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
