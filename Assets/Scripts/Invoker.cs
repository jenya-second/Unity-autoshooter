using UnityEngine;
using System.Collections.Generic;


public delegate void Invokable();

public class Invoker : MonoBehaviour {
	private struct InvokableItem
	{
		public Invokable func;
		public float executeAtTime;
		public static Invoker _instance = null;

		public InvokableItem(Invokable func, float delaySeconds)
		{
			this.func = func;

			if(Time.time == 0) 
				this.executeAtTime = delaySeconds;
			else
				this.executeAtTime = Time.realtimeSinceStartup + delaySeconds;
			
		}
	}
	
	private static Invoker _instance = null;
	public static Invoker Instance
	{
		get
		{
			if( _instance == null )
			{
				GameObject go = new GameObject();
				go.AddComponent<Invoker>();
				go.name = "_FunoniumInvoker";
				_instance = go.GetComponent<Invoker>();
			}
			return _instance;
		}
	}

	float fRealTimeLastFrame = 0;
	float fRealDeltaTime = 0;

	List<InvokableItem> invokeList = new List<InvokableItem>();
	List<InvokableItem> invokeListPendingAddition = new List<InvokableItem>();
	List<InvokableItem> invokeListExecuted = new List<InvokableItem>();
	
	public float RealDeltaTime()
	{
		return fRealDeltaTime;	
	}
	/// <summary>
	/// Invokes the function with a time delay.  This is NOT 
	/// affected by timeScale like the Invoke function in Unity.
	/// </summary>
	/// <param name='func'>
	/// Function to invoke
	/// </param>
	/// <param name='delaySeconds'>
	/// Delay in seconds.
	/// </param>
	public static void InvokeDelayed(Invokable func, float delaySeconds)
	{
		Instance.invokeListPendingAddition.Add(new InvokableItem(func, delaySeconds));
	}
	
	// must be maanually called from a game controller or something similar every frame
	public void Update()
	{
		fRealDeltaTime = Time.realtimeSinceStartup - fRealTimeLastFrame;
		fRealTimeLastFrame = Time.realtimeSinceStartup;

		foreach(InvokableItem item in invokeListPendingAddition)
		{
			invokeList.Add(item);	
		}
		invokeListPendingAddition.Clear();
		

		foreach(InvokableItem item in invokeList)
		{
			if(item.executeAtTime <= Time.realtimeSinceStartup)	
			{
				if(item.func != null)
					item.func();

				invokeListExecuted.Add(item);
			}
		}
		
		foreach(InvokableItem item in invokeListExecuted)
		{
			invokeList.Remove(item);
		}
		invokeListExecuted.Clear();
	}
}