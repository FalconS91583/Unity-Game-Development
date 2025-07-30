using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

// Attribute to run this only on client
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
partial struct TestNetcodeEntitiesClientSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    //Using Debug requires to comment burst
    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //Send RPC with Input
            Entity rpcEntity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(rpcEntity, new SimpleRPC
            {
                value = 56
            });
            //SendRpcCommandRequest acually sending the message
            state.EntityManager.AddComponentData(rpcEntity, new SendRpcCommandRequest());

            Debug.Log("Sending RPC...");
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
