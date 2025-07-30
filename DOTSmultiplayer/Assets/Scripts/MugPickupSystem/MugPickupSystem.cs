using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct MugPickupSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        //Player iterate
        foreach (var (playerTransform, mugCount, playerEntity)
            in SystemAPI.Query<RefRO<LocalTransform>, RefRW<MugCount>>().WithAll<Player, Simulate>().WithEntityAccess())
        {
            // Mug iterate
            foreach (var (mugTransform, mugEntity)
                in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Mug>().WithEntityAccess())
            {
                //Calculating distance and pickup distance
                float distance = math.distance(playerTransform.ValueRO.Position, mugTransform.ValueRO.Position);
                if (distance < 1.5f)
                {
                    mugCount.ValueRW.value += 1;
                    entityCommandBuffer.DestroyEntity(mugEntity);
                    break;
                }
            }
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}
