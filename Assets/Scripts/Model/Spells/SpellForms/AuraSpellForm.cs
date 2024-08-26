using UnityEngine;

public class AuraSpellForm : SpellForm
{
    public AuraSpellForm()
    {
        _description = "����";
    }

    public override int CalculateWorkLoad(SpellEffect effect)
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
        int complexity = Random.Range(0, 101);
        int accuracy = spellSuccessPercent + userAccuracy;

        if (accuracy >= complexity)
        {
            //���������
            return (defaultTarget, 100);
        }
        else
        {
            //����� ������ ����
        }

        //��������
        return (null, 0);
    }

    public override string GetSaveString() => nameof(SpellFormType.Aura);
}
