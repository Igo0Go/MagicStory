using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

public static class Utility_SpellSaver
{
    public static string stringSeparator = "\n";

    public static void SaveSpellInTheFile(Spell spell, string fileName)
    {
        string saveString = spell.GetSaveString();

        string filePath = Path.Combine(Application.streamingAssetsPath, "Saves", "Spells", fileName + ".txt");

        Debug.Log(filePath);

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }

        using (TextWriter writer = new StreamWriter(filePath, true))
        {
            writer.Write(saveString);
        }
    }
    public static List<string> GetAllSpellFilesNames()
    {
        List<string> list = new List<string>();

        string directoryPath = Path.Combine(Application.streamingAssetsPath, "Saves", "Spells");

        DirectoryInfo di = new DirectoryInfo(directoryPath);
        FileInfo[] files = di.GetFiles("*.txt");
        foreach (FileInfo file in files)
        {
            list.Add(file.FullName);
        }

        return list;
    }
    public static Spell LoadSpellFromTheFile(string fileName, bool nameOnly)
    {
        Spell spell = new Spell();

        if(nameOnly)
        {
            fileName = Path.Combine(Application.streamingAssetsPath, "Saves", "Spells", fileName + ".txt");
        }

        string info = string.Empty;
        using (StreamReader reader = new StreamReader(fileName))
        {
            info = reader.ReadToEnd();
        }

        string[] SpellProperties = info.Split(stringSeparator, StringSplitOptions.RemoveEmptyEntries);

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
                string[] effects = bufer[1].Split(new char[] {'[',']'}, StringSplitOptions.RemoveEmptyEntries);

                if(effects.Length > 0)
                {
                    SpellEffect effect = GetSpellEffect(effects[0]);

                    if (effect != null)
                    {
                        spell.Effect = effect;

                        SpellEffect currentEffect = spell.Effect;

                        for (int i = 1; i < effects.Length; i++)
                        {
                            effect = GetSpellEffect(effects[i]);

                            if(effect != null)
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
    private static SpellEffect GetSpellEffect(string effectString)
    {
        string[] strings = effectString.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

        if (strings[0].Equals(nameof(DamageEffect))) 
        {
            DamageEffect damageEffect = new DamageEffect();
            string[] values = strings[1].Split(',');
            damageEffect.damage = int.Parse(values[0]);
            damageEffect.effectTargetType = (int.Parse(values[1]) == 1? 
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
            changeAccuracyEffect.accuracyPoints = int.Parse(values[0]);
            changeAccuracyEffect.effectTargetType = (int.Parse(values[1]) == 1 ?
                EffectTargetType.Target : EffectTargetType.User);
            return changeAccuracyEffect;
        }
        else if (strings[0].Equals(nameof(ChangeForceEffect)))
        {
            ChangeForceEffect changeForceEffect = new ChangeForceEffect();
            string[] values = strings[1].Split(',');
            changeForceEffect.forcePoints = int.Parse(values[0]);
            changeForceEffect.effectTargetType = (int.Parse(values[1]) == 1 ?
                EffectTargetType.Target : EffectTargetType.User);
            return changeForceEffect;
        }
        else if (strings[0].Equals(nameof(ChangeInitiativeEffect)))
        {
            ChangeInitiativeEffect ChangeInitiativeEffect
                = new ChangeInitiativeEffect();
            string[] values = strings[1].Split(',');
            ChangeInitiativeEffect.initiativePoints = int.Parse(values[0]);
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
            else if(values[1].Equals(MagicanState.MindControl))
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
}
