using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ����
/// </summary>
[System.Serializable]
public class Magican
{
    #region �������� ��������������

    /// <summary>
    /// ��� ����
    /// </summary>
    public string Name;

    /// <summary>
    /// ������� ���������
    /// </summary>
    public int CharacterPortraitIndex = 0;

    /// <summary>
    /// ������� ����� ��������
    /// </summary>
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = Mathf.Clamp(value, 0, _maxHealth);
        }
    }
    private int _health;
    private int _maxHealth = 50;

    /// <summary>
    /// ������� ����� ����
    /// </summary>
    public int Force
    {
        get
        {
            return _force;
        }
        set
        {
            _force = Mathf.Clamp(value, 0, _defaultForce);
        }
    }
    private int _force;
    private int _defaultForce = 50;

    /// <summary>
    /// ������� �������� � ������ ���������
    /// </summary>
    public int Accuracy => Mathf.Clamp(_accuracy, 0, 100);
    private int _accuracy;

    /// <summary>
    /// ������� ���������� � ������ ���������
    /// </summary>
    public int Initiative => _initiative;
    private int _initiative;

    #endregion

    #region ���������� ��������������

    /// <summary>
    /// ��������� �����
    /// </summary>
    public MagicanState State { get; set; } = MagicanState.None;

    /// <summary>
    /// ����� ���������� ����
    /// </summary>
    public List<Spell> SpellsBook { get; set; } = new List<Spell>();

    /// <summary>
    /// ���, ��� �������� �������
    /// </summary>
    public Magican MindControllMaster { get; set; }

    /// <summary>
    /// �������������� � ������ ������ ����������
    /// </summary>
    public Spell CurrentSpell { get; private set; }

    public Dictionary<MagicanState, int> CurrentMagicanStates { get; set; } = new Dictionary<MagicanState, int>();

    #endregion

    /// <summary>
    /// ������ ��������� ���� �� ������ ����������������
    /// </summary>
    /// <param name="name">���</param>
    /// <param name="maxHealth">������������ ����� ��������</param>
    /// <param name="defaultForce">����������� �������� ����</param>
    /// <param name="color">���� � �������</param>
    public Magican(string name, int maxHealth, int defaultForce)
    {
        Name = name;
        _maxHealth = maxHealth;
        Health = maxHealth;
        _defaultForce = defaultForce;
        Force = defaultForce;
        _initiative = 0;
        _accuracy = 0;
    }

    #region ��������

    /// <summary>
    /// �������� ���� ������ �� ����������� ���������� �����
    /// </summary>
    /// <param name="state">������</param>
    /// <param name="moviesNumber">���������� �����</param>
    public void SetState(MagicanState state, int moviesNumber)
    {
        if(state == MagicanState.None)
        {
            CurrentMagicanStates.Clear();
        }

        if(CurrentMagicanStates.ContainsKey(state))
        {
            CurrentMagicanStates[state] += moviesNumber;
        }
        else
        {
            CurrentMagicanStates.Add(state, moviesNumber);
        }
    }

    /// <summary>
    /// ��������� ������������ �������� �� ���� ���
    /// </summary>
    public void LowerStatsDuration()
    {
        foreach(var state in CurrentMagicanStates)
        {
            if(state.Value > 0)
            {
                CurrentMagicanStates[state.Key]--;
            }
        }
    }

    /// <summary>
    /// ����� ��� ������ � ������������
    /// </summary>
    public void ReturnStats()
    {
        _accuracy = 0;
        _initiative = 0;
    }

    #endregion

    #region ��������� �������� �������������

    /// <summary>
    /// ���������� �������� ������ ����
    /// </summary>
    /// <returns>�������� ���� � �������� ��������</returns>
    public int CalculateMagicanStatsWorkLoad()
    {
        int result = 0;

        result += _maxHealth * StatsMultiplicatorPack.healPointMultiplicator;
        result += _defaultForce * StatsMultiplicatorPack.forcePointMultiplicator;

        return result;
    }
    /// <summary>
    /// ���������� �������� ����� ���������� ����
    /// </summary>
    /// <returns>�������� ����� ���������� ���� � �������� ��������</returns>
    public int CalculateMagicanSpellsWorkload()
    {
        int result = 0;
        foreach (Spell spell in SpellsBook)
        {
            result += spell.CalculateWorkLoad();
        }
        return result;
    }

    /// <summary>
    /// �������� ���������� �������� �������� ����. ���� ������������ ��� ������
    /// </summary>
    /// <param name="accuracyPoints">�������� ���������� ��������</param>
    public void ChangeAccuracty(int accuracyPoints) => _accuracy += accuracyPoints;
    /// <summary>
    /// �������� ���������� �������� ���������� ����. ���� ������������ ��� ������
    /// </summary>
    /// <param name="initiativePoints">�������� ���������� ����������</param>
    public void ChangeInitiative(int initiativePoints) => _initiative += initiativePoints;

    /// <summary>
    /// ��������� ����� �������� ������������� �������� ����
    /// </summary>
    /// <param name="value">����� �������� ������������� ��������</param>
    public void SetMaxHealth(int value) => _maxHealth = value;
    /// <summary>
    /// �������� ����� �������� ������������ ���� ����
    /// </summary>
    /// <param name="forcePoints">����� �������� ������������ ����</param>
    public void SetMaxForce(int forcePoints) => _defaultForce = forcePoints;

    #endregion

    #region ������ � �������
    /// <summary>
    /// ������������� ������ ������ � ��������� ����
    /// </summary>
    /// <param name="dataString">������ ������</param>
    /// <returns></returns>
    public static Magican GetMagican(string dataString)
    {
        string[] MagicanProperties = dataString.Split(FileAccessUtility.stringSeparator,
            StringSplitOptions.RemoveEmptyEntries);

        string name = string.Empty;
        int maxHealth = 0;
        int defaultForce = 0;
        int defaultInitiative = 0;
        int portraitIndex = 0;

        int spellBeginningIndex = -1;

        for (int i = 0; i < MagicanProperties.Length; i++)
        {
            string MagicanProperty = MagicanProperties[i];
            string[] bufer = MagicanProperty.Split(":");

            if (bufer[0].Equals("SPELLS"))
            {
                spellBeginningIndex = i;
                break;
            }
            else
            {
                if (bufer[0].Equals(nameof(Name)))
                {
                    name = bufer[1];
                }
                else if (bufer[0].Equals(nameof(CharacterPortraitIndex)))
                {
                    portraitIndex = int.Parse(bufer[1]);
                }
                else if (bufer[0].Equals(nameof(_maxHealth)))
                {
                    maxHealth = int.Parse(bufer[1]);
                }
                else if (bufer[0].Equals(nameof(_defaultForce)))
                {
                    defaultForce = int.Parse(bufer[1]);
                }
                else if (bufer[0].Equals(nameof(_initiative)))
                {
                    defaultInitiative = int.Parse(bufer[1]);
                }
            }
        }

        Magican magican = new Magican(name, maxHealth, defaultForce);

        magican.CharacterPortraitIndex = portraitIndex;
        magican.SpellsBook = new List<Spell>();

        for (int i = spellBeginningIndex + 1; i < MagicanProperties.Length; i++)
        {
            magican.SpellsBook.Add(Spell.GetSpellByInfo(MagicanProperties[i]));
        }

        return magican;
    }
    /// <summary>
    /// �������� ������ ������ ��� ����� ����
    /// </summary>
    /// <returns></returns>
    public string GetSaveString()
    {
        string result = string.Empty;

        result += nameof(Name) + ":" + Name + FileAccessUtility.stringSeparator;
        result += nameof(CharacterPortraitIndex) + ":" + CharacterPortraitIndex + FileAccessUtility.stringSeparator;
        result += nameof(_maxHealth) + ":" + _maxHealth + FileAccessUtility.stringSeparator;
        result += nameof(_defaultForce) + ":" + _defaultForce + FileAccessUtility.stringSeparator;
        result += nameof(_initiative) + ":" + _initiative + FileAccessUtility.stringSeparator;
        result += "SPELLS"+ FileAccessUtility.stringSeparator;

        foreach (var item in SpellsBook)
        {
            result += item.GetDataString() + FileAccessUtility.stringSeparator;
        }
        return result;
    }
    #endregion
}