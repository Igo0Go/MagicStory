using UnityEngine;

/// <summary>
/// ������ �������� ��� �������� ���������� ���������� ����, ������� � � ������� �����
/// </summary>
public class ChangeInitiativeEffect : SpellEffect
{
    /// <summary>
    /// �������� ����������
    /// </summary>
    public int initiativeOffset = 1;

    public ChangeInitiativeEffect()
    {
        if (initiativeOffset > 0)
        {
            _description = "�������� ���������� ���� �� " + initiativeOffset;
        }
        else
        {
            _description = "�������� ���������� ���� �� " + Mathf.Abs(initiativeOffset);
        }
    }

    /// <summary>
    /// ��������� ������ �� ���� - �������� ��� �������� ���������� ����, ����� �������� � � ������� �����
    /// </summary>
    /// <param name="user">�����������</param>
    /// <param name="target">����</param>
    /// <param name="effectPercent">������������ �������</param>
    public override void UseEffectToTarget(Magican user, Magican target, int effectPercent)
    {
        if (effectTargetType == EffectTargetType.User)
        {
            user.ChangeInitiative(GetEffectValue(initiativeOffset, effectPercent));
        }
        else
        {
            target.ChangeInitiative(GetEffectValue(initiativeOffset, effectPercent));
        }
    }

    public override int CalculateWorkLoad()
    {
        int result = CalculateReqareForceForThisEffectOnly();
        if (insideEffect != null)
        {
            result += insideEffect.CalculateWorkLoad();
        }
        return result;
    }

    public override int CalculateReqareForceForThisEffectOnly()
    {
        int result = 0;

        if(initiativeOffset > 0)
        {
            result = initiativeOffset *StatsMultiplicatorPack.initiativePointMultiplicator;
        }
        else
        {
            if(effectTargetType == EffectTargetType.Target)
            {
                result = Mathf.Abs(initiativeOffset) * StatsMultiplicatorPack.initiativePointMultiplicator;
            }
            else
            {
                result = initiativeOffset * StatsMultiplicatorPack.initiativePointMultiplicator;
            }
        }
        return result;
    }

    public override string GetDataString()
    {
        string result = string.Empty;

        result += "[" + nameof(ChangeInitiativeEffect) + "|" + initiativeOffset + ","
            + (effectTargetType == EffectTargetType.Target ? 1 : 0) + "]";

        SpellEffect buferEffect = this;

        if (buferEffect.insideEffect != null)
        {
            buferEffect = buferEffect.insideEffect;
            result += FileAccessUtility.propertyPartSeparator + buferEffect.GetDataString();
        }
        return result;
    }
}
