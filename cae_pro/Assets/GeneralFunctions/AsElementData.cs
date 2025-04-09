using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsElementData  {

	public static List<string> oElemData;//单元数据的初始内容，从文件中刚提取
	public static List<string[]> fElemData;//单元数据第一次处理
	public static int SingleElemCol = 20;//单元个数（一组）
	public static int SingleElemRow = 6;//单元内容（MAT,TYP,REL,ESY,SEC,NODES）
	public static int AddCount = 3;
	//重要：没项内容对应的个数【1,1,1,1,1,4】
	public static int[][] ANSElemData;//每组4个数字===四边形
	public static int[][] UniElemData;//在unity里面节点的检索值
    public static int ElemIndex = 18960;

	/// <summary>
	/// 单元数据第一次处理，清除空行，逐行提取非空内容
	/// </summary>
	/// <returns>非空数据.</returns>
	/// <param name="ond">源文件数据.</param>
	public static List<string[]> ExtractElemData(List<string> ond)
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
				output = new string[SingleElemRow + 1 + AddCount];//初始化
				trans = temp.Split(' ');
				int idx = -1;
				for (int j = 0; j < trans.Length; j++) {
					if (!string.IsNullOrEmpty(trans[j])) {
//						Debug.Log (trans [j]);
						idx++;
						if (idx < SingleElemRow + AddCount + 1) {
							output [idx] = trans [j];
						} else {
							Debug.Log("单元数据读取时，一行的个数超出异常！发生在第 " + i + " 的单元 " + output[0] + " 处！");
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
				if(flag)
					results.Add (output);
			}
		}
		ond.Clear ();//清空原始单元数据，释放内存
		return results;
	}
	/// <summary>
	/// 获得一组单元数据的标题行序号
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
			if(trans.Length >= 7)
			{
				if (trans[0] == "ELEM" && trans[1] == "MAT" && trans[2] == "TYP" && trans[3] == "REL"
					&& trans[4] == "ESY" && trans[5] == "SEC" && trans[6] == "NODES") {
					temp.Add (i);
				}
			}
		}
		int[] result = temp.ToArray ();
		temp.Clear ();//清空原始单元数据，释放内存
		return result;
	}
	/// <summary>
	/// 获得节点坐标值
	/// </summary>
	/// <returns>节点坐标值.</returns>
	/// <param name="fnd">第一次处理后的非空数据.</param>
	public static int[][] GetElemData(List<string[]> fnd)
	{
		if (fnd == null)
			return null;
		if (fnd.Count == 0)
			return null;

		int[] starts = GetDataStart (fnd);
		List<int[]> data = new List<int[]> ();
		int[] temp;
		for (int i = 0; i < starts.Length; i++) {
			for (int j = 1; j <= SingleElemCol; j++) {
				if (i*SingleElemCol + j <= ElemIndex) {
					temp = new int[4];
					//
					int.TryParse (fnd [starts[i] + j] [6], out temp[0]);
					int.TryParse (fnd [starts[i] + j] [7], out temp[1]);
					int.TryParse (fnd [starts[i] + j] [8], out temp[2]);
					int.TryParse (fnd [starts[i] + j] [9], out temp[3]);
					data.Add (temp);
				}
			}
		}
		fnd.Clear ();////清空非空节点数据，释放内存
		int[][] result = data.ToArray ();
		return result;
	}

	/// <summary>
	/// ANSYS节点检索值转为unity检索值
	/// </summary>
	/// <returns>The to uni element data.</returns>
	/// <param name="data">Data.</param>
	public static int[] AsToUniElemData(int[][] data)
	{
		int[] Uni = new int[data.Length * 4];
		for (int i = 0; i < data.Length; i++) 
		{
			Uni [4 * i] = data [i] [0] - 1;
			Uni [4 * i + 1] = data [i] [1] - 1;
			Uni [4 * i + 2] = data [i] [2] - 1;
			Uni [4 * i + 3] = data [i] [3] - 1;
		}
		return Uni;
	}
}
