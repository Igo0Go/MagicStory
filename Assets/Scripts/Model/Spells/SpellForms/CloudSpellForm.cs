using UnityEngine;

public class CloudSpellForm : SpellForm
{
    public CloudSpellForm()
    {
        _description = "Облако";
    }


    public override int CalculateWorkLoad(SpellEffect effect)
    {
        return (int)SpellFormType.Cloud * effect.CalculateReqareForce();
    }

    public override SpellFormType GetFormType()
    {
        return SpellFormType.Cloud;
    }

    public override string GetSaveString() => nameof(SpellFormType.Cloud);

    public override (Magican target, int effectPercent) GetTarget
        (Magican defaultTarget, int userAccuracy, int spellSuccessPercent)
    {
        int maxPercent = Random.Range(0, spellSuccessPercent + userAccuracy + 1);

        //Вычисляем концентрацию облака, которая попадает по цели
        return (defaultTarget, maxPercent);
    }
}
