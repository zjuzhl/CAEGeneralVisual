using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class test : MonoBehaviour {

	public Material myModelmat;
	public Material myElemmat;
	public Material myNodemat;

	Mesh testmesh;
	MeshRenderer testrender;
	MeshOperation MO;
	Mesh testmesh50;
	MeshRenderer testrender50;
	MeshOperation MO50;


	Vector3[] NodesDisplay;
	List<int[]> indexsDispaly;

	bool isElemDisplay = false;
	bool isElemDisplay1 = false;
	bool isElemDisplay10 = false;
	bool isElemDisplay50 = false;
	Vector3[] DispDisplay1;
	Vector3[] DispDisplay10;
	Vector3[] DispDisplay50;

	public static int MoveCount = 80;
	public static int NowMoveCount = 80;
	private bool startMove = false;

	public GameObject NodeRoot;
	public GameObject ElementRoot;

	public GameObject Plane1;//必须有mesh//用于构建mesh
	public GameObject Plane2;//必须有mesh//用于构建mesh
	public GameObject ModelPlane;//必须有mesh//用于构建mesh
	public GameObject ModelPlane50;//必须有mesh//用于构建mesh

	// Use this for initialization
	void Start () {

		AsNodeData.oNodeData = TextFileOperation.Read ( Application.dataPath + "\\Resources\\ansys\\Nodes.lis");
		AsNodeData.fNodeData = AsNodeData.ExtractNodeData (AsNodeData.oNodeData);
		AsNodeData.ANSNodeData = AsNodeData.GetNodeData (AsNodeData.fNodeData);
		AsNodeData.UniNodeData = AsNodeData.AsToUniPositons (AsNodeData.ANSNodeData);

//		for (int i = 0; i < AsNodeData.vNodeData.Length; i++) {
//			Debug.Log (i + " : " + AsNodeData.vNodeData[i].x + " , " + AsNodeData.vNodeData[i].y + " , " + AsNodeData.vNodeData[i].z);
//		}
//

		AsElementData.oElemData = TextFileOperation.Read (Application.dataPath + "\\Resources\\ansys\\Elements.lis");
		AsElementData.fElemData = AsElementData.ExtractElemData (AsElementData.oElemData);
		AsElementData.ANSElemData = AsElementData.GetElemData (AsElementData.fElemData);

//		for (int i = 0; i < AsElementData.iElemData.Length; i++) {
//			Debug.Log (i + " : " + AsElementData.iElemData[i][0] + " , " + AsElementData.iElemData[i][1] + " , " 
//				+ AsElementData.iElemData[i][2] + " , " + AsElementData.iElemData[i][3]);
//		}

		AsDisplaceData.vDispData = Oiginaldata.DISPLACEMENT;

		ModelPlane.GetComponent<MeshRenderer> ().enabled = true;
		testmesh = ModelPlane.GetComponent<MeshFilter>().mesh;
		testmesh.Clear ();
		MO = new MeshOperation (testmesh);
		testrender = ModelPlane.GetComponent<MeshRenderer> ();
		testrender.sharedMaterial = myModelmat;

		ModelPlane50.GetComponent<MeshRenderer> ().enabled = true;
		testmesh50 = ModelPlane50.GetComponent<MeshFilter>().mesh;
		testmesh50.Clear ();
		MO50 = new MeshOperation (testmesh50);
		testrender50 = ModelPlane50.GetComponent<MeshRenderer> ();
		testrender50.sharedMaterial = myModelmat;

		NonShowElementLine ();
	}
	
	public void NonShowElementLine()
	{
		isElemDisplay = false;
		isElemDisplay1 = false;
		isElemDisplay50 = false;
		isElemDisplay10 = false;
	}

	Vector3[] ReConstructVer(Vector3[] verti, Vector3[] disp, int scale)
	{
		Vector3[] vec = new Vector3[verti.Length];
		for (int i = 0; i < verti.Length; i++) {
			vec [i] = verti [i] + disp [i] * scale;
		}
		return vec;
	}

	void FixedUpdate()
	{
		if (startMove) 
		{
			if (NowMoveCount < MoveCount) 
			{
				NowMoveCount++;
				BeforeOperationOn();
				YingOperationOn();

				AsFiniteRender.ElemMater = myElemmat;
				NodesDisplay = AsFiniteRender.WorldToScreenPos (AsFiniteRender.AsToUniPos (AsNodeData.ANSNodeData), new Vector3(0,0,3));
				DispDisplay50 = AsFiniteRender.WorldToScreenPos (AsFiniteRender.AsToUniPos (ReConstructVer (AsNodeData.ANSNodeData, AsDisplaceData.vDispData, NowMoveCount)), new Vector3 (22, 0, 3));
				indexsDispaly = AsFiniteRender.ElementsRenderIndex (AsElementData.ANSElemData);
			} else {
				startMove = false;
			}

		}
	}


	void OnGUI()
	{
		if (GUI.Button (new Rect (Screen.width - 160, 260, 160, 40), "动画")) {
			startMove = !startMove;
			if (!startMove) {
				NowMoveCount = 0;
			} else {
				NowMoveCount = 0;
				BeforeOperationOn ();
				YingOperationOn ();
				isElemDisplay1 = false;
				isElemDisplay10 = false;
			}
		}

		if (GUI.Button (new Rect (Screen.width - 160, 60, 160, 40), "原始模型")) 
		{
			isElemDisplay = !isElemDisplay;

			if (isElemDisplay) {
				BeforeOperationOff();
				BeforeOperationOn();
				AsFiniteRender.ElemMater = myElemmat;
				NodesDisplay = AsFiniteRender.WorldToScreenPos (AsFiniteRender.AsToUniPos (AsNodeData.ANSNodeData), new Vector3(0,0,3));
				indexsDispaly = AsFiniteRender.ElementsRenderIndex (AsElementData.ANSElemData);
			} else {
				NodesDisplay = new Vector3[]{ };
			}
		}
		if (GUI.Button (new Rect (Screen.width - 160, 110, 160, 40), "应变分析")) 
		{
			isElemDisplay50 = !isElemDisplay50;

			if (isElemDisplay50) {
				YingOperationOff();
				YingOperationOn();
				AsFiniteRender.ElemMater = myElemmat;
				DispDisplay50 = AsFiniteRender.WorldToScreenPos (AsFiniteRender.AsToUniPos (ReConstructVer (AsNodeData.ANSNodeData, AsDisplaceData.vDispData, NowMoveCount)), new Vector3 (22, 0, 3));
				indexsDispaly = AsFiniteRender.ElementsRenderIndex (AsElementData.ANSElemData);
			} else {
				DispDisplay50 = new Vector3[]{ };
			}
		}
		if (GUI.Button (new Rect (Screen.width - 160, 160, 160, 40), "节点应力分析（SINT）")) 
		{
			isElemDisplay1 = !isElemDisplay1;

			if (isElemDisplay1) {
				NodeOperationOff();
				NodeOperationOn();
				AsFiniteRender.ElemMater = myElemmat;
				DispDisplay1 = AsFiniteRender.WorldToScreenPos (AsFiniteRender.AsToUniPos (ReConstructVer (AsNodeData.ANSNodeData, AsDisplaceData.vDispData, NowMoveCount)), new Vector3 (0, 0, -22));
				indexsDispaly = AsFiniteRender.ElementsRenderIndex (AsElementData.ANSElemData);
			} else {
				DispDisplay1 = new Vector3[]{ };
			}
		}
		if (GUI.Button (new Rect (Screen.width - 160, 210, 160, 40), "单元应力分析（SINT）")) 
		{	
			isElemDisplay10 = !isElemDisplay10;

			if (isElemDisplay10) 
			{
				ElemOperationOff();
				ElemOperationOn();
				AsFiniteRender.ElemMater = myElemmat;
				DispDisplay10 = AsFiniteRender.WorldToScreenPos (AsFiniteRender.AsToUniPos (ReConstructVer (AsNodeData.ANSNodeData, AsDisplaceData.vDispData, NowMoveCount)), new Vector3 (22, 0, -22));
				indexsDispaly = AsFiniteRender.ElementsRenderIndex (AsElementData.ANSElemData);
			} else {
				DispDisplay10 = new Vector3[]{ };
			}
		}
	}

	void BeforeOperationOn()
	{
		MO.ReSetVetices (AsNodeData.ANSNodeData, true);
		MO.ReConstructTriangles (AsElementData.ANSElemData, true);//不确定单元类型
		testmesh.RecalculateNormals ();
		testmesh.RecalculateBounds ();

		Vector2[] uvs = new Vector2[AsNodeData.UniNodeData.Length];
		for(int i = 0; i < uvs.Length; i ++)
		{
			uvs [i] = new Vector2 (AsNodeData.UniNodeData[i].x/20, AsNodeData.UniNodeData[i].z/20);
		}
		testmesh.uv = uvs;
	}

	void BeforeOperationOff()
	{
		testmesh.Clear ();
	}

	void YingOperationOn()
	{
		MO50.ReSetVetices (ReConstructVer (AsNodeData.ANSNodeData, AsDisplaceData.vDispData, NowMoveCount), true);
		ElementType et = ElementType.Square;
		MO50.ReConstructTriangles (AsElementData.ANSElemData, et, true);
		testmesh50.RecalculateNormals ();
		testmesh50.RecalculateBounds ();
	}

	void YingOperationOff()
	{
		testmesh50.Clear ();
	}

	void NodeOperationOn()
	{
		this.GetComponent<color_sample>().NodeSolution(NodeRoot, Plane1, myNodemat);
	}

	void NodeOperationOff()
	{
		try{
			GameObject.DestroyImmediate (GameObject.Find("Plane10"));
			GameObject.DestroyImmediate (GameObject.Find("Plane11"));
			GameObject.DestroyImmediate (GameObject.Find("Plane12"));
			GameObject.DestroyImmediate (GameObject.Find("Plane13"));
			GameObject.DestroyImmediate (GameObject.Find("Plane14"));
			GameObject.DestroyImmediate (GameObject.Find("Plane15"));
			GameObject.DestroyImmediate (GameObject.Find("Plane16"));
			GameObject.DestroyImmediate (GameObject.Find("Plane17"));
			GameObject.DestroyImmediate (GameObject.Find("Plane18"));
		}catch{

		}
	}

	void ElemOperationOn()
	{
		new_date_input ndi = new new_date_input ();
		data _DATA = new data();
		Color [] c_L = new Color[]{};//设置色带
		ndi.ElemSoloution (ElementRoot, Plane2, _DATA.vecs, _DATA._sln, _DATA.index, 4, c_L);
	}

	void ElemOperationOff()
	{
		try{
			GameObject.DestroyImmediate (GameObject.Find("Plane20"));
			GameObject.DestroyImmediate (GameObject.Find("Plane21"));
			GameObject.DestroyImmediate (GameObject.Find("Plane22"));
			GameObject.DestroyImmediate (GameObject.Find("Plane23"));
			GameObject.DestroyImmediate (GameObject.Find("Plane24"));
			GameObject.DestroyImmediate (GameObject.Find("Plane25"));
			GameObject.DestroyImmediate (GameObject.Find("Plane26"));
			GameObject.DestroyImmediate (GameObject.Find("Plane27"));
			GameObject.DestroyImmediate (GameObject.Find("Plane28"));				
		}catch{

		}
	}

	void OnPostRender()
	{
		if (isElemDisplay) {
			AsFiniteRender.DrawElementsLines (NodesDisplay, indexsDispaly, myElemmat);
		}
		if (isElemDisplay1) {
			AsFiniteRender.DrawElementsLines (DispDisplay1, indexsDispaly, myElemmat);
		}
		if (isElemDisplay10) {
			AsFiniteRender.DrawElementsLines (DispDisplay10, indexsDispaly, myElemmat);
		}
		if (isElemDisplay50) {
			AsFiniteRender.DrawElementsLines (DispDisplay50, indexsDispaly, myElemmat);
		}
	}


}
