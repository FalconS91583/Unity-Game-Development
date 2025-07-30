using Unity.Entities;
using UnityEngine;

public class WallBlockAuthoring : MonoBehaviour
{
    //Conver to entity
    public class Baker : Baker<WallBlockAuthoring>
    {
        public override void Bake(WallBlockAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<WallBlockTag>(entity);
        }
    }
}
//Data
public struct WallBlockTag : IComponentData { }
