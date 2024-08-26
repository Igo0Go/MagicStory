using UnityEngine;

/// <summary>
///Форма ауры предполагает, что маг либо попадает и применяет эффект полностью по планируемой цели,
///либо применяет эффект на какую-то другую живую цель
/// либо промахивается.
/// </summary>
public class AuraSpellForm : SpellForm
{
    public AuraSpellForm()
    {
        _description = "Аура";
    }

    public override int CalculateWorkLoad(SpellEffect effect)
    {
        return (int)SpellFormType.Aura * effect.CalculateWorkLoad();
    }

    public override SpellFormType GetFormType()
    {
        return SpellFormType.Aura;
    }

    public override string GetSaveString() => nameof(SpellFormType.Aura);

    /// <summary>
    /// Получить итоговую цель с учётом промахов и концентрацию эффекта по ней. 
    /// Для ауры в случае промаха цель - это другой враг.
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

        if (accuracy >= complexity)
        {
            //попадание
            return (defaultTarget, 100);
        }
        else
        {
            //Найти другую цель
        }

        //Заглушка
        return (null, 0);
    }
}
