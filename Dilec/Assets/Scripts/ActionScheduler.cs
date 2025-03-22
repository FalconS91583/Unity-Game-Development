using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BramaBadura.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;

      public void StartAction(IAction Action)
        {
            if (currentAction == Action) return;

            if(currentAction != null)
            {
                currentAction.Cancel();
            }

            currentAction = Action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}

