using UnityEngine;

public class ArrowSpellForm : SpellForm
{
    public ArrowSpellForm()
    {
        _description = "Стрела";
    }

    public override int CalculateReqareForce(SpellEffect effect)
    {
        return (int)SpellFormType.Arrow * effect.CalculateReqareForce();
    }

    public override SpellFormType GetFormType()
    {
        return SpellFormType.Arrow;
    }

    public override (Magican target, int effectPercent) 
        GetTarget(Magican defaultTarget, int userAccuracy, int spellSuccessPercent)
    {
        if(spellSuccessPercent+userAccuracy > Random.Range(0,101))
        {
            return (defaultTarget, 100);
        }
        return (null, 0);
    }

    public override string GetSaveString() => nameof(SpellFormType.Arrow);
}
