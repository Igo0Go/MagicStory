using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ����
/// </summary>
[System.Serializable]
public class Magican
{
    #region ���� ��� ����������

    /// <summary>
    /// ��� ����
    /// </summary>
    public string Name;

    /// <summary>
    /// ���� � ������� ���������
    /// </summary>
    public Color Color = Color.white;

    /// <summary>
    /// ������� ���������
    /// </summary>
    public Sprite CharacterPortrait;

    [SerializeField, Min(0)]
    private int _maxHealth;

    [SerializeField]
    private int _defaultForce;

    [SerializeField]
    private int _defaultAccuracy;

    [SerializeField]
    private int _defaultInitiative;

    public List<string> spellsFilesNames;

    #endregion

    #region �������� ��������

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
            _health = value;
            if (_health < 0)
            {
                _health = 0;
            }
            if (_health > MaxHealth)
            {
                _health = MaxHealth;
            }
        }
    }
    private int _health;

    /// <summary>
    /// ������� ������������ ����� ��������
    /// </summary>
    public int MaxHealth
    { 
        get
        {
            return _maxHealth;
        }
        private set
        {
            _maxHealth = value;
        }
    }

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

    /// <summary>
    /// ������� �������� � ������ ���������
    /// </summary>
    public int Accuracy => Mathf.Clamp(_defaultAccuracy + _changedAccuracy, 0, 100);
    private int _changedAccuracy;

    /// <summary>
    /// ������� ���������� � ������ ���������
    /// </summary>
    public int Initiative => _defaultInitiative + _changedInitiative;
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

    #endregion

    /// <summary>
    /// ������ ��������� ���� �� ������ ����������������
    /// </summary>
    /// <param name="name">���</param>
    /// <param name="maxHealth">������������ ����� ��������</param>
    /// <param name="defaultForce">����������� �������� ����</param>
    /// <param name="color">���� � �������</param>
    public Magican(string name, int maxHealth, int defaultForce, int defaultAccuracy, 
        int defaultInitiative, Color color)
    {
        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
        _defaultForce = defaultForce;
        Force = defaultForce;
        Color = color;
        _defaultInitiative = defaultInitiative;
        _defaultAccuracy = defaultAccuracy;
    }

    public void SaveSpellNames()
    {
        foreach (var spell in spellsBook)
        {
            spellsFilesNames.Add(spell.Name);
        }
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
    public void ChangeAccuracty(int accuracyPoints)
    {
        _changedAccuracy += accuracyPoints;
        
    }
    public void ChangeInitiative(int initiativePoints)
    {
        _changedInitiative += initiativePoints;
    }
    public void SetMaxForce(int forcePoints)
    {
        _defaultForce = forcePoints;
    }
}