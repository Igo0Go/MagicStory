using UnityEngine;

/// <summary>
/// ����� ������ ������������, ��� ��� ���� �������� � ��������� ������ ���������, ���� �������������.
/// </summary>
public class ArrowSpellForm : SpellForm
{
    public ArrowSpellForm()
    {
        _description = "������";
    }

    public override int CalculateWorkLoad(SpellEffect effect)
    {
        return (int)SpellFormType.Arrow * effect.CalculateWorkLoad();
    }

    public override SpellFormType GetFormType()
    {
        return SpellFormType.Arrow;
    }

    public override string GetSaveString() => nameof(SpellFormType.Arrow);

    /// <summary>
    /// �������� �������� ���� � ������ �������� � ������������ ������� �� ���. 
    /// ��� ������ � ������ ������� ���� - ��� null. ��� �� �������� �� ����� �����
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

        if (accuracy > complexity)
        {
            //���������
            return (defaultTarget, 100);
        }
        return (null, 0);
    }
}
