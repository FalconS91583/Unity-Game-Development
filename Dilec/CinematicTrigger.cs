using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


namespace BramaBadura.Cinema
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool alreadyTriggerd = false;

        private void OnTriggerEnter(Collider other)
        {
            if(!alreadyTriggerd && other.CompareTag("Player"))
            {
                alreadyTriggerd = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}

