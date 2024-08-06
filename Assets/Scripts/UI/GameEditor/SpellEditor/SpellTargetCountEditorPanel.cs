using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpellTargetCountEditorPanel : MonoBehaviour, ISpellEditorUIPart
{
    [SerializeField]
    private GameObject speleTargetRunePrefab;
    [SerializeField]
    private Transform spellTargetsRingPrefab;
    [SerializeField]
    private TMP_Text targetsCountText;
    [SerializeField]
    private Button AddTargetButton;
    [SerializeField]
    private Button RemoveTargetButton;
    [SerializeField]
    private TMP_InputField nameInputField;
    [SerializeField]
    private TMP_InputField descriptionInputField;


    public SpellEditorUICenter Center { get; private set; }

    public void ActivatePanel()
    {
        nameInputField.text = Center.CurrentSpell.Name;
        descriptionInputField.text = Center.CurrentSpell.Description;
        RebuildRunes();
    }

    public void Init(SpellEditorUICenter center)
    {
        Center = center;
        AddTargetButton.onClick.AddListener(AddtargetsCount);
        RemoveTargetButton.onClick.AddListener(RemovetargetsCount);
        nameInputField.onValueChanged.AddListener(OnNameChanged);
        descriptionInputField.onValueChanged.AddListener(OnDescriptionChanged);
    }

    public void AddtargetsCount()
    {
        Center.CurrentSpell.TargetsCount++;
        Center.spellEditor.DrawSpellForceRequared();
        RebuildRunes();
    }
    public void RemovetargetsCount()
    {
        Center.CurrentSpell.TargetsCount--;
        Center.spellEditor.DrawSpellForceRequared();
        RebuildRunes();
    }

    public void ClearRunes()
    {
        int count = spellTargetsRingPrefab.childCount;

        for (int i = 0; i < count; i++)
        {
            Destroy(spellTargetsRingPrefab.GetChild(i).gameObject);
        }
    }

    public void RebuildRunes()
    {
        if(Center.CurrentSpell.TargetsCount <= 1)
        {
            RemoveTargetButton.gameObject.SetActive(false);
        }
        else if(Center.CurrentSpell.TargetsCount >= 12)
        {
            AddTargetButton.gameObject.SetActive(false);
        }
        else
        {
            AddTargetButton.gameObject.SetActive(true);
            RemoveTargetButton.gameObject.SetActive(true);
        }

        targetsCountText.text = Center.CurrentSpell.TargetsCount.ToString();
        StopAllCoroutines();

        float angle = 360 / Center.CurrentSpell.TargetsCount;
        float currentAngle = 0;

        ClearRunes();

        for (int i = 0;i < Center.CurrentSpell.TargetsCount; i++)
        {
            StartCoroutine(AddTargetRune(currentAngle));
            currentAngle += angle;
        }
    }

    private IEnumerator AddTargetRune(float angle)
    {
        yield return null;
        Transform rune = Instantiate(speleTargetRunePrefab, spellTargetsRingPrefab).transform;
        SpriteRenderer rend = rune.GetChild(0).GetComponent<SpriteRenderer>();
        rend.GetComponent<RuneRotationHolder>().origin = spellTargetsRingPrefab;

        float t = 0;

        Quaternion startRotation = rune.rotation;
        Quaternion endRotation = rune.rotation * Quaternion.Euler(0, 0, angle);

        while (t < 1)
        {
            t += Time.deltaTime;
            rune.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }
    }

    private void OnNameChanged(string name)
    {
        Center.CurrentSpell.Name = name;
    }

    private void OnDescriptionChanged(string description)
    {
        Center.CurrentSpell.Description = description;
    }
}
