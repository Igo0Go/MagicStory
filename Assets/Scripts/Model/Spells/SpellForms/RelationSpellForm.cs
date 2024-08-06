using UnityEngine;

public class RelationSpellForm : SpellForm
{
    public RelationSpellForm()
    {
        _description = "Связь";
    }

    public override int CalculateReqareForce(SpellEffect effect)
    {
        return (int)SpellFormType.Relation * effect.CalculateReqareForce();
    }

    public override SpellFormType GetFormType()
    {
        return SpellFormType.Relation;
    }

    public override (Magican target, int effectPercent)
        GetTarget(Magican defaultTarget, int userAccuracy, int spellSuccessPercent)
    {
        if (spellSuccessPercent + userAccuracy > Random.Range(0, 101))
        {
            return (defaultTarget, 100);
        }
        else if (spellSuccessPercent + userAccuracy > Random.Range(0, 76))
        {
            //Найти другую цель
        }
        return (null, 0);
    }

    public override string GetSaveString() => nameof(SpellFormType.Relation);
}
