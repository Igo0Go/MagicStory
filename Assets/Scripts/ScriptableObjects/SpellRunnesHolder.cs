using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IgoGoTools/SpellRunesHolder")]
public class SpellRunnesHolder : ScriptableObject
{
    public List<FormSpriteItem> formSprites;
    public List<EffectSpriteItem> effectSprites;
    public Sprite targetSprite;

    public EffectSpriteItem FindEffectItem(EffectType type)
    {
        for (int i = 0; i < effectSprites.Count; i++)
        {
            if (effectSprites[i].key == type)
            {
                return effectSprites[i];
            }
        }
        return null;
    }

    public FormSpriteItem FindFormItem(SpellFormType type)
    {
        for (int i = 0; i < formSprites.Count; i++)
        {
            if (formSprites[i].key == type)
            {
                return formSprites[i];
            }
        }
        return null;
    }
}


[Serializable]
public class FormSpriteItem
{
    public SpellFormType key;
    public Color color = Color.white;
    public Sprite sprite;
    [TextArea]
    public string description;
}

[Serializable]
public class EffectSpriteItem
{
    public EffectType key;
    public Color color = Color.white;
    public Sprite sprite;
    [TextArea]
    public string description;
}

public enum SpellFormType
{
    Arrow = 1,
    Relation = 3,
    Aura = 7,
    Cloud = 4
}

public enum EffectType
{
    Damage = 0,
    Heal = 1,
    ImproveAccuracy = 2,
    LowerAccuracy = 3,
    ImproveForce = 4,
    LowerForce = 5,
    ImproveInitiative = 6,
    LowerInitiative = 7,
    Shield = 8,
    Mirror = 9,
    SkipAction = 10,
    MindControl = 11,
    ClearStates = 12,
    BloodSpell = 13
}

public static class SpellForeceRequreSettings
{
    public static int changeAccuracyPointMultiplicator = 1;
    public static int damagePointMultiplicator = 2;
    public static int healPointMultiplicator = 3;
    public static int forcePointMultiplicator = 3;
    public static int initiativePointMultiplicator = 10;

    public static Dictionary<MagicanState, int> statesPrice = new Dictionary<MagicanState, int>();

    static SpellForeceRequreSettings()
    {
        statesPrice.Add(MagicanState.None, 100);
        statesPrice.Add(MagicanState.Shield, 12);
        statesPrice.Add(MagicanState.Mirror, 24);
        statesPrice.Add(MagicanState.Stunned, 40);
        statesPrice.Add(MagicanState.MindControl, 70);
    }
}
