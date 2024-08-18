using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс мага
/// </summary>
[System.Serializable]
public class Magican
{
    /// <summary>
    /// Имя мага
    /// </summary>
    public string Name;

    /// <summary>
    /// Портрет персонажа
    /// </summary>
    public int CharacterPortraitIndex = 0;

    /// <summary>
    /// Текущий запас здоровья
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
    /// Текущий запас мощи
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
    /// Текущая точность с учётом изменений
    /// </summary>
    public int Accuracy => Mathf.Clamp(_defaultAccuracy + _changedAccuracy, 0, 100);
    private int _defaultAccuracy;
    private int _changedAccuracy;

    /// <summary>
    /// Текущая инициатива с учётом изменений
    /// </summary>
    public int Initiative => _defaultInitiative + _changedInitiative;
    private int _defaultInitiative;
    private int _changedInitiative;

    /// <summary>
    /// Состояние героя
    /// </summary>
    public MagicanState State { get; set; } = MagicanState.None;

    /// <summary>
    /// Книга заклинаний мага
    /// </summary>
    public List<Spell> spellsBook { get; set; } = new List<Spell>();

    /// <summary>
    /// Тот, кто завладел разумом
    /// </summary>
    public Magican MindControllMaster { get; set; }

    /// <summary>
    /// Подготовленное в данный момент заклинание
    /// </summary>
    public Spell CurrentSpell { get; private set; }

    public Dictionary<MagicanState, int> CurrentMagicanStates { get; set; } = new Dictionary<MagicanState, int>();

    /// <summary>
    /// Создаёт экземпляр мага со своими характеристиками
    /// </summary>
    /// <param name="name">Имя</param>
    /// <param name="maxHealth">Максимальный запас здоровья</param>
    /// <param name="defaultForce">Стандартное значение мощи</param>
    /// <param name="color">Цвет в консоли</param>
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
        Color color = Color.white;
        int maxHealth = 0;
        int defaultForce = 0;
        int defaultAccuracy = 0;
        int defaultInitiative = 0;
        int portraitIndex = 0;

        foreach (string MagicanProperty in MagicanProperties)
        {
            string[] bufer = MagicanProperty.Split(":");

            if (bufer[0].Equals(nameof(Name)))
            {
                name = bufer[1];
            }
            else if (bufer[0].Equals(nameof(Color)))
            {
                char[] separators = { '-', '(', ')' };
                string[] parts = bufer[1].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                color = new Color(float.Parse(parts[0]), float.Parse(parts[1]),
                    float.Parse(parts[2]), float.Parse(parts[3]));
            }
            else if (bufer[0].Equals(nameof(CharacterPortraitIndex)))
            {
                portraitIndex = int.Parse(bufer[1]);
            }
            else if (bufer[0].Equals(nameof(_maxHealth)))
            {
                maxHealth = int.Parse(bufer[1]);
            }
            else if (bufer[0].Equals(nameof(Magican._defaultForce)))
            {
                defaultForce = int.Parse(bufer[1]);
            }
            else if (bufer[0].Equals(nameof(Magican._defaultAccuracy)))
            {
                defaultAccuracy = int.Parse(bufer[1]);
            }
            else if (bufer[0].Equals(nameof(Magican._defaultInitiative)))
            {
                defaultInitiative = int.Parse(bufer[1]);
            }
        }

        Magican magican = new Magican(name, maxHealth, defaultForce, defaultAccuracy, defaultInitiative);

        magican.spellsBook = new List<Spell>();
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