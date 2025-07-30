using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class BulletAuthoting : MonoBehaviour
{
    // Convert to entity
    public class Baker : Baker<BulletAuthoting>
    {
        public override void Bake(BulletAuthoting authoring)
        {
            //Add component
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Bullet
            {
                timer = 1f
            });
            AddComponent(entity, new BulletDirection());
        }
    }
}
//Data
public struct Bullet : IComponentData
{
    public float timer;
}

public struct BulletDirection : IComponentData
{
    public float3 value;
}

