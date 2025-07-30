using Unity.NetCode;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;

public class NetcodePlayerInputAuthoring : MonoBehaviour
{
    // Convert basic GameObject to Entity
    public class Baker : Baker<NetcodePlayerInputAuthoring>
    {
        public override void Bake(NetcodePlayerInputAuthoring authoring)
        {
            // Creating new entity
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            // Add input
            AddComponent(entity, new NetcodePlayerInput());
        }
    }
}
// Store input player values
public struct NetcodePlayerInput : IInputComponentData
{
    public float2 inputVector;
    public InputEvent shoot;
}
