using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class new_date_input {
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
	public void ElemSoloution(GameObject father, GameObject obj, Vector3[] vNodes, float[] fElemSlns, int[] Inds, int LAN, Color[] cData){
		//			this.transform.GetComponent<color_sample>().clear();
		ElemClear();
		//			Debug.Log(vNodes.Length+" "+fElemSlns.Length);
		//处理色带
		Color_Set (ref cData);

		Vector3[] vNodeCoord = new Vector3[]{};//保存节点坐标
		float[] fElemValues;
		//获得节点坐标
		GetNodeCoord (ref vNodeCoord, vNodes);
		//基于变形物体上的孕吐显示//若考虑不基于变形物体可设置为0
		NodeCoordFunction(vNodeCoord, Oiginaldata.DISPLACEMENT, 1, 80);//需要设定参数
		create_f_area(fElemSlns, cData.Length);


		color_change_new cc_script=new color_change_new();
		cc_script.init(cData.Length,f_area);
		//四边形模型
		if (LAN == 4) {
			for (int i = 0; i < Inds.Length / 4; i++) {//
				cc_script.deal_tria (fElemSlns [4 * i + 0], fElemSlns [4 * i + 1], fElemSlns [4 * i + 2], fElemSlns [4 * i + 3],
					vNodeCoord [Inds [4 * i + 0] - 1], vNodeCoord [Inds [4 * i + 1] - 1], vNodeCoord [Inds [4 * i + 2] - 1], vNodeCoord [Inds [4 * i + 3] - 1]);
			}
		} else {
			for (int i = 0; i < Inds.Length / 3; i++) {//
				cc_script.deal_tria (fElemSlns [3 * i + 0], fElemSlns [3 * i + 1], fElemSlns [3 * i + 2], fElemSlns [3 * i + 2],
					vNodeCoord [Inds [3 * i + 0]], vNodeCoord [Inds [3 * i + 1]], vNodeCoord [Inds [3 * i + 2]], vNodeCoord [Inds [3 * i + 2]]);
			}
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
		Shader ss=obj.GetComponent<MeshRenderer>().sharedMaterials[0].shader;
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
			c_L=new Color[9];
			c_L[0]=new Color(0,0,255/255.0f);
			c_L[1]=new Color(0,180/255.0f,255/255.0f);
			c_L[2]=new Color(0,255/255.0f,255/255.0f);
			c_L[3]=new Color(0,255/255.0f,140/255.0f);
			c_L[4]=new Color(0,255/255.0f,0);
			c_L[5]=new Color(180/255.0f,255/255.0f,0);
			c_L[6]=new Color(255/255.0f,255/255.0f,0);
			c_L[7]=new Color(255/255.0f,180/255.0f,0);
			c_L[8]=new Color(255/255.0f,0,0);

//			c_L = new Color[16];//设置色带
//			c_L[0] = new Color(255,3,0);
//			c_L[1] = new Color(255,68,3);
//			c_L[2] = new Color(255,136,13);
//			c_L[3] = new Color(255,205,26);
//			c_L[4] = new Color(238,255,34);
//			c_L[5] = new Color(170,255,34);
//			c_L[6] = new Color(101,255,33);
//			c_L[7] = new Color(37,255,37);
//			c_L[8] = new Color(12,255,53);
//			c_L[9] = new Color(11,255,108);
//			c_L[10] = new Color(29,255,174);
//			c_L[11] = new Color(64,255,239);
//			c_L[12] = new Color(26,205,255);
//			c_L[13] = new Color(9,135,255);
//			c_L[14] = new Color(16,66,255);
//			c_L[15] = new Color(21,0,255);
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
	//处理节点坐标
	//v:节点坐标
	//p:位移形变量
	//s1：节点坐标缩放倍数
	//s2:位移形变量缩放倍数
	void NodeCoordFunction(Vector3[] v, Vector3[] p, float s1, float s2)
	{
		for(int i=0;i<v.Length;i++)
		{
			//此处有坐标变换，可根据需求进行修改
			if (p != null) {
				v[i]=new Vector3(v[i].x ,0,v[i].y) * s1 + new Vector3(p[i].x,0,p[i].y) * s2;
			} else {
				v[i]=new Vector3(v[i].x ,0,v[i].y) * s1;
			}

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
		Debug.Log (max);
		Debug.Log (min);
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
