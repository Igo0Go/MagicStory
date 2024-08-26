using UnityEngine;

/// <summary>
///����� ����� ������������, ��� ��� �������� ���� �� ����������� ����,
/// ���� �� ������� �����. �� � ������ ������������ ������� ����� ������� � �� ��������.
/// </summary>
public class RelationSpellForm : SpellForm
{
    public RelationSpellForm()
    {
        _description = "�����";
    }

    public override int CalculateWorkLoad(SpellEffect effect)
    {
        return (int)SpellFormType.Relation * effect.CalculateWorkLoad();
    }

    public override SpellFormType GetFormType()
    {
        return SpellFormType.Relation;
    }

    public override string GetSaveString() => nameof(SpellFormType.Relation);

    /// <summary>
    /// �������� �������� ���� � ������ �������� � ������������ ������� �� ���. 
    /// ��� ����� � ������ ������� ���� - ��� ������ ����, ���� ������� ����� ������� ��������� � ���������� ������ 10.
    /// ����� ���� �������� �������� ������ 10, �� ��� �������� �� ��������. � ��������� ������� ��� �������������.
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
        else if (complexity - accuracy > 10)
        {
            //����� ������ ���� - �����
        }
        else if(accuracy < 10)
        {
            //����� ������ ���� - ��������
        }
        return (null, 0);
    }
}
