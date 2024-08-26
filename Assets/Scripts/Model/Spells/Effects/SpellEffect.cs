using System;

/// <summary>
/// Эффект заклинания
/// </summary>
[Serializable]
public abstract class SpellEffect
{
    /// <summary>
    /// Описание
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
    /// Вложенный эффект
    /// </summary>
    public SpellEffect insideEffect;

    /// <summary>
    /// К какой цели применяется
    /// </summary>
    public EffectTargetType effectTargetType;

    /// <summary>
    /// Применить эффект на цель
    /// </summary>
    /// <param name="user">Заклинатель</param>
    /// <param name="target">Цель</param>
    /// <param name="effectPercent">Концентрация эффекта</param>
    public abstract void UseEffectToTarget(Magican user, Magican target, int effectPercent);

    /// <summary>
    /// Рассчитать нагрузку отдельно для этого эффекта в единицах маны
    /// </summary>
    /// <returns>Требуемое количество маны</returns>
    public abstract int CalculateWorkLoad();

    /// <summary>
    /// Рассчитать нагрузку для этого эффекта и всех вложенных в единицах маны
    /// </summary>
    /// <returns>Требуемое количество маны</returns>
    public abstract int CalculateReqareForceForThisEffectOnly();

    /// <summary>
    /// Рассчитать значение эффекта с учётом концентрации
    /// </summary>
    /// <param name="value">исходный эффект</param>
    /// <param name="percent">процент концентрации</param>
    /// <returns>итоговое значение эфекта с учётом концентрации</returns>
    public static int GetEffectValue(int value, int percent)
    {
        float result = value * (percent / 100);

        return (int)MathF.Round(result);
    }

    /// <summary>
    /// Преобразовать эффект и все вложенные в строку данных
    /// </summary>
    /// <returns>Строка данных  с информацией об эффекте и всех вложенных</returns>
    public abstract string GetDataString();
}

/// <summary>
/// Выбор, на кого применить эффект: на цель, или на заклинателя
/// </summary>
public enum EffectTargetType
{
    Target,
    User
}
