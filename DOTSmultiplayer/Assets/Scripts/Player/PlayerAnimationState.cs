using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

// Dupliacation between server and client
[GhostComponent(PrefabType = GhostPrefabType.All)]
//Animations data
public struct PlayerAnimationState : IComponentData
{
    //GhostFiled indicates which fields should be synchronized in real time.
    [GhostField] public float2 moveDirection;
    [GhostField] public bool isMoving;
}
