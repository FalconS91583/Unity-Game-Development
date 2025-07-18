using UnityEngine;
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Heal OnHit", fileName = "Item Effect Data - Heal On Hit")]
public class ItemEffect_HealOnDoingDamage : ItemEffectDataSO
{
    [SerializeField] private float precentHealOnAttack = 0.2f;

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.playerCombat.OnDoingPhyusicalDamage += HealOnDoingDamage;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.playerCombat.OnDoingPhyusicalDamage -= HealOnDoingDamage;
        player = null;
    }

    private void HealOnDoingDamage(float damage)
    {
        player.health.IncreaseHealth(damage * precentHealOnAttack);
    }
}
