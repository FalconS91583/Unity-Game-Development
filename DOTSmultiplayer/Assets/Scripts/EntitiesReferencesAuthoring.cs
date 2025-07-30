using Unity.Entities;
using UnityEngine;


public class EntitiesReferencesAuthoring : MonoBehaviour
{
    // Gameobjects references
    public GameObject playerPrefab;
    public GameObject bulletPrefab;
    public GameObject pickupPrefab;
    public GameObject wallPefab;

    public class Baker : Baker<EntitiesReferencesAuthoring>
    {
        // Convert to entity
        public override void Bake(EntitiesReferencesAuthoring authoring)
        {
            // Add components
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntitiesReferences
            {
                playerPrefabEntity = GetEntity(authoring.playerPrefab, TransformUsageFlags.Dynamic),
                bulletPrefabEntity = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                pickupPrefabEntity = GetEntity(authoring.pickupPrefab, TransformUsageFlags.Dynamic),
                wallPefabEntity = GetEntity(authoring.wallPefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}
//Data
public struct EntitiesReferences : IComponentData
{
    public Entity playerPrefabEntity;
    public Entity bulletPrefabEntity;
    public Entity pickupPrefabEntity;
    public Entity wallPefabEntity;
}
