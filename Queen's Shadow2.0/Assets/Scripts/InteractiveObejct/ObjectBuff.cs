using System.Collections;
using UnityEngine;

public class ObjectBuff : MonoBehaviour
{
    private Player_Stats statsToModify;

    [SerializeField] private float floatSpeed = 1;
    [SerializeField] private float floatRange = .1f;
    private Vector3 startPosition;

    [Header("Buff Details")]
    [SerializeField] private BuffEffectData[] buffs;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 4f;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPosition + new Vector3(0, yOffset);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        statsToModify = collision.GetComponent<Player_Stats>();

        if (statsToModify.CanApplyBuff(buffName))
        {
            statsToModify.ApplyBuff(buffs, buffDuration, buffName);
            Destroy(gameObject);
        }

    }
}
