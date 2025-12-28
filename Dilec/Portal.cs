using BramaBadura.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace BramaBadura.SceneMenagment
{
    public class Portal : MonoBehaviour
    {
        enum DestinationID
        {
            A,B,C,D
        }

        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnpoint;
        [SerializeField] private DestinationID destination;
        [SerializeField] private float fadeOutTime = 1f;
        [SerializeField] private float fadeInTime = 2f;
        [SerializeField] private float fadeWaitTime = 0.5f;
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                StartCoroutine(SceneTransition());
            }
        }
        private IEnumerator SceneTransition()
        {
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;

            yield return fader.FadeOut(fadeOutTime);


            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;

            wrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);

            newPlayerController.enabled = true;
            Destroy(gameObject);
        }
        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;

                return portal;
            }

            return null;
        }
        private void UpdatePlayer(Portal otherPortal)
        {
            if (otherPortal == null)
            {
                Debug.LogError("Other portal not found!");
                return;
            }

            GameObject player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player not found in scene!");
                return;
            }

            NavMeshAgent playerAgent = player.GetComponent<NavMeshAgent>();
            if (playerAgent != null)
            {
                playerAgent.enabled = false;
                player.transform.position = otherPortal.spawnpoint.position;
                player.transform.rotation = otherPortal.spawnpoint.rotation;
                playerAgent.enabled = true;
            }
            else
            {
                Debug.LogError("Player does not have a NavMeshAgent component!");
            }
        }

    }
}

