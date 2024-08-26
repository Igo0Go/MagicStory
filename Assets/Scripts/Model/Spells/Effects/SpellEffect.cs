using System;

/// <summary>
/// ������ ����������
/// </summary>
[Serializable]
public abstract class SpellEffect
{
    /// <summary>
    /// ��������
    /// </summary>
    public string Description
    {
        get
        {
            string result = _description;
            if(insideEffect != null)
            {
                result += "\n" + insideEffect.Description;
            }
            return _description;
        }
    }
    protected string _description;

    /// <summary>
    /// ��������� ������
    /// </summary>
    public SpellEffect insideEffect;

    /// <summary>
    /// � ����� ���� �����������
    /// </summary>
    public EffectTargetType effectTargetType;

    /// <summary>
    /// ��������� ������ �� ����
    /// </summary>
    /// <param name="user">�����������</param>
    /// <param name="target">����</param>
    /// <param name="effectPercent">������������ �������</param>
    public abstract void UseEffectToTarget(Magican user, Magican target, int effectPercent);

    /// <summary>
    /// ���������� �������� �������� ��� ����� ������� � �������� ����
    /// </summary>
    /// <returns>��������� ���������� ����</returns>
    public abstract int CalculateWorkLoad();

    /// <summary>
    /// ���������� �������� ��� ����� ������� � ���� ��������� � �������� ����
    /// </summary>
    /// <returns>��������� ���������� ����</returns>
    public abstract int CalculateReqareForceForThisEffectOnly();

    /// <summary>
    /// ���������� �������� ������� � ������ ������������
    /// </summary>
    /// <param name="value">�������� ������</param>
    /// <param name="percent">������� ������������</param>
    /// <returns>�������� �������� ������ � ������ ������������</returns>
    public static int GetEffectValue(int value, int percent)
    {
        float result = value * (percent / 100);

        return (int)MathF.Round(result);
    }

    /// <summary>
    /// ������������� ������ � ��� ��������� � ������ ������
    /// </summary>
    /// <returns>������ ������  � ����������� �� ������� � ���� ���������</returns>
    public abstract string GetDataString();
}

/// <summary>
/// �����, �� ���� ��������� ������: �� ����, ��� �� �����������
/// </summary>
public enum EffectTargetType
{
    Target,
    User
}
