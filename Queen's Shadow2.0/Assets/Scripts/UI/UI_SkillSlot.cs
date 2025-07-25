using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;
    private Image skillicon;
    private RectTransform rect;
    private Button button;

    private SkillDataSO skillData;

    public SkillType skillType;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private string inputKeyName;
    [SerializeField] private TextMeshProUGUI inputKeyText;
    [SerializeField] private GameObject conflictSLot;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        button = GetComponent<Button>();
        skillicon = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    public void SetupSkillSlot(SkillDataSO selectedSkill)
    {
        this.skillData = selectedSkill;

        Color color = Color.black; color.a = 0.6f;
        cooldownImage.color = color;

        inputKeyText.text = inputKeyName;
        skillicon.sprite = selectedSkill.icon;
    
        if(conflictSLot != null ) 
            conflictSLot.SetActive(false);
    }

    public void StartCooldown(float cooldown)
    {
        cooldownImage.fillAmount = 1;
        StartCoroutine(CooldownCo(cooldown)); 
    }

    public void ResetCooldown()
    {
        cooldownImage.fillAmount = 0;
    }

    private IEnumerator CooldownCo(float duration)
    {
        float timePassed = 0;

        while (timePassed < duration)
        {
            timePassed = timePassed + Time.deltaTime;
            cooldownImage.fillAmount = 1f - (timePassed / duration);
            yield return null;
        }
        cooldownImage.fillAmount = 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skillData == null)
            return; 

        ui.skillToolTip.ShowToolTip(true, rect, skillData, null);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, null);
    }
}
