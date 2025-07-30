using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;

// Prediction group 
[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
partial struct BulletSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        // Going through every bullet
        foreach ((RefRW<LocalTransform> localTransform,
                  RefRW<Bullet> bullet,
                  RefRO<BulletDirection> bulletDir,
                  Entity entity)
            in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Bullet>, RefRO<BulletDirection>>()
                        .WithEntityAccess()
                        .WithAll<Simulate>())
        {
            //Bullet move
            float moveSpeed = 30f;
            localTransform.ValueRW.Position += bulletDir.ValueRO.value * moveSpeed * SystemAPI.Time.DeltaTime;

            // Server timer calculation
            if (state.World.IsServer())
            {
                bullet.ValueRW.timer -= SystemAPI.Time.DeltaTime;
                if (bullet.ValueRW.timer <= 0f)
                {
                    ecb.DestroyEntity(entity);
                }
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}