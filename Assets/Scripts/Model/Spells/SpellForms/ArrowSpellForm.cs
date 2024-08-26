using UnityEngine;

/// <summary>
/// Форма стрелы предполагает, что маг либо попадает и применяет эффект полностью, либо промахивается.
/// </summary>
public class ArrowSpellForm : SpellForm
{
    public ArrowSpellForm()
    {
        _description = "Стрела";
    }

    public override int CalculateWorkLoad(SpellEffect effect)
    {
        return (int)SpellFormType.Arrow * effect.CalculateWorkLoad();
    }

    public override SpellFormType GetFormType()
    {
        return SpellFormType.Arrow;
    }

    public override string GetSaveString() => nameof(SpellFormType.Arrow);

    /// <summary>
    /// Получить итоговую цель с учётом промахов и концентрацию эффекта по ней. 
    /// Для стрелы в случае промаха цель - это null. Маг не попадает по живым целям
    /// </summary>
    /// <param name="defaultTarget">Планируемая цель</param>
    /// <param name="userAccuracy">Точность заклинателя - бонус или штраф</param>
    /// <param name="spellSuccessPercent">Исходная точность заклинания</param>
    /// <returns>Кортеж (Итоговая цель с учётом промаха, концентрация эффекта по ней)</returns>
    public override (Magican target, int effectPercent)
        GetTargetEndEffectPercent(Magican defaultTarget, int userAccuracy, int spellSuccessPercent)
    {
        int complexity = Random.Range(0, 101);
        int accuracy = spellSuccessPercent + userAccuracy;

        if (accuracy > complexity)
        {
            //попадание
            return (defaultTarget, 100);
        }
        return (null, 0);
    }
}
