using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI ui;
    private UI_SkillTree skillTree;

    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillCooldown;
    [SerializeField] private TextMeshProUGUI skillRequirements;
    [Space]
    [SerializeField] private string metCondidiotionHex;
    [SerializeField] private string notMetConditionHex;
    [SerializeField] private string importantInfoHex;
    [SerializeField] private Color exampleColor;
    [SerializeField] private string lockedSkillText = "You've taken a diffrent path - this skill is now locked.";
    [SerializeField] private Coroutine textEffectCo;

    protected override void Awake()
    {
        base.Awake();
        ui = GetComponentInParent<UI>();
        skillTree = ui.GetComponentInChildren<UI_SkillTree>(true);
    }

    public override void ShowToolTip(bool show, RectTransform targetRect)
    {
        base.ShowToolTip(show, targetRect);
    }

    public void ShowToolTip(bool show, RectTransform targetRect, SkillDataSO skillData,UI_TreeNode node)
    {
        base.ShowToolTip(show, targetRect);

        if (show == false)
            return;

        skillName.text = skillData.skillName;
        skillDescription.text = skillData.description;
        skillCooldown.text = "Cooldown: " + skillData.upgradeData.cooldown + " s.";

        if(node == null)
        {
            skillRequirements.text = "";
            return;
        }

        string skillLockedText = $"<color={importantInfoHex}>- {lockedSkillText} </color>";
        string requirements = 
            node.isLocked ? skillLockedText : GetRequirements(node.skillData.cost, node.neededNodes, node.conflictNodes);

        skillRequirements.text = requirements;
    }


    private string GetRequirements(int skillCost, UI_TreeNode[] neededNodes, UI_TreeNode[] conflictNodes)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Requirements:");
        string costColor = skillTree.EnoughSkillPoints(skillCost) ? metCondidiotionHex : notMetConditionHex;
        sb.AppendLine($"<color={costColor}>- {skillCost} skill point(s) </color>");

        foreach (var node in neededNodes)
        {
            if (node == null) continue;
            string nodeColor = node.isUnlocked ? metCondidiotionHex : notMetConditionHex;
            sb.AppendLine($"<color={nodeColor}>- {node.skillData.skillName} </color>");
        }

        if(conflictNodes.Length <= 0)
            return sb.ToString();

        sb.AppendLine(); //Space
        sb.AppendLine($"<color={importantInfoHex}> Locks out: </color>");

        foreach (var node in conflictNodes)
        {
            if(node == null) continue;
            sb.AppendLine($"<color={importantInfoHex}>- {node.skillData.skillName} </color>");
        }

        return sb.ToString();

    }

    public void LockedSkillEffect()
    {
        StopLockedSkillEffect();

        textEffectCo = StartCoroutine(TextBlinkEffectCo(skillRequirements, 0.15f, 3));
    }

    public void StopLockedSkillEffect()
    {
        if (textEffectCo != null)
            StopCoroutine(textEffectCo);
    }

    private IEnumerator TextBlinkEffectCo(TextMeshProUGUI text, float blinkInterval, int blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            text.text = GetColoredText(notMetConditionHex, lockedSkillText);
            yield return new WaitForSeconds(blinkInterval);
            text.text = GetColoredText(importantInfoHex, lockedSkillText);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
