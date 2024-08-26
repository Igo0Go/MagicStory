using UnityEngine;

public class RelationSpellForm : SpellForm
{
    public RelationSpellForm()
    {
        _description = "Связь";
    }

    public override int CalculateWorkLoad(SpellEffect effect)
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
        int complexity = Random.Range(0, 101);
        int accuracy = spellSuccessPercent + userAccuracy;

        if (accuracy >= complexity)
        {
            //попадание
            return (defaultTarget, 100);
        }
        else if (complexity - accuracy > 10)
        {
            //Найти другую цель
        }
        return (null, 0);
    }

    public override string GetSaveString() => nameof(SpellFormType.Relation);
}
