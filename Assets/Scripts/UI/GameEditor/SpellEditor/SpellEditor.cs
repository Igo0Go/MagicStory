using TMPro;
using UnityEngine;

public class SpellEditor : MonoBehaviour, ISpellEditorUIPart
{
    [SerializeField]
    private TMP_Text spellForceRequaredText;

    public SpellEditorUICenter Center { get; private set; }

    public void Init(SpellEditorUICenter center)
    {
        this.Center = center;
    }

    [SerializeField]
    private void ClearRunes()
    {
        Center.effectPanel.ClearRune();
        Center.targetCountPanel.ClearRunes();
        Center.formPanel.ClearRune();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            string s;
            if(CheckSpell(out s))
            {
                ClearRunes();
                Center.spellListPanel.DrawCurrentMagicanSpells();
                Center.changer.ChangeViewToCharacter();
            }
            else
            {
                Center.ShowTipPanel(s);
            }

        }
    }

    private bool CheckSpell(out string s)
    {
        s = string.Empty;
        bool result = true;
        if(Center.CurrentSpell != null)
        {
            if(string.IsNullOrEmpty(Center.CurrentSpell.Name))
            {
                s += "���������� �� �������\n";
                result = false;
            }

            if(Center.CurrentSpell.SpellForm == null)
            {
                s += "� ���������� �� ������ �����\n";
                result = false;
            }

            if(string.IsNullOrEmpty(Center.CurrentSpell.Description))
            {
                s += "� ���������� �� ������ ��������\n";
                result = false;
            }

            if(Center.CurrentSpell.Effect == null)
            {
                s += "� ���������� �� ����� ������";
            }
        }
        else
        {
            result = false;
        }

        if(!result)
        {
            s = "���������� �� ���������\n===============\n" + s;
        }

        return result;
    }

    public void DrawSpellForceRequared()
    {
        spellForceRequaredText.text = "��������: " + Center.CurrentSpell.CalculateForce();
    }
}
