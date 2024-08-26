using UnityEngine;

/// <summary>
/// ������ ������� �������� ��������, ������� ��� ���������� � ������� �����
/// </summary>
public class ChangeAccuracyEffect : SpellEffect
{
    /// <summary>
    /// �������� ��������
    /// </summary>
    public int accuracyOffset = 1;

    public ChangeAccuracyEffect()
    {
        if (accuracyOffset > 0)
        {
            _description = "�������� �������� ���� �� " + accuracyOffset;
        }
        else
        {
            _description = "�������� �������� ���� �� " + Mathf.Abs(accuracyOffset);
        }
    }

    /// <summary>
    /// ��������� ������ �� ���� - �������� ��� �������� ���������� �������� ����
    /// </summary>
    /// <param name="user">�����������</param>
    /// <param name="target">����</param>
    /// <param name="effectPercent">������������ �������</param>
    public override void UseEffectToTarget(Magican user, Magican target, int effectPercent)
    {
        if (effectTargetType == EffectTargetType.User)
        {
            user.ChangeAccuracty(GetEffectValue(accuracyOffset, effectPercent));
        }
        else
        {
            target.ChangeAccuracty(GetEffectValue(accuracyOffset, effectPercent));
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

        if (accuracyOffset > 0)
        {
            result = accuracyOffset * StatsMultiplicatorPack.changeAccuracyPointMultiplicator;
        }
        else
        {
            if (effectTargetType == EffectTargetType.Target)
            {
                result = Mathf.Abs(accuracyOffset) * StatsMultiplicatorPack.changeAccuracyPointMultiplicator;
            }
            else
            {
                result = accuracyOffset * StatsMultiplicatorPack.changeAccuracyPointMultiplicator;
            }
        }
        return result;
    }

    public override string GetDataString()
    {
        string result = string.Empty;

        result += "[" + nameof(ChangeAccuracyEffect) + "|" + accuracyOffset + ","
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
