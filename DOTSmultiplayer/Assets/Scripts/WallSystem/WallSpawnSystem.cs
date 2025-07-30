using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.NetCode;
using Unity.Physics;
using Unity.Collections;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
public partial struct WallSpawnSystem : ISystem
{
    private bool _wallsSpawned;

    public void OnUpdate(ref SystemState state)
    {

        //Safty checks
        if (_wallsSpawned)
            return;

        if (!SystemAPI.HasSingleton<EntitiesReferences>())
            return;
        // Init
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        var refs = SystemAPI.GetSingleton<EntitiesReferences>();

        Entity wallBlockPrefab = refs.wallPefabEntity;

        int numberOfWalls = 5;
        float mapRadius = 15f;

        // Handling wall spawn logic
        for (int i = 0; i < numberOfWalls; i++)
        {
            float3 wallOrigin = new float3(
                UnityEngine.Random.Range(-mapRadius, mapRadius),
                1,
                UnityEngine.Random.Range(-mapRadius, mapRadius)
            );

            int width = UnityEngine.Random.Range(3, 6);
            int height = UnityEngine.Random.Range(2, 5);
            float spacing = 1.1f;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Entity block = entityCommandBuffer.Instantiate(wallBlockPrefab);
                    float3 position = wallOrigin + new float3(x * spacing, y * spacing, 0);
                    entityCommandBuffer.SetComponent(block, LocalTransform.FromPosition(position));
                }
            }
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
        _wallsSpawned = true;
    }
}
