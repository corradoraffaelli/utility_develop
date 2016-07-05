using UnityEngine;
using System.Collections;


//using Android.App;
//using Android.Content;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using Android.OS;

[ExecuteInEditMode]
public class ChangeSortingLayer : MonoBehaviour {

	public string sortingLayerString;

	public int sortingLayerOrder;

	public bool applyToChildren = false;

	public int indexString = 0;



	public void Change()
	{
		if (applyToChildren)
		{
			Renderer[] childRend = GetComponentsInChildren<Renderer>(true);
			
			for (int i = 0; i < childRend.Length; i++)
			{
				if (childRend[i] != null)
				{
					childRend[i].sortingLayerName = sortingLayerString;
					childRend[i].sortingOrder = sortingLayerOrder;
				}
			}
		}
		else
		{
			Renderer[] rend = GetComponents<Renderer>();
			
			for (int i = 0; i < rend.Length; i++)
			{
				if (rend[i] != null)
				{
					rend[i].sortingLayerName = sortingLayerString;
					rend[i].sortingOrder = sortingLayerOrder;
				}
			}
		}
	}
}
