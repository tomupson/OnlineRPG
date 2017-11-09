using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    #region Singleton
    public static NetworkPlayer singleton;
    #endregion

    public string Username { get; private set; }

    void Awake()
    {
        singleton = this;

        Username = "UppyMeister#" + Random.Range(1000, 9999);
    }
}