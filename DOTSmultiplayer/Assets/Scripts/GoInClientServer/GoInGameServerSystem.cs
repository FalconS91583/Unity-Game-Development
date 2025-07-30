using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

//Run on server
[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]    
partial struct GoInGameServerSystem : ISystem
{
    // Components required to run update
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EntitiesReferences>();
        state.RequireForUpdate<NetworkId>();
    }


    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        //Getting access to prefabs
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();
        // Searchinf for all RPC requesting to go in game
        foreach ((RefRO<ReceiveRpcCommandRequest> receiveRpcCommandRequest, Entity entity)
            in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<GoInGameRequestRPC>().WithEntityAccess())
        {
            //Mark as in game
            entityCommandBuffer.AddComponent<NetworkStreamInGame>(receiveRpcCommandRequest.ValueRO.SourceConnection);

            Debug.Log("Client Connected To Server");
            // Creating player 
            Entity playerEntity = entityCommandBuffer.Instantiate(entitiesReferences.playerPrefabEntity);
            // Randol location
            entityCommandBuffer.SetComponent(playerEntity, LocalTransform.FromPosition(new float3(
                UnityEngine.Random.Range(-10, +10), 0 ,0
                )));

            //Getting networkID
            NetworkId networkId =
                SystemAPI.GetComponent<NetworkId>(receiveRpcCommandRequest.ValueRO.SourceConnection);

            // Set ID
            entityCommandBuffer.AddComponent(playerEntity, new GhostOwner
            {
                NetworkId = networkId.Value
            });

            // Connecting player with his connection
            entityCommandBuffer.AppendToBuffer(receiveRpcCommandRequest.ValueRO.SourceConnection, 
                new LinkedEntityGroup
            {
                Value = playerEntity,
            });

            //Choosing a prefab for the player by the networkID
            //int prefabIndex = UnityEngine.Random.Range(0, 2); 
            int prefabIndex = networkId.Value % 2;
            // Attaching the player component
            entityCommandBuffer.SetComponent(playerEntity, new Player
            {
                prefabIndex = prefabIndex
            });

            //Delete this entity
            entityCommandBuffer.DestroyEntity(entity);
        }
        // Do all the operations
        entityCommandBuffer.Playback(state.EntityManager);
    }


}
