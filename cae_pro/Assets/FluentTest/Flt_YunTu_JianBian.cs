using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flt_YunTu_JianBian  
{

    public static Dictionary<string, List<double>[]> dGD_Store = new Dictionary<string, List<double>[]>();
    public static Dictionary<string, eDisplayStyle> dGD_DisplayStore = new Dictionary<string, eDisplayStyle>();
   //记录显示状态，用于标记：单面模型云图，单面模型普通，双面模型云图，双面模型普通
    public enum eDisplayStyle
    {
        single_yun = 0,
        single_mat,
        double_yun,
        double_mat
    }

	/// <summary>
	/// 获得根据有限元数据值对应的模型云图
	/// </summary>
	/// <param name="obj">原始模型（保证有meshfilter和meshrender）.</param>
	/// <param name="gdd">有限元数据值.</param>
	/// <param name="indexer">物理量编号.</param>
    /// <param name="max">物理量最大值（用于渐变色的显示）.</param>
    /// <param name="min">物理量最小值（用于渐变色的显示）.</param>
	/// <param name="isVerse">I <c>true</c>默认为true，用于设置面片的外法线.</param>
	/// <param name="Scale">模型数据缩放倍数.</param>
	public void ShowColors(GameObject obj, GeneralData gdd, int indexer, double max, double min, bool isVerse, float Scale)
	{
		if (obj == null) {
			return;
		}
        //记录状态
        eDisplayStyle ds;
        ds = eDisplayStyle.single_yun;
        dGD_DisplayStore.Add(obj.name, ds);
        //记录数据
        dGD_Store.Add(obj.name, gdd.nSlnTypeValue);


		int NodeNumber = gdd.NodeCoord.Count;
        int ElemNumber = gdd.Elem_NodesIndex.Count;
		//获取模型顶点坐标值
		Vector3[] pos = new Vector3[NodeNumber];
		for (int i = 0; i < NodeNumber; i++) {
			pos [i] = new Vector3 ((float)gdd.NodeCoord[i][0] * Scale, (float)gdd.NodeCoord[i][1] * Scale, (float)gdd.NodeCoord[i][2] * Scale);
		}
		
        if (pos.Length < 65000)
        {



            //获取顶点的检索值
            int[] idx = new int[ElemNumber * 3];
            for (int j = 0; j < ElemNumber; j++)
            {
                idx[j * 3 + 0] = gdd.Elem_NodesIndex[j][0];
                idx[j * 3 + 1] = isVerse ? gdd.Elem_NodesIndex[j][2] : gdd.Elem_NodesIndex[j][1];
                idx[j * 3 + 2] = isVerse ? gdd.Elem_NodesIndex[j][1] : gdd.Elem_NodesIndex[j][2];
            }
            //设置顶点的颜色
            Color[] cor = new Color[NodeNumber];
            if (indexer >= gdd.nSlnTypeNames.Count)
            {
                Debug.Log("错误： 物理量序号超出范围，请确认！");
                for (int i = 0; i < NodeNumber; i++)
                {
                    cor[i] = Color.HSVToRGB(0, 1, 1);//将HSV的颜色值转为RGB的颜色值
                }
            }
            else
            {
                double fenmu = max - min;
                double value = 0;
                for (int i = 0; i < NodeNumber; i++)
                {
                    value = (max - gdd.nSlnTypeValue[indexer][i]) / fenmu;
                    value = value < 0 ? 0 : value;
                    value = value > 1 ? 1 : value;
                    cor[i] = Color.HSVToRGB((float)value * 2 / 3, 1, 1);//将HSV的颜色值转为RGB的颜色值
                }
            }
            Mesh mr = obj.GetComponent<MeshFilter>().mesh;//获取物体的mesh
            mr.Clear();//清空当前mesh
            mr.vertices = pos;//设置顶点
            mr.triangles = idx;//设置顶点将所致
            mr.colors = cor;//设置顶点颜色
            mr.RecalculateBounds();//
            mr.RecalculateNormals();//计算顶点的法向值
            if (obj.GetComponent<MeshCollider>() == null) 
            {
                obj.AddComponent<MeshCollider>();
            }
        }
        else {
            //修改格式为：
            //Tristan：012345678
            //vertice:点012345678
            //保证每个点索引都有一个点坐标，保证与索引一一对应。
            //以上方法的缺点是点数据有很多的冗余
            //以上方法便于模型的分割
            int[] tris = new int[ElemNumber * 3];
            Vector3[] poss = new Vector3[ElemNumber * 3];
            Color[] cors = new Color[ElemNumber * 3];

            double fenmu = max - min;
            double value = 0;
            for (int i = 0; i < ElemNumber; i++)
            {
                tris[3 * i + 0] = 3 * i + 0;
                tris[3 * i + 1] = 3 * i + 1;
                tris[3 * i + 2] = 3 * i + 2;
                poss[3 * i + 0] = new Vector3((float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][0]][0] * Scale,
                    (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][0]][2] * Scale, (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][0]][1] * Scale);
                poss[3 * i + 1] = new Vector3((float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][1]][0] * Scale,
                    (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][1]][2] * Scale, (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][1]][1] * Scale);
                poss[3 * i + 2] = new Vector3((float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][2]][0] * Scale,
                    (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][2]][2] * Scale, (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][2]][1] * Scale);
                value = (max - gdd.nSlnTypeValue[indexer][gdd.Elem_NodesIndex[i][0]]) / fenmu;
                value = value < 0 ? 0 : value;
                value = value > 1 ? 1 : value;
                cors[3 * i + 0] = Color.HSVToRGB((float)value * 2 / 3, 1, 1);//将HSV的颜色值转为RGB的颜色值
                value = (max - gdd.nSlnTypeValue[indexer][gdd.Elem_NodesIndex[i][1]]) / fenmu;
                value = value < 0 ? 0 : value;
                value = value > 1 ? 1 : value;
                cors[3 * i + 1] = Color.HSVToRGB((float)value * 2 / 3, 1, 1);//将HSV的颜色值转为RGB的颜色值
                value = (max - gdd.nSlnTypeValue[indexer][gdd.Elem_NodesIndex[i][2]]) / fenmu;
                value = value < 0 ? 0 : value;
                value = value > 1 ? 1 : value;
                cors[3 * i + 2] = Color.HSVToRGB((float)value * 2 / 3, 1, 1);//将HSV的颜色值转为RGB的颜色值
            }
            int maxObjLen = ElemNumber / 21666;
            int lastObjLen = ElemNumber % 21666;
            if (lastObjLen != 0)
                maxObjLen++;

            int MaxVerticeCount = 21666 * 3;//最大顶点数
            //Debug.Log(" :  " + MaxVerticeCount);

            int[] m_tri = new int[] { };//声明检索
            Vector3[] m_ver = new Vector3[] { };//声明顶点
            Color[] m_cor = new Color[] { };//声明颜色
            int VerticeCount = 0;//声明当前顶点数
            GameObject[] objShow = new GameObject[maxObjLen];
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow[i] = GameObject.Instantiate(obj);
                objShow[i].name = obj.name + "_" + i.ToString();
                Mesh mr = objShow[i].GetComponent<MeshFilter>().mesh;//获取物体的mesh
                mr.Clear();//清空当前mesh
                VerticeCount = i < maxObjLen - 1 ? MaxVerticeCount : lastObjLen * 3;
                m_tri = new int[VerticeCount];
                m_ver = new Vector3[VerticeCount];
                m_cor = new Color[VerticeCount];
                for (int j = 0; j < VerticeCount; j++)
                {
                    m_tri[j] = tris[i * MaxVerticeCount + j] - i * MaxVerticeCount;
                    m_ver[j] = poss[i * MaxVerticeCount + j];
                    m_cor[j] = cors[i * MaxVerticeCount + j];
                }
                mr.vertices = m_ver;//设置顶点
                mr.triangles = m_tri;//设置顶点将所致
                mr.colors = m_cor;//设置顶点颜色
                //mr.RecalculateBounds();//
                //mr.RecalculateNormals();//计算顶点的法向值
            }
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow[i].transform.parent = obj.transform;
            }
        }
        
	}

    /// <summary>
    /// 获得根据有限元数据值对应的模型(默认材质显示)
    /// </summary>
    /// <param name="obj">原始模型.</param>
    /// <param name="gdd">有限元数据值.</param>
    /// <param name="mat">物体材质.</param>
    /// <param name="isVerse">I <c>true</c>默认为true，用于设置面片的外法线.</param>
    /// <param name="Scale">模型数据缩放倍数.</param>
    public void ShowObjMat(GameObject obj, GeneralData gdd, Material mat, bool isVerse, float Scale)
    {
        if (obj == null)
        {
            return;
        }
        int NodeNumber = gdd.NodeCoord.Count;
        int ElemNumber = gdd.Elem_NodesIndex.Count;
        //获取模型顶点坐标值
        Vector3[] pos = new Vector3[NodeNumber];
        for (int i = 0; i < NodeNumber; i++)
        {
            pos[i] = new Vector3((float)gdd.NodeCoord[i][0] * Scale, (float)gdd.NodeCoord[i][1] * Scale, (float)gdd.NodeCoord[i][2] * Scale);
        }

        if (pos.Length < 65000)
        {
            //获取顶点的检索值
            int[] idx = new int[ElemNumber * 3];
            for (int j = 0; j < ElemNumber; j++)
            {
                idx[j * 3 + 0] = gdd.Elem_NodesIndex[j][0];
                idx[j * 3 + 1] = isVerse ? gdd.Elem_NodesIndex[j][2] : gdd.Elem_NodesIndex[j][1];
                idx[j * 3 + 2] = isVerse ? gdd.Elem_NodesIndex[j][1] : gdd.Elem_NodesIndex[j][2];
            }
            Mesh mr = obj.GetComponent<MeshFilter>().mesh;//获取物体的mesh
            mr.Clear();//清空当前mesh
            mr.vertices = pos;//设置顶点
            mr.triangles = idx;//设置顶点将所致
            obj.GetComponent<MeshRenderer>().material = mat;
        }
        else
        {
            //修改格式为：
            //Tristan：012345678
            //vertice:点012345678
            //保证每个点索引都有一个点坐标，保证与索引一一对应。
            //以上方法的缺点是点数据有很多的冗余
            //以上方法便于模型的分割
            int[] tris = new int[ElemNumber * 3];
            Vector3[] poss = new Vector3[ElemNumber * 3];
            for (int i = 0; i < ElemNumber; i++)
            {
                tris[3 * i + 0] = 3 * i + 0;
                tris[3 * i + 1] = 3 * i + 1;
                tris[3 * i + 2] = 3 * i + 2;
                poss[3 * i + 0] = new Vector3((float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][0]][0] * Scale,
                    (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][0]][2] * Scale, (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][0]][1] * Scale);
                poss[3 * i + 1] = new Vector3((float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][1]][0] * Scale,
                    (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][1]][2] * Scale, (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][1]][1] * Scale);
                poss[3 * i + 2] = new Vector3((float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][2]][0] * Scale,
                    (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][2]][2] * Scale, (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][2]][1] * Scale);
            }
            int maxObjLen = ElemNumber / 21666;
            int lastObjLen = ElemNumber % 21666;
            if (lastObjLen != 0)
                maxObjLen++;

            int MaxVerticeCount = 21666 * 3;//最大顶点数
            //Debug.Log(" :  " + MaxVerticeCount);

            int[] m_tri = new int[] { };//声明检索
            Vector3[] m_ver = new Vector3[] { };//声明顶点
            int VerticeCount = 0;//声明当前顶点数
            GameObject[] objShow = new GameObject[maxObjLen];
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow[i] = GameObject.Instantiate(obj);
                objShow[i].name = obj.name + "_" + i.ToString();
                Mesh mr = objShow[i].GetComponent<MeshFilter>().mesh;//获取物体的mesh
                mr.Clear();//清空当前mesh
                VerticeCount = i < maxObjLen - 1 ? MaxVerticeCount : lastObjLen * 3;
                m_tri = new int[VerticeCount];
                m_ver = new Vector3[VerticeCount];
                for (int j = 0; j < VerticeCount; j++)
                {
                    m_tri[j] = tris[i * MaxVerticeCount + j] - i * MaxVerticeCount;
                    m_ver[j] = poss[i * MaxVerticeCount + j];
                }
                mr.vertices = m_ver;//设置顶点
                mr.triangles = m_tri;//设置顶点将所致
            }
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow[i].transform.parent = obj.transform;
                objShow[i].GetComponent<MeshRenderer>().material = mat;
            }
        }

    }


	/// <summary>
	/// 获得根据有限元数据值对应的模型云图
	/// 1.区别于第一种，该种用于显示双层模型，防止从内部观察时出现看不到物体的现象
	/// 2.原理，用两个物体构建基于同一套坐标数据的正反法向的模型
	/// </summary>
	/// <param name="obj">原始模型.</param>
	/// <param name="obj_1">原始模型1.</param>
	/// <param name="gdd">有限元数据值.</param>
	/// <param name="max">颜色为红色（标尺上端）对应的最大值.</param>
	/// <param name="min">颜色为蓝色（标尺下端）对应的最小值.</param>
	/// <param name="indexer">物理量编号.</param>
	/// <param name="isVerse">I <c>true</c>默认为true，用于设置面片的外法线.</param>
	/// <param name="Scale">模型数据缩放倍数.</param>
	public void ShowColors(GameObject obj, GameObject obj_1, GeneralData gdd, int indexer, double max, double min, bool isVerse, float Scale)
	{
		if (obj == null) {
			return;
		}
        //vertices的数量大于65000
		int NodeNumber = gdd.NodeCoord.Count;
        int ElemNumber = gdd.Elem_NodesIndex.Count;
		//获取模型顶点坐标值
		Vector3[] pos = new Vector3[NodeNumber];
		for (int i = 0; i < NodeNumber; i++) {
			pos [i] = new Vector3 ((float)gdd.NodeCoord[i][0] * Scale, (float)gdd.NodeCoord[i][2] * Scale, (float)gdd.NodeCoord[i][1] * Scale);
		}
		
        //判断顶点的个数是否超过65000
        if (pos.Length < 65000)
        {
            //获取顶点的检索值
            int[] idx = new int[ElemNumber * 3];
            for (int j = 0; j < ElemNumber; j++)
            {
                idx[j * 3 + 0] = gdd.Elem_NodesIndex[j][0];
                idx[j * 3 + 1] = isVerse ? gdd.Elem_NodesIndex[j][2] : gdd.Elem_NodesIndex[j][1];
                idx[j * 3 + 2] = isVerse ? gdd.Elem_NodesIndex[j][1] : gdd.Elem_NodesIndex[j][2];
            }
            //设置顶点的颜色
            Color[] cor = new Color[NodeNumber];
            if (indexer >= gdd.nSlnTypeNames.Count)
            {
                Debug.Log("错误： 物理量序号超出范围，请确认！");
                for (int i = 0; i < NodeNumber; i++)
                {
                    cor[i] = Color.HSVToRGB(0, 1, 1);//将HSV的颜色值转为RGB的颜色值//全为红色
                }
            }
            else
            {
                double fenmu_c = max - min;
                double value_c = 0;
                for (int i = 0; i < NodeNumber; i++)
                {
                    value_c = (max - gdd.nSlnTypeValue[indexer][i]) / fenmu_c;
                    value_c = value_c < 0 ? 0 : value_c;
                    value_c = value_c > 1 ? 1 : value_c;
                    cor[i] = Color.HSVToRGB((float)value_c * 2 / 3, 1, 1);//将HSV的颜色值转为RGB的颜色值
                }
            }
            Mesh mr = obj.GetComponent<MeshFilter>().mesh;//获取物体的mesh
            mr.Clear();//清空当前mesh
            mr.vertices = pos;//设置顶点
            mr.triangles = idx;//设置顶点将所致
            mr.colors = cor;//设置顶点颜色
            //mr.RecalculateBounds();//
            //mr.RecalculateNormals();//计算顶点的法向值

            //for (int i = 0; i < 30; i++)
            //{
            //    Debug.Log(i + " :  " + mr.vertices[mr.triangles[i]].x + "  " + mr.vertices[mr.triangles[i]].y + "  " + mr.vertices[mr.triangles[i]].z);
            //    Debug.Log(i + " :  " + mr.colors[mr.triangles[i]].r + "  " + mr.colors[mr.triangles[i]].g + "  " + mr.colors[mr.triangles[i]].b);
            //}

            Mesh mr_1 = obj_1.GetComponent<MeshFilter>().mesh;//获取物体的mesh
            mr_1.Clear();//清空当前mesh
            //获取顶点的检索值
            int[] idx_1 = new int[ElemNumber * 3];
            for (int j = 0; j < ElemNumber; j++)
            {
                idx_1[j * 3 + 0] = gdd.Elem_NodesIndex[j][0];
                idx[j * 3 + 1] = isVerse ? gdd.Elem_NodesIndex[j][1] : gdd.Elem_NodesIndex[j][2];
                idx[j * 3 + 2] = isVerse ? gdd.Elem_NodesIndex[j][2] : gdd.Elem_NodesIndex[j][1];
            }

            mr_1.vertices = pos;//设置顶点
            mr_1.triangles = idx_1;//设置顶点将所致
            mr_1.colors = cor;//设置顶点颜色
            mr_1.RecalculateBounds();//
            mr_1.RecalculateNormals();//计算顶点的法向值
        }
        else {
            //修改格式为：
            //Tristan：012345678
            //vertice:点012345678
            //保证每个点索引都有一个点坐标，保证与索引一一对应。
            //以上方法的缺点是点数据有很多的冗余
            //以上方法便于模型的分割
            int[] tris = new int[ElemNumber * 3];
            Vector3[] poss = new Vector3[ElemNumber * 3];
            Color[] cors = new Color[ElemNumber * 3];

            double fenmu = max - min;
            double value = 0;
            for (int i = 0; i < ElemNumber; i++)
            {
                tris[3 * i + 0] = 3 * i + 0;
                tris[3 * i + 1] = 3 * i + 1;
                tris[3 * i + 2] = 3 * i + 2;
                poss[3 * i + 0] = new Vector3((float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][0]][0] * Scale,
                    (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][0]][2] * Scale, (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][0]][1] * Scale);
                poss[3 * i + 1] = new Vector3((float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][1]][0] * Scale,
                    (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][1]][2] * Scale, (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][1]][1] * Scale);
                poss[3 * i + 2] = new Vector3((float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][2]][0] * Scale,
                    (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][2]][2] * Scale, (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][2]][1] * Scale);
                value = (max - gdd.nSlnTypeValue[indexer][gdd.Elem_NodesIndex[i][0]]) / fenmu;
                value = value < 0 ? 0 : value;
                value = value > 1 ? 1 : value;
                cors[3 * i + 0] = Color.HSVToRGB((float)value * 2 / 3, 1, 1);//将HSV的颜色值转为RGB的颜色值
                value = (max - gdd.nSlnTypeValue[indexer][gdd.Elem_NodesIndex[i][1]]) / fenmu;
                value = value < 0 ? 0 : value;
                value = value > 1 ? 1 : value;
                cors[3 * i + 1] = Color.HSVToRGB((float)value * 2 / 3, 1, 1);//将HSV的颜色值转为RGB的颜色值
                value = (max - gdd.nSlnTypeValue[indexer][gdd.Elem_NodesIndex[i][2]]) / fenmu;
                value = value < 0 ? 0 : value;
                value = value > 1 ? 1 : value;
                cors[3 * i + 2] = Color.HSVToRGB((float)value * 2 / 3, 1, 1);//将HSV的颜色值转为RGB的颜色值
            }
            //for (int i = 0; i < poss.Length; i++)
            //{
            //    Debug.Log(i + " :  " + poss[i].x + "  " + poss[i].y + "  " + poss[i].z);
            //    Debug.Log(i + " :  " + cors[i].r + "  " + cors[i].g + "  " + cors[i].b);
            //}
            int maxObjLen = ElemNumber/21666;
            int lastObjLen = ElemNumber % 21666;
            if(lastObjLen != 0)
                maxObjLen ++;

            int MaxVerticeCount = 21666 * 3;//最大顶点数
            Debug.Log(" :  " + MaxVerticeCount);

            int[] m_tri = new int[] { };//声明检索
            Vector3[] m_ver = new Vector3[] { };//声明顶点
            Color[] m_cor = new Color[] { };//声明颜色
            int VerticeCount = 0;//声明当前顶点数
            GameObject[] objShow = new GameObject[maxObjLen];
            for (int i = 0; i < maxObjLen; i++) 
            {
                objShow[i] = GameObject.Instantiate(obj);
                objShow[i].name = obj.name + "_" + i.ToString();
                Mesh mr = objShow[i].GetComponent<MeshFilter>().mesh;//获取物体的mesh
                mr.Clear();//清空当前mesh
                VerticeCount = i < maxObjLen - 1 ? MaxVerticeCount : lastObjLen * 3;
                m_tri = new int[VerticeCount];
                m_ver = new Vector3[VerticeCount];
                m_cor = new Color[VerticeCount];
                for (int j = 0; j < VerticeCount; j++) 
                {
                    m_tri[j] = tris[i * MaxVerticeCount + j] - i * MaxVerticeCount;
                    m_ver[j] = poss[i * MaxVerticeCount + j];
                    m_cor[j] = cors[i * MaxVerticeCount + j];
                }
                mr.vertices = m_ver;//设置顶点
                mr.triangles = m_tri;//设置顶点将所致
                mr.colors = m_cor;//设置顶点颜色
                //mr.RecalculateBounds();//
                //mr.RecalculateNormals();//计算顶点的法向值
            }
            //for (int i = 0; i < maxObjLen; i++)
            //{
            //    objShow[i].transform.parent = obj.transform;
            //}

            GameObject[] objShow_1 = new GameObject[maxObjLen];
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow_1[i] = GameObject.Instantiate(obj_1);
                objShow_1[i].name = obj_1.name + "_" + i.ToString();
                Mesh mr = objShow_1[i].GetComponent<MeshFilter>().mesh;//获取物体的mesh
                mr.Clear();//清空当前mesh
                VerticeCount = i < maxObjLen - 1 ? MaxVerticeCount : lastObjLen * 3;
                m_tri = new int[VerticeCount];
                m_ver = new Vector3[VerticeCount];
                m_cor = new Color[VerticeCount];
                for (int j = 0; j < VerticeCount; j++)
                {
                    //设置外法向的方向
                    m_tri[j] = 
                        j % 3 == 1 ? tris[i * MaxVerticeCount + j] - i * MaxVerticeCount + 1 : 
                        j % 3 == 2 ? tris[i * MaxVerticeCount + j] - i * MaxVerticeCount - 1 : 
                        tris[i * MaxVerticeCount + j] - i * MaxVerticeCount;
                    m_ver[j] = poss[i * MaxVerticeCount + j];
                    m_cor[j] = cors[i * MaxVerticeCount + j];
                }
                mr.vertices = m_ver;//设置顶点
                mr.triangles = m_tri;//设置顶点将所致
                mr.colors = m_cor;//设置顶点颜色
                mr.RecalculateBounds();//
                mr.RecalculateNormals();//计算顶点的法向值
            }
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow[i].transform.parent = obj.transform;
                objShow_1[i].transform.parent = obj_1.transform;
            }
        }
	}

    /// <summary>
    /// 获得根据有限元数据值对应的模型(默认材质显示)
    /// 1.区别于第一种，该种用于显示双层模型，防止从内部观察时出现看不到物体的现象
    /// 2.原理，用两个物体构建基于同一套坐标数据的正反法向的模型
    /// </summary>
    /// <param name="obj">原始模型.</param>
    /// <param name="obj_1">原始模型1.</param>
    /// <param name="gdd">有限元数据值.</param>
    /// <param name="mat">模型的材质.</param>
    /// <param name="indexer">物理量编号.</param>
    /// <param name="Scale">模型数据缩放倍数.</param>
    public void ShowObjMat(GameObject obj, GameObject obj_1, GeneralData gdd, Material mat, float Scale)
    {
        if (obj == null || obj_1 == null)
        {
            return;
        }
        //vertices的数量大于65000
        int NodeNumber = gdd.NodeCoord.Count;
        int ElemNumber = gdd.Elem_NodesIndex.Count;
        //获取模型顶点坐标值
        Vector3[] pos = new Vector3[NodeNumber];
        for (int i = 0; i < NodeNumber; i++)
        {
            pos[i] = new Vector3((float)gdd.NodeCoord[i][0] * Scale, (float)gdd.NodeCoord[i][2] * Scale, (float)gdd.NodeCoord[i][1] * Scale);
        }

        //判断顶点的个数是否超过65000
        if (pos.Length < 65000)
        {
            //获取顶点的检索值
            int[] idx = new int[ElemNumber * 3];
            for (int j = 0; j < ElemNumber; j++)
            {
                idx[j * 3 + 0] = gdd.Elem_NodesIndex[j][0];
                idx[j * 3 + 1] = gdd.Elem_NodesIndex[j][2];
                idx[j * 3 + 2] = gdd.Elem_NodesIndex[j][1];
            }

            Mesh mr = obj.GetComponent<MeshFilter>().mesh;//获取物体的mesh
            mr.Clear();//清空当前mesh
            mr.vertices = pos;//设置顶点
            mr.triangles = idx;//设置顶点将所致
            obj.GetComponent<MeshRenderer>().material = mat;

            Mesh mr_1 = obj_1.GetComponent<MeshFilter>().mesh;//获取物体的mesh
            mr_1.Clear();//清空当前mesh
            //获取顶点的检索值
            int[] idx_1 = new int[ElemNumber * 3];
            for (int j = 0; j < ElemNumber; j++)
            {
                idx_1[j * 3 + 0] = gdd.Elem_NodesIndex[j][0];
                idx_1[j * 3 + 1] = gdd.Elem_NodesIndex[j][1];
                idx_1[j * 3 + 2] = gdd.Elem_NodesIndex[j][2];
            }
            mr_1.vertices = pos;//设置顶点
            mr_1.triangles = idx_1;//设置顶点将所致
            obj_1.GetComponent<MeshRenderer>().material = mat;
        }
        else
        {
            //修改格式为：
            //Tristan：012345678
            //vertice:点012345678
            //保证每个点索引都有一个点坐标，保证与索引一一对应。
            //以上方法的缺点是点数据有很多的冗余
            //以上方法便于模型的分割
            int[] tris = new int[ElemNumber * 3];
            Vector3[] poss = new Vector3[ElemNumber * 3];
            Color[] cors = new Color[ElemNumber * 3];
            for (int i = 0; i < ElemNumber; i++)
            {
                tris[3 * i + 0] = 3 * i + 0;
                tris[3 * i + 1] = 3 * i + 1;
                tris[3 * i + 2] = 3 * i + 2;
                poss[3 * i + 0] = new Vector3((float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][0]][0] * Scale,
                    (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][0]][2] * Scale, (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][0]][1] * Scale);
                poss[3 * i + 1] = new Vector3((float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][1]][0] * Scale,
                    (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][1]][2] * Scale, (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][1]][1] * Scale);
                poss[3 * i + 2] = new Vector3((float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][2]][0] * Scale,
                    (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][2]][2] * Scale, (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][2]][1] * Scale);
            }
            int maxObjLen = ElemNumber / 21666;
            int lastObjLen = ElemNumber % 21666;
            if (lastObjLen != 0)
                maxObjLen++;

            int MaxVerticeCount = 21666 * 3;//最大顶点数

            int[] m_tri = new int[] { };//声明检索
            Vector3[] m_ver = new Vector3[] { };//声明顶点
            int VerticeCount = 0;//声明当前顶点数
            Mesh mr = new Mesh();
            GameObject[] objShow = new GameObject[maxObjLen];
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow[i] = GameObject.Instantiate(obj);
                objShow[i].name = obj.name + "_" + i.ToString();
                mr = objShow[i].GetComponent<MeshFilter>().mesh;//获取物体的mesh
                mr.Clear();//清空当前mesh
                VerticeCount = i < maxObjLen - 1 ? MaxVerticeCount : lastObjLen * 3;
                m_tri = new int[VerticeCount];
                m_ver = new Vector3[VerticeCount];
                for (int j = 0; j < VerticeCount; j++)
                {
                    m_tri[j] = tris[i * MaxVerticeCount + j] - i * MaxVerticeCount;
                    m_ver[j] = poss[i * MaxVerticeCount + j];
                }
                mr.vertices = m_ver;//设置顶点
                mr.triangles = m_tri;//设置顶点将所致
            }
            GameObject[] objShow_1 = new GameObject[maxObjLen];
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow_1[i] = GameObject.Instantiate(obj_1);
                objShow_1[i].name = obj_1.name + "_" + i.ToString();
                mr = objShow_1[i].GetComponent<MeshFilter>().mesh;//获取物体的mesh
                mr.Clear();//清空当前mesh
                VerticeCount = i < maxObjLen - 1 ? MaxVerticeCount : lastObjLen * 3;
                m_tri = new int[VerticeCount];
                m_ver = new Vector3[VerticeCount];
                for (int j = 0; j < VerticeCount; j++)
                {
                    //设置外法向的方向
                    m_tri[j] =
                        j % 3 == 1 ? tris[i * MaxVerticeCount + j] - i * MaxVerticeCount + 1 :
                        j % 3 == 2 ? tris[i * MaxVerticeCount + j] - i * MaxVerticeCount - 1 :
                        tris[i * MaxVerticeCount + j] - i * MaxVerticeCount;
                    m_ver[j] = poss[i * MaxVerticeCount + j];
                }
                mr.vertices = m_ver;//设置顶点
                mr.triangles = m_tri;//设置顶点将所致
            }
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow[i].transform.parent = obj.transform;
                objShow_1[i].transform.parent = obj_1.transform;
                objShow[i].GetComponent<MeshRenderer>().material = mat;
                objShow_1[i].GetComponent<MeshRenderer>().material = mat;
            }
        }
    }


    public void NewShowObjMat(GameObject obj, GeneralData gdd, Material mat, bool isVerse, float Scale)
    {
        if (obj == null)
        {
            return;
        }
        int NodeNumber = gdd.NodeCoord.Count;
        int ElemNumber = gdd.Elem_NodesIndex.Count;
        //获取模型顶点坐标值
        Vector3[] pos = new Vector3[NodeNumber];
        for (int i = 0; i < NodeNumber; i++)
        {
            pos[i] = new Vector3((float)gdd.NodeCoord[i][0] * Scale, (float)gdd.NodeCoord[i][1] * Scale, (float)gdd.NodeCoord[i][2] * Scale);
        }

        Debug.Log(gdd.Elem_NodesIndex.Count);

        if (gdd.Elem_NodesIndex.Count < 65000/3)
        {
            //获取顶点的检索值
            int[] idx = new int[ElemNumber * 3];
            for (int j = 0; j < ElemNumber; j++)
            {
                idx[j * 3 + 0] = gdd.Elem_NodesIndex[j][0];
                idx[j * 3 + 1] = isVerse ? gdd.Elem_NodesIndex[j][2] : gdd.Elem_NodesIndex[j][1];
                idx[j * 3 + 2] = isVerse ? gdd.Elem_NodesIndex[j][1] : gdd.Elem_NodesIndex[j][2];
            }
            Mesh mr = obj.GetComponent<MeshFilter>().mesh;//获取物体的mesh
            mr.Clear();//清空当前mesh
            mr.vertices = pos;//设置顶点
            mr.triangles = idx;//设置顶点将所致
            obj.GetComponent<MeshRenderer>().material = mat;
        }
        else
        {
            //修改格式为：
            //Tristan：012345678
            //vertice:点012345678
            //保证每个点索引都有一个点坐标，保证与索引一一对应。
            //以上方法的缺点是点数据有很多的冗余
            //以上方法便于模型的分割
            int[] tris = new int[ElemNumber * 3];
            Vector3[] poss = new Vector3[ElemNumber * 3];
            for (int i = 0; i < ElemNumber; i++)
            {
                tris[3 * i + 0] = 3 * i + 0;
                tris[3 * i + 1] = 3 * i + 1;
                tris[3 * i + 2] = 3 * i + 2;
                poss[3 * i + 0] = new Vector3((float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][0] - 1][0] * Scale,
                    (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][0] - 1][2] * Scale, (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][0] - 1][1] * Scale);
                poss[3 * i + 1] = new Vector3((float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][1] - 1][0] * Scale,
                    (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][1] - 1][2] * Scale, (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][1] - 1][1] * Scale);
                poss[3 * i + 2] = new Vector3((float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][2] - 1][0] * Scale,
                    (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][2] - 1][2] * Scale, (float)gdd.NodeCoord[gdd.Elem_NodesIndex[i][2] - 1][1] * Scale);
            }
            int maxObjLen = ElemNumber / 21666;
            int lastObjLen = ElemNumber % 21666;
            if (lastObjLen != 0)
                maxObjLen++;

            int MaxVerticeCount = 21666 * 3;//最大顶点数
            //Debug.Log(" :  " + MaxVerticeCount);

            int[] m_tri = new int[] { };//声明检索
            Vector3[] m_ver = new Vector3[] { };//声明顶点
            int VerticeCount = 0;//声明当前顶点数
            GameObject[] objShow = new GameObject[maxObjLen];
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow[i] = GameObject.Instantiate(obj);
                objShow[i].name = obj.name + "_" + i.ToString();
                Mesh mr = objShow[i].GetComponent<MeshFilter>().mesh;//获取物体的mesh
                mr.Clear();//清空当前mesh
                VerticeCount = i < maxObjLen - 1 ? MaxVerticeCount : lastObjLen * 3;
                m_tri = new int[VerticeCount];
                m_ver = new Vector3[VerticeCount];
                for (int j = 0; j < VerticeCount; j++)
                {
                    m_tri[j] = tris[i * MaxVerticeCount + j] - i * MaxVerticeCount;
                    m_ver[j] = poss[i * MaxVerticeCount + j];
                }
                mr.vertices = m_ver;//设置顶点
                mr.triangles = m_tri;//设置顶点将所致
            }
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow[i].transform.parent = obj.transform;
                objShow[i].GetComponent<MeshRenderer>().material = mat;
            }
        }
    }

    public void NewShowObjMat(GameObject obj, GameObject obj_1, GeneralData gdd, Material mat, bool isVerse, float Scale)
    {
        if (obj == null || obj_1 == null)
        {
            return;
        }
        GameObject gg =  GameObject.Find(obj.name + "_0");
        if (gg != null) {
            GameObject.DestroyImmediate(gg);
        }
        gg = GameObject.Find(obj.name + "_1");
        if (gg != null)
        {
            GameObject.DestroyImmediate(gg);
        }
        gg = GameObject.Find(obj_1.name + "_0");
        if (gg != null)
        {
            GameObject.DestroyImmediate(gg);
        }
        gg = GameObject.Find(obj_1.name + "_1");
        if (gg != null)
        {
            GameObject.DestroyImmediate(gg);
        }

        obj.GetComponent<MeshRenderer>().sharedMaterial = mat;
        obj_1.GetComponent<MeshRenderer>().sharedMaterial = mat;
       
        //vertices的数量大于65000
        int NodeNumber = gdd.NodeCoord.Count;
        int ElemNumber = gdd.Elem_NodesIndex.Count;
        //获取模型顶点坐标值
        Vector3[] pos = new Vector3[NodeNumber];
        for (int i = 0; i < NodeNumber; i++)
        {
            pos[i] = new Vector3((float)(gdd.NodeCoord[i][0] + gdd.NodeOffPos[i][0]) * Scale,
                (float)(gdd.NodeCoord[i][2] + gdd.NodeOffPos[i][2]) * Scale, (float)(gdd.NodeCoord[i][1] + gdd.NodeOffPos[i][1]) * Scale);
        }

        //判断顶点的个数是否超过65000
        if (gdd.Elem_NodesIndex.Count < 65000/3)
        {
            //获取顶点的检索值
            int[] idx = new int[ElemNumber * 3];
            for (int j = 0; j < ElemNumber; j++)
            {
                idx[j * 3 + 0] = gdd.Elem_NodesIndex[j][0];
                idx[j * 3 + 1] = gdd.Elem_NodesIndex[j][2];
                idx[j * 3 + 2] = gdd.Elem_NodesIndex[j][1];
            }

            Mesh mr = obj.GetComponent<MeshFilter>().mesh;//获取物体的mesh
            mr.Clear();//清空当前mesh
            mr.vertices = pos;//设置顶点
            mr.triangles = idx;//设置顶点将所致
            

            Mesh mr_1 = obj_1.GetComponent<MeshFilter>().mesh;//获取物体的mesh
            mr_1.Clear();//清空当前mesh
            //获取顶点的检索值
            int[] idx_1 = new int[ElemNumber * 3];
            for (int j = 0; j < ElemNumber; j++)
            {
                idx_1[j * 3 + 0] = gdd.Elem_NodesIndex[j][0];
                idx_1[j * 3 + 1] = gdd.Elem_NodesIndex[j][1];
                idx_1[j * 3 + 2] = gdd.Elem_NodesIndex[j][2];
            }
            mr_1.vertices = pos;//设置顶点
            mr_1.triangles = idx_1;//设置顶点将所致
            
        }
        else
        {
            //修改格式为：
            //Tristan：012345678
            //vertice:点012345678
            //保证每个点索引都有一个点坐标，保证与索引一一对应。
            //以上方法的缺点是点数据有很多的冗余
            //以上方法便于模型的分割
            int[] tris = new int[ElemNumber * 3];
            Vector3[] poss = new Vector3[ElemNumber * 3];
            Color[] cors = new Color[ElemNumber * 3];
            for (int i = 0; i < ElemNumber; i++)
            {
                tris[3 * i + 0] = 3 * i + 0;
                tris[3 * i + 1] = 3 * i + 1;
                tris[3 * i + 2] = 3 * i + 2;
                poss[3 * i + 0] = new Vector3(
                    (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][0] - 1][0] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][0] - 1][0]) * Scale,
                    (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][0] - 1][2] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][0] - 1][2]) * Scale, 
                    (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][0] - 1][1] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][0] - 1][1]) * Scale);
                poss[3 * i + 1] = new Vector3(
                   (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][1] - 1][0] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][1] - 1][0]) * Scale,
                   (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][1] - 1][2] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][1] - 1][2]) * Scale,
                   (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][1] - 1][1] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][1] - 1][1]) * Scale);
                poss[3 * i + 2] = new Vector3(
                   (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][2] - 1][0] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][2] - 1][0]) * Scale,
                   (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][2] - 1][2] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][2] - 1][2]) * Scale,
                   (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][2] - 1][1] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][2] - 1][1]) * Scale);
            }


            int maxObjLen = ElemNumber / 21666;
            int lastObjLen = ElemNumber % 21666;
            if (lastObjLen != 0)
                maxObjLen++;

            int MaxVerticeCount = 21666 * 3;//最大顶点数

            int[] m_tri = new int[] { };//声明检索
            Vector3[] m_ver = new Vector3[] { };//声明顶点
            int VerticeCount = 0;//声明当前顶点数
            Mesh mr = new Mesh();
            GameObject[] objShow = new GameObject[maxObjLen];
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow[i] = GameObject.Instantiate(obj);
                objShow[i].name = obj.name + "_" + i.ToString();
                mr = objShow[i].GetComponent<MeshFilter>().mesh;//获取物体的mesh
                mr.Clear();//清空当前mesh
                mr.name = objShow[i].name;
                VerticeCount = i < maxObjLen - 1 ? MaxVerticeCount : lastObjLen * 3;
                m_tri = new int[VerticeCount];
                m_ver = new Vector3[VerticeCount];
                for (int j = 0; j < VerticeCount; j++)
                {
                    m_tri[j] = tris[i * MaxVerticeCount + j] - i * MaxVerticeCount;
                    m_ver[j] = poss[i * MaxVerticeCount + j];
                }
                mr.vertices = m_ver;//设置顶点
                mr.triangles = m_tri;//设置顶点将所致
            }
            GameObject[] objShow_1 = new GameObject[maxObjLen];
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow_1[i] = GameObject.Instantiate(obj_1);
                objShow_1[i].name = obj_1.name + "_" + i.ToString();
                mr = objShow_1[i].GetComponent<MeshFilter>().mesh;//获取物体的mesh
                mr.Clear();//清空当前mesh
                VerticeCount = i < maxObjLen - 1 ? MaxVerticeCount : lastObjLen * 3;
                m_tri = new int[VerticeCount];
                m_ver = new Vector3[VerticeCount];
                for (int j = 0; j < VerticeCount; j++)
                {
                    //设置外法向的方向
                    m_tri[j] =
                        j % 3 == 1 ? tris[i * MaxVerticeCount + j] - i * MaxVerticeCount + 1 :
                        j % 3 == 2 ? tris[i * MaxVerticeCount + j] - i * MaxVerticeCount - 1 :
                        tris[i * MaxVerticeCount + j] - i * MaxVerticeCount;
                    m_ver[j] = poss[i * MaxVerticeCount + j];
                }
                mr.vertices = m_ver;//设置顶点
                mr.triangles = m_tri;//设置顶点将所致
            }
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow[i].transform.parent = obj.transform;
                objShow_1[i].transform.parent = obj_1.transform;
                objShow[i].GetComponent<MeshRenderer>().sharedMaterial = mat;
                objShow_1[i].GetComponent<MeshRenderer>().sharedMaterial = mat;
            }
        }
    }



    public void NewShowColors(GameObject obj, GameObject obj_1, GeneralData gdd, int indexer, double max, double min, Material mat,  bool isVerse, float Scale)
    {
        if (obj == null)
        {
            return;
        }

        GameObject gg = GameObject.Find(obj.name + "_0");
        if (gg != null)
        {
            GameObject.DestroyImmediate(gg);
        }
        gg = GameObject.Find(obj.name + "_1");
        if (gg != null)
        {
            GameObject.DestroyImmediate(gg);
        }
        gg = GameObject.Find(obj_1.name + "_0");
        if (gg != null)
        {
            GameObject.DestroyImmediate(gg);
        }
        gg = GameObject.Find(obj_1.name + "_1");
        if (gg != null)
        {
            GameObject.DestroyImmediate(gg);
        }

        obj.GetComponent<MeshRenderer>().material = mat;
        obj_1.GetComponent<MeshRenderer>().material = mat;

        //vertices的数量大于65000
        int NodeNumber = gdd.NodeCoord.Count;
        int ElemNumber = gdd.Elem_NodesIndex.Count;

        //bool isVerse = false;
        //判断顶点的个数是否超过65000
        if (gdd.Elem_NodesIndex.Count < 65000/3)
        {
            //获取模型顶点坐标值
            Vector3[] pos = new Vector3[NodeNumber];
            for (int i = 0; i < NodeNumber; i++)
            {
                pos[i] = new Vector3((float)(gdd.NodeCoord[i][0] + gdd.NodeOffPos[i][0]) * Scale,
               (float)(gdd.NodeCoord[i][2] + gdd.NodeOffPos[i][2]) * Scale, (float)(gdd.NodeCoord[i][1] + gdd.NodeOffPos[i][1]) * Scale);
            }
            //获取顶点的检索值
            int[] idx = new int[ElemNumber * 3];
            for (int j = 0; j < ElemNumber; j++)
            {
                idx[j * 3 + 0] = gdd.Elem_NodesIndex[j][0];
                idx[j * 3 + 1] = isVerse ? gdd.Elem_NodesIndex[j][2] : gdd.Elem_NodesIndex[j][1];
                idx[j * 3 + 2] = isVerse ? gdd.Elem_NodesIndex[j][1] : gdd.Elem_NodesIndex[j][2];
            }
            //设置顶点的颜色
            Color[] cor = new Color[NodeNumber];
            if (indexer >= gdd.nSlnTypeNames.Count)
            {
                Debug.Log("错误： 物理量序号超出范围，请确认！");
                for (int i = 0; i < NodeNumber; i++)
                {
                    cor[i] = Color.HSVToRGB(0, 1, 1);//将HSV的颜色值转为RGB的颜色值//全为红色
                }
            }
            else
            {
                double fenmu_c = max - min;
                double value_c = 0;
                for (int i = 0; i < NodeNumber; i++)
                {
                    value_c = (max - gdd.nSlnTypeValue[indexer][i]) / fenmu_c;
                    value_c = value_c < 0 ? 0 : value_c;
                    value_c = value_c > 1 ? 1 : value_c;
                    cor[i] = Color.HSVToRGB((float)value_c * 2 / 3, 1, 1);//将HSV的颜色值转为RGB的颜色值
                }
            }
            

            Mesh mr = obj.GetComponent<MeshFilter>().mesh;//获取物体的mesh
            mr.Clear();//清空当前mesh
            mr.vertices = pos;//设置顶点
            mr.triangles = idx;//设置顶点将所致
            mr.colors = cor;//设置顶点颜色
            //mr.RecalculateBounds();//
            //mr.RecalculateNormals();//计算顶点的法向值

            Mesh mr_1 = obj_1.GetComponent<MeshFilter>().mesh;//获取物体的mesh
            mr_1.Clear();//清空当前mesh
            //获取顶点的检索值
            int[] idx_1 = new int[ElemNumber * 3];
            for (int j = 0; j < ElemNumber; j++)
            {
                idx_1[j * 3 + 0] = gdd.Elem_NodesIndex[j][0];
                idx_1[j * 3 + 1] = isVerse ? gdd.Elem_NodesIndex[j][1] : gdd.Elem_NodesIndex[j][2];
                idx_1[j * 3 + 2] = isVerse ? gdd.Elem_NodesIndex[j][2] : gdd.Elem_NodesIndex[j][1];
            }

            mr_1.vertices = pos;//设置顶点
            mr_1.triangles = idx_1;//设置顶点将所致
            mr_1.colors = cor;//设置顶点颜色
            //mr_1.RecalculateBounds();//
            //mr_1.RecalculateNormals();//计算顶点的法向值
        }
        else
        {
            //修改格式为：
            //Tristan：012345678
            //vertice:点012345678
            //保证每个点索引都有一个点坐标，保证与索引一一对应。
            //以上方法的缺点是点数据有很多的冗余
            //以上方法便于模型的分割
            int[] tris = new int[ElemNumber * 3];
            Vector3[] poss = new Vector3[ElemNumber * 3];
            Color[] cors = new Color[ElemNumber * 3];

            double fenmu = max - min;
            double value = 0;
            for (int i = 0; i < ElemNumber; i++)
            {
                tris[3 * i + 0] = 3 * i + 0;
                tris[3 * i + 1] = 3 * i + 1;
                tris[3 * i + 2] = 3 * i + 2;
                //tris[3 * i + 1] = isVerse ? 3 * i + 1 : 3 * i + 2;
                //tris[3 * i + 2] = isVerse ? 3 * i + 2 : 3 * i + 1;
                poss[3 * i + 0] = new Vector3(
                     (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][0] - 1][0] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][0] - 1][0]) * Scale,
                     (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][0] - 1][2] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][0] - 1][2]) * Scale,
                     (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][0] - 1][1] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][0] - 1][1]) * Scale);
                value = (max - gdd.nSlnTypeValue[indexer][gdd.Elem_NodesIndex[i][0] - 1]) / fenmu;
                value = value < 0 ? 0 : value;
                value = value > 1 ? 1 : value;
                cors[3 * i + 0] = Color.HSVToRGB((float)value * 2 / 3, 1, 1);//将HSV的颜色值转为RGB的颜色值
                if (isVerse) 
                {
                   
                    poss[3 * i + 1] = new Vector3(
                       (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][1] - 1][0] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][1] - 1][0]) * Scale,
                       (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][1] - 1][2] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][1] - 1][2]) * Scale,
                       (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][1] - 1][1] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][1] - 1][1]) * Scale);
                    poss[3 * i + 2] = new Vector3(
                       (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][2] - 1][0] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][2] - 1][0]) * Scale,
                       (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][2] - 1][2] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][2] - 1][2]) * Scale,
                       (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][2] - 1][1] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][2] - 1][1]) * Scale);
                   

                    value = (max - gdd.nSlnTypeValue[indexer][gdd.Elem_NodesIndex[i][1] - 1]) / fenmu;
                    value = value < 0 ? 0 : value;
                    value = value > 1 ? 1 : value;
                    cors[3 * i + 1] = Color.HSVToRGB((float)value * 2 / 3, 1, 1);//将HSV的颜色值转为RGB的颜色值
                    value = (max - gdd.nSlnTypeValue[indexer][gdd.Elem_NodesIndex[i][2] - 1]) / fenmu;
                    value = value < 0 ? 0 : value;
                    value = value > 1 ? 1 : value;
                    cors[3 * i + 2] = Color.HSVToRGB((float)value * 2 / 3, 1, 1);//将HSV的颜色值转为RGB的颜色值

                } else {
                    poss[3 * i + 1] = new Vector3(
                       (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][2] - 1][0] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][2] - 1][0]) * Scale,
                       (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][2] - 1][2] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][2] - 1][2]) * Scale,
                       (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][2] - 1][1] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][2] - 1][1]) * Scale);
                    poss[3 * i + 2] = new Vector3(
                       (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][1] - 1][0] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][1] - 1][0]) * Scale,
                       (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][1] - 1][2] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][1] - 1][2]) * Scale,
                       (float)(gdd.NodeCoord[gdd.Elem_NodesIndex[i][1] - 1][1] + gdd.NodeOffPos[gdd.Elem_NodesIndex[i][1] - 1][1]) * Scale);

                    value = (max - gdd.nSlnTypeValue[indexer][gdd.Elem_NodesIndex[i][2] - 1]) / fenmu;
                    value = value < 0 ? 0 : value;
                    value = value > 1 ? 1 : value;
                    cors[3 * i + 1] = Color.HSVToRGB((float)value * 2 / 3, 1, 1);//将HSV的颜色值转为RGB的颜色值
                    value = (max - gdd.nSlnTypeValue[indexer][gdd.Elem_NodesIndex[i][1] - 1]) / fenmu;
                    value = value < 0 ? 0 : value;
                    value = value > 1 ? 1 : value;
                    cors[3 * i + 2] = Color.HSVToRGB((float)value * 2 / 3, 1, 1);//将HSV的颜色值转为RGB的颜色值
                }
               

               
            }
            int maxObjLen = ElemNumber / 21666;
            int lastObjLen = ElemNumber % 21666;
            if (lastObjLen != 0)
                maxObjLen++;

            int MaxVerticeCount = 21666 * 3;//最大顶点数
            //Debug.Log(" :  " + MaxVerticeCount);

            int[] m_tri = new int[] { };//声明检索
            Vector3[] m_ver = new Vector3[] { };//声明顶点
            Color[] m_cor = new Color[] { };//声明颜色
            int VerticeCount = 0;//声明当前顶点数
            GameObject[] objShow = new GameObject[maxObjLen];
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow[i] = GameObject.Instantiate(obj);
                objShow[i].name = obj.name + "_" + i.ToString();
                Mesh mr = objShow[i].GetComponent<MeshFilter>().mesh;//获取物体的mesh
                mr.Clear();//清空当前mesh
                VerticeCount = i < maxObjLen - 1 ? MaxVerticeCount : lastObjLen * 3;
                m_tri = new int[VerticeCount];
                m_ver = new Vector3[VerticeCount];
                m_cor = new Color[VerticeCount];
                for (int j = 0; j < VerticeCount; j++)
                {
                    m_tri[j] = tris[i * MaxVerticeCount + j] - i * MaxVerticeCount;
                    m_ver[j] = poss[i * MaxVerticeCount + j];
                    m_cor[j] = cors[i * MaxVerticeCount + j];
                }
                mr.vertices = m_ver;//设置顶点
                mr.triangles = m_tri;//设置顶点将所致
                mr.colors = m_cor;//设置顶点颜色
                //mr.RecalculateBounds();//
                //mr.RecalculateNormals();//计算顶点的法向值
            }
            //for (int i = 0; i < maxObjLen; i++)
            //{
            //    objShow[i].transform.parent = obj.transform;
            //}

            GameObject[] objShow_1 = new GameObject[maxObjLen];
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow_1[i] = GameObject.Instantiate(obj_1);
                objShow_1[i].name = obj_1.name + "_" + i.ToString();
                Mesh mr = objShow_1[i].GetComponent<MeshFilter>().mesh;//获取物体的mesh
                mr.Clear();//清空当前mesh
                VerticeCount = i < maxObjLen - 1 ? MaxVerticeCount : lastObjLen * 3;
                m_tri = new int[VerticeCount];
                m_ver = new Vector3[VerticeCount];
                m_cor = new Color[VerticeCount];
                for (int j = 0; j < VerticeCount; j++)
                {
                    //设置外法向的方向
                    m_tri[j] =
                        j % 3 == 1 ? tris[i * MaxVerticeCount + j] - i * MaxVerticeCount + 1 :
                        j % 3 == 2 ? tris[i * MaxVerticeCount + j] - i * MaxVerticeCount - 1 :
                        tris[i * MaxVerticeCount + j] - i * MaxVerticeCount;
                    m_ver[j] = poss[i * MaxVerticeCount + j];
                    m_cor[j] = cors[i * MaxVerticeCount + j];
                }
                mr.vertices = m_ver;//设置顶点
                mr.triangles = m_tri;//设置顶点将所致
                mr.colors = m_cor;//设置顶点颜色
                //mr.RecalculateBounds();//
                //mr.RecalculateNormals();//计算顶点的法向值
            }
            for (int i = 0; i < maxObjLen; i++)
            {
                objShow[i].transform.parent = obj.transform;
                objShow_1[i].transform.parent = obj_1.transform;
                objShow[i].GetComponent<MeshRenderer>().material = mat;
                objShow_1[i].GetComponent<MeshRenderer>().material = mat;
            }
        }
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
	public void CreatePrefabStaff(string Objname, string Typename, double max, double min, Vector3 pos, float scale)
	{
		GameObject Biaochi = (GameObject)Resources.Load ("Flt_Assets/Staff_8_1");
		GameObject Target = GameObject.Find (Objname);
		if (Target == null) {
			Target = GameObject.Instantiate (Biaochi);
			Target.name = Objname;
			Target.transform.parent = GameObject.Find ("Canvas").transform;
			Target.transform.localPosition = pos;
		}
		GameObject tf0 = GameObject.Find ("Text (0)");
		if (tf0.transform.parent == Target.transform) {
			tf0.GetComponent<Text> ().text = Typename;
		}

		//计算标值
		double step = (max - min) / 7;
		string[] values = new string[8];
		for (int i = 6; i >= 0; i--) {
			values [i] = ((max - step * i) * scale).ToString ("0.000");
			int j = i + 1;
			GameObject tf = GameObject.Find ("Text (" + j + ")");
			if (tf.transform.parent == Target.transform) {
				tf.GetComponent<Text> ().text = values [i];
			}
		}
		values [0] = min.ToString ("0.000");
		GameObject tf8 = GameObject.Find ("Text (8)");
		if (tf8.transform.parent == Target.transform) {
			tf8.GetComponent<Text> ().text = values [0];
		}
	}
}
