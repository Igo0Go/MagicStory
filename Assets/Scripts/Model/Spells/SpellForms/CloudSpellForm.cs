using UnityEngine;

/// <summary>
///Форма облака предполагает, что маг всегда попадает по планируемой цели.
/// Вопрос в том, полностью ли применится эффект
/// </summary>
public class CloudSpellForm : SpellForm
{
    public CloudSpellForm()
    {
        _description = "Облако";
    }

    public override int CalculateWorkLoad(SpellEffect effect)
    {
        return (int)SpellFormType.Cloud * effect.CalculateWorkLoad();
    }

    public override SpellFormType GetFormType()
    {
        return SpellFormType.Cloud;
    }

    public override string GetSaveString() => nameof(SpellFormType.Cloud);

    /// <summary>
    /// Получить итоговую цель с учётом промахов и концентрацию эффекта по ней. 
    /// Для облака даже в случае промаха цель - это исходная цель.
    /// Концентрация может варьироваться от половины до полного эффекта
    /// </summary>
    /// <param name="defaultTarget">Планируемая цель</param>
    /// <param name="userAccuracy">Точность заклинателя - бонус или штраф</param>
    /// <param name="spellSuccessPercent">Исходная точность заклинания</param>
    /// <returns>Кортеж (Итоговая цель с учётом промаха, концентрация эффекта по ней)</returns>
    public override (Magican target, int effectPercent) GetTargetEndEffectPercent
        (Magican defaultTarget, int userAccuracy, int spellSuccessPercent)
    {
        int maxPercent = Mathf.Clamp(Random.Range(25, spellSuccessPercent + userAccuracy + 1), 50, 100);

        //Вычисляем концентрацию облака, которая попадает по цели
        return (defaultTarget, maxPercent);
    }
}
