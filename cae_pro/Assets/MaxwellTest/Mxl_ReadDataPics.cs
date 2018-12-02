using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;

public class ErWeiData
{
	public string expression;
	public string x_infoname;
	public string y_infoname;
	public List<double> x_data = new List<double>();
	public List<double> y_data = new List<double>();
}

public class Mxl_ReadDataPics : MonoBehaviour {
	public GameObject graphPrefab;
	public Vector2 graphWH;
	public Vector3 graphPos;
	public GameObject seriesPrefab;

	//删除
	public void ObjectDelete(){
		GameObject target = GameObject.Find ("LineGraph");
		if (target) { DestroyImmediate (target); }

		this.GetComponent<Mxl_ReadDataInfo> ().mys.ElemClear ();

		target = GameObject.Find ("Canvas/Staff_16");
		if (target) { DestroyImmediate (target); }
	}

	GameObject ObjectCreate(GameObject obj)
	{
		GameObject target = Instantiate (obj) as GameObject;
		target.name = "LineGraph";
		target.transform.parent = GameObject.Find ("Canvas").transform;
		target.transform.localPosition = graphPos;
		RectTransform rt = target.GetComponent<RectTransform> ();
		rt.sizeDelta = graphWH;
		return target;
	}



	void OnGUI()
	{
		if (GUI.Button (new Rect (10, 130, 100, 60), "功率因数")) {
			string path = UnityEngine.Application.dataPath + @"/Resources/Mxl_Assets/n_cosa.dat";
			List<ErWeiData> ewdd = FirstGetData (path);
			this.GetComponent<Mxl_ReadDataInfo> ().SceneInitClear ();
			DrawLines (ewdd, graphPrefab, new string[]{"功率因数"},new Vector2(0,180), new Vector2(0,1), 19, 
				new List<string> (){ "0", "0.2", "0.4", "0.6", "0.8", "1"}, new string[]{"转矩角(°)", "功率因数"}, new Color[]{Color.red});
		}
		if (GUI.Button (new Rect (10, 190, 100, 60), "输入线电流")) {
			string path = UnityEngine.Application.dataPath + @"/Resources/Mxl_Assets/n_curr.dat";
			List<ErWeiData> ewdd = FirstGetData (path);
			this.GetComponent<Mxl_ReadDataInfo> ().SceneInitClear ();
			DrawLines (ewdd, graphPrefab, new string[]{"输入线电流"},new Vector2(0,180), new Vector2(0,120), 19, 7, new string[]{"转矩角(°)", "输入线电流(A)"}, new Color[]{Color.blue});
		}
		if (GUI.Button (new Rect (10, 250, 100, 60), "效率")) {
			string path = UnityEngine.Application.dataPath + @"/Resources/Mxl_Assets/n_effi.dat";
			List<ErWeiData> ewdd = FirstGetData (path);
			this.GetComponent<Mxl_ReadDataInfo> ().SceneInitClear ();
			DrawLines (ewdd, graphPrefab, new string[]{"效率"},new Vector2(0,180), new Vector2(0,100), 19, 6, new string[]{"转矩角(°)", "效率(%)"}, new Color[]{Color.red});
		}
		if (GUI.Button (new Rect (10, 310, 100, 60), "转矩")) {
			string path = UnityEngine.Application.dataPath + @"/Resources/Mxl_Assets/st_torq.dat";
			List<ErWeiData> ewdd = FirstGetData (path);
			this.GetComponent<Mxl_ReadDataInfo> ().SceneInitClear ();
			DrawLines (ewdd, graphPrefab, new string[]{"感应转矩", "磁转矩", "总转矩"}, 
				new Vector2(0,1), new Vector2(-200,500), new List<string>(){"0", "0.1","0.2", "0.3", "0.4","0.5","0.6","0.7","0.8","0.9","1"}, 
				8, new string[]{"速度", "转矩(N.m)"}, new Color[]{Color.red, Color.yellow, Color.gray});
		}
		if (GUI.Button (new Rect (10, 370, 100, 60), "气隙磁通密度")) {
			string path = UnityEngine.Application.dataPath + @"/Resources/Mxl_Assets/wv0_flux.dat";
			List<ErWeiData> ewdd = FirstGetData (path);
			this.GetComponent<Mxl_ReadDataInfo> ().SceneInitClear ();
			DrawLines (ewdd, graphPrefab, new string[]{"气隙磁通密度"},new Vector2(0,360), new Vector2(-0.75f,0.75f), 19, 
				new List<string>(){"-0.75", "-0.50", "-0.25", "0", "0.25", "0.50", "0.75"}, new string[]{"电度", "磁通(T)"}, new Color[]{Color.green});
		}
		if (GUI.Button (new Rect (10, 430, 100, 60), "电压")) {
			string path = UnityEngine.Application.dataPath + @"/Resources/Mxl_Assets/wv0_volt.dat";
			List<ErWeiData> ewdd = FirstGetData (path);
			this.GetComponent<Mxl_ReadDataInfo> ().SceneInitClear ();
			DrawLines (ewdd, graphPrefab, new string[]{"相电压", "线电压"}, 
				new Vector2(0,360), new Vector2(-600,600), 25, 7, new string[]{"电度", "电压(V)"}, new Color[]{Color.red, Color.green});
		}
	}

	/// <summary>
	/// 绘制曲线
	/// </summary>
	/// <param name="ewdd">数据组（最多三组）.</param>
	/// <param name="graphPrefab">图片UI.</param>
	/// <param name="names">曲线名称.</param>
	/// <param name="xvalues">X轴方向的最小值和最大值.</param>
	/// <param name="yvalues">Y轴方向的最小值和最大值.</param>
	/// <param name="x_labels">X轴方向标尺.</param>
	/// <param name="x_labels">X轴方向标尺.</param>
	/// <param name="labelnames">xy标尺的名称.</param>
	/// <param name="cols">颜色组.</param>
	void DrawLines(List<ErWeiData> ewdd, GameObject graphPrefab, string[] names, Vector2 xvalues, Vector2 yvalues, List<string> x_labels,  List<string> y_labels,string[] labelnames, Color[] cols)
	{
		ObjectDelete ();
		GameObject target = ObjectCreate (graphPrefab);

		WMG_Axis_Graph wmgag = target.GetComponent<WMG_Axis_Graph> ();
		if (wmgag) {
			WMG_Series wmgs = target.GetComponentInChildren<WMG_Series> ();
			List<Vector2> temp = new List<Vector2> ();
			for (int i = 0; i < ewdd[0].x_data.Count; i++) {
				temp.Add (new Vector2 ((float)ewdd[0].x_data [i], (float)ewdd[0].y_data [i]));
			}
			wmgs._pointValues = temp;
			wmgs.seriesName = names[0];
			wmgs.lineColor = cols[0];
			wmgs.pointWidthHeight = 5;
			wmgs.hidePoints = true;
//			Debug.Log ("ewdd" + 0 + ": "+ ewdd [0].x_data.Count);
			if (ewdd.Count > 2) {
				for (int i = 1; i < ewdd.Count; i++) {
					if (ewdd [i].x_data.Count != 0) {
//						Debug.Log ("ewdd" + i + ": "+ ewdd [i].x_data.Count);
						WMG_Series wmgs1 = wmgag.addSeries ();
						List<Vector2> temp1 = new List<Vector2> ();
						for (int j = 0; j < ewdd[i].x_data.Count; j++) {
							temp1.Add (new Vector2 ((float)ewdd[i].x_data [j], (float)ewdd[i].y_data [j]));
						}
						wmgs1.pointValues.SetList (temp1);
						if(wmgs1.pointValues.Count == 0){
							wmgs1._pointValues = temp1;
						}
						wmgs1.seriesName = names[i];
						wmgs1.lineColor = cols[i];

						wmgs1.pointWidthHeight = 5;
						wmgs1.hidePoints = true;
						wmgs1.lineScale = 0.5f;
					}
				}
			}
			wmgag.xAxis.AxisMinValue = xvalues.x;
			wmgag.xAxis.AxisMaxValue = xvalues.y;
			wmgag.xAxis.SetLabelsUsingMaxMin = false;
			wmgag.xAxis.axisLabels.SetList(x_labels);
			if(wmgag.xAxis._axisLabels.Count == 0){
				wmgag.xAxis._axisLabels = x_labels;
			}
			wmgag.xAxis.AxisNumTicks = x_labels.Count;
			wmgag.xAxis.AxisTitleString = labelnames [0];
			wmgag.xAxis.AxisTitleFontSize = 14;

			wmgag.yAxis.AxisMinValue = yvalues.x;
			wmgag.yAxis.AxisMaxValue = yvalues.y;
			wmgag.yAxis.SetLabelsUsingMaxMin = false;
			wmgag.yAxis.axisLabels.SetList(y_labels);
			if(wmgag.yAxis._axisLabels.Count == 0){
				wmgag.yAxis._axisLabels = y_labels;
			}
			wmgag.yAxis.AxisNumTicks = y_labels.Count;
			wmgag.yAxis.AxisTitleString = labelnames [1];
			wmgag.yAxis.AxisTitleFontSize = 14;
		}
	}
	//绘制曲线方法3
	void DrawLines(List<ErWeiData> ewdd, GameObject graphPrefab, string[] names, Vector2 xvalues, Vector2 yvalues, int xnum, List<string> y_labels, string[] labelnames, Color[] cols)
	{
		float diff = (xvalues.y - xvalues.x) / (xnum - 1);
		List<string> x_labels = new List<string> ();
		x_labels.Add (xvalues.x.ToString ());
		for (int i = 1; i < xnum - 1; i++) {
			x_labels.Add ((xvalues.x + i * diff).ToString ());
		}
		x_labels.Add (xvalues.y.ToString ());
		DrawLines (ewdd, graphPrefab, names, xvalues, yvalues, x_labels, y_labels, labelnames, cols);
	}
	//绘制曲线放方法2
	void DrawLines(List<ErWeiData> ewdd, GameObject graphPrefab, string[] names, Vector2 xvalues, Vector2 yvalues, List<string> x_labels, int ynum, string[] labelnames, Color[] cols)
	{
		float diff = (yvalues.y - yvalues.x)/(ynum - 1);
		List<string> y_labels = new List<string> ();
		y_labels.Add (yvalues.x.ToString());
		for (int i = 1; i < ynum-1; i++) {
			y_labels.Add ((yvalues.x + i * diff).ToString ());
		}
		y_labels.Add (yvalues.y.ToString());

		DrawLines (ewdd, graphPrefab, names, xvalues, yvalues, x_labels, y_labels, labelnames, cols);
	}
	//绘制曲线方法1
	void DrawLines(List<ErWeiData> ewdd, GameObject graphPrefab, string[] names, Vector2 xvalues, Vector2 yvalues, int xnum, int ynum, string[] labelnames, Color[] cols)
	{
		float diff = (yvalues.y - yvalues.x)/(ynum - 1);
		List<string> y_labels = new List<string> ();
		y_labels.Add (yvalues.x.ToString());
		for (int i = 1; i < ynum-1; i++) {
			y_labels.Add ((yvalues.x + i * diff).ToString ());
		}
		y_labels.Add (yvalues.y.ToString());

		float diffx = (xvalues.y - xvalues.x)/(xnum - 1);
		List<string> x_labels = new List<string> ();
		x_labels.Add (xvalues.x.ToString());
		for (int i = 1; i < xnum-1; i++) {
			x_labels.Add ((xvalues.x + i * diffx).ToString ());
		}
		x_labels.Add (xvalues.y.ToString());

		DrawLines (ewdd, graphPrefab, names, xvalues, yvalues, x_labels, y_labels, labelnames, cols);
	}

	/// <summary>
	/// 获得数据
	/// </summary>
	/// <returns>数据组（最多为三个）</returns>
	/// <param name="path">数据文件路径.</param>
	/// 最多读取此种格式的文件中的三组数据
	List<ErWeiData> FirstGetData(string path)
	{
		if (File.Exists (path)) {
			ErWeiData ewd = new ErWeiData ();
			//数据流的方式读取
			StreamReader sr = new StreamReader (path, System.Text.Encoding.ASCII);
			string line = "";

			line = sr.ReadLine ();
			ewd.expression = line;
			line = sr.ReadLine ();
			ewd.x_infoname = line;
			line = sr.ReadLine ();
			ewd.y_infoname = line;

			List<double> aa = new List<double> ();
			List<double> bb = new List<double> ();
			bool OtherData = false;

			line = sr.ReadLine ();
			while (line != null) {
				string _str = "";
				List<string> lits = new List<string> ();
				char[] dd = line.ToCharArray ();
				for (int i = 0; i < dd.Length; i++) {
					if (dd [i] != '	') {
						_str += dd [i].ToString ();
					} else {
						lits.Add (_str);
						_str = "";
					}
					if (i == dd.Length - 1) {
						lits.Add (_str);
					}
				}
				if (lits.Count >= 2) {
					aa.Add (double.Parse(lits[0]));
					bb.Add (double.Parse(lits[1]));
				} else {
//					Debug.Log ("test");
					OtherData = true;
					break;
				}
				lits.Clear ();
				line = sr.ReadLine ();
			}
			ewd.x_data = aa;	
			ewd.y_data = bb;

			List<ErWeiData> lewd = new List<ErWeiData>();
			lewd.Add (ewd);
			if (!OtherData) {
				return lewd;
			}
			OtherData = false;
			ErWeiData ewd1 = new ErWeiData ();
			List<double> aa1 = new List<double> ();
			List<double> bb1 = new List<double> ();
//			Debug.Log (line + "sdsfsf");
			ewd1.y_infoname = line;
			line = sr.ReadLine ();
//			Debug.Log (line + "sdsfsf");
			while (line != null) {
				string _str = "";
				List<string> lits = new List<string> ();
				char[] dd = line.ToCharArray ();
				for (int i = 0; i < dd.Length; i++) {
					if (dd [i] != '	') {
						_str += dd [i].ToString ();
					} else {
						lits.Add (_str);
						_str = "";
					}
					if (i == dd.Length - 1) {
						lits.Add (_str);
					}
				}
				if (lits.Count >= 2) {
					aa1.Add (double.Parse(lits[0]));
					bb1.Add (double.Parse(lits[1]));
				} else {
//					Debug.Log ("test");
					OtherData = true;
					break;
				}
				lits.Clear ();
				line = sr.ReadLine ();
			}
			ewd1.x_data = aa1;
			ewd1.y_data = bb1;
			lewd.Add (ewd1);
			if (!OtherData) {
				return lewd;
			}
			OtherData = false;
			ErWeiData ewd2 = new ErWeiData ();
			List<double> aa2 = new List<double> ();
			List<double> bb2 = new List<double> ();
			ewd2.y_infoname = line;
			line = sr.ReadLine ();
			while (line != null) {
				string _str = "";
				List<string> lits = new List<string> ();
				char[] dd = line.ToCharArray ();
				for (int i = 0; i < dd.Length; i++) {
					if (dd [i] != '	') {
						_str += dd [i].ToString ();
					} else {
						lits.Add (_str);
						_str = "";
					}
					if (i == dd.Length - 1) {
						lits.Add (_str);
					}
				}
				if (lits.Count >= 2) {
					aa2.Add (double.Parse(lits[0]));
					bb2.Add (double.Parse(lits[1]));
				} else {
//					Debug.Log ("test");
					OtherData = true;
					break;
				}
				lits.Clear ();
				line = sr.ReadLine ();
			}
			ewd2.x_data = aa2;
			ewd2.y_data = bb2;
//			Debug.Log (ewd2.y_data.Count);
			lewd.Add (ewd2);
			if (!OtherData) {
				return lewd;
			}
			return lewd;
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
}
