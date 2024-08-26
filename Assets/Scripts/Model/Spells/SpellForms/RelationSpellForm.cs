using UnityEngine;

/// <summary>
///Форма связи предполагает, что маг попадает либо по планируемой цели,
/// либо по другому врагу. Но в случае критического промаха может попасть и по союзнику.
/// </summary>
public class RelationSpellForm : SpellForm
{
    public RelationSpellForm()
    {
        _description = "Связь";
    }

    public override int CalculateWorkLoad(SpellEffect effect)
    {
        return (int)SpellFormType.Relation * effect.CalculateWorkLoad();
    }

    public override SpellFormType GetFormType()
    {
        return SpellFormType.Relation;
    }

    public override string GetSaveString() => nameof(SpellFormType.Relation);

    /// <summary>
    /// Получить итоговую цель с учётом промахов и концентрацию эффекта по ней. 
    /// Для связи в случае промаха цель - это другой враг, если разница между выпашим значением и сложнастью меньше 10.
    /// Иначе если выпавшее значение меньше 10, то маг попадает по союзнику. В остальных случаях маг промахивается.
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
        else if (complexity - accuracy > 10)
        {
            //Найти другую цель - врага
        }
        else if(accuracy < 10)
        {
            //Найти другую цель - союзника
        }
        return (null, 0);
    }
}
