using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mxl_YunTu_SeDai {
	float [] f_area;
	List<GameObject> g_L;

	//清除当前的各个颜色对应的物体
	public void ElemClear()
	{
		if (g_L != null) 
		{
			if(g_L.Count>0)
				for(int i=0;i<g_L.Count;i++)
				{
					GameObject.DestroyImmediate(g_L[i]);
				}
		}
	}

	//cData:色带颜色
	//vNodes:节点坐标
	//fElemSlns:单元解（根据节点编号的一一对应的单元解）
	//Inds:构成单元节点编号
	public void ElemSoloution(GameObject father, GameObject obj, Vector3[] vNodes, float[] fElemSlns, int[] Inds, int LAN, Color[] cData, Material mat){
		//			this.transform.GetComponent<color_sample>().clear();
		ElemClear();
		//			Debug.Log(vNodes.Length+" "+fElemSlns.Length);
		//处理色带：默认设置颜色
		Color_Set (ref cData);

		Vector3[] vNodeCoord = new Vector3[]{};//保存节点坐标
		float[] fElemValues;
		//获得节点坐标
		GetNodeCoord (ref vNodeCoord, vNodes);
		create_f_area(fElemSlns, cData.Length);

		color_change_new cc_script=new color_change_new();
		cc_script.init(cData.Length,f_area);
		//四边形模型
		if (LAN == 4) {
			
			for (int i = 0; i < Inds.Length / 4; i++) {//
				cc_script.deal_tria (fElemSlns [4 * i + 0], fElemSlns [4 * i + 1], fElemSlns [4 * i + 2], fElemSlns [4 * i + 3],
					vNodeCoord [Inds [4 * i + 0] - 1], vNodeCoord [Inds [4 * i + 1] - 1], vNodeCoord [Inds [4 * i + 2] - 1], vNodeCoord [Inds [4 * i + 3] - 1]);
			}
		} else if (LAN == 3) {
			for (int i = 0; i < Inds.Length / 3; i++) {//
				cc_script.deal_tria (fElemSlns [3 * i + 0], fElemSlns [3 * i + 1], fElemSlns [3 * i + 2], fElemSlns [3 * i + 2],
					vNodeCoord [Inds [3 * i + 0]], vNodeCoord [Inds [3 * i + 1]], vNodeCoord [Inds [3 * i + 2]], vNodeCoord [Inds [3 * i + 2]]);
                //if (vNodeCoord [Inds [3 * i + 2]] == new Vector3 (79.4245f, -7.31393f, 0)
                //    && vNodeCoord [Inds [3 * i + 1]] == new Vector3 (80.0151f, -5.49911f, 0)
                //    && vNodeCoord [Inds [3 * i + 0]] == new Vector3 (80.2790f, -7.39805f, 0)) {
                //    Debug.Log (fElemSlns [3 * i + 2]);
                //    Debug.Log (fElemSlns [3 * i + 1]);
                //    Debug.Log (fElemSlns [3 * i + 0]);
                //}
			}
		} else {
			Debug.Log ("Other Mesh!");
		}

		g_L=new List<GameObject>();
		for(int i=0;i<cData.Length;i++)
		{
			GameObject ob1=new GameObject(obj.name+i.ToString(), typeof(MeshFilter), typeof(MeshRenderer));
			g_L.Add(ob1);
			ob1.transform.parent = father.transform;
			ob1.transform.localPosition = new Vector3 (0, 0, 0);
		}
		mesh_create mc=new mesh_create();
		Shader ss=mat.shader;
		Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
		mesh.Clear();
		for(int i=0;i<cData.Length;i++)
		{
			List<Vector3> vl=new List<Vector3>();
			List<Vector3> nl=new List<Vector3>();
//				Debug.Log(i+"@@"+cc_script.ct_L[i].Count);
			for(int j=0;j<cc_script.ct_L[i].Count;j++)
			{
				vl.Add(cc_script.ct_L[i][j].v1);
				vl.Add(cc_script.ct_L[i][j].v2);
				vl.Add(cc_script.ct_L[i][j].v3);
				nl.Add(new Vector3(0,1,0));
				nl.Add(new Vector3(0,1,0));
				nl.Add(new Vector3(0,1,0));
			}
			Material m=new Material(ss);
			m.color=cData[i];
			mc.create(vl,nl,g_L[i],m);
		}
	}

	//正确性校验
	void Color_Set (ref Color[] c_L) 
	{
		if (c_L == null || c_L.Length == 0) 
		{
//			c_L=new Color[9];
//			c_L[0]=new Color(0,0,255/255.0f);
//			c_L[1]=new Color(0,180/255.0f,255/255.0f);
//			c_L[2]=new Color(0,255/255.0f,255/255.0f);
//			c_L[3]=new Color(0,255/255.0f,140/255.0f);
//			c_L[4]=new Color(0,255/255.0f,0);
//			c_L[5]=new Color(180/255.0f,255/255.0f,0);
//			c_L[6]=new Color(255/255.0f,255/255.0f,0);
//			c_L[7]=new Color(255/255.0f,180/255.0f,0);
//			c_L[8]=new Color(255/255.0f,0,0);

			c_L = new Color[16];//设置色带
			//颜色从红到蓝
			//对应从最大值到最小值
			c_L[15] = new Color(255/255.0f,3/255.0f,0);
			c_L[14] = new Color(255/255.0f,68/255.0f,3/255.0f);
			c_L[13] = new Color(255/255.0f,136/255.0f,13/255.0f);
			c_L[12] = new Color(255/255.0f,205/255.0f,26/255.0f);
			c_L[11] = new Color(238/255.0f,255/255.0f,34/255.0f);
			c_L[10] = new Color(170/255.0f,255/255.0f,34/255.0f);
			c_L[9] = new Color(101/255.0f,255/255.0f,33/255.0f);
			c_L[8] = new Color(37/255.0f,255/255.0f,37/255.0f);
			c_L[7] = new Color(12/255.0f,255/255.0f,53/255.0f);
			c_L[6] = new Color(11/255.0f,255/255.0f,108/255.0f);
			c_L[5] = new Color(29/255.0f,255/255.0f,174/255.0f);
			c_L[4] = new Color(64/255.0f,255/255.0f,239/255.0f);
			c_L[3] = new Color(26/255.0f,205/255.0f,255/255.0f);
			c_L[2] = new Color(9/255.0f,135/255.0f,255/255.0f);
			c_L[1] = new Color(16/255.0f,90/255.0f,255/255.0f);
			c_L[0] = new Color(21/255.0f,0,252/255.0f);
		}
	}

	//获取节点坐标
	//v:节点坐标
	//oNC：原始坐标
	void GetNodeCoord(ref Vector3[] v, Vector3[] oNC)
	{
		v = new Vector3[oNC.Length];
		for (int i = 0; i < v.Length; i++) {
			v[i] = oNC [i];
		}
	}
	//f:色带边界的值
	//lth：色带的数量
	void create_f_area(float[] f, int lth)
	{
		float min=Mathf.Infinity;
		float max=-Mathf.Infinity;
		for(int i=0;i<f.Length;i++)
		{
			if(i==0)
			{
				min=f[i];
				max=f[i];
			}
			else
			{
				if(min>f[i])
					min=f[i];
				else if(max<f[i])
					max=f[i];
			}
		}
		f_area=new float[lth + 1];
		float size=(max-min)/(float)lth;
		f_area[0]=min;
		f_area[lth]=max;
		for(int i=1;i<lth;i++)
		{
			f_area[i]=min+size*i;
		}
	}

}
