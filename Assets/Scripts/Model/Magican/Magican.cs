using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Progress;

/// <summary>
/// Класс мага
/// </summary>
[System.Serializable]
public class Magican
{
    #region Поля для инспектора

    /// <summary>
    /// Имя мага
    /// </summary>
    public string Name;

    /// <summary>
    /// Цвет в подписи персонажа
    /// </summary>
    public Color Color = Color.white;

    /// <summary>
    /// Портрет персонажа
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

    #region Сокрытые свойства

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
    /// Текущий максимальный запас здоровья
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

    /// <summary>
    /// Текущая точность с учётом изменений
    /// </summary>
    public int Accuracy => Mathf.Clamp(_defaultAccuracy + _changedAccuracy, 0, 100);
    private int _changedAccuracy;

    /// <summary>
    /// Текущая инициатива с учётом изменений
    /// </summary>
    public int Initiative => _defaultInitiative + _changedInitiative;
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

    #endregion

    /// <summary>
    /// Создаёт экземпляр мага со своими характеристиками
    /// </summary>
    /// <param name="name">Имя</param>
    /// <param name="maxHealth">Максимальный запас здоровья</param>
    /// <param name="defaultForce">Стандартное значение мощи</param>
    /// <param name="color">Цвет в консоли</param>
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

    public string GetSaveString()
    {
        string result = string.Empty;

        result += nameof(Name) + ":" + Name + FileAccessUtility.stringSeparator;
        result += nameof(Color) + ":" + 
            "(" + Color.r + FileAccessUtility.propertyPartSeparator + 
            Color.g + FileAccessUtility.propertyPartSeparator + 
            Color.b + FileAccessUtility.propertyPartSeparator + 
            Color.a + ")" + FileAccessUtility.stringSeparator;
        
        //result += nameof(CharacterPortrait) + ":" +
        //    CharacterPortrait.texture.width + FileAccessUtility.propertyPartSeparator +
        //    CharacterPortrait.texture.height + FileAccessUtility.propertyPartSeparator;

        //byte[] data = CharacterPortrait.texture.EncodeToPNG();

        //for (int i = 0; i < data.Length; i++)
        //{
        //    result += data[i].ToString();
        //}

        //result += FileAccessUtility.stringSeparator;

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

    public static Magican GetMagican(string dataString)
    {
        string[] MagicanProperties = dataString.Split(FileAccessUtility.stringSeparator, 
            StringSplitOptions.RemoveEmptyEntries);

        string name = string.Empty;
        Color color = Color.white;
        Sprite portrait = null;
        int maxHealth = 0;
        int defaultForce = 0;
        int defaultAccuracy = 0;
        int defaultInitiative = 0;


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
            else if (bufer[0].Equals(nameof(CharacterPortrait)))
            {
                char[] separators = { '-' };
                string[] parts = bufer[1].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                byte[] bytes = File.ReadAllBytes(parts[2]);

                Texture2D tex = new Texture2D(int.Parse(parts[0]), int.Parse(parts[1]));
                tex.LoadImage(bytes);
                portrait = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
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

        Magican magican = new Magican(name, maxHealth, defaultForce, defaultAccuracy, defaultInitiative, color);

        magican.CharacterPortrait = portrait;

        magican.spellsBook = new List<Spell>();



        return magican;
    }
}