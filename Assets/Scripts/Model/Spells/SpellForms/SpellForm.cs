/// <summary>
/// Форма заклинания
/// </summary>
public abstract class SpellForm
{
    /// <summary>
    /// Описание заклинания
    /// </summary>
    public string Description => _description;
    protected string _description;

    /// <summary>
    /// Рассчитать нагрузку эффекта заклинания после применения формы
    /// </summary>
    /// <param name="effect"></param>
    /// <returns></returns>
    public abstract int CalculateWorkLoad(SpellEffect effect);

    /// <summary>
    /// Получить тип формы заклинания
    /// </summary>
    /// <returns>Тип формы заклинания</returns>
    public abstract SpellFormType GetFormType();

    /// <summary>
    /// Получить строку данных для сохранения
    /// </summary>
    /// <returns>Строка данных</returns>
    public abstract string GetSaveString();

    /// <summary>
    /// Получить итоговую цель с учётом промахов и концентрацию эффекта по ней
    /// </summary>
    /// <param name="defaultTarget">Планируемая цель</param>
    /// <param name="userAccuracy">Точность заклинателя - бонус или штраф</param>
    /// <param name="spellSuccessPercent">Исходная точность заклинания</param>
    /// <returns>Кортеж (Итоговая цель с учётом промаха, концентрация эффекта по ней)</returns>
    public abstract (Magican target, int effectPercent)
        GetTargetEndEffectPercent(Magican defaultTarget, int userAccuracy, int spellSuccessPercent);
}
