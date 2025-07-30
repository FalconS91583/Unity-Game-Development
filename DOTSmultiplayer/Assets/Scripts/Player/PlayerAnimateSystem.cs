using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
// Client run
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
partial struct PlayerAnimateSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

        // Creating animator
        foreach (var (prefabRef, player, entity) in
                 SystemAPI.Query<PlayerGameObjectPrefab, RefRO<Player>>()
                 .WithNone<PlayerAnimatorReference>().WithEntityAccess())
        {
            //Creating a gameobject with animator
            int index = player.ValueRO.prefabIndex;
            var go = Object.Instantiate(prefabRef.values[index]);
            var animator = go.GetComponentInChildren<Animator>();
            // Save value
            entityCommandBuffer.AddComponent(entity, new PlayerAnimatorReference { value = animator });
        }

        // Synchronize position and rotation with animation
        foreach (var (animRef, transform, animState)
                 in SystemAPI.Query<PlayerAnimatorReference, RefRO<LocalTransform>, RefRO<PlayerAnimationState>>())
        {
            float3 position = transform.ValueRO.Position;
            float2 inputVector = animState.ValueRO.moveDirection;

            // New pos for a model to touching the ground
            float3 correctedPosition = position + new float3(0, -1f, 0);
            animRef.value.transform.position = correctedPosition;

            // Rotate gameobject
            if (math.lengthsq(inputVector) > 0.001f)
            {
                float3 forward = new float3(inputVector.x, 0, inputVector.y);
                quaternion targetRotation = quaternion.LookRotationSafe(forward, math.up());
                quaternion currentRotation = animRef.value.transform.rotation;
                quaternion smoothRotation = math.slerp(currentRotation, targetRotation, 10f * SystemAPI.Time.DeltaTime);
                animRef.value.transform.rotation = smoothRotation;
            }
            // Set moving animations
            animRef.value.SetBool("isMoving", animState.ValueRO.isMoving);
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}
