using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsDisplaceData {

	public static List<string> oDispData = new List<string>();//位移数据的初始内容，从文件中刚提取
	public static List<string[]> fDispData = new List<string[]>();//位移数据第一次处理
	public static float[][] sDispData = new float[][]{};//位移数据第二次处理
	public static int SingleDispCol = 37;//位移个数（一组）
	public static int SingleDispRow = 5;//位移内容（UX,UY,UZ,USUM）
	public static int AddCount = 0;
	//重要：没项内容对应的个数【1,1,1,1】
	public static Vector3[] vDispData = new Vector3[]{};
	public static float[] SumDispData;
	//重要，该数据可以重新读取
	public static int DispIndex = 121;
	public static int[] MaxDispNodes = new int[]{};
	public static float[] MaxDispValue = new float[]{};

	/// <summary>
	/// 位移数据第一次处理，清除空行，逐行提取非空内容
	/// </summary>
	/// <returns>非空数据.</returns>
	/// <param name="ond">源文件数据.</param>
	public static List<string[]> ExtractDispData(List<string> ond)
	{
		if (ond == null)
			return null;
		if (ond.Count == 0)
			return null;

		int cou = ond.Count;
		string temp = "";
		string[] trans;
		List<string> output;//包含节点标号
		List<string[]> results = new List<string[]>();
		for (int i = 0; i < cou; i++) 
		{
			temp = ond [i];
			if (temp != "") {
				output = new List<string>();//初始化
				trans = temp.Split(' ');
                //int idx = -1;
				for (int j = 0; j < trans.Length; j++) {
					if (!string.IsNullOrEmpty(trans[j])) {
//						Debug.Log (trans[j]);
						output.Add (trans[j]);
					}
				}
				if(output.Count != 0)
					results.Add (output.ToArray());
			}
		}
        ond.Clear();//清空原始位移数据，释放内存
		return results;
	}
	/// <summary>
	/// 获得一组位移数据的标题行序号
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
			if(trans.Length >= 5)
			{
				if (trans[0] == "NODE" && trans[1] == "UX" && trans[2] == "UY" && trans[3] == "UZ" && trans[4] == "USUM") {
					temp.Add (i);
				}
			}
			//获得节点的最大值数据
			if (trans.Length >= 3) {
				if (trans[0] == "MAXIMUM" && trans[1] == "ABSOLUTE" && trans[2] == "VALUES") {
					temp.Add (i);
				}
			}
		}
		int[] result = temp.ToArray ();
		temp.Clear ();//清空原始位移数据，释放内存
		return result;
	}
	/// <summary>
	/// 获得节点坐标值
	/// </summary>
	/// <returns>.</returns>
	/// <param name="fnd">第一次处理后的非空数据.</param>
	public static void GetDispData(List<string[]> fnd, out float[][] disps, out int[] MaxNodes, out float[] MaxValues)
	{
		int[] starts = GetDataStart (fnd);
		List<float[]> data = new List<float[]> ();
		float[] temp;
		//标题行序号:最大值***************************************//
		for (int i = 0; i < starts.Length - 1; i++) {
			for (int j = 1; j <= SingleDispCol; j++) {
                //if (i*SingleDispCol+j <= DispIndex) {
					string[] trans = fnd [starts [i] + j];
                    
					if (trans.Length < SingleDispRow) {//数据结构处理（此处为异常）
						List<string> lstr = new List<string> ();
						for (int k = 0; k < trans.Length; k++) {
							string[] rst = ReFormat (trans [k]);
							for (int m = 0; m < rst.Length; m++) {
								lstr.Add (rst[m]);
							}
						}
						trans = lstr.ToArray ();
                        Debug.Log(fnd[starts[i] + j][0]);
                        Debug.Log(trans[0] + "  " + trans[1] + "  " + trans[2] + "  " + trans[3]);
//						Debug.Log (trans [0] + "  " + trans [1] + "  " + trans [2] + "  "+ trans [3] + "  "+ trans [4]);
					} else {
						//数据结构处理（此处为正常）
					}

					if (trans.Length < SingleDispRow) {
						Debug.Log ("位移数据出错，发生在第 " + trans[0] + " 节点处！");
					} else {
						temp = new float[4];
						//第二个开始
						float.TryParse (trans [1], out temp[0]);
						float.TryParse (trans [2], out temp[1]);
						float.TryParse (trans [3], out temp[2]);
						float.TryParse (trans [4], out temp[3]);
//						Debug.Log ("i = " + (i*SingleDispCol+j));
//						Debug.Log (trans [1] + "  " + trans [2] + "  "+ trans [3] + "  "+ trans [4]);
						data.Add (temp);
					}
                //}
			}
		}
		disps = data.ToArray ();

		//标题行序号:最大值所在的节点号***************************************//
		int maxIdx = starts[starts.Length - 1];
		string[] nodes = fnd [maxIdx + 1];
		MaxNodes = new int[4];
		int.TryParse (nodes [1], out MaxNodes[0]);
		int.TryParse (nodes [2], out MaxNodes[1]);
		int.TryParse (nodes [3], out MaxNodes[2]);
		int.TryParse (nodes [4], out MaxNodes[3]);
		//标题行序号:最大值**************************************************//
		string[] values = fnd [maxIdx + 2];
		if (values.Length < SingleDispRow) {//数据结构处理（此处为异常）
			List<string> lstr = new List<string> ();
			for (int k = 0; k < values.Length; k++) {
				string[] rst = ReFormat (values [k]);
				for (int m = 0; m < rst.Length; m++) {
					lstr.Add (rst[m]);
				}
			}
			values = lstr.ToArray ();
		} else {
			//数据结构处理（此处为正常）
		}
		MaxValues = new float[4];
		float.TryParse (values [1], out MaxValues[0]);
		float.TryParse (values [2], out MaxValues[1]);
		float.TryParse (values [3], out MaxValues[2]);
		float.TryParse (values [4], out MaxValues[3]);

		fnd.Clear ();////清空非空节点数据，释放内存
	}

	/// <summary>
	/// 位移数据异常处理
	/// </summary>
	/// <returns>The format.</returns>
	/// <param name="str">String.</param>
	public static string[] ReFormat(string str)
	{
		string[] result = new string[]{str};
		if (str.Length > 12) {
			//要求每隔12为可编译数据
			int idx = str.Length / 12;
			result = new string[idx];
			for (int i = 0; i < idx; i++) {
				result [i] = str.Substring (i * 12, 12);//必须前后两个数据位数为满，占位12个长度
//				Debug.Log ("idx" + str.Substring (i * 12, 12));
			}
		} else {
			//后一个数据正常
		}
		return result;
	}

	public static Vector3[] DefineDispData(float[][] data)
	{

		int idx = data.Length;
		Vector3[] result = new Vector3[idx];
		for (int i = 0; i < idx; i++) {
			result [i].x = data [i] [0];
			result [i].y = data [i] [1];
			result [i].z = data [i] [2];
		}
		return result;
	}

	public static float[] DefineSumDispData(float[][] data)
	{

		int idx = data.Length;
		float[] result = new float[idx];
		for (int i = 0; i < idx; i++) {
			result [i] = data [i] [3];
		}
		return result;
	}

}
