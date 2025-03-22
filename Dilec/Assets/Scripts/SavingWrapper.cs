using BramaBadura.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BramaBadura.SceneMenagment
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        IEnumerator Start()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediete();
            yield return fader.FadeIn(0.2f);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }
        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile); 
        }
    }
}


