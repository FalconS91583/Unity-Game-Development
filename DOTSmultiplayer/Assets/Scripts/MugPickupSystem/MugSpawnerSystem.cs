using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.NetCode;
using Unity.Collections;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct MugSpawnerSystem : ISystem
{
    private bool _spawned;

    public void OnUpdate(ref SystemState state)
    {
        if (_spawned) return;

        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        var refs = SystemAPI.GetSingleton<EntitiesReferences>();

        // Calculating position and spawn the mug pickup
        for (int i = 0; i < 50; i++)
        {
            var mug = entityCommandBuffer.Instantiate(refs.pickupPrefabEntity);
            float3 pos = new float3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
            entityCommandBuffer.SetComponent(mug, LocalTransform.FromPosition(pos));
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();

        _spawned = true;
    }
}
