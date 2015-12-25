using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T instance;
	
	/**
      Returns the instance of this singleton.
   */
	public static T Instance
	{
		get
		{
			if(instance == null)
			{
				instance = (T) FindObjectOfType(typeof(T));
		
				if (instance == null)
				{
					Debug.LogWarning("An instance of " + typeof(T) + 
					               " was not found in the scene, instantiating one.");
					instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
					if (instance == null) {
						Debug.LogWarning("Error instantiating instance of " + typeof(T));
					}
				}
			}
			
			return instance;
		}
	}
}