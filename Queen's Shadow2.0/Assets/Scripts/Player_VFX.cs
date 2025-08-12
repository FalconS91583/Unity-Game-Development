using System.Collections;
using UnityEngine;

public class Player_VFX : Entity_VFX
{

    public void CreateEffectOf(GameObject effect, Transform targer)
    {
        Instantiate(effect, targer.position, Quaternion.identity);
    }

}
