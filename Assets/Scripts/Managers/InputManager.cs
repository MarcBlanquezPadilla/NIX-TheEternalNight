using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	#region Instance

	private static InputManager _instance;
	public static InputManager Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<InputManager>();
				//DontDestroyOnLoad(_instance);
			}
			return _instance;
		}
	}

	#endregion

	public Dictionary<string, KeyCode> keys = new Dictionary<string,KeyCode>();


	public void ChangeKey(string KeyName, KeyCode keyCode)
    {
		if (keys.ContainsKey(KeyName))
        {
			keys[KeyName] = keyCode;
		}
    }
}
