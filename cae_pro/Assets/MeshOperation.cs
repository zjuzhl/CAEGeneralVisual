using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshOperation {

	Mesh MyMesh;
	Vector3[] KeyPoints;
	keyline[] KeyLines;


	public MeshOperation(Mesh mesh){
		MyMesh = mesh;
	}

	/// <summary>
	/// 将笛卡尔坐标转为Unity坐标（yz轴数据交换）
	/// </summary>
	/// <returns>unity坐标参数.</returns>
	/// <param name="vec">笛卡尔坐标参数.</param>
	Vector3[] CoordSystemTransfar(Vector3[] vec)
	{
		if (vec.Length <= 0)
			return new Vector3[0];
		int len = vec.Length;
		Vector3 temp = new Vector3 ();
		Vector3[] rst = new Vector3[len];
		for (int i = 0; i < len; i++) {
			temp.x = vec [i].x;
			temp.y = vec [i].z;
			temp.z = vec [i].y;
			rst [i] = temp;
		}
		return rst;
	}
	/// <summary>
	/// 设置mesh的顶点
	/// </summary>
	/// <param name="vec">Vec.</param>
	/// <param name="isCoord">If set to <c>true</c> is 需要进行ANSYS与unity之间的坐标转换.</param>
	public void ReSetVetices(Vector3[] vec, bool isCoord)
	{
		if (isCoord) {
			MyMesh.vertices = CoordSystemTransfar (vec);
		} else {
			MyMesh.vertices = vec;
		}
	}

	/// <summary>
	/// 设置mesh的顶点索引值
	/// </summary>
	/// <param name="Indexs">Indexs.</param>
	/// <param name="et">Et.</param>
	public void ReConstructTriangles(int[][] Indexs, ElementType et, bool reverse)
	{
		if (Indexs.Length <= 0)
			return;
		//重要说明
		//遵循左手外法线方向原则，reverse为true时，即在XOY平面上，逆时针编号为0,1,2，外法线为（0,0,1）
		if (et == ElementType.Triangles) {//全为三角形单元
			int num = Indexs.Length;
			int[] squ = new int[num*3];
			if (reverse) {
				for (int i = 0; i < num; i++) {
					squ [i * 3 + 0] = Indexs [i][1] - 1;//ANSYS中的检索值从1开始
					squ [i * 3 + 1] = Indexs [i][0] - 1;
					squ [i * 3 + 2] = Indexs [i][2] - 1;
				}
				MyMesh.triangles = squ;
			} else {
				for (int i = 0; i < num; i++) {
					squ [i * 3 + 0] = Indexs [i][0] - 1;
					squ [i * 3 + 1] = Indexs [i][1] - 1;
					squ [i * 3 + 2] = Indexs [i][2] - 1;
				}
			}
			MyMesh.triangles = squ;
		} else if (et == ElementType.Square) {//全为四面体单元
			int num = Indexs.Length;
			int[] squ = new int[num * 6];
			if (reverse) {
				for (int i = 0; i < num; i++) {
					squ [i * 6 + 0] = Indexs [i][1] - 1;
					squ [i * 6 + 1] = Indexs [i][0] - 1;
					squ [i * 6 + 2] = Indexs [i][2] - 1;
					squ [i * 6 + 3] = Indexs [i][2] - 1;
					squ [i * 6 + 4] = Indexs [i][0] - 1;
					squ [i * 6 + 5] = Indexs [i][3] - 1;
				}
			} else {
				for (int i = 0; i < num; i++) {
					squ [i * 6 + 0] = Indexs [i][0] - 1;
					squ [i * 6 + 1] = Indexs [i][1] - 1;
					squ [i * 6 + 2] = Indexs [i][2] - 1;
					squ [i * 6 + 3] = Indexs [i][0] - 1;
					squ [i * 6 + 4] = Indexs [i][2] - 1;
					squ [i * 6 + 5] = Indexs [i][3] - 1;
				}
			}
			MyMesh.triangles = squ;
		} else {//其他单元

		}
	}

	/// <summary>
	/// 设置mesh的顶点索引值(不确定单元类型)
	/// </summary>
	/// <param name="Indexs">Indexs.</param>
	/// <param name="et">Et.</param>
	public void ReConstructTriangles(int[][] Indexs, bool reverse)
	{
		if (Indexs.Length <= 0)
			return;
		//重要说明
		//遵循左手外法线方向原则，reverse为true时，即在XOY平面上，逆时针编号为0,1,2，外法线为（0,0,1）
		int len = Indexs.Length;
		List<int> temp = new List<int> ();
		int dd;
		int[] trans;
		for (int i = 0; i < len; i++) 
		{
			trans = Indexs [i];
			dd = trans.Length;
			if (dd < 3) {
				Debug.Log ("单元中该组顶点数量小于3个，发生在第 " + i + "处！");
				return;
			} else {
				int[] squ = new int[3 * dd - 6];
				if (reverse) {
					for (int j = 1; j < dd - 1; j++) {
						temp.Add (trans[j] - 1);//ANSYS中的检索值从1开始
						temp.Add (trans[0] - 1);
						temp.Add (trans[j+1] - 1);
					}
				} else {
					for (int j = 1; j < dd - 1; j++) {
						temp.Add (trans[0] - 1);
						temp.Add (trans[j] - 1);
						temp.Add (trans[j+1] - 1);
					}
				}
			}
		}
		if (temp.Count != 0) {
			MyMesh.triangles = temp.ToArray ();
			temp.Clear ();//及时清空
		}
	}
		
	public void SetKeyPoints(Vector3[] key){

	}

	public void SetKeyPointsVisible(bool isVisible){
		
	}

	public void SetKeyLinesVisible(bool isVisible){
	
	}

	public void SetElementsVisible(bool isVisible){
		
	}
}
