using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс мага
/// </summary>
[System.Serializable]
public class Magican
{
    #region Основные характеристики

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
    private int _defaultForce = 50;

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

    #endregion

    #region Изменчивые характеристики

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

    #endregion

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

    #region Геймплей

    /// <summary>
    /// Добавить магу статус на определённое количество ходов
    /// </summary>
    /// <param name="state">Статус</param>
    /// <param name="moviesNumber">Количество ходов</param>
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
    /// Уменьшить длительность статусов на один ход
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
    /// Снять все штрафы и преимущества
    /// </summary>
    public void ReturnStats()
    {
        _accuracy = 0;
        _initiative = 0;
    }

    #endregion

    #region Изменение значения характеристик

    /// <summary>
    /// Рассчитать нагрузку только мага
    /// </summary>
    /// <returns>Нагрузка мага в единицах прокачки</returns>
    public int CalculateMagicanStatsWorkLoad()
    {
        int result = 0;

        result += _maxHealth * StatsMultiplicatorPack.healPointMultiplicator;
        result += _defaultForce * StatsMultiplicatorPack.forcePointMultiplicator;

        return result;
    }
    /// <summary>
    /// Рассчитать нагрузку книги заклинаний мага
    /// </summary>
    /// <returns>Нагрузка книги заклинаний мага в единицах прокачки</returns>
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
    /// Сместить добавочное значение точности мага. Дать преимущество или помеху
    /// </summary>
    /// <param name="accuracyPoints">смещение добавочной точности</param>
    public void ChangeAccuracty(int accuracyPoints) => _accuracy += accuracyPoints;
    /// <summary>
    /// Сместить добавочное значение инициативы мага. Дать преимущество или помеху
    /// </summary>
    /// <param name="initiativePoints">смещение добавочной инициативы</param>
    public void ChangeInitiative(int initiativePoints) => _initiative += initiativePoints;

    /// <summary>
    /// Назначить новое значение максимального здоровья мага
    /// </summary>
    /// <param name="value">новое значение максимального здоровья</param>
    public void SetMaxHealth(int value) => _maxHealth = value;
    /// <summary>
    /// азначить новое значение максимальной маны мага
    /// </summary>
    /// <param name="forcePoints">новое значение максимальной маны</param>
    public void SetMaxForce(int forcePoints) => _defaultForce = forcePoints;

    #endregion

    #region Работа с данными
    /// <summary>
    /// Преобразовать строку данных в экземпляр мага
    /// </summary>
    /// <param name="dataString">строка данных</param>
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
    /// Получить строку данных для этого мага
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