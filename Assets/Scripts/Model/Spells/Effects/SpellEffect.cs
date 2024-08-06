using System;

[Serializable]
public abstract class SpellEffect
{
    public string Description
    {
        get
        {
            string result = _description;
            if(insideEffect != null)
            {
                result += "\n" + insideEffect.Description;
            }
            return _description;
        }
    }
    protected string _description;

    public SpellEffect insideEffect;

    public EffectTargetType effectTargetType;

    public abstract void UseEffectToTarget(Magican user, Magican target, int effectPercent);

    public abstract int CalculateReqareForce();

    public abstract int CalculateReqareForceForThisEffectOnly();

    public static int GetEffectValue(int value, int percent)
    {
        float result = value * (percent / 100);

        return (int)MathF.Round(result);
    }

    public abstract string GetSaveString();
}

public enum EffectTargetType
{
    Target,
    User
}
