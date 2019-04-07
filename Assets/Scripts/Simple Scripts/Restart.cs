using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    public void RestartToScene (string SceneName)
    {
        PersistantGameManager.Instance.Restart(SceneName);
    }
}
