using TMPro;
using UnityEngine;

public class SpellEditorUICenter : MonoBehaviour
{
    public SpellRunnesHolder spellRunnesHolder;
    public Spell CurrentSpell { get; set; }

    public SpellListWindow spellListPanel;
    public SpellEditor spellEditor;
    public SpellEditorStateChanger changer;
    public SpellFormEditorPanel formPanel;
    public SpellEffectEditorPanel effectPanel;
    public SpellTargetCountEditorPanel targetCountPanel;

    [SerializeField]
    private GameObject tipPanel;
    [SerializeField]
    private TMP_Text tipText;

    public void Awake()
    {
        spellEditor.Init(this);
        spellListPanel.Init(this);
        changer.Init(this);
        formPanel.Init(this);
        effectPanel.Init(this);
        targetCountPanel.Init(this);
    }

    public void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(tipPanel.activeSelf)
            {
                tipPanel.SetActive(false);
            }
        }
    }

    public void ShowTipPanel(string message)
    {
        tipPanel.SetActive(true);
        tipText.text = message;
    }
}

public interface ISpellEditorUIPart
{
    SpellEditorUICenter Center { get; }
    void Init(SpellEditorUICenter center);
}