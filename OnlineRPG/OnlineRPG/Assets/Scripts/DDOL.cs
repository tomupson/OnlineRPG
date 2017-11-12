using UnityEngine;

public class DDOL : MonoBehaviour
{
	void Awake()
	{
        DontDestroyOnLoad(this);
        
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
	}
}