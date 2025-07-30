using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    //References for diffrent models
    public GameObject animatorPrefab1;
    public GameObject animatorPrefab2;

    // Convert
    public class Baker : Baker<PlayerAuthoring>
    {
        // Adding all the requires components
        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Player());
            AddComponent(entity, new PlayerAnimationState());
            AddComponent(entity, new MugCount { value = 0 });
            AddComponentObject(entity, new PlayerGameObjectPrefab
            {
                values = new GameObject[] { authoring.animatorPrefab1, authoring.animatorPrefab2 }
            });
        }
    }
}
// Components Data
public struct Player : IComponentData {
    [GhostField] public int prefabIndex;
}

public class PlayerGameObjectPrefab : IComponentData
{
    public GameObject[] values;
}

public class PlayerAnimatorReference : ICleanupComponentData
{
    public Animator value;
}