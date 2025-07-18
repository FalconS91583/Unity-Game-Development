using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    [SerializeField] private GameObject[] uiElements;
    public bool alternativeInput {  get; private set; }
    private PlayerInputSet input;

    public UI_SkillToolTip skillToolTip { get; private set; }
    public UI_ItemToolTip itemToolTip { get; private set; }
    public UI_StatToolTip statToolTip {  get; private set; }

    public UI_SkillTree skillTreeUI { get; private set; }
    public UI_Inventory inventoryUI { get; private set; }
    public UI_Storage storageUI { get; private set; }
    public UI_Craft craftUI { get; private set; }
    public UI_Merchant merchantUI { get; private set; }
    public UI_InGame inGameUI { get; private set; }
    public UI_Options optionsUI { get; private set; }
    public UI_DeathScreen deathScreenUI { get; private set; }
    public UI_FadeScreen fadeScreenUI {  get; private set; }
    public UI_Quest questUI { get; private set; }
    public UI_Dialogue dialogueUI { get; private set; }


    public bool skillTreeEnabled;
    private bool inventoryEnabled;

    private void Awake()
    {
        instance = this;

        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
        statToolTip = GetComponentInChildren<UI_StatToolTip>();

        skillTreeUI = GetComponentInChildren<UI_SkillTree>(true);
        inventoryUI = GetComponentInChildren<UI_Inventory>(true);
        storageUI = GetComponentInChildren<UI_Storage>(true);
        craftUI = GetComponentInChildren<UI_Craft>(true);
        merchantUI = GetComponentInChildren<UI_Merchant>(true);
        optionsUI = GetComponentInChildren<UI_Options>(true);

        inGameUI = GetComponentInChildren<UI_InGame>(true);

        deathScreenUI = GetComponentInChildren<UI_DeathScreen>(true);

        fadeScreenUI = GetComponentInChildren<UI_FadeScreen>(true);

        questUI = GetComponentInChildren<UI_Quest>(true);

        dialogueUI = GetComponentInChildren<UI_Dialogue>(true);

        skillTreeEnabled = skillTreeUI.gameObject.activeSelf;
        inventoryEnabled = inventoryUI.gameObject.activeSelf;
    }
    private void Start()
    {
        skillTreeUI.UnlockDefaultSkills();
    }

    public void SetupControlsUI(PlayerInputSet inputSet)
    {
        input = inputSet;

        input.UI.ToggleSkillTreeUI.performed += ctx => ToggleSkillTreeUI();
        input.UI.ToggleCharacterUI.performed += ctx => ToggleInventoryUI();

        input.UI.AlternativeInput.performed += ctx => alternativeInput = true;
        input.UI.AlternativeInput.canceled += ctx => alternativeInput = false;

        input.UI.ToggleOptionsUI.performed += ctx =>
        {
            foreach (var element in uiElements)
            {
                if (element.activeSelf)
                {
                    Time.timeScale = 1;
                    SwitchToInGameUI();
                    return;
                }
            }
            Time.timeScale = 0;
            OpenOptionsUI(); 
        };

        input.UI.DialogueUI_Interaction.performed += ctx =>
        {
            if (dialogueUI.gameObject.activeInHierarchy)
                dialogueUI.DialogueInteraction();
        };

        input.UI.DialogueNavigation.performed += ctx =>
        {
            int direction = Mathf.RoundToInt(ctx.ReadValue<float>());

            if (dialogueUI.gameObject.activeInHierarchy)
                dialogueUI.NavigateChoice(direction);
        };
    }

    public void OpenDeathScreenUI()
    {
        SwitchTo(deathScreenUI.gameObject);
        StopPlayerControlls(true);
        input.Disable();

    }
    public void OpenOptionsUI()
    {
        SwitchTo(optionsUI.gameObject);

        SwitchOffAllTooltips();
        StopPlayerControlls(true);

    }

    public void SwitchToInGameUI()
    {
        SwitchTo(inGameUI.gameObject);
        SwitchOffAllTooltips();
        StopPlayerControlls(false);

        skillTreeEnabled = false;
        inventoryEnabled = false;
    }

    private void SwitchTo(GameObject objectToSwitchOn)
    {
        foreach (var element in uiElements)
        {
            element.gameObject.SetActive(false);
        }

        objectToSwitchOn.SetActive(true);
    }

    private void StopPlayerControlls(bool stopControls)
    {
        if (stopControls)
            input.Player.Disable();
        else
            input.Player.Enable();
    }

    private void StopPlayerControllsIfNeeded()
    {
        foreach (var element in uiElements)
        {
            if (element.activeSelf)
            {
                StopPlayerControlls(true);
                return;
            }   
        }

        StopPlayerControlls(false);
    }

    public void SwitchOffAllTooltips()
    {
        itemToolTip.ShowToolTip(false, null);
        skillToolTip.ShowToolTip(false, null);
        statToolTip.ShowToolTip(false, null);
    }

    public void ToggleSkillTreeUI()
    {
        skillTreeUI.transform.SetAsLastSibling();
        SetToolTipsAboveAnotherElemets();
        fadeScreenUI.transform.SetAsLastSibling();

        skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);
        SwitchOffAllTooltips();

        StopPlayerControlls(skillTreeEnabled);
        // Or this: StopPlayerControllsIfNeeded();
    }

    public void ToggleInventoryUI()
    {
        inventoryUI.transform.SetAsLastSibling();
        SetToolTipsAboveAnotherElemets();
        fadeScreenUI.transform.SetAsLastSibling();

        inventoryEnabled = !inventoryEnabled;
        inventoryUI.gameObject.SetActive(inventoryEnabled);
        SwitchOffAllTooltips();

        StopPlayerControlls(inventoryEnabled);
    }

    public void OpenStorageUI(bool openStorageUI)
    {
        storageUI.gameObject.SetActive(openStorageUI);
        StopPlayerControlls(openStorageUI);

        if(openStorageUI == false)
        {
            craftUI.gameObject.SetActive(false);
            SwitchOffAllTooltips();
        }
    }

        public void OpenCraftUI(bool openCraftUI)
    {
        craftUI.gameObject.SetActive(openCraftUI);
        StopPlayerControlls(openCraftUI);

        if(openCraftUI == false)
        {
            storageUI.gameObject.SetActive(false);
            SwitchOffAllTooltips();
        }
    }

    public void OpenDialogueUI(DialogueLineSO firstLine, DialogueNPCData npcData)
    {
        StopPlayerControlls(true);
        SwitchOffAllTooltips();

        dialogueUI.gameObject.SetActive(true);
        dialogueUI.SetupNPCData(npcData);
        dialogueUI.PlayDialogueLine(firstLine);
    }

    public void OpenQustUI(QuestDataSO[] questsToShow)
    {
        StopPlayerControlls(true);
        SwitchOffAllTooltips();

        questUI.gameObject.SetActive(true);
        questUI.SetupQuestUI(questsToShow);
    }

    public void OpenMerchantUI(bool openMerchantUI)
    {
        merchantUI.gameObject.SetActive(openMerchantUI);
        StopPlayerControlls(openMerchantUI);

        if(openMerchantUI == false)
            SwitchOffAllTooltips();
    }

    private void SetToolTipsAboveAnotherElemets()
    {
        itemToolTip.transform.SetAsLastSibling();
        skillToolTip.transform.SetAsLastSibling();
        statToolTip.transform.SetAsLastSibling();
    }
}
