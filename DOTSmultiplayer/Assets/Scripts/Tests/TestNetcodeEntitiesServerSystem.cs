using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

// Attribute to run this only on servver
[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct TestNetcodeEntitiesServerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Buffor for etc.creating and destroying entities
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        // Going throught all entities with requires components
        foreach ((
            //RO reading component
            RefRO<SimpleRPC> simpleRPC,
            RefRO<ReceiveRpcCommandRequest> receiveRpcCommandRequest,
            // ID of entity
            Entity entity)
            //WithEntityAccess allows to get the ID of entity in foreach
            in SystemAPI.Query<
                RefRO<SimpleRPC>, RefRO<ReceiveRpcCommandRequest>>().WithEntityAccess())
        {
            Debug.Log("Received RPC: " + simpleRPC.ValueRO.value + " :: " + receiveRpcCommandRequest.ValueRO.SourceConnection);
            entityCommandBuffer.DestroyEntity(entity);//Realising entit
        }
        //Doing all actions like destroying after the end of foreach
        entityCommandBuffer.Playback(state.EntityManager);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
