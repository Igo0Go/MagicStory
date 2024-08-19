using System;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

/// <summary>
/// ����� ����
/// </summary>
[System.Serializable]
public class Magican
{
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
    private int _defaultForce;

    /// <summary>
    /// ������� �������� � ������ ���������
    /// </summary>
    public int Accuracy => Mathf.Clamp(_defaultAccuracy + _changedAccuracy, 0, 100);
    private int _defaultAccuracy;
    private int _changedAccuracy;

    /// <summary>
    /// ������� ���������� � ������ ���������
    /// </summary>
    public int Initiative => _defaultInitiative + _changedInitiative;
    private int _defaultInitiative;
    private int _changedInitiative;

    /// <summary>
    /// ��������� �����
    /// </summary>
    public MagicanState State { get; set; } = MagicanState.None;

    /// <summary>
    /// ����� ���������� ����
    /// </summary>
    public List<Spell> spellsBook { get; set; } = new List<Spell>();

    /// <summary>
    /// ���, ��� �������� �������
    /// </summary>
    public Magican MindControllMaster { get; set; }

    /// <summary>
    /// �������������� � ������ ������ ����������
    /// </summary>
    public Spell CurrentSpell { get; private set; }

    public Dictionary<MagicanState, int> CurrentMagicanStates { get; set; } = new Dictionary<MagicanState, int>();

    /// <summary>
    /// ������ ��������� ���� �� ������ ����������������
    /// </summary>
    /// <param name="name">���</param>
    /// <param name="maxHealth">������������ ����� ��������</param>
    /// <param name="defaultForce">����������� �������� ����</param>
    /// <param name="color">���� � �������</param>
    public Magican(string name, int maxHealth, int defaultForce, int defaultAccuracy, 
        int defaultInitiative)
    {
        Name = name;
        _maxHealth = maxHealth;
        Health = maxHealth;
        _defaultForce = defaultForce;
        Force = defaultForce;
        _defaultInitiative = defaultInitiative;
        _defaultAccuracy = defaultAccuracy;
    }

    public static Magican GetMagican(string dataString)
    {
        string[] MagicanProperties = dataString.Split(FileAccessUtility.stringSeparator,
            StringSplitOptions.RemoveEmptyEntries);

        string name = string.Empty;
        int maxHealth = 0;
        int defaultForce = 0;
        int defaultAccuracy = 0;
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
                else if (bufer[0].Equals(nameof(_defaultAccuracy)))
                {
                    defaultAccuracy = int.Parse(bufer[1]);
                }
                else if (bufer[0].Equals(nameof(_defaultInitiative)))
                {
                    defaultInitiative = int.Parse(bufer[1]);
                }
            }
        }

        Magican magican = new Magican(name, maxHealth, defaultForce, defaultAccuracy, defaultInitiative);

        magican.CharacterPortraitIndex = portraitIndex;
        magican.spellsBook = new List<Spell>();

        for (int i = spellBeginningIndex+1; i < MagicanProperties.Length; i++)
        {
            magican.spellsBook.Add(Spell.GetSpellByInfo(MagicanProperties[i]));
        }

        return magican;
    }

    public void SetState(MagicanState state, int moviesNumber)
    {
        if(state == MagicanState.None)
        {
            CurrentMagicanStates.Clear();
        }

        if(CurrentMagicanStates.ContainsKey(state))
        {
            CurrentMagicanStates[state] = moviesNumber;
        }
        else
        {
            CurrentMagicanStates.Add(state, moviesNumber);
        }
    }

    public void ChangeAccuracty(int accuracyPoints) => _changedAccuracy += accuracyPoints;
    public void ChangeInitiative(int initiativePoints) => _changedInitiative += initiativePoints;

    public void SetMaxHealth(int value) => _maxHealth = value;
    public void SetMaxForce(int forcePoints) => _defaultForce = forcePoints;
    public void SetDefaultAccuracy(int value) => _defaultAccuracy = value;
    public void SetDefaultInitiative(int value) => _defaultInitiative = value;

    public string GetSaveString()
    {
        string result = string.Empty;

        result += nameof(Name) + ":" + Name + FileAccessUtility.stringSeparator;
        result += nameof(CharacterPortraitIndex) + ":" + CharacterPortraitIndex + FileAccessUtility.stringSeparator;
        result += nameof(_maxHealth) + ":" + _maxHealth + FileAccessUtility.stringSeparator;
        result += nameof(_defaultAccuracy) + ":" + _defaultAccuracy + FileAccessUtility.stringSeparator;
        result += nameof(_defaultForce) + ":" + _defaultForce + FileAccessUtility.stringSeparator;
        result += nameof(_defaultInitiative) + ":" + _defaultInitiative + FileAccessUtility.stringSeparator;
        result += "SPELLS"+ FileAccessUtility.stringSeparator;

        foreach (var item in spellsBook)
        {
            result += item.GetSaveString() + FileAccessUtility.stringSeparator;
        }
        return result;
    }
}