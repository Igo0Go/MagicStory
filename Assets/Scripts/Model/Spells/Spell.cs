using System;

/// <summary>
/// Класс-шаблон для всех заклинаний
/// </summary>
[Serializable]
public class Spell
{
    #region Характеристики

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

    #endregion

    /// <summary>
    /// Использовать заклинание
    /// </summary>
    /// <param name="user">Заклинатель</param>
    /// <param name="target">Цель</param>
    public void UseSpell(Magican user, Magican target)
    {
        (Magican target, int percent) targetPack = 
            SpellForm.GetTargetEndEffectPercent(target, user.Accuracy, user.CurrentSpell.SuccessPercent);

        if(targetPack.target != null)
        {
            Effect.UseEffectToTarget(user, targetPack.target, targetPack.percent);
        }
    }

    /// <summary>
    /// Рассчитать нагрузку заклинания в единицах маны
    /// </summary>
    /// <returns>Количество маны, требуемое для использования заклинания</returns>
    public int CalculateWorkLoad()
    {

        if( Effect == null || SpellForm == null)
        {
            return 0;
        }

       return (int)MathF.Round(SpellForm.CalculateWorkLoad(Effect) * TargetsCount/100f * SuccessPercent);


    }

    #region Работа с данными
    /// <summary>
    /// Получить строку данных из этого заклинания
    /// </summary>
    /// <returns>строка данных с информацией о заклинании</returns>
    public string GetDataString()
    {
        string result = "{";

        result += nameof(Name) +":" + Name + FileAccessUtility.stringPartSeparator;
        result += nameof(Description) + ":" + Description + FileAccessUtility.stringPartSeparator;
        result += nameof(TargetsCount) + ":" + TargetsCount + FileAccessUtility.stringPartSeparator;
        result += nameof(SuccessPercent) + ":" + SuccessPercent + FileAccessUtility.stringPartSeparator;
        result += nameof(SpellForm) + ":" + SpellForm.GetSaveString() + FileAccessUtility.stringPartSeparator;
        result += nameof(Effect) + (Effect == null ? "null" : 
            (":" + Effect.GetDataString() + FileAccessUtility.stringPartSeparator));
        return result + "}";
    }
    /// <summary>
    /// Преобразовать строку данных в экземпляр заклинания
    /// </summary>
    /// <param name="dataString">Строка данных с информацией о заклинании</param>
    /// <returns>Экземпляр заклинание</returns>
    public static Spell GetSpellByInfo(string dataString)
    {
        Spell spell = new Spell();

        char[] c = { '{', '}' };
        dataString = dataString.Split(c, StringSplitOptions.RemoveEmptyEntries)[0];

        string[] SpellProperties = dataString.Split(FileAccessUtility.stringPartSeparator,
            StringSplitOptions.RemoveEmptyEntries);

        foreach (string SpellProperty in SpellProperties)
        {
            string[] bufer = SpellProperty.Split(":");

            if (bufer[0].Equals(nameof(spell.Name)))
            {
                spell.Name = bufer[1];
            }
            else if (bufer[0].Equals(nameof(spell.Description)))
            {
                spell.Description = bufer[1];
            }
            else if (bufer[0].Equals(nameof(spell.TargetsCount)))
            {
                spell.TargetsCount = int.Parse(bufer[1]);
            }
            else if (bufer[0].Equals(nameof(spell.SuccessPercent)))
            {
                spell.SuccessPercent = int.Parse(bufer[1]);
            }
            else if (bufer[0].Equals(nameof(spell.SpellForm)))
            {
                if (bufer[1].Equals(SpellFormType.Arrow.ToString()))
                {
                    spell.SpellForm = new ArrowSpellForm();
                }
                else if (bufer[1].Equals(SpellFormType.Aura.ToString()))
                {
                    spell.SpellForm = new AuraSpellForm();
                }
                else if (bufer[1].Equals(SpellFormType.Cloud.ToString()))
                {
                    spell.SpellForm = new CloudSpellForm();
                }
                else if (bufer[1].Equals(SpellFormType.Relation.ToString()))
                {
                    spell.SpellForm = new RelationSpellForm();
                }
            }
            else if (bufer[0].Equals(nameof(spell.Effect)))
            {
                string[] effects = bufer[1].Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

                if (effects.Length > 0)
                {
                    SpellEffect effect = GetSpellEffect(effects[0]);

                    if (effect != null)
                    {
                        spell.Effect = effect;

                        SpellEffect currentEffect = spell.Effect;

                        for (int i = 1; i < effects.Length; i++)
                        {
                            effect = GetSpellEffect(effects[i]);

                            if (effect != null)
                            {
                                currentEffect.insideEffect = effect;
                                currentEffect = currentEffect.insideEffect;
                            }
                        }
                    }
                }
            }
        }

        return spell;
    }
    /// <summary>
    /// Преобразовать строку с информацией об эффекте в экземпляр
    /// </summary>
    /// <param name="effectString">строка с информацией об эффекте</param>
    /// <returns>Экземпляр эффекта заклинания</returns>
    private static SpellEffect GetSpellEffect(string effectString)
    {
        string[] strings = effectString.Split("|", StringSplitOptions.RemoveEmptyEntries);

        if (strings[0].Equals(nameof(DamageEffect)))
        {
            DamageEffect damageEffect = new DamageEffect();
            string[] values = strings[1].Split(',');
            damageEffect.damage = int.Parse(values[0]);
            damageEffect.effectTargetType = (int.Parse(values[1]) == 1 ?
                EffectTargetType.Target : EffectTargetType.User);
            return damageEffect;
        }
        else if (strings[0].Equals(nameof(HealEffect)))
        {
            HealEffect healEffect = new HealEffect();
            string[] values = strings[1].Split(',');
            healEffect.healPoint = int.Parse(values[0]);
            healEffect.effectTargetType = (int.Parse(values[1]) == 1 ?
                EffectTargetType.Target : EffectTargetType.User);
            return healEffect;
        }
        else if (strings[0].Equals(nameof(ChangeAccuracyEffect)))
        {
            ChangeAccuracyEffect changeAccuracyEffect = new ChangeAccuracyEffect();
            string[] values = strings[1].Split(',');
            changeAccuracyEffect.accuracyOffset = int.Parse(values[0]);
            changeAccuracyEffect.effectTargetType = (int.Parse(values[1]) == 1 ?
                EffectTargetType.Target : EffectTargetType.User);
            return changeAccuracyEffect;
        }
        else if (strings[0].Equals(nameof(ChangeForceEffect)))
        {
            ChangeForceEffect changeForceEffect = new ChangeForceEffect();
            string[] values = strings[1].Split(',');
            changeForceEffect.forceOffset = int.Parse(values[0]);
            changeForceEffect.effectTargetType = (int.Parse(values[1]) == 1 ?
                EffectTargetType.Target : EffectTargetType.User);
            return changeForceEffect;
        }
        else if (strings[0].Equals(nameof(ChangeInitiativeEffect)))
        {
            ChangeInitiativeEffect ChangeInitiativeEffect
                = new ChangeInitiativeEffect();
            string[] values = strings[1].Split(',');
            ChangeInitiativeEffect.initiativeOffset = int.Parse(values[0]);
            ChangeInitiativeEffect.effectTargetType = (int.Parse(values[1]) == 1 ?
                EffectTargetType.Target : EffectTargetType.User);
            return ChangeInitiativeEffect;
        }
        else if (strings[0].Equals(nameof(SetStateEffect)))
        {
            SetStateEffect setStateEffect = new SetStateEffect();
            string[] values = strings[1].Split(',');
            setStateEffect.duration = int.Parse(values[0]);

            if (values[1].Equals(MagicanState.None))
            {
                setStateEffect.state = MagicanState.None;
            }
            else if (values[1].Equals(MagicanState.MindControl))
            {
                setStateEffect.state = MagicanState.MindControl;
            }
            else if (values[1].Equals(MagicanState.Mirror))
            {
                setStateEffect.state = MagicanState.Mirror;
            }
            else if (values[1].Equals(MagicanState.Stunned))
            {
                setStateEffect.state = MagicanState.Stunned;
            }
            else if (values[1].Equals(MagicanState.Shield))
            {
                setStateEffect.state = MagicanState.Shield;
            }

            setStateEffect.effectTargetType = (int.Parse(values[2]) == 1 ?
                EffectTargetType.Target : EffectTargetType.User);
            return setStateEffect;
        }

        return null;
    }
    #endregion
}
