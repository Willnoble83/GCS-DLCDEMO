using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key
{
	public string id
	{
		get
		{
			return keyID;
		}
	}

	public string key
	{
		get
		{
			return keyString;
		}
	}
	string keyID;
	string keyString;

	public Key(string id, string key)
	{
		keyID = id;
		keyString = key;
	}
}
