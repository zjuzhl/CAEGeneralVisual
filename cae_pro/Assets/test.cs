using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class test : MonoBehaviour {

	public Material my;
	public Material myElem;
	public Material MyNodeSolutionMat;

	Mesh testmesh;
	MeshRenderer testrender;
	MeshOperation MO;
	Mesh testmesh50;
	MeshRenderer testrender50;
	MeshOperation MO50;

	bool isModelDisplay = false;
	bool isModelDisplay50 = false;
	bool isElemDisplay = false;
	bool isNodeDisplay = false;
	Vector3[] NodesDisplay;
	List<int[]> indexsDispaly;
//	List<int[]> indexsDispaly50;

	bool isElemDisplay1 = false;
	bool isElemDisplay10 = false;
	bool isElemDisplay50 = false;
	[HideInInspector]
	public Vector3[] DispDisplay1;
	[HideInInspector]
	public Vector3[] DispDisplay10;
	[HideInInspector]
	public Vector3[] DispDisplay50;

	Vector3[] tt;

	bool ShowNodeSolution = false;
	bool ShowElemSolution = false;

	public Texture2D t2dBestView;

	public Texture2D t2d;
	public Texture2D t2dHelp;
	public Texture2D t2dHelp_1;
	CameraControl cc;

	public static int MoveCount = 80;
	public static int NowMoveCount = 80;
	private bool startMove = false;
	private bool Moveing = false;

	public GameObject father;//必须有mesh//用于构建mesh
	public GameObject ob;//必须有mesh//用于构建mesh

	// Use this for initialization
	void Start () {
//		BinaryOperation.BinaryTest ();
		cc = new CameraControl ();
		t2d = t2dHelp;

		AsNodeData.oNodeData = TextFileOperation.Read ( Application.dataPath + "\\Resources\\Nodes.lis");
		AsNodeData.fNodeData = AsNodeData.ExtractNodeData (AsNodeData.oNodeData);
		AsNodeData.ANSNodeData = AsNodeData.GetNodeData (AsNodeData.fNodeData);
		AsNodeData.UniNodeData = AsNodeData.AsToUniPositons (AsNodeData.ANSNodeData);

//		for (int i = 0; i < AsNodeData.vNodeData.Length; i++) {
//			Debug.Log (i + " : " + AsNodeData.vNodeData[i].x + " , " + AsNodeData.vNodeData[i].y + " , " + AsNodeData.vNodeData[i].z);
//		}
//
//		Debug.Log(Application.dataPath);

		AsElementData.oElemData = TextFileOperation.Read (Application.dataPath + "\\Resources\\Elements.lis");
		AsElementData.fElemData = AsElementData.ExtractElemData (AsElementData.oElemData);
		AsElementData.ANSElemData = AsElementData.GetElemData (AsElementData.fElemData);

//		for (int i = 0; i < AsElementData.iElemData.Length; i++) {
//			Debug.Log (i + " : " + AsElementData.iElemData[i][0] + " , " + AsElementData.iElemData[i][1] + " , " 
//				+ AsElementData.iElemData[i][2] + " , " + AsElementData.iElemData[i][3]);
//		}

//		AsDisplaceData.oDispData = TextFileOperation.Read (Application.dataPath + "\\Resources\\Displacements.lis");
//		AsDisplaceData.fDispData = AsDisplaceData.ExtractDispData (AsDisplaceData.oDispData);
//		Debug.Log (AsDisplaceData.fDispData.Count);
//		Debug.Log (AsDisplaceData.sDispData.Length);
//		Debug.Log (AsDisplaceData.MaxDispNodes.Length);
//		Debug.Log (AsDisplaceData.MaxDispValue.Length);
//		AsDisplaceData.GetDispData (AsDisplaceData.fDispData, out AsDisplaceData.sDispData, out AsDisplaceData.MaxDispNodes, out AsDisplaceData.MaxDispValue);
//
//		AsDisplaceData.vDispData = AsDisplaceData.DefineDispData (AsDisplaceData.sDispData);
//		AsDisplaceData.SumDispData = AsDisplaceData.DefineSumDispData (AsDisplaceData.sDispData);

		AsDisplaceData.vDispData = Oiginaldata.DISPLACEMENT;

//		AsDisplaceData.ReFormat ("-0.43467E-02-0.12638E-03");

		GameObject.Find ("Plane").GetComponent<MeshRenderer> ().enabled = true;
		testmesh = GameObject.Find ("Plane").GetComponent<MeshFilter>().mesh;
		testmesh.Clear ();
		MO = new MeshOperation (testmesh);
		testrender = GameObject.Find ("Plane").GetComponent<MeshRenderer> ();
		if (my != null) {
			testrender.sharedMaterial = my;
		}
		GameObject.Find ("Plane50").GetComponent<MeshRenderer> ().enabled = true;
		testmesh50 = GameObject.Find ("Plane50").GetComponent<MeshFilter>().mesh;
		testmesh50.Clear ();
		MO50 = new MeshOperation (testmesh50);
		testrender50 = GameObject.Find ("Plane50").GetComponent<MeshRenderer> ();
		if (my != null) {
			testrender50.sharedMaterial = my;
		}

		for(int i = 0; i < testmesh.uv.Length; i ++)
		{
			Debug.Log(testmesh.uv[i].x + "  " + testmesh.uv[i].y);
		}


		NonShowElementLine ();

		GameObject.Find ("Canvas/Text1").GetComponent<Text>().enabled = true;
		GameObject.Find ("Canvas/Text2").GetComponent<Text>().enabled = true;
		GameObject.Find ("Canvas/Text3").GetComponent<Text>().enabled = true;
		GameObject.Find ("Canvas/Text4").GetComponent<Text>().enabled = true;
	}
	
	public void NonShowElementLine(){
		isElemDisplay = false;
		isElemDisplay1 = false;
		isElemDisplay50 = false;
		isElemDisplay10 = false;
		try{
			GameObject.Find ("Canvas/Text1").GetComponent<Text>().enabled = false;
			GameObject.Find ("Canvas/Text2").GetComponent<Text>().enabled = false;
			GameObject.Find ("Canvas/Text3").GetComponent<Text>().enabled = false;
			GameObject.Find ("Canvas/Text4").GetComponent<Text>().enabled = false;
		}catch{
			
		}

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
		if (startMove) {
			if (NowMoveCount < 80) {
				NowMoveCount++;
				BeforeOperationOn ();
				YingOperationOn ();

				AsFiniteRender.ElemMater = myElem;
				NodesDisplay = AsFiniteRender.WorldToScreenPos (AsFiniteRender.AsToUniPos (AsNodeData.ANSNodeData), new Vector3(0,0,3));
				DispDisplay50 = AsFiniteRender.WorldToScreenPos (AsFiniteRender.AsToUniPos (ReConstructVer (AsNodeData.ANSNodeData, AsDisplaceData.vDispData, NowMoveCount)), new Vector3 (22, 0, 3));
				indexsDispaly = AsFiniteRender.ElementsRenderIndex (AsElementData.ANSElemData);
				Moveing = true;
			} else {
				startMove = false;
				Moveing = false;
			}

		}
	}


	void OnGUI()
	{
		if (GUI.Button (new Rect (Screen.width - 80, 260, 54, 66), t2dBestView)) {
			cc.SetBestView ();
			AsFiniteRender.ElemMater = myElem;
			if (isElemDisplay) {
				NodesDisplay = AsFiniteRender.WorldToScreenPos (AsFiniteRender.AsToUniPos (AsNodeData.ANSNodeData), new Vector3(0,0,3));
			}
			if (isElemDisplay50) {
				DispDisplay50 = AsFiniteRender.WorldToScreenPos (AsFiniteRender.AsToUniPos (ReConstructVer (AsNodeData.ANSNodeData, AsDisplaceData.vDispData, NowMoveCount)), new Vector3 (22, 0, 3));
			}
			if (isElemDisplay1) {
				DispDisplay1 = AsFiniteRender.WorldToScreenPos (AsFiniteRender.AsToUniPos (ReConstructVer (AsNodeData.ANSNodeData, AsDisplaceData.vDispData, NowMoveCount)), new Vector3 (0, 0, -22));
			}
			if (isElemDisplay10) {
				DispDisplay10 = AsFiniteRender.WorldToScreenPos (AsFiniteRender.AsToUniPos (ReConstructVer (AsNodeData.ANSNodeData, AsDisplaceData.vDispData, NowMoveCount)), new Vector3 (22, 0, -22));
			}
			indexsDispaly = AsFiniteRender.ElementsRenderIndex (AsElementData.ANSElemData);
		}

		if (GUI.Button (new Rect (Screen.width - 160, 260, 54, 66), t2dHelp)) {
			startMove = !startMove;
			if (!startMove) {
				NowMoveCount = 0;
				if (Moveing) {
					BeforeOperationOff ();
					YingOperationOff ();
				}
			} else {
				NowMoveCount = 0;
				BeforeOperationOn ();
				YingOperationOn ();
				ShowNodeSolution = false;
				ShowElemSolution = false;
				isElemDisplay1 = false;
				isElemDisplay10 = false;
				NodeOperationOff ();
				ElemOperationOff ();
			}
		}

		if (GUI.Button (new Rect (Screen.width - 160, 60, 160, 40), "原始模型")) {
			isModelDisplay = !isModelDisplay;
			isElemDisplay = !isElemDisplay;

			if (isModelDisplay) {
				

				BeforeOperationOn ();

			} else {
				BeforeOperationOff ();
			}


			if (isElemDisplay) {
				AsFiniteRender.ElemMater = myElem;
				NodesDisplay = AsFiniteRender.WorldToScreenPos (AsFiniteRender.AsToUniPos (AsNodeData.ANSNodeData), new Vector3(0,0,3));
				indexsDispaly = AsFiniteRender.ElementsRenderIndex (AsElementData.ANSElemData);
			} else {
				NodesDisplay = new Vector3[]{ };
				//				indexsDispaly.Clear ();
			}

			//			isElemDisplay1 = false;
			//			isElemDisplay50 = false;
			//			isElemDisplay10 = false;
		}
		if (GUI.Button (new Rect (Screen.width - 160, 110, 160, 40), "应变分析")) {
			isModelDisplay50 = !isModelDisplay50;
			isElemDisplay50 = !isElemDisplay50;

			if (isModelDisplay50) {
				
				YingOperationOn ();
			} else {
				YingOperationOff ();
			}



			//			isElemDisplay10 = false;
			//			isElemDisplay1 = false;

			if (isElemDisplay50) {
				AsFiniteRender.ElemMater = myElem;
				DispDisplay50 = AsFiniteRender.WorldToScreenPos (AsFiniteRender.AsToUniPos (ReConstructVer (AsNodeData.ANSNodeData, AsDisplaceData.vDispData, NowMoveCount)), new Vector3 (22, 0, 3));
				indexsDispaly = AsFiniteRender.ElementsRenderIndex (AsElementData.ANSElemData);
			} else {
				DispDisplay50 = new Vector3[]{ };
				//				indexsDispaly.Clear ();
			}
		}
		if (GUI.Button (new Rect (Screen.width - 160, 160, 160, 40), "节点应力分析（SINT）")) {

			ShowNodeSolution = !ShowNodeSolution;

			isElemDisplay1 = !isElemDisplay1;
			if (ShowNodeSolution) {
				NodeOperationOn ();

			} else {
				NodeOperationOff ();
				isElemDisplay1 = false;
			}

			if (isElemDisplay1) {
				AsFiniteRender.ElemMater = myElem;
				DispDisplay1 = AsFiniteRender.WorldToScreenPos (AsFiniteRender.AsToUniPos (ReConstructVer (AsNodeData.ANSNodeData, AsDisplaceData.vDispData, NowMoveCount)), new Vector3 (0, 0, -22));
				indexsDispaly = AsFiniteRender.ElementsRenderIndex (AsElementData.ANSElemData);
			} else {
				DispDisplay1 = new Vector3[]{ };
			}
		}
		if (GUI.Button (new Rect (Screen.width - 160, 210, 160, 40), "单元应力分析（SINT）")) {
			

			ShowElemSolution = !ShowElemSolution;
			isElemDisplay10 = !isElemDisplay10;
			if (ShowElemSolution) {
				ElemOperationOn ();
			} else {
				ElemOperationOff ();
				isElemDisplay10 = false;
			}

			if (isElemDisplay10) {
				AsFiniteRender.ElemMater = myElem;
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
		ElementType et = ElementType.Square;
		MO.ReConstructTriangles (AsElementData.ANSElemData, true);//不确定单元类型
		//			MO.ReConstructTriangles (AsElementData.ANSElemData, et, true);//确定单元类型
		testmesh.RecalculateNormals ();
		testmesh.RecalculateBounds ();

		Vector2[] uvs = new Vector2[AsNodeData.UniNodeData.Length];
		for(int i = 0; i < uvs.Length; i ++)
		{
			uvs [i] = new Vector2 (AsNodeData.UniNodeData[i].x/20, AsNodeData.UniNodeData[i].z/20);
		}
		testmesh.uv = uvs;
//		GameObject.Find ("Canvas/Text3").GetComponent<Text>().enabled = true;
	}

	void BeforeOperationOff()
	{
		isElemDisplay = false;
		testmesh.Clear ();
		GameObject.Find ("Canvas/Text3").GetComponent<Text>().enabled = false;
	}

	void YingOperationOn()
	{
		MO50.ReSetVetices (ReConstructVer (AsNodeData.ANSNodeData, AsDisplaceData.vDispData, NowMoveCount), true);
		ElementType et = ElementType.Square;
		MO50.ReConstructTriangles (AsElementData.ANSElemData, et, true);
		testmesh50.RecalculateNormals ();
		testmesh50.RecalculateBounds ();

//		GameObject.Find ("Canvas/Text4").GetComponent<Text>().enabled = true;
	}

	void YingOperationOff()
	{
		testmesh50.Clear ();
		isElemDisplay50 = false;
		GameObject.Find ("Canvas/Text4").GetComponent<Text>().enabled = false;
	}

	void NodeOperationOn()
	{
		this.GetComponent<color_sample> ().NodeSolution ();
//		GameObject.Find ("Canvas/Text1").GetComponent<Text>().enabled = true;
	}

	void NodeOperationOff()
	{
		try{
			GameObject.Find ("Canvas/Text1").GetComponent<Text>().enabled = false;
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
		ndi.ElemSoloution (father, ob, _DATA.vecs, _DATA._sln, _DATA.index, 4, c_L);
//		GameObject.Find ("Canvas/Text2").GetComponent<Text>().enabled = true;
	}

	void ElemOperationOff()
	{
		try{
			GameObject.Find ("Canvas/Text2").GetComponent<Text>().enabled = false;
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



	void CreateNodesDispaly(Vector3[] dat, string name)
	{
		for (int i = 0; i < dat.Length; i++) {
			GameObject obj = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			obj.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
			obj.transform.position = dat [i];
			obj.name = name + i;
			obj.transform.parent = GameObject.Find ("Sphere").transform; 
			obj.tag = "Node";
		}
	}
	void DestroyNodesDisplay()
	{
		GameObject[] objs = GameObject.FindGameObjectsWithTag ("Node");
		for (int i = 0; i < objs.Length; i++) {
			DestroyImmediate (objs[i]);
		}
	}

	void OnPostRender()
	{
		if (isElemDisplay) {
			AsFiniteRender.DrawElementsLines (NodesDisplay, indexsDispaly, myElem);
		}
		if (isElemDisplay1) {
			AsFiniteRender.DrawElementsLines (DispDisplay1, indexsDispaly, myElem);
		}
		if (isElemDisplay10) {
			AsFiniteRender.DrawElementsLines (DispDisplay10, indexsDispaly, myElem);
		}
		if (isElemDisplay50) {
			AsFiniteRender.DrawElementsLines (DispDisplay50, indexsDispaly, myElem);
		}

//		if (ShowNodeSolution) {
//			AsFiniteRender.DrawElementsLines (DispDisplay1, indexsDispaly, myElem);
//		}
//
//		if (ShowElemSolution) {
//			AsFiniteRender.DrawElementsLines (DispDisplay10, indexsDispaly, myElem);
//		}
	}


}
