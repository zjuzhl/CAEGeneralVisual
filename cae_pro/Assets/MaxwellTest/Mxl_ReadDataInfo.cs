using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;

public class Mxl_ReadDataInfo : MonoBehaviour {

	private string gd_path = "";
	private string gd_path_1 = "";

	public GameObject father;//必须有mesh//用于构建mesh
	public GameObject ob;//必须有mesh//用于构建mesh
	public Material mat;//物体的材质球
	public GameObject Jian_obj;

	public GameObject Xian_obj_1;
	public GameObject father_1;//必须有mesh//用于构建mesh
	public GameObject ob_1;//必须有mesh//用于构建mesh


	public Mxl_YunTu_SeDai mys = new Mxl_YunTu_SeDai ();
	public Mxl_YunTu_Xian myx = new Mxl_YunTu_Xian ();

	void Start()
	{
		GameObject.Find ("mpp").transform.localPosition = new Vector3 (-1000, -1000, -1000);
	}

	public void SceneInitClear()
	{
		mys.ElemClear ();
		myx.ElemClear ();
		DestroyImmediate (GameObject.Find ("Staff_16"));
		DestroyImmediate (GameObject.Find ("Staff_16_1"));
		GameObject.Find ("mpp").transform.localPosition = new Vector3 (-1000, -1000, -1000);
	}

	void OnGUI()
	{
		if (GUI.Button (new Rect (10, 10, 100, 60), "磁感线_云图")) {
			gd_path = UnityEngine.Application.dataPath + @"/Resources/Mxl_Assets/cgx.dsp";
			GeneralData gdd = FirstGetData (gd_path);

			int number = gdd.NodeCoord.Count;
			Vector3[] vecs = new Vector3[number];
			for (int i = 0; i < number; i++) {
				vecs [i] = new Vector3 ((float)gdd.NodeCoord[i][0], (float)gdd.NodeCoord[i][1], (float)gdd.NodeCoord[i][2]);
			}
			int[] indx = new int[gdd.Elem_NodesIndex.Count * 3];
			//			Debug.Log (gdd.Elem_NodesIndex.Count);
			for (int i = 0; i < gdd.Elem_NodesIndex.Count; i++) {
				indx [3 * i + 0] = gdd.Elem_NodesIndex [i] [0]-1;
				indx [3 * i + 1] = gdd.Elem_NodesIndex [i] [1]-1;
				indx [3 * i + 2] = gdd.Elem_NodesIndex [i] [2]-1;
			}
			float[] slns = new float[gdd.nSlnTypeValue [0].Count];
			for (int i = 0; i < slns.Length; i++) {
				slns [i] = (float)gdd.nSlnTypeValue [0] [i];
			}
//			Debug.Log ("testing: " + indx.Length + "  " + slns.Length);
			SceneInitClear ();
			this.GetComponent<Mxl_ReadDataPics> ().ObjectDelete ();
			GameObject.Find ("mpp").transform.localPosition = new Vector3 (-5.3f, 3.6f, 0.1f);
			Color[] c_L = new Color[]{ };
			myx.XianSoloution (father_1, ob_1, vecs, slns, indx, 3, c_L, 0.08f, mat);
			CreatePrefabStaff (Xian_obj_1, "Staff_16_1", gdd.nSlnTypeNames [0], gdd.nSlnMaxValue [0], gdd.nSlnMinValue [0], Color.black);
		}

		if (GUI.Button (new Rect (10, 70, 100, 60), "磁钢_云图")) {
			gd_path = UnityEngine.Application.dataPath + @"/Resources/Mxl_Assets/ct.dsp";
			GeneralData gdd = FirstGetData (gd_path);

			int number = gdd.NodeCoord.Count;
			Vector3[] vecs = new Vector3[number];
			for (int i = 0; i < number; i++) {
				vecs [i] = new Vector3 ((float)gdd.NodeCoord[i][0], (float)gdd.NodeCoord[i][1], (float)gdd.NodeCoord[i][2]);
			}
			int[] indx = new int[gdd.Elem_NodesIndex.Count * 3];
//			Debug.Log (gdd.Elem_NodesIndex.Count);
			for (int i = 0; i < gdd.Elem_NodesIndex.Count; i++) {
				indx [3 * i + 0] = gdd.Elem_NodesIndex [i] [0]-1;
				indx [3 * i + 1] = gdd.Elem_NodesIndex [i] [1]-1;
				indx [3 * i + 2] = gdd.Elem_NodesIndex [i] [2]-1;
			}
			float[] slns = new float[gdd.nSlnTypeValue [0].Count];
			for (int i = 0; i < slns.Length; i++) {
				slns [i] = (float)gdd.nSlnTypeValue [0] [i];
			}
//			Debug.Log ("testing: " + indx.Length + "  " + slns.Length);
			SceneInitClear ();
			Color[] c_L = new Color[]{ };
//			DestroyImmediate (GameObject.Find ("LineGraph"));
			this.GetComponent<Mxl_ReadDataPics> ().ObjectDelete ();
			mys.ElemSoloution (father, ob, vecs, slns, indx, 3, c_L, mat);
			CreatePrefabStaff (Jian_obj, "Staff_16", gdd.nSlnTypeNames [0], gdd.nSlnMaxValue [0], gdd.nSlnMinValue [0], Color.black);
		}
	}
		
	/// <summary>
	/// 获得有限元导出的数据，存储在数据结构中
	///读取条件：
	///1.三维模型
	///2.导出的原始文件（重新保存后格式有变化）
	///3.物理量为标量
	/// </summary>
	/// <returns>保存有分析数据的数据结构.</returns>
	/// <param name="path">有限元导出数据的文件路径.</param>
	GeneralData FirstGetData(string path)
	{
		if (File.Exists (path)) {
			GeneralData gddd = new GeneralData ();
			//数据流的方式读取
			StreamReader sr = new StreamReader (path, System.Text.Encoding.ASCII);
			string line = "";
			string[] ary = new string[]{ };

			line = sr.ReadLine ();
			ary = RemoveComma (line, ", ");
			while (!ary [0].Contains( "PlotName" )) {
				line = sr.ReadLine ();
				ary = RemoveComma (line, ", ");
			}
			ary = RemoveComma (line, "'");
			string PlotName = ary [1];

			line = sr.ReadLine ();
			ary = RemoveComma (line, ", ");
			while (!ary [0].Contains( "PlotFolder" )) {
				line = sr.ReadLine ();
				ary = RemoveComma (line, ", ");
			}
			ary = RemoveComma (line, "'");
			List<string> names = new List<string> ();
			names.Add (ary[1] + ": '" + PlotName + "'");
			gddd.nSlnTypeNames = names;
			Debug.Log ("物理量名称： " + gddd.nSlnTypeNames[0]);

			line = sr.ReadLine ();
			ary = RemoveComma (line, ", ");
			while (!ary [0].Contains( "Elements" )) {
				line = sr.ReadLine ();
				ary = RemoveComma (line, ", ");
			}
			ary = RemoveComma (line, ", ");
			if (ary [0].Contains ("Elements")) {
				ary [0] = ary [0].Remove (0, 12);
				Debug.Log ("Elem第一个数据：" + ary[0]);
			}

			if (ary [ary.Length - 1].Contains (")")) {
				ary [ary.Length - 1] = ary [ary.Length - 1].Remove (ary [ary.Length - 1].Length - 1, 1);
				Debug.Log ("Elem最后一个数据： " + ary[ary.Length-1]);
			}
			Debug.Log ("Elem总个数： " + ary.Length);

			List<string> ttt = new List<string>();



			//剔除23306
			for (int i = 0; i < ary.Length; i++) {
				if (i + 4 < ary.Length) {
					if (ary [i].Equals ("2") && ary [i + 1].Equals ("3") && ary [i + 2].Equals ("3") && ary [i + 3].Equals ("0") && ary [i + 4].Equals ("6")) {
						i += 4;
					} else {
						ttt.Add (ary[i]);
					}
				} else {
					ttt.Add (ary[i]);
				}

			}
			//转到单元解得处理


			line = sr.ReadLine ();
			ary = RemoveComma (line, ", ");
			while (!ary [0].Contains( "Nodes" )) {
				line = sr.ReadLine ();
				ary = RemoveComma (line, ", ");
			}
			ary = RemoveComma (line, ", ");
			if (ary [0].Contains ("Nodes")) {
				ary [0] = ary [0].Remove (0, 9);
				Debug.Log ("Node第一个数据： " + ary[0]);
			}
			if (ary [ary.Length - 1].Contains (")")) {
				ary [ary.Length - 1] = ary [ary.Length - 1].Remove (ary [ary.Length - 1].Length - 1, 1);
				Debug.Log ("Node最后一个数据： " + ary[ary.Length-1]);
			}
			Debug.Log ("Node总个数： " + ary.Length/3);

			List<double[]> ddd = new List<double[]> ();
			for (int i = 0; i < ary.Length; i = i + 3) {
				ddd.Add (new double[]{ double.Parse (ary [i]), double.Parse (ary [i + 1]), double.Parse (ary [i + 2])});
			}
//			Debug.Log (ddd[226][0] + "  "  + ddd[226][1] + "  " + ddd[226][2]);
//			Debug.Log (ddd[7310][0] + "  "  + ddd[7310][1] + "  " + ddd[7310][2]);
//			Debug.Log (ddd[828][0] + "  "  + ddd[828][1] + "  " + ddd[828][2]);
//			Debug.Log (ddd[7309][0] + "  "  + ddd[7309][1] + "  " + ddd[7309][2]);
//			Debug.Log (ddd[9320][0] + "  "  + ddd[9320][1] + "  " + ddd[9320][2]);
//			Debug.Log (ddd[827][0] + "  "  + ddd[827][1] + "  " + ddd[827][2]);
//
//			Debug.Log (ddd[914][0] + "  "  + ddd[914][1] + "  " + ddd[914][2]);
//			Debug.Log (ddd[9486][0] + "  "  + ddd[9486][1] + "  " + ddd[9486][2]);
			gddd.NodeCoord = ddd;

			line = sr.ReadLine ();
			ary = RemoveComma (line, ", ");
			while (!ary [0].Contains( "ElemSolution" )) {
				line = sr.ReadLine ();
				ary = RemoveComma (line, ", ");
			}
			ary = RemoveComma (line, ", ");
			if (ary [0].Contains ("ElemSolution")) {
				ary [0] = ary [0].Remove (0, 16);
				Debug.Log ("ElemSolution第一个数据： " + ary[0]);
			}
			if (ary [ary.Length - 1].Contains (")")) {
				ary [ary.Length - 1] = ary [ary.Length - 1].Remove (ary [ary.Length - 1].Length - 1, 1);
				Debug.Log ("ElemSolution最后一个数据： " + ary[ary.Length-1]);
			}
			Debug.Log ("ElemSolution总个数： " + ary.Length);
			gddd.nSlnMaxValue = new double[1];
			gddd.nSlnMinValue = new double[1];
			gddd.nSlnTypeValue = new List<double>[1];

			gddd.nSlnMinValue [0] = double.Parse(ary[0]);
			gddd.nSlnMaxValue [0] = double.Parse(ary[1]);

			List<int[]> nodesIdx = new List<int[]> ();
			List<double> rst = new List<double> {};

			int[] tean = new int[3];
			Debug.Log ("jiansuo: " + (ttt.Count - 2));
			for (int i = 2; i < ttt.Count; i = i + 6) 
			{
				tean = new int[]{int.Parse(ttt[i]), int.Parse(ttt[i + 1]), int.Parse(ttt[i + 2]), int.Parse(ttt[i + 3]), int.Parse(ttt[i + 4]), int.Parse(ttt[i + 5])};
				nodesIdx.Add (new int[]{tean[0], tean[3], tean[1]});
				nodesIdx.Add (new int[]{tean[1], tean[4], tean[2]});
				nodesIdx.Add (new int[]{tean[3], tean[4], tean[1]});
				nodesIdx.Add (new int[]{tean[3], tean[5], tean[4]});
			}
			gddd.Elem_NodesIndex = nodesIdx;
			Debug.Log ("jiansuo: " + (ary.Length - 3));
			for (int i = 3; i < ary.Length; i = i + 6) 
			{
				if (i + 5 < ary.Length) {
					rst.Add (double.Parse(ary [i + 0]));
					rst.Add (double.Parse(ary [i + 3]));
					rst.Add (double.Parse(ary [i + 1]));

					rst.Add (double.Parse(ary [i + 1]));
					rst.Add (double.Parse(ary [i + 4]));
					rst.Add (double.Parse(ary [i + 2]));

					rst.Add (double.Parse(ary [i + 3]));
					rst.Add (double.Parse(ary [i + 4]));
					rst.Add (double.Parse(ary [i + 1]));

					rst.Add (double.Parse(ary [i + 3]));
					rst.Add (double.Parse(ary [i + 5]));
					rst.Add (double.Parse(ary [i + 4]));
				} else {
					Debug.Log ("youwu");
				}
			}
			gddd.nSlnTypeValue [0] = rst;
			return gddd;
		} else {

			return null;
		}
	}
	
	string[] RemoveComma(string str, string lit)
    {
        if (str == "" || str == null)
        {
			string[] st = new string[]{""};
            return st;
        }
		return Regex.Split(str, lit, RegexOptions.IgnoreCase);
    }

	/// <summary>
	/// 创建显示物理量标尺的显示
	/// 1.根据最大值和最小值得差值均分成七个区间
	/// 2.物理量精确到小数点后面三位
	/// 3.颜色渐变规则为HSV(0，1,1)到HSV(2/3,1,1)(直接通过贴图实现)
	/// </summary>
	/// <param name="Objname">标尺的名称.</param>
	/// <param name="Typename">物理量名称.</param>
	/// <param name="max">最大值.</param>
	/// <param name="min">最小值.</param>
	void CreatePrefabStaff(GameObject prefab, string Objname, string Typename, double max, double min, Color col)
	{
//		GameObject Biaochi = (GameObject)Resources.Load ("Mxl_Assets/Staff_16");
//		GameObject Biaochi = 
		GameObject Target = GameObject.Find (Objname);
		if (Target == null) {
			Target = Instantiate (prefab);
			Target.name = Objname;
			Target.transform.parent = GameObject.Find ("Canvas").transform;
			Target.transform.localPosition = new Vector3 (345, 0, 0);
		}
		GameObject tf0 = GameObject.Find ("Text (0)");
		if (tf0.transform.parent == Target.transform) {
			tf0.GetComponent<Text> ().text = Typename;
			tf0.GetComponent<Text> ().color = col;
		}

		//计算标值
		float step = ((float)max - (float)min) / 16;
		string[] values = new string[17];
		for (int i = 15; i >= 0; i--) {
			values [i] = (max - step * i).ToString ("#.####e+000");
			int j = i + 1;
			GameObject tf = GameObject.Find ("Text (" + j + ")");
			if (tf.transform.parent == Target.transform) {
				tf.GetComponent<Text> ().text = values [i];
				tf.GetComponent<Text> ().color = col;
			}
		}
		values [0] = min.ToString ("#.####e+000");
		GameObject tf17 = GameObject.Find ("Text (17)");
		if (tf17.transform.parent == Target.transform) {
			tf17.GetComponent<Text> ().text = values [0];
			tf17.GetComponent<Text> ().color = col;
		}
	}
}
