using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Heal Effect Item", fileName = "Item Effect Data - heal")]
public class ItemEffect_Heal : ItemEffectDataSO
{
    [SerializeField] private float healprecent = 0.1f;
    public override void ExecuteEffect()
    {
        Player player = FindFirstObjectByType<Player>();

        float healAmout = player.stats.GetMaxHealth() * healprecent;

        player.health.IncreaseHealth(healAmout);
    }
}
