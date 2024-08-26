public abstract class SpellForm
{
    public string Description => _description;
    protected string _description;

    public abstract int CalculateWorkLoad(SpellEffect effect);

    public abstract SpellFormType GetFormType();

    public abstract (Magican target, int effectPercent) 
        GetTarget(Magican defaultTarget, int userAccuracy, int spellSuccessPercent);

    public abstract string GetSaveString();
}
