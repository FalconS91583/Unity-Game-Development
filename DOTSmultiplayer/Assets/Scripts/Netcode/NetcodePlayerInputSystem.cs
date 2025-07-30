using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using Unity.Mathematics;
using UnityEngine;

//Only clients
[UpdateInGroup(typeof(GhostInputSystemGroup))]
partial struct NetcodePlayerInputSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<NetworkStreamInGame>();
        state.RequireForUpdate<NetcodePlayerInput>();
    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //For all inpunts
        foreach((RefRW<NetcodePlayerInput> netcodePlayerInput,
            RefRW<MyValue> myValue) 
            in SystemAPI.Query<RefRW<NetcodePlayerInput>, RefRW<MyValue>>().WithAll<GhostOwnerIsLocal>())
            //GhostOwner - player controlling the object
        {
            //Empty vector
            float2 inputVector = new float2();

            // Input movement
            if (Input.GetKey(KeyCode.W))
            {
                inputVector.y = -1f;
            }

            if (Input.GetKey(KeyCode.S))
            {
                inputVector.y = +1f;
            }

            if (Input.GetKey(KeyCode.A))
            {
                inputVector.x = +1f;
            }

            if (Input.GetKey(KeyCode.D))
            {
                inputVector.x = -1f;
            }

            // save the vector
            netcodePlayerInput.ValueRW.inputVector = inputVector;

            // Set the shoot flag with mouse
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Shoot");
                netcodePlayerInput.ValueRW.shoot.Set();
            }
            else// Flag reset
            {
                netcodePlayerInput.ValueRW.shoot = default;
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
