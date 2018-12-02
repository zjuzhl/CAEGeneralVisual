using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;

public class Flt_ReadDataInfo : MonoBehaviour {

	public GameObject obj;
	public GameObject obj_1;
	public GameObject obj_2;
	public GameObject obj_3;

    public Material mat;//物体的材质球

    public string str;
    public string str_1;
    public string str_2;
    public string str_3;

	private string[] SplitFlag = new string[]{  "Name", "Data", "Faces"};

    private string idx = "3";

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI()
	{
		//gd.ModelName//模型名称
		//gd.nSlnTypeNames//所有物理量
		//gd.NodeNumber//节点数量
		//gd.NodeCoord//节点坐标
		//gd.nSlnTypeValue//节点解（有三个）
		//gd.ElemNumber//单元数量
		//gd.Elem_NodesIndex//节点组成单元的编号
		//gdd.nSlnMinValue//最小值
		//gdd.nSlnMaxValue//最大值
//        if (GUI.Button (new Rect (10, 10, 100, 60), "磁钢_云图")) {
//            obj.GetComponent<MeshRenderer> ().enabled = false;
//            obj_1.GetComponent<MeshRenderer> ().enabled = false;
//            obj_2.GetComponent<MeshRenderer> ().enabled = false;
//            obj_3.GetComponent<MeshRenderer> ().enabled = false;

//            obj.GetComponent<MeshRenderer> ().enabled = true;
//            obj_1.GetComponent<MeshRenderer> ().enabled = true;
//            obj_2.GetComponent<MeshRenderer> ().enabled = true;
//            obj_3.GetComponent<MeshRenderer> ().enabled = true;

//            string gd_path = UnityEngine.Application.dataPath + @"/Resources/Flt_Assets/Exterior-waikewall_woliu-Temperature-1-1.csv";
//            string gd_path_1 = UnityEngine.Application.dataPath + @"/Resources/Flt_Assets/Exterior-waikewall_woliu-Temperature-1-2.csv";

//            Flt_YunTu_JianBian fyj = new Flt_YunTu_JianBian ();
//            int indexer = 0;//设置物理量序号
//            GeneralData gdd = FirstGetData (gd_path);
//            GeneralData gdd_1 = FirstGetData (gd_path_1);
//            fyj.ShowColors (obj, obj_1, gdd_1, indexer, gdd_1.nSlnMaxValue[indexer], gdd_1.nSlnMinValue[indexer], true, 2000);
//            fyj.ShowColors (obj_2, obj_3, gdd, indexer, gdd.nSlnMaxValue[indexer], gdd.nSlnMinValue[indexer], true, 2000);
//            fyj.CreatePrefabStaff ("Staff_", gdd.nSlnTypeNames[indexer], gdd.nSlnMaxValue[indexer], gdd.nSlnMinValue[indexer], new Vector3(345,0,0));
//        }

//        if (GUI.Button (new Rect (10, 70, 100, 60), "转子铁芯_云图")) {
//            obj.GetComponent<MeshRenderer> ().enabled = false;
//            obj_1.GetComponent<MeshRenderer> ().enabled = false;
//            obj_2.GetComponent<MeshRenderer> ().enabled = false;
//            obj_3.GetComponent<MeshRenderer> ().enabled = false;

//            obj.GetComponent<MeshRenderer> ().enabled = true;
//            obj_1.GetComponent<MeshRenderer> ().enabled = true;

//            Flt_YunTu_JianBian fyj = new Flt_YunTu_JianBian ();
//            string gd_path = UnityEngine.Application.dataPath + @"/Resources/Flt_Assets/Interior-Rotatepart-Temperature-2-1.csv";
//            int indexer = 0;
//            GeneralData gdd = FirstGetData (gd_path);

//            fyj.ShowColors(obj, obj_1, gdd, indexer, gdd.nSlnMaxValue[indexer], gdd.nSlnMinValue[indexer], true, 2000);
//            fyj.CreatePrefabStaff ("Staff_", gdd.nSlnTypeNames[indexer], gdd.nSlnMaxValue[indexer], gdd.nSlnMinValue[indexer], new Vector3(345,0,0));
//        }

        //if (GUI.Button(new Rect(10, 130, 100, 60), "定子绕组_云图"))
        //{
        //    obj.GetComponent<MeshRenderer>().enabled = false;
        //    obj_1.GetComponent<MeshRenderer>().enabled = false;
        //    obj_2.GetComponent<MeshRenderer>().enabled = false;
        //    obj_3.GetComponent<MeshRenderer>().enabled = false;

        //    obj.GetComponent<MeshRenderer>().enabled = true;
        //    obj_1.GetComponent<MeshRenderer>().enabled = true;

        //    Flt_YunTu_JianBian fyj = new Flt_YunTu_JianBian();
        //    //string gd_path = UnityEngine.Application.dataPath + @"/Resources/Flt_Assets/Interior-dingzi_wall_tong-Temperature-3-1.csv";
        //    int indexer = 0;
        //    GeneralData gdd = FirstGetData(gd_path);
        //    //fyj.ShowColors(obj, obj_1, gdd, indexer, gdd.nSlnMaxValue[indexer], gdd.nSlnMinValue[indexer], false, 2000);

        //    fyj.CreatePrefabStaff("Staff_", gdd.nSlnTypeNames[indexer], gdd.nSlnMaxValue[indexer], gdd.nSlnMinValue[indexer], new Vector3(345, 0, 0));
        //}

//        if (GUI.Button (new Rect (10, 190, 100, 60), "定子铁芯_云图")) {
//            obj.GetComponent<MeshRenderer> ().enabled = false;
//            obj_1.GetComponent<MeshRenderer> ().enabled = false;
//            obj_2.GetComponent<MeshRenderer> ().enabled = false;
//            obj_3.GetComponent<MeshRenderer> ().enabled = false;

//            obj.GetComponent<MeshRenderer> ().enabled = true;
//            obj_1.GetComponent<MeshRenderer> ().enabled = true;
//            obj_2.GetComponent<MeshRenderer> ().enabled = true;
//            obj_3.GetComponent<MeshRenderer> ().enabled = true;

//            Flt_YunTu_JianBian fyj = new Flt_YunTu_JianBian ();

//            string gd_path = UnityEngine.Application.dataPath + @"/Resources/Flt_Assets/Interior-dingzi_wall_tie-Temperature-4-1.csv";
//            string gd_path_1 = UnityEngine.Application.dataPath + @"/Resources/Flt_Assets/Interior-dingzi_wall_tie-Temperature-4-2.csv";
////			string gd_path = UnityEngine.Application.dataPath + @"/Resources/Flt_Assets/自然定子铁心1.csv";
////			string gd_path_1 = UnityEngine.Application.dataPath + @"/Resources/Flt_Assets/自然定子铁心2.csv";
		
//            int indexer = 0;
//            GeneralData gdd = FirstGetData (gd_path);
//            GeneralData gdd_1 = FirstGetData (gd_path_1);
//            double _max = gdd.nSlnMaxValue [indexer];
//            double _min = gdd.nSlnMinValue [indexer];
////			Debug.Log (_min);
////			Debug.Log (gdd_1.nSlnMaxValue.Length);
//            if (gdd_1.nSlnMaxValue [indexer] > _max)
//                _max = gdd_1.nSlnMaxValue [indexer];
//            if (gdd_1.nSlnMinValue [indexer] < _min)
//                _min = gdd_1.nSlnMinValue [indexer];
////			Debug.Log (_min);
//            fyj.ShowColors(obj, obj_1, gdd, indexer, _max, _min, true, 2000);
//            fyj.ShowColors(obj_2, obj_3, gdd_1, indexer, _max, _min, false, 2000);
//            fyj.CreatePrefabStaff ("Staff_", gdd.nSlnTypeNames[indexer], _max, _min, new Vector3(345,0,0));

//        }

        //新demo
        GUI.Label(new Rect(10, 100, 100, 20), "选择物理量序号：") ;
        
        idx = GUI.TextField(new Rect(110, 100, 100, 20), idx);

        if (GUI.Button(new Rect(10, 130, 100, 40), "DEMO"))
        {
            obj.GetComponent<MeshRenderer>().enabled = false;
            obj_1.GetComponent<MeshRenderer>().enabled = false;
            obj_2.GetComponent<MeshRenderer>().enabled = false;
            obj_3.GetComponent<MeshRenderer>().enabled = false;

            obj.GetComponent<MeshRenderer>().enabled = true;
            obj_1.GetComponent<MeshRenderer>().enabled = true;

            Flt_YunTu_JianBian fyj = new Flt_YunTu_JianBian();
            //string gd_path = UnityEngine.Application.dataPath + @"/Resources/Flt_Assets/Exterior-buildings-Pressure.csv";
            string gd_path = UnityEngine.Application.dataPath + @"/Resources/Flt_Assets/Exterior-windyInlet-Pressure.csv";
            //string gd_path = UnityEngine.Application.dataPath + @"/Resources/Flt_Assets/Exterior-jianzhu-Pressure.csv";
          
            int indexer = int.Parse(idx);
            if (indexer != null) {

                GeneralData gdd = FirstGetData(gd_path);

                //fyj.ShowColors(obj, obj_1, gdd, indexer, gdd.nSlnMaxValue[indexer], gdd.nSlnMinValue[indexer], false, 2);
                //fyj.ShowObjMat(obj, obj_1, gdd, indexer, mat, 2);
                //fyj.CreatePrefabStaff("Staff_", gdd.nSlnTypeNames[indexer], gdd.nSlnMaxValue[indexer], gdd.nSlnMinValue[indexer], new Vector3(345, 0, 0));

                fyj.ShowColors(obj, gdd, indexer, gdd.nSlnMaxValue[indexer], gdd.nSlnMinValue[indexer], false, 2);
                Debug.Log(Flt_YunTu_JianBian.dGD_Store.Count);
            }
            
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
			line = sr.ReadLine ();
//			Debug.Log (line);
			string[] ary = RemoveComma (line);
			while (ary [0] == "") {
				line = sr.ReadLine ();
				ary = RemoveComma (line);
			}
//			Debug.Log ("line: " + line);
//			Debug.Log ("标志位1： " + ary [0]);
			if (ary [0].Contains( SplitFlag [0] )) {
				line = sr.ReadLine ();
				ary = RemoveComma (line);
				gddd.ModelName = ary [0];
//				Debug.Log ("模型名称： " + ary [0]);
			}

			line = sr.ReadLine ();
			ary = RemoveComma (line);
			while (ary [0] == "") {
				line = sr.ReadLine ();
				ary = RemoveComma (line);
			}
//			Debug.Log ("line: " + line);
//			Debug.Log ("标志位2： " + ary [0]);
			if (ary [0].Contains( SplitFlag [1] )) {
				//数据说明行
				line = sr.ReadLine ();
				ary = RemoveComma (line);
				if (ary.Length > 4) {
					gddd.nSlnTypeNames = new List<string> ();
					for (int i = 4; i < ary.Length; i++) {
						gddd.nSlnTypeNames.Add (ary [i]);
					}
				}
				//初始化数据组
				int SlnLen = gddd.nSlnTypeNames.Count;
				gddd.nSlnTypeValue = new List<double>[SlnLen];
				gddd.nSlnMaxValue = new double[SlnLen];
				gddd.nSlnMinValue = new double[SlnLen];

				for (int j = 0; j < SlnLen; j++) {
					gddd.nSlnTypeValue [j] = new List<double> ();
					gddd.nSlnMaxValue [j] = double.MinValue;
					gddd.nSlnMinValue [j] = double.MaxValue;
				}

				gddd.NodeCoord = new List<double[]> ();

				while (ary.Length != 0 && ary [0] != "") {
					line = sr.ReadLine ();
					ary = RemoveComma (line);
					if (ary.Length >= 4) {
						double[] cd = new double[3];
						double.TryParse (ary [1], out cd [0]);
						double.TryParse (ary [2], out cd [1]);
						double.TryParse (ary [3], out cd [2]);
						gddd.NodeCoord.Add (cd);

						//添加物理量对应的数据
						if (ary.Length - 4 == SlnLen) {
							for (int j = 0; j < SlnLen; j++) {
								double db;
								if (double.TryParse (ary [j + 4], out db)) {
									gddd.nSlnTypeValue [j].Add (db);
									if (db > gddd.nSlnMaxValue [j]) {
										gddd.nSlnMaxValue [j] = db;
									}
									if (db < gddd.nSlnMinValue [j]) {
										gddd.nSlnMinValue [j] = db;
									}
								} else {
									Debug.Log ("nimeimei" + ary [j + 4] + "  "+ ary [0] + "  " +  ary [1] + "  "+ ary [2] + "  "+ ary [3]);
								}
							}
						} else {
							Debug.Log ("错误：物理量的数据与结构中的长度不一致！");
							return null;
						}
					} else {
						if (ary.Length != 1) {
							Debug.Log (ary.Length - 4);
							Debug.Log (SlnLen);
							Debug.Log ("提示：当前文件没有任何物理量数据或者为二维坐标参数，请确认！");
						}
					}
				}
//				//处理多读的无效行
//				gd.NodeCoord.RemoveAt (gd.NodeCoord.Count - 1);
//				for (int j = 0; j < gd.nSlnTypeNames.Count; j++) {
//					gd.nSlnTypeValue [j].RemoveAt (gd.nSlnTypeValue [j].Count - 1);
//				}

				gddd.NodeNumber = gddd.NodeCoord.Count;
				gddd.NodeOutterNumber = gddd.NodeNumber;

//				Debug.Log ("节点坐标总数：" + gddd.NodeNumber);
//				Debug.Log ("第一个： " + gddd.NodeCoord [0] [0] + "  " + gddd.NodeCoord [0] [1] + "  " + gddd.NodeCoord [0] [2]);
//				Debug.Log ("最后一个： " + gddd.NodeCoord [gddd.NodeCoord.Count - 1] [0] + "  " + gddd.NodeCoord [gddd.NodeCoord.Count - 1] [1] + "  " + gddd.NodeCoord [gddd.NodeCoord.Count - 1] [2]);

//				Debug.Log ("物理量1总数：" + gd.nSlnTypeValue [0].Count);
//				Debug.Log ("第一个： " + gd.nSlnTypeValue [0] [0]);
//				Debug.Log ("最后一个： " + gd.nSlnTypeValue [0] [gd.nSlnTypeValue [0].Count - 1]);
//				Debug.Log ("最大值： " + gddd.nSlnMaxValue [0] );
//				Debug.Log ("最小值： " + gddd.nSlnMinValue [0]);

				line = sr.ReadLine ();
				ary = RemoveComma (line);
				while (ary [0] == "") {
					line = sr.ReadLine ();
					ary = RemoveComma (line);
				}

//				Debug.Log ("标志位3： " + ary [0]);
				if (ary [0].Contains(SplitFlag [2])) {
					gddd.Elem_NodesIndex = new List<int[]> ();
					while (ary.Length != 0 && ary [0] != "") {
						line = sr.ReadLine ();
						ary = RemoveComma (line);
						if (ary.Length >= 3) {
							int[] cd = new int[3];
							int.TryParse (ary [0], out cd [0]);
							int.TryParse (ary [1], out cd [1]);
							int.TryParse (ary [2], out cd [2]);
							gddd.Elem_NodesIndex.Add (cd);
						} else {

						}
					}
					gddd.ElemNumber = gddd.Elem_NodesIndex.Count;
					gddd.ElemOutterNumber = gddd.ElemNumber;

//					Debug.Log ("节点单元总数：" + gddd.ElemNumber);
//					Debug.Log ("第一个： " + gddd.Elem_NodesIndex [0] [0] + "  " + gddd.Elem_NodesIndex [0] [1] + "  " + gddd.Elem_NodesIndex [0] [2]);
//					Debug.Log ("最后一个： " + gddd.Elem_NodesIndex [gddd.Elem_NodesIndex.Count - 1] [0] + "  " + gddd.Elem_NodesIndex [gddd.Elem_NodesIndex.Count - 1] [1] + "  " + gddd.Elem_NodesIndex [gddd.Elem_NodesIndex.Count - 1] [2]);
				}

			}
			sr.Close ();
			return gddd;
		} else {
			Debug.Log ("错误：文件不存在！路径为：" + path);
			return null;
		}

	}
	
	static string[] RemoveComma(string str)
    {
        if (str == "" || str == null)
        {
			string[] st = new string[]{""};
            return st;
        }
        return Regex.Split(str, ",", RegexOptions.IgnoreCase);
    }



}
