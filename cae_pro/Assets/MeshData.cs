using UnityEngine;
using System.Collections;

public class  keyline: MeshData
{
	public Vector3 lstart;
	public Vector3 lend;
}


public enum ElementType{
	Unkown = 0,
	Triangles,
	Square
}

public class Element: MeshData
{
	
}

public class ElementTri: Element
{
	public Vector3 lstart;
	public Vector3 lstop;
	public Vector3 lend;
	public void test(){
		Debug.Log ("tri");
	}
}

public class ElementSquare: Element
{
	public Vector3 lstart;
	public Vector3 lstop0;
	public Vector3 lstop1;
	public Vector3 lend;
	public void test(){
		Debug.Log ("square");
	}
}


public class MeshData  
{

	public ElementType MyEt;


}
