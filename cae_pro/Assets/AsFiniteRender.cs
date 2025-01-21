using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsFiniteRender {
	//先将ANSYS坐标转换为unity坐标
	//将unity空间坐标转换为屏幕坐标
	//通过屏幕坐标进行点、线的绘制

	public static Material ElemMater;

	public static Vector3[] AsToUniPos(Vector3[] Nodes)
	{
		return AsNodeData.AsToUniPositons (Nodes);
	}

	public static Vector3[] WorldToScreenPos(Vector3[] Nodes, Vector3 offset)
	{
		int len = Nodes.Length;
		Vector3[] results = new Vector3[len];
		for (int i = 0; i < len; i++) {
			results[i] = Camera.main.WorldToScreenPoint (Nodes[i] + offset);
		}
		return results;
	}

	public static void DrawElementsLines(Vector3[] Nodes, List<int[]> indexs, Material elemMat)
	{
		if (indexs.Count <= 0) return;
		if (Nodes.Length <= 0) return;
		for (int i = 0; i < indexs.Count; i++) 
		{
			DrawStraightLine (Nodes[indexs[i][0]], Nodes[indexs[i][1]], elemMat);
		}
	}
	/// <summary>
	/// 画直线
	/// </summary>
	public static void DrawStraightLine(Vector3 start_point, Vector3 end_point, Material elemMat)
	{
		elemMat.SetPass(0);
		elemMat.color = Color.white;
		GL.Color(Color.white);
		start_point.x = start_point.x / Screen.width;
		start_point.y = start_point.y / Screen.height;
		end_point.x = end_point.x / Screen.width;
		end_point.y = end_point.y / Screen.height;
        GL.PushMatrix();
		GL.LoadOrtho();
        GL.Begin(GL.LINES);
		GL.Vertex3(start_point.x, start_point.y, 0);
		GL.Vertex3(end_point.x, end_point.y, 0);
        GL.End();
        GL.PopMatrix();
	}	

	public static List<int[]> ElementsRenderIndex(int[][] data){
		int[] Sin = new int[2];
		int len = data.Length;
		List<int[]> result = new List<int[]> ();

		for (int i = 0; i < len; i++) {
			Sin = new int[]{ data [i] [0] - 1, data [i] [1]  - 1};
			if (!IndexCheck (result, Sin)) {
				result.Add (Sin);
			}
			Sin = new int[]{ data [i] [1] - 1, data [i] [2]  - 1};
			if (!IndexCheck (result, Sin)) {
				result.Add (Sin);
			}
			Sin = new int[]{ data [i] [2] - 1, data [i] [3]  - 1};
			if (!IndexCheck (result, Sin)) {
				result.Add (Sin);
			}
			Sin = new int[]{ data [i] [3] - 1, data [i] [0]  - 1};
			if (!IndexCheck (result, Sin)) {
				result.Add (Sin);
			}
		}
		return result;
	}

	static bool IndexCheck(List<int[]> list, int[] dat)
	{
		for (int i = 0; i < list.Count; i++) {
			if (list [i].Length == 2 && dat.Length == 2) 
			{
				if (list [i] [0] == dat [0] && list [i] [1] == dat [1]) {
					return true;//检索器中已包含这个数据组合
				}else if(list [i] [0] == dat [1] && list [i] [1] == dat [0]){
					return true;//检索器中已包含这个数据组合
				}
			}
		}
		return false;
	}
}
