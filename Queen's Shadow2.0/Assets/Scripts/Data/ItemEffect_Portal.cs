using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Portal Scroll", fileName = "Item Effect Data - PortalScroll")]
public class ItemEffect_Portal : ItemEffectDataSO
{
    public override void ExecuteEffect()
    {
        if(SceneManager.GetActiveScene().name == "Level 0")
        {
            return;
        }

        Player player = Player.instance;
        Vector3 portalPosition = player.transform.position + new Vector3(player.facingDir * 1.5f, 0);

        Object_Portal.instance.ActivatePortal(portalPosition, player.facingDir);
    }
}
