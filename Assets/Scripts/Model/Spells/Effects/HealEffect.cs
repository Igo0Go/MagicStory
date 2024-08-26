/// <summary>
/// ������ �������� ����, ���������� (�� � ���������) ���������� ����� ��������
/// </summary>
public class HealEffect : SpellEffect
{
    /// <summary>
    /// ���������� ����� �������, ������� ����� ������������� ����
    /// </summary>
    public int healPoint;

    public HealEffect()
    {
        _description = "��������� ���� " + healPoint + " ��������";
    }

    /// <summary>
    /// ��������� ������ �� ���� - �������� �
    /// </summary>
    /// <param name="user">�����������</param>
    /// <param name="target">����</param>
    /// <param name="effectPercent">������������ �������</param>
    public override void UseEffectToTarget(Magican user, Magican target, int effectPercent)
    {
        target.Health -= GetEffectValue(healPoint, effectPercent);
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
        return healPoint * StatsMultiplicatorPack.healPointMultiplicator;
    }

    public override string GetDataString()
    {
        string result = string.Empty;

        result += "[" + nameof(HealEffect) + "|" + healPoint + ","
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
