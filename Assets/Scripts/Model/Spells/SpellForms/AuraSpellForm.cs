using UnityEngine;

public class AuraSpellForm : SpellForm
{
    public AuraSpellForm()
    {
        _description = "Аура";
    }

    public override int CalculateReqareForce(SpellEffect effect)
    {
        return (int)SpellFormType.Aura * effect.CalculateReqareForce();
    }

    public override SpellFormType GetFormType()
    {
        return SpellFormType.Aura;
    }

    public override (Magican target, int effectPercent)
        GetTarget(Magican defaultTarget, int userAccuracy, int spellSuccessPercent)
    {
        if (spellSuccessPercent + userAccuracy > Random.Range(0, 101))
        {
            return (defaultTarget, 100);
        }
        else
        {
            //Найти другую цель
        }
        return (null, 0);
    }

    public override string GetSaveString() => nameof(SpellFormType.Aura);
}
