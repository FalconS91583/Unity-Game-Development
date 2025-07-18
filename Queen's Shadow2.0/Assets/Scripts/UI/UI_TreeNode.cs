using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI UI;
    private RectTransform rect;
    private UI_SkillTree skillTree;
    private UI_TreeConnectedHandler treeConnectedHandler;

    [Header("Unlock Details")]
    public UI_TreeNode[] neededNodes;
    public UI_TreeNode[] conflictNodes;
    public bool isUnlocked;
    public bool isLocked;

    [Header("Skill Details")]
    public SkillDataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private int skillCost;
    [SerializeField] private string lockedColorHex = "#9F9797";
    private Color lastColor;



    private void OnValidate()
    {
        if (skillData == null) return;

        skillName = skillData.skillName;
        skillIcon.sprite = skillData.icon;
        skillCost = skillData.cost;
        gameObject.name = "UI_TreeNode - " + skillData.skillName;
    }

    private void Start()
    {
        if (isUnlocked == false)
            UpdateIconColor(GetColorByHex(lockedColorHex));

        UnlockDefaultSkills();
    }
    private void GetNeededComponents()
    {
        UI = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>(true);
        treeConnectedHandler = GetComponent<UI_TreeConnectedHandler>();
    }

    public void UnlockDefaultSkills()
    {
        GetNeededComponents();

        if (skillData.unlockByDefault)
            Unlock();
    }

    public void Refund()
    {
        if (isUnlocked == false || skillData.unlockByDefault)
            return;

        isUnlocked = false;
        isLocked = false;
        UpdateIconColor(GetColorByHex(lockedColorHex));

        skillTree.AddSkillPoints(skillData.cost);
        treeConnectedHandler.ConnectionImageUnlocked(false);
    }

    private void Unlock()
    {
        if (isUnlocked)
            return;

        UpdateIconColor(Color.white);
        isUnlocked = true;
        skillTree.RemoveSkillPoints(skillData.cost);
        LockConflictNodes();
        treeConnectedHandler.ConnectionImageUnlocked(true);

        skillTree.skillManager.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData);
    }

    public void UnlockWithSaveData()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        LockConflictNodes();

        treeConnectedHandler.ConnectionImageUnlocked(true);
    }


    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked)
            return false;

        if (skillTree.EnoughSkillPoints(skillData.cost) == false)
            return false;

        foreach (var node in neededNodes)
        {
            if (node.isUnlocked == false)
                return false;
        }

        foreach (var node in conflictNodes)
        {
            if (node.isUnlocked)
                return false;
        }


        return true;
    }

    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
            node.LockChildNode();
        }
    }

    public void LockChildNode()
    {
        isLocked = true;

        foreach (var node in treeConnectedHandler.GetChildNodes())
        {
            node.LockChildNode();
        }
    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
            return;

        lastColor = skillIcon.color;
        skillIcon.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
            Unlock();
        else if (isLocked)
            UI.skillToolTip.LockedSkillEffect();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI.skillToolTip.ShowToolTip(true, rect, skillData, this);


        if (isUnlocked || isLocked)
            return;

        ToggleNodeHighlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI.skillToolTip.ShowToolTip(false, rect);
        UI.skillToolTip.StopLockedSkillEffect();

        if (isUnlocked || isLocked)
            return;

        ToggleNodeHighlight(false);
    }

    private void ToggleNodeHighlight(bool highlight)
    {
        Color highlightColor = Color.white * 0.8f; highlightColor.a = 1;
        Color colorToApply = highlight ? highlightColor : lastColor;

        UpdateIconColor(colorToApply);
    }

    private Color GetColorByHex(string hexNumber)
    {
        ColorUtility.TryParseHtmlString(hexNumber, out Color color);

        return color;
    }

    private void OnDisable()
    {
        if (isLocked)
            UpdateIconColor(GetColorByHex(lockedColorHex));

        if (isUnlocked)
            UpdateIconColor(Color.white);
    }
}
