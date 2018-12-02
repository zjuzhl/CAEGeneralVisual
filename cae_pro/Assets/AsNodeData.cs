using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AsNodeData {


	public static List<string> oNodeData;//节点数据的初始内容，从文件中刚提取
	public static List<string[]> fNodeData;//节点数据第一次处理
	public static int SingleDataCol = 20;//节点个数（一组）
	public static int SingleDataRow = 6;//节点内容（X,Y,Z,THXY,THYZ,THZX）
	public static int AddCount = 0;
//重要：没项内容对应的个数【1,1,1,1,1,1】
	public static Vector3[] ANSNodeData;//节点坐标（在ANSYS中的坐标）
	public static Vector3[] UniNodeData;//节点在unity里面的坐标
	public static int NodeIndex = 19081;

	/// <summary>
	/// 节点数据第一次处理，清除空行，逐行提取非空内容
	/// </summary>
	/// <returns>非空数据.</returns>
	/// <param name="ond">源文件数据.</param>
	public static List<string[]> ExtractNodeData(List<string> ond)
	{
		if (ond == null)
			return null;
		if (ond.Count == 0)
			return null;

		int cou = ond.Count;
		string temp = "";
		string[] trans;
		string[] output;//包含节点标号
		List<string[]> results = new List<string[]>();
		for (int i = 0; i < cou; i++) 
		{
			temp = ond [i];
			if (temp != "") {
				output = new string[SingleDataRow + 1 + AddCount];//初始化
				trans = temp.Split(' ');
				int idx = -1;
				for (int j = 0; j < trans.Length; j++) {
					if (!string.IsNullOrEmpty(trans[j])) {
						idx++;
						if (idx < SingleDataRow + AddCount + 1) {
							output [idx] = trans [j];
						} else {
							Debug.Log("节点数据读取时，一行的个数超出异常！发生在第 " + i + " 的节点 " + output[0] + " 处！");
							return null;
						}
					}
				}
				bool flag = false;
				for (int j = 0; j < output.Length; j++) {
					if (!string.IsNullOrEmpty(output[j])) {
						flag = true;
						break;
					}
				}
				if(flag){
					results.Add (output);
				}
			}
		}
		ond.Clear ();//清空原始节点数据，释放内存
		return results;
	}
	/// <summary>
	/// 获得一组节点数据的标题行序号
	/// </summary>
	/// <returns>序号.</returns>
	/// <param name="fnd">第一次处理后的数据.</param>
	private static int[] GetDataStart(List<string[]> fnd)
	{
		int cou = fnd.Count;
		List<int> temp = new List<int> ();
		string[] trans = new string[1];
		for (int i = 0; i < cou; i++) {
			//节点坐标一组数据的标题（在节点数据的上一行）
			trans = fnd[i];
			if (trans.Length >= 4) {
				if (trans[0] == "NODE" && trans[1] == "X" && trans[2] == "Y" && trans[3] == "Z") {
					temp.Add (i);
//				Debug.Log ("NodeStart: " + i + fnd [i] [0].ToLower () + " ; " + fnd [i] [1].ToLower ()  + " ; " + fnd [i] [2].ToLower ()  + " ; " + fnd [i] [3].ToLower ());
				}			
			}
		}
		int[] result = temp.ToArray ();
		temp.Clear ();
		return result;
	}
	/// <summary>
	/// 获得节点坐标值
	/// </summary>
	/// <returns>节点坐标值.</returns>
	/// <param name="fnd">第一次处理后的非空数据.</param>
	public static Vector3[] GetNodeData(List<string[]> fnd)
	{
		if (fnd == null)
			return null;
		if (fnd.Count == 0)
			return null;

		int[] starts = GetDataStart (fnd);
		List<Vector3> data = new List<Vector3> ();
		Vector3 temp;
		for (int i = 0; i < starts.Length; i++) {
			for (int j = 1; j <= SingleDataCol; j++) {
				if (i*SingleDataCol+j <= NodeIndex) {
					temp = new Vector3 ();
					float.TryParse (fnd [starts[i] + j] [1], out temp.x);
					float.TryParse (fnd [starts[i] + j] [2], out temp.y);
					float.TryParse (fnd [starts[i] + j] [3], out temp.z);
					data.Add (temp);
				}
			}
		}
		fnd.Clear ();////清空非空节点数据，释放内存
		Vector3[] result = data.ToArray ();
		return result;
	}


	/// <summary>
	/// 将ANSYS坐标转为Unity坐标（yz轴数据交换）
	/// </summary>
	/// <returns>unity坐标参数.</returns>
	/// <param name="vec">ANSYS坐标参数.</param>
	public static Vector3[] AsToUniPositons(Vector3[] vec)
	{
		if (vec.Length <= 0)
			return new Vector3[0];
		int len = vec.Length;
		Vector3 temp = new Vector3 ();
		Vector3[] rst = new Vector3[len];
		for (int i = 0; i < len; i++) {
			temp.x = vec [i].x;
			temp.y = vec [i].z;//yz坐标值进行转换
			temp.z = vec [i].y;
			rst [i] = temp;
		}
		return rst;
	}
}
