using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BramaBadura.FollowCamera
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private void LateUpdate()
        {
            transform.position = target.position;
        }
    }

}

