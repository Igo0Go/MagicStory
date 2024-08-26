using UnityEngine;

/// <summary>
///����� ���� ������������, ��� ��� ���� �������� � ��������� ������ ��������� �� ����������� ����,
///���� ��������� ������ �� �����-�� ������ ����� ����
/// ���� �������������.
/// </summary>
public class AuraSpellForm : SpellForm
{
    public AuraSpellForm()
    {
        _description = "����";
    }

    public override int CalculateWorkLoad(SpellEffect effect)
    {
        return (int)SpellFormType.Aura * effect.CalculateWorkLoad();
    }

    public override SpellFormType GetFormType()
    {
        return SpellFormType.Aura;
    }

    public override string GetSaveString() => nameof(SpellFormType.Aura);

    /// <summary>
    /// �������� �������� ���� � ������ �������� � ������������ ������� �� ���. 
    /// ��� ���� � ������ ������� ���� - ��� ������ ����.
    /// </summary>
    /// <param name="defaultTarget">����������� ����</param>
    /// <param name="userAccuracy">�������� ����������� - ����� ��� �����</param>
    /// <param name="spellSuccessPercent">�������� �������� ����������</param>
    /// <returns>������ (�������� ���� � ������ �������, ������������ ������� �� ���)</returns>
    public override (Magican target, int effectPercent)
        GetTargetEndEffectPercent(Magican defaultTarget, int userAccuracy, int spellSuccessPercent)
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
}
