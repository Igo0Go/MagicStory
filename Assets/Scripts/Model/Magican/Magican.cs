using System;
using System.Collections.Generic;
using System.Drawing.Text;
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
    public int Accuracy => Mathf.Clamp(_accuracy, 0, 100);
    private int _accuracy;

    /// <summary>
    /// Текущая инициатива с учётом изменений
    /// </summary>
    public int Initiative => _initiative;
    private int _initiative;

    /// <summary>
    /// Состояние героя
    /// </summary>
    public MagicanState State { get; set; } = MagicanState.None;

    /// <summary>
    /// Книга заклинаний мага
    /// </summary>
    public List<Spell> SpellsBook { get; set; } = new List<Spell>();

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

        for (int i = spellBeginningIndex+1; i < MagicanProperties.Length; i++)
        {
            magican.SpellsBook.Add(Spell.GetSpellByInfo(MagicanProperties[i]));
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

    public void ChangeAccuracty(int accuracyPoints) => _accuracy += accuracyPoints;
    public void ChangeInitiative(int initiativePoints) => _initiative += initiativePoints;

    public void SetMaxHealth(int value) => _maxHealth = value;
    public void SetMaxForce(int forcePoints) => _defaultForce = forcePoints;
    public void SetDefaultInitiative(int value) => _initiative = value;

    public int CalculateMagicanStatsWorkLoad()
    {
        int result = 0;

        result += _maxHealth * StatsMultiplicatorPack.healPointMultiplicator;
        result += _defaultForce * StatsMultiplicatorPack.forcePointMultiplicator;

        return result;
    }
    public int CalculateMagicanSpellsWorkload()
    {
        int result = 0;
        foreach(Spell spell in SpellsBook)
        {
            result += spell.CalculateWorkLoad();
        }
        return result;
    }

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
            result += item.GetSaveString() + FileAccessUtility.stringSeparator;
        }
        return result;
    }
}