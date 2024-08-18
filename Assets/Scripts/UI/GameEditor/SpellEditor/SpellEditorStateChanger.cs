using UnityEngine;
using Cinemachine;

public class SpellEditorStateChanger : MonoBehaviour, ISpellEditorUIPart
{
    [SerializeField]
    private EditorUIStates uiPack;

    public SpellEditorUICenter Center { get; private set; }

    public void Init(SpellEditorUICenter center)
    {
        this.Center = center;
    }

    public void ChangeViewToCharacter()
    {
        uiPack.DisactiveAllPanels();
        uiPack.AllCamerasToMinPriority();
        uiPack.characterPanel.SetActive(true);
        uiPack.characterViewCamera.Priority = 1;
    }

    public void ChangeViewToSpellForm()
    {
        uiPack.characterPanel.SetActive(false);
        uiPack.spellEditorMainPanel.SetActive(true);
        uiPack.spellEditorFormPanel.SetActive(true);
        uiPack.spellEditorEffectPanel.SetActive(false);
        uiPack.spellEditorTargetPanel.SetActive(false);

        uiPack.AllCamerasToMinPriority();
        uiPack.spellFormViewCamera.Priority = 1;
    }

    public void ChangeViewToSpellEffects()
    {
        uiPack.characterPanel.SetActive(false);
        uiPack.spellEditorMainPanel.SetActive(true);
        uiPack.spellEditorFormPanel.SetActive(false);
        uiPack.spellEditorEffectPanel.SetActive(true);
        uiPack.spellEditorTargetPanel.SetActive(false);

        uiPack.AllCamerasToMinPriority();
        uiPack.spellEffectViewCamera.Priority = 1;
    }

    public void ChangeViewToSpellTargets()
    {
        uiPack.characterPanel.SetActive(false);
        uiPack.spellEditorMainPanel.SetActive(true);
        uiPack.spellEditorFormPanel.SetActive(false);
        uiPack.spellEditorEffectPanel.SetActive(false);
        uiPack.spellEditorTargetPanel.SetActive(true);

        uiPack.AllCamerasToMinPriority();
        uiPack.spellTargetsViewCamera.Priority = 1;
    }
}

[System.Serializable]
public class EditorUIStates
{
    public CinemachineVirtualCamera characterViewCamera;
    public CinemachineVirtualCamera spellFormViewCamera;
    public CinemachineVirtualCamera spellEffectViewCamera;
    public CinemachineVirtualCamera spellTargetsViewCamera;

    [Space(20)]
    public GameObject characterPanel;
    public GameObject spellEditorMainPanel;
    public GameObject spellEditorFormPanel;
    public GameObject spellEditorEffectPanel;
    public GameObject spellEditorTargetPanel;

    public void AllCamerasToMinPriority()
    {
        characterViewCamera.Priority = 0;
        spellFormViewCamera.Priority = 0;
        spellEffectViewCamera.Priority = 0;
        spellTargetsViewCamera.Priority = 0;
    }

    public void DisactiveAllPanels()
    {
        characterPanel.SetActive(false);
        spellEditorMainPanel.SetActive(false);
        spellEditorFormPanel.SetActive(false);
        spellEditorEffectPanel.SetActive(false);
        spellEditorTargetPanel.SetActive(false);
    }
}