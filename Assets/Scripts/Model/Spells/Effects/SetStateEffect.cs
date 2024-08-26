/// <summary>
/// ������ ����� ���� ����������� ������ �� ��������� ���������� �����
/// </summary>
public class SetStateEffect : SpellEffect
{
    /// <summary>
    /// ������
    /// </summary>
    public MagicanState state;
    /// <summary>
    /// ������������ ������� � �����
    /// </summary>
    public int duration;

    public SetStateEffect()
    {
        switch(state)
        {
            case MagicanState.Mirror:
                _description = "��������� ���� �������� �����. ������������: " + duration;
                break;
            case MagicanState.Stunned:
                _description = "��������� ���� ���������� ����. ������������: " + duration;
                break;
            case MagicanState.MindControl:
                _description = "����� ����� ���� ��� ��������. ������������: " + duration;
                break;
            case MagicanState.Shield:
                _description = "��������� ���� ��������� �����. ������������: " + duration;
                break;
            case MagicanState.None:
                _description = "����� � ���� ��� �������";
                break;
        }
    }

    /// <summary>
    /// ��������� ������ �� ���� - ������ �� ��������� ������ �� ��������� ���������� �����
    /// </summary>
    /// <param name="user">�����������</param>
    /// <param name="target">����</param>
    /// <param name="effectPercent">������������ �������</param>
    public override void UseEffectToTarget(Magican user, Magican target, int effectPercent)
    {
        if (effectTargetType == EffectTargetType.User)
        {
            user.SetState(state, duration);
        }
        else
        {
            target.SetState(state, duration);
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
        if(state == MagicanState.None)
        {
            result = StatsMultiplicatorPack.statesPrice[state];
        }
        else
        {
            result = StatsMultiplicatorPack.statesPrice[state] * duration;
        }
        return result;
    }

    public override string GetDataString()
    {
        string result = string.Empty;

        result += "[" + nameof(SetStateEffect) + "|" + duration + ","+ state.ToString() + "," 
            + (effectTargetType == EffectTargetType.Target? 1 : 0) + "]";

        SpellEffect buferEffect = this;

        if (buferEffect.insideEffect != null)
        {
            buferEffect = buferEffect.insideEffect;
            result += FileAccessUtility.propertyPartSeparator + buferEffect.GetDataString();
        }
        return result;
    }
}
