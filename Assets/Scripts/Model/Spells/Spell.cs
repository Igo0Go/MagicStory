using System;

/// <summary>
/// Класс-шаблон для всех заклинаний
/// </summary>
[Serializable]
public class Spell
{
    /// <summary>
    /// Название заклинания
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание заклинания
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Количество целей: на сколько целей за раз можно применить заклинание [1;12]
    /// </summary>
    public int TargetsCount
    {
        get
        {
            return _targetCount;
        }
        set
        {
            _targetCount = Math.Clamp(value, 1, 13);
        }
    }
    private int _targetCount = 1;

    /// <summary>
    /// Процент успеха: вероятность того, что заклинание сработает
    /// </summary>
    public int SuccessPercent
    {
        get 
        { 
            return _successPercent; 
        }
        set
        {
            _successPercent = Math.Clamp(value, 0,101);
        }
    }
    private int _successPercent;

    /// <summary>
    /// Форма заклинания
    /// </summary>
    public SpellForm SpellForm { get; set; }

    /// <summary>
    /// Эффект заклинания
    /// </summary>
    public SpellEffect Effect { get; set; }

    public void UseSpell(Magican user, Magican target)
    {
        (Magican target, int percent) targetPack = 
            SpellForm.GetTarget(target, user.Accuracy, user.CurrentSpell.SuccessPercent);

        if(targetPack.target != null)
        {
            Effect.UseEffectToTarget(user, targetPack.target, targetPack.percent);
        }
    }

    public float CalculateForce()
    {

        if( Effect == null || SpellForm == null)
        {
            return 0f;
        }

       return SpellForm.CalculateReqareForce(Effect) * TargetsCount/100f * SuccessPercent;


    }

    public string GetSaveString()
    {
        string result = string.Empty;

        result += nameof(Name) +":" + Name + Utility_SpellSaver.stringSeparator;
        result += nameof(Description) + ":" + Description + Utility_SpellSaver.stringSeparator;
        result += nameof(TargetsCount) + ":" + TargetsCount + Utility_SpellSaver.stringSeparator;
        result += nameof(SuccessPercent) + ":" + SuccessPercent + Utility_SpellSaver.stringSeparator;
        result += nameof(SpellForm) + ":" + SpellForm.GetSaveString() + Utility_SpellSaver.stringSeparator;
        result += nameof(Effect) + (Effect == null ? "null" : (":" + Effect.GetSaveString() + Utility_SpellSaver.stringSeparator));
        return result;
    }
}
