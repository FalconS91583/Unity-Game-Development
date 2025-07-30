using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

//Prediction group for better performance online
[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
partial struct NetcodePlayerMovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        // for all entitiy with these components
        foreach ((RefRO<NetcodePlayerInput> input,
                   RefRW<LocalTransform> transform, //RW for reading and setting the component
                   RefRW<PlayerAnimationState> animState)
                 in SystemAPI.Query<RefRO<NetcodePlayerInput>, RefRW<LocalTransform>, RefRW<PlayerAnimationState>>()
                 .WithAll<Simulate>()) // simulate predict locally it used for situation like clinet have high ping
        {
            // Get vector
            float2 inputVec = input.ValueRO.inputVector;
            // Calculatin movmeent
            float3 move = new float3(inputVec.x, 0, inputVec.y);
            float moveSpeed = 10f;
            // Move player with frame rate independent
            transform.ValueRW.Position += move * moveSpeed * SystemAPI.Time.DeltaTime;
            // Set the values to animations for direction
            animState.ValueRW.moveDirection = inputVec;
            animState.ValueRW.isMoving = math.lengthsq(inputVec) > 0.001f;
        }
    }
}