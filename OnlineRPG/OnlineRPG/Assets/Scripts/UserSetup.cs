using TMPro;
using UnityEngine;

public class UserSetup : MonoBehaviour
{
    #region Singleton
    public static UserSetup singleton;
    #endregion

    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("Multiple UserSetups on the client!");
            return;
        }

        singleton = this;
    }
}