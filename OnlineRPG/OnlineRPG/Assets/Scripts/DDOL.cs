using UnityEngine;

public class DDOL : MonoBehaviour
{
	void Awake()
	{
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
	}
}