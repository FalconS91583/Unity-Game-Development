using UnityEngine;

public class Object_NPC : MonoBehaviour, IInteractble
{
    protected Transform player;
    protected UI ui;
    protected Player_QuestManager questManager;

    [Header("NPC reward")]
    [SerializeField] protected RewardType rewardNPC;
    [SerializeField] private string npcTargetQuestID;
    [Space]
    [SerializeField] private Transform NPC;
    [SerializeField] private GameObject interactTooltip;
    private bool facingRight = true;

    [Header("Floating")]
    [SerializeField] private float floatSpeed = 8;
    [SerializeField] private float floatRange = .1f;
    private Vector3 startPosition;

    protected virtual void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        startPosition = interactTooltip.transform.position;
        interactTooltip.SetActive(false);
    }

    protected virtual void Start()
    {
        questManager = Player.instance.questManager;
    }

    protected virtual void Update()
    {
        HandleNPCFlip();
        HandleTooltipFloat();
    }

    private void HandleTooltipFloat()
    {
        if (interactTooltip.activeSelf)
        {
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
            interactTooltip.transform.position = startPosition + new Vector3(0, yOffset);
        }
    }

    private void HandleNPCFlip()
    {
        if (player == null || NPC == null)
            return;

        if(NPC.position.x > player.position.x && facingRight)
        {
            NPC.transform.Rotate(0, 180, 0);
            facingRight = false;
        }
        else if(NPC.position.x < player.position.x && facingRight == false)
        {
            NPC.transform.Rotate(0,180,0);
            facingRight = true;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.transform;
        interactTooltip.SetActive(true);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        interactTooltip.SetActive(false);   
    }

    public virtual void Interact()
    {
        questManager.AddProgress(npcTargetQuestID);
        //questManager.TryGiveRewardFrom(rewardNPC);
    }
}
