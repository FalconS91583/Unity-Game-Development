using UnityEngine;

public class Scorer : MonoBehaviour
{
    private int hits = 0;

    private void OnCollisionEnter(Collision other)
    {     
        if(other.gameObject.tag != "Hit")
        {
            hits++;
            Debug.Log("You collide with somethg this many times: " + hits);
        }

    }
}
