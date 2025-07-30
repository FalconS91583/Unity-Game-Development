using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

//Running on client
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
partial struct GoInGameClientSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<NetworkId>();
    }

    //[BurstCompile]
    //Mark all clients as ,,InGame" with ,,message" that they can start the game
    public void OnUpdate(ref SystemState state)
    {
        
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach ((RefRO<NetworkId> networkId, Entity entity) 
            //Searching in client that are not ,,in game", to add this component only once
            in SystemAPI.Query<RefRO<NetworkId>>().WithNone<NetworkStreamInGame>().WithEntityAccess())
        {
            //Makr as ,,InGame" with component
            entityCommandBuffer.AddComponent<NetworkStreamInGame>(entity);
            Debug.Log("Setting Clinet In Game");

            // new entity with requires components
            Entity rpcEntity = entityCommandBuffer.CreateEntity();
            entityCommandBuffer.AddComponent(rpcEntity, new GoInGameRequestRPC());
            entityCommandBuffer.AddComponent(rpcEntity, new SendRpcCommandRequest());

        }
        // Do all actions
        entityCommandBuffer.Playback(state.EntityManager);

    }


}

public struct GoInGameRequestRPC : IRpcCommand
{

}
