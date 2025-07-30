using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;

[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
partial struct ShootSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        //Init
        NetworkTime networkTime = SystemAPI.GetSingleton<NetworkTime>();
        EntitiesReferences entityReferences = SystemAPI.GetSingleton<EntitiesReferences>();

        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach ((RefRO<NetcodePlayerInput> netcodePlayerInput,
                  RefRO<LocalTransform> localTransform,
                  RefRO<GhostOwner> ghostOwner,
                  RefRW<MugCount> mugCount)
            in SystemAPI.Query<RefRO<NetcodePlayerInput>, RefRO<LocalTransform>, RefRO<GhostOwner>, RefRW<MugCount>>().WithAll<Simulate>())
        {
            //Ensure that one shoot is one tick
            if (!networkTime.IsFirstTimeFullyPredictingTick)
                continue;
            //Shoot logic
            if (mugCount.ValueRO.value >= 4 && netcodePlayerInput.ValueRO.shoot.IsSet)
            {
                Entity bulletEntity = entityCommandBuffer.Instantiate(entityReferences.bulletPrefabEntity);
                entityCommandBuffer.SetComponent(bulletEntity, LocalTransform.FromPosition(localTransform.ValueRO.Position));
                entityCommandBuffer.SetComponent(bulletEntity, new GhostOwner { NetworkId = ghostOwner.ValueRO.NetworkId });

                //Calculate the shoot direction
                float3 dir = new float3(netcodePlayerInput.ValueRO.inputVector.x, 0, netcodePlayerInput.ValueRO.inputVector.y);
                if (math.lengthsq(dir) < 0.001f)
                    dir = new float3(0, 0, 1); //Default

                dir = math.normalize(dir);
                entityCommandBuffer.AddComponent(bulletEntity, new BulletDirection { value = dir });
                //Update mugcount
                mugCount.ValueRW.value -= 4;
            }
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}
