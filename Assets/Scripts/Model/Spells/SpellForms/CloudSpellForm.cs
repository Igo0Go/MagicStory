using UnityEngine;

/// <summary>
///����� ������ ������������, ��� ��� ������ �������� �� ����������� ����.
/// ������ � ���, ��������� �� ���������� ������
/// </summary>
public class CloudSpellForm : SpellForm
{
    public CloudSpellForm()
    {
        _description = "������";
    }

    public override int CalculateWorkLoad(SpellEffect effect)
    {
        return (int)SpellFormType.Cloud * effect.CalculateWorkLoad();
    }

    public override SpellFormType GetFormType()
    {
        return SpellFormType.Cloud;
    }

    public override string GetSaveString() => nameof(SpellFormType.Cloud);

    /// <summary>
    /// �������� �������� ���� � ������ �������� � ������������ ������� �� ���. 
    /// ��� ������ ���� � ������ ������� ���� - ��� �������� ����.
    /// ������������ ����� ������������� �� �������� �� ������� �������
    /// </summary>
    /// <param name="defaultTarget">����������� ����</param>
    /// <param name="userAccuracy">�������� ����������� - ����� ��� �����</param>
    /// <param name="spellSuccessPercent">�������� �������� ����������</param>
    /// <returns>������ (�������� ���� � ������ �������, ������������ ������� �� ���)</returns>
    public override (Magican target, int effectPercent) GetTargetEndEffectPercent
        (Magican defaultTarget, int userAccuracy, int spellSuccessPercent)
    {
        int maxPercent = Mathf.Clamp(Random.Range(25, spellSuccessPercent + userAccuracy + 1), 50, 100);

        //��������� ������������ ������, ������� �������� �� ����
        return (defaultTarget, maxPercent);
    }
}
