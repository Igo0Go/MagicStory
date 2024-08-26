/// <summary>
/// ����� ����������
/// </summary>
public abstract class SpellForm
{
    /// <summary>
    /// �������� ����������
    /// </summary>
    public string Description => _description;
    protected string _description;

    /// <summary>
    /// ���������� �������� ������� ���������� ����� ���������� �����
    /// </summary>
    /// <param name="effect"></param>
    /// <returns></returns>
    public abstract int CalculateWorkLoad(SpellEffect effect);

    /// <summary>
    /// �������� ��� ����� ����������
    /// </summary>
    /// <returns>��� ����� ����������</returns>
    public abstract SpellFormType GetFormType();

    /// <summary>
    /// �������� ������ ������ ��� ����������
    /// </summary>
    /// <returns>������ ������</returns>
    public abstract string GetSaveString();

    /// <summary>
    /// �������� �������� ���� � ������ �������� � ������������ ������� �� ���
    /// </summary>
    /// <param name="defaultTarget">����������� ����</param>
    /// <param name="userAccuracy">�������� ����������� - ����� ��� �����</param>
    /// <param name="spellSuccessPercent">�������� �������� ����������</param>
    /// <returns>������ (�������� ���� � ������ �������, ������������ ������� �� ���)</returns>
    public abstract (Magican target, int effectPercent)
        GetTargetEndEffectPercent(Magican defaultTarget, int userAccuracy, int spellSuccessPercent);
}
