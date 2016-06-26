using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {
	
	public delegate void MessageEventDelegate(object[] args);

	public static bool debug = false;

	private static Dictionary<string, List<MessageEventDelegate>> dic = new Dictionary<string, List<MessageEventDelegate>>();

	public static void InvokeEvent(string name, params object[] args)
	{
		if (debug)
		{
			Debug.Log(name);
		}

		foreach (var item in dic) {
			if (item.Key == name)
			{
				for (int i = 0; i < item.Value.Count; i++) {
					try {
						item.Value[i](args);
					}
					catch (System.Exception) { }
				}
			}
		}
	}

	public static void SubscribeEvent(string name, MessageEventDelegate action)
	{
		if (!dic.ContainsKey(name))
		{
            //throw new System.Exception("Event '" + name + "' hasn't been invoked previously");
            dic.Add(name, new List<MessageEventDelegate>());
        }

		if (!dic[name].Contains(action))
		{
			dic[name].Add(action);
		}
        else
        {
            throw new System.Exception("Event '" + name + "' has already been subscribed to with the specified action.");
        }
	}

	public static void UnsubscribeEvent(string name, MessageEventDelegate action)
	{
		if (dic.ContainsKey(name) && dic[name].Contains(action))
		{
			dic[name].Remove(action);
		}
	}
}