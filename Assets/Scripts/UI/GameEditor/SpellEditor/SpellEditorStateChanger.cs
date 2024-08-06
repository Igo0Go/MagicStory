using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEditorStateChanger : MonoBehaviour, ISpellEditorUIPart
{
    [SerializeField]
    private SpellEditorUIPanels uiPack;

    public SpellEditorUICenter Center { get; private set; }

    public void Init(SpellEditorUICenter center)
    {
        this.Center = center;
    }

    public void ToSpellListState()
    {
        uiPack.spellList.SetActive(true);
        uiPack.spellEditorPanel.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(ChangeViewCoroutine(uiPack.mainView, uiPack.spellList));
    }

    public void ToSpellEditorStateFast()
    {
        uiPack.spellList.SetActive(false);
        uiPack.spellEditorPanel.SetActive(true);
    }

    public void ToSpellEditorState()
    {
        uiPack.spellList.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(ChangeViewCoroutine(uiPack.mainView, uiPack.spellEditorPanel));
    }

    public void ToSpellFormStage()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeViewCoroutine(uiPack.formRingView, uiPack.formPanel));
    }

    public void ToSpellEffectsStage()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeViewCoroutine(uiPack.effectsRingView, uiPack.effectsPanel));
    }

    public void ToSpellTargetsStage()
    {
        StopAllCoroutines();
        Center.targetCountPanel.ActivatePanel();
        StartCoroutine(ChangeViewCoroutine(uiPack.targetsRingView, uiPack.targetsPanel));
    }

    private IEnumerator ChangeViewCoroutine(GameObject cameraPoint, GameObject screenAfterCange)
    {
            cameraPoint.SetActive(true);
            uiPack.DisableAllEditorScreens();
            yield return new WaitForSeconds(2);
            uiPack.DisableCamerasWithoutCurrent(cameraPoint);
            screenAfterCange.SetActive(true);
    }
}

[System.Serializable]
public class SpellEditorUIPanels
{
    public GameObject mainView;
    public GameObject formRingView;
    public GameObject effectsRingView;
    public GameObject targetsRingView;

    [Space]
    public GameObject spellList;
    public GameObject spellEditorPanel;
    public GameObject formPanel;
    public GameObject effectsPanel;
    public GameObject targetsPanel;

    public void DisableAllEditorScreens()
    {
        formPanel.SetActive(false);
        effectsPanel.SetActive(false);
        targetsPanel.SetActive(false);
    }

    public void DisableCamerasWithoutCurrent(GameObject camera)
    {
        if(camera != mainView)
        {
            mainView.SetActive(false);
        }
        if(camera != formRingView)
        {
            formRingView.SetActive(false);
        }
        if(camera != effectsRingView)
        {
            effectsRingView.SetActive(false);
        }
        if (camera != targetsRingView)
        {
            targetsRingView.SetActive(false);
        }
    }
}
