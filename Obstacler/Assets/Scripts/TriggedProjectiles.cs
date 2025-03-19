using UnityEngine;

public class TriggedProjectiles : MonoBehaviour
{
    [SerializeField] GameObject[] projectile;

    private void OnTriggerEnter(Collider other)
    {
        foreach (var item in projectile) 
        {
            if(item == null) return;

            item.SetActive(true);   
        }

    }
}
