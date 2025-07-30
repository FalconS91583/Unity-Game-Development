using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public class MugAuthoring : MonoBehaviour
{
    //Convert to entity
    public class Baker : Baker<MugAuthoring> 
    {
        public override void Bake(MugAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Mug());
            AddComponent(entity, new MugCount());
        }
    }

}
//Data 
public struct Mug : IComponentData { }

public struct MugCount : IComponentData
{
    [GhostField] public int value;
}
