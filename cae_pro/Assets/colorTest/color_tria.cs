using UnityEngine;
using System.Collections;

public class color_tria  {

	public Vector3 v1;
	public Vector3 v2;
	public Vector3 v3;
	public int color_type;
	public color_tria(Vector3 va,Vector3 vb,Vector3 vc,int type)
	{
		v1=va;
		v2=vb;
		v3=vc;
		color_type=type;
	}
}
