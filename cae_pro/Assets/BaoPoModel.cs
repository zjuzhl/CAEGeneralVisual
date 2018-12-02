using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BaoPoModel : MonoBehaviour {

    public List<string> oNodeData;//节点数据的初始内容，从文件中刚提取
    public List<string[]> fNodeData;//节点数据第一次处理
    public Vector3[] ANSNodeData;//节点坐标（在ANSYS中的坐标）
    //public Vector3[] UniNodeData;//节点在unity里面的坐标

    public List<string> oElemData;//单元数据的初始内容，从文件中刚提取
    public List<string[]> fElemData;//单元数据第一次处理
    public int[][] ANSElemData;//每组4个数字===四边形
    public int[][] UniElemData;//在unity里面节点的检索值

    //public List<string> oDispData = new List<string>();//位移数据的初始内容，从文件中刚提取
    public List<string[]> fDispData = new List<string[]>();//位移数据第一次处理
    public static float[][] sDispData = new float[][] { };//位移数据第二次处理
    public static int[] MaxDispNodes = new int[] { };
    public static float[] MaxDispValue = new float[] { };

    public GameObject obj;
    public GameObject obj_1;
    public Material mat;
    public Material Colmat;
    private Material mymat;


    public bool isYuntu = false;


    GeneralData gdd = new GeneralData();

    public Camera mainCamera;
    public Camera aidCamera;

    public Texture2D t2dZJU;

    //public Texture2D t2dDongHua;

    public int flag = 0;

	// Use this for initialization
	void Start () {

        mymat = Colmat;
        isYuntu = true;

        oNodeData = TextFileOperation.Read(Application.dataPath + "\\Resources\\NLIST.lis");
        fNodeData = AsNodeData.ExtractNodeData(oNodeData);
        ANSNodeData = AsNodeData.GetNodeData(fNodeData);

        Debug.Log("n: " + ANSNodeData.Length);

        //for (int i = 0; i < 10; i++)
        //{
        //    Debug.Log(i + " : " + ANSNodeData[i].x + " , " + ANSNodeData[i].y + " , " + ANSNodeData[i].z);
        //}

        oElemData = TextFileOperation.Read(Application.dataPath + "\\Resources\\ELIST.lis");
        fElemData = AsElementData.ExtractElemData(oElemData);
        ANSElemData = AsElementData.GetElemData(fElemData);

        Debug.Log("e: " + ANSElemData.Length);

        //for (int i = 0; i < 10; i++)
        //{
        //    Debug.Log(i + " : " + ANSElemData[i][0] + " , " + ANSElemData[i][1] + " , "
        //        + ANSElemData[i][2] + " , " + ANSElemData[i][3]);
        //}

        gdd.NodeCoord = new List<double[]>();

        for (int i = 0; i < ANSNodeData.Length; i++)
        {
            gdd.NodeCoord.Add(new double[] { ANSNodeData[i].x, ANSNodeData[i].y, ANSNodeData[i].z });
        }
        //gdd.NodeOffPos = dd;
        //Debug.Log(gdd.NodeOffPos[gdd.NodeOffPos.Count - 1][0] + "  " + gdd.NodeOffPos[gdd.NodeOffPos.Count - 1][1] + "  " + gdd.NodeOffPos[gdd.NodeOffPos.Count - 1][2]);

        gdd.Elem_NodesIndex = new List<int[]>();
        for (int i = 0; i < ANSElemData.Length; i++)
        {
            gdd.Elem_NodesIndex.Add(new int[] { ANSElemData[i][0], ANSElemData[i][1], ANSElemData[i][2] });
            gdd.Elem_NodesIndex.Add(new int[] { ANSElemData[i][0], ANSElemData[i][2], ANSElemData[i][3] });
        }

        //Debug.Log(gdd.Elem_NodesIndex.Count);


        //Button btn = yourButton.GetComponent<Button>();
        //btn.onClick.AddListener(TaskOnClick);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartMovie()
    {
        StopAllCoroutines();
            if (isYuntu)
            {
                StartCoroutine("SlutionDEAL");
            }
            else {
                StartCoroutine("DisplaceDEAL");
            }
    }
    public void SwichMaterial()
    {
        if (isYuntu)
        {
            mymat = mat;
            isYuntu = false;
        }
        else
        {
            mymat = Colmat;
            isYuntu = true;
        }
    }

    public void SwichViewPort()
    {
        if (mainCamera.rect.x == 0)
        {
            aidCamera.depth = -1;
            mainCamera.depth = 0;
            aidCamera.rect = new Rect(0, 0, 1, 1);
            mainCamera.rect = new Rect(0.8f, 0.7f, 0.2f, 0.3f);
        }
        else
        {
            aidCamera.depth = 0;
            mainCamera.depth = -1;
            mainCamera.rect = new Rect(0, 0, 1, 1);
            aidCamera.rect = new Rect(0.8f, 0.7f, 0.2f, 0.3f);
        }
    }

    void Function(string path, string path1, Material mat)
    {
        List<string> oDispData = TextFileOperation.Read(Application.dataPath + @"\\" + path);
        int[] idx = new int[3];
        double[] max = new double[3];
        List<double[]> dd = ExtractDispData(oDispData, ANSNodeData.Length, ref idx, ref max);
        gdd.NodeOffPos = dd;
        //Debug.Log(gdd.NodeOffPos[gdd.NodeOffPos.Count - 1][0] + "  " + gdd.NodeOffPos[gdd.NodeOffPos.Count - 1][1] + "  " + gdd.NodeOffPos[gdd.NodeOffPos.Count - 1][2]);
        
        List<string> sln = TextFileOperation.Read(Application.dataPath + @"\\" + path1);
        int[] _idx = new int[2];
        double[] _max = new double[2];
        List<double> cc = ExtractSlnData(sln, ANSNodeData.Length, ref _idx, ref _max);

        gdd.nSlnTypeValue = new List<double>[1];//物理量结果(标量)
        gdd.nSlnTypeValue[0] = cc;
        //Debug.Log(gdd.NodeOffPos.Count);

        Flt_YunTu_JianBian fyj = new Flt_YunTu_JianBian();
        fyj.NewShowColors(obj, obj_1, gdd, 0, _max[1], _max[0], mat, true, 0.001f * 100);
        if (isYuntu)
        {
            fyj.CreatePrefabStaff("Staff_", @"应变(×0.01)", _max[1], _max[0], new Vector3(0, -350, 0), 100);
        }
        else {
            GameObject gg = GameObject.Find("Staff_");
            if (gg != null)
            {
                GameObject.DestroyImmediate(gg);
            }
        }



    }

    void Function(string path, Material mat) 
    {
        List<string> oDispData = TextFileOperation.Read(Application.dataPath + @"\\" + path);
        int[] idx = new int[3];
        double[] max = new double[3];
        List<double[]> dd = ExtractDispData(oDispData, ANSNodeData.Length, ref idx, ref max);
        gdd.NodeOffPos = dd;
        //Debug.Log(gdd.NodeOffPos[gdd.NodeOffPos.Count - 1][0] + "  " + gdd.NodeOffPos[gdd.NodeOffPos.Count - 1][1] + "  " + gdd.NodeOffPos[gdd.NodeOffPos.Count - 1][2]);

        GameObject gg = GameObject.Find("Staff_");
        if (gg != null)
        {
            GameObject.DestroyImmediate(gg);
        }

        Flt_YunTu_JianBian fyj = new Flt_YunTu_JianBian();
        fyj.NewShowObjMat(obj, obj_1, gdd, mat, true, 0.001f * 100);    
       
    }
    IEnumerator SlutionDEAL()
    {
        Function(@"Resources\\BaoPo\\1\\nsol-d-1-1.lis", @"Resources\\BaoPo\\1\\nsol-s-1-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\1\\nsol-d-1-2.lis", @"Resources\\BaoPo\\1\\nsol-s-1-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\1\\nsol-d-1-3.lis", @"Resources\\BaoPo\\1\\nsol-s-1-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\1\\nsol-d-1-4.lis", @"Resources\\BaoPo\\1\\nsol-s-1-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\1\\nsol-d-1-5.lis", @"Resources\\BaoPo\\1\\nsol-s-1-5.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\2\\nsol-d-2-1.lis", @"Resources\\BaoPo\\2\\nsol-s-2-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\2\\nsol-d-2-2.lis", @"Resources\\BaoPo\\2\\nsol-s-2-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\2\\nsol-d-2-3.lis", @"Resources\\BaoPo\\2\\nsol-s-2-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\2\\nsol-d-2-4.lis", @"Resources\\BaoPo\\2\\nsol-s-2-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\2\\nsol-d-2-5.lis", @"Resources\\BaoPo\\2\\nsol-s-2-5.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\3\\nsol-d-3-1.lis", @"Resources\\BaoPo\\3\\nsol-s-3-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\3\\nsol-d-3-2.lis", @"Resources\\BaoPo\\3\\nsol-s-3-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\3\\nsol-d-3-3.lis", @"Resources\\BaoPo\\3\\nsol-s-3-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\3\\nsol-d-3-4.lis", @"Resources\\BaoPo\\3\\nsol-s-3-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\3\\nsol-d-3-5.lis", @"Resources\\BaoPo\\3\\nsol-s-3-5.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\4\\nsol-d-4-1.lis", @"Resources\\BaoPo\\4\\nsol-s-4-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-2.lis", @"Resources\\BaoPo\\4\\nsol-s-4-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-3.lis", @"Resources\\BaoPo\\4\\nsol-s-4-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-4.lis", @"Resources\\BaoPo\\4\\nsol-s-4-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-5.lis", @"Resources\\BaoPo\\4\\nsol-s-4-5.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-6.lis", @"Resources\\BaoPo\\4\\nsol-s-4-6.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-7.lis", @"Resources\\BaoPo\\4\\nsol-s-4-7.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-8.lis", @"Resources\\BaoPo\\4\\nsol-s-4-8.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-9.lis", @"Resources\\BaoPo\\4\\nsol-s-4-9.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-10.lis", @"Resources\\BaoPo\\4\\nsol-s-4-10.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-11.lis", @"Resources\\BaoPo\\4\\nsol-s-4-11.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-12.lis", @"Resources\\BaoPo\\4\\nsol-s-4-12.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-13.lis", @"Resources\\BaoPo\\4\\nsol-s-4-13.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-14.lis", @"Resources\\BaoPo\\4\\nsol-s-4-14.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-15.lis", @"Resources\\BaoPo\\4\\nsol-s-4-15.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-16.lis", @"Resources\\BaoPo\\4\\nsol-s-4-16.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-17.lis", @"Resources\\BaoPo\\4\\nsol-s-4-17.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-18.lis", @"Resources\\BaoPo\\4\\nsol-s-4-18.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-19.lis", @"Resources\\BaoPo\\4\\nsol-s-4-19.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-20.lis", @"Resources\\BaoPo\\4\\nsol-s-4-20.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\5\\nsol-d-5-1.lis", @"Resources\\BaoPo\\5\\nsol-s-5-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-2.lis", @"Resources\\BaoPo\\5\\nsol-s-5-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-3.lis", @"Resources\\BaoPo\\5\\nsol-s-5-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-4.lis", @"Resources\\BaoPo\\5\\nsol-s-5-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-5.lis", @"Resources\\BaoPo\\5\\nsol-s-5-5.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-6.lis", @"Resources\\BaoPo\\5\\nsol-s-5-6.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-7.lis", @"Resources\\BaoPo\\5\\nsol-s-5-7.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-8.lis", @"Resources\\BaoPo\\5\\nsol-s-5-8.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-9.lis", @"Resources\\BaoPo\\5\\nsol-s-5-9.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-10.lis", @"Resources\\BaoPo\\5\\nsol-s-5-10.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-11.lis", @"Resources\\BaoPo\\5\\nsol-s-5-11.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-12.lis", @"Resources\\BaoPo\\5\\nsol-s-5-12.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-13.lis", @"Resources\\BaoPo\\5\\nsol-s-5-13.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-14.lis", @"Resources\\BaoPo\\5\\nsol-s-5-14.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-15.lis", @"Resources\\BaoPo\\5\\nsol-s-5-15.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-16.lis", @"Resources\\BaoPo\\5\\nsol-s-5-16.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-17.lis", @"Resources\\BaoPo\\5\\nsol-s-5-17.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-18.lis", @"Resources\\BaoPo\\5\\nsol-s-5-18.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-19.lis", @"Resources\\BaoPo\\5\\nsol-s-5-19.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-20.lis", @"Resources\\BaoPo\\5\\nsol-s-5-20.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\6\\nsol-d-6-1.lis", @"Resources\\BaoPo\\6\\nsol-s-6-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-2.lis", @"Resources\\BaoPo\\6\\nsol-s-6-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-3.lis", @"Resources\\BaoPo\\6\\nsol-s-6-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-4.lis", @"Resources\\BaoPo\\6\\nsol-s-6-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-5.lis", @"Resources\\BaoPo\\6\\nsol-s-6-5.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-6.lis", @"Resources\\BaoPo\\6\\nsol-s-6-6.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-7.lis", @"Resources\\BaoPo\\6\\nsol-s-6-7.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-8.lis", @"Resources\\BaoPo\\6\\nsol-s-6-8.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-9.lis", @"Resources\\BaoPo\\6\\nsol-s-6-9.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-10.lis", @"Resources\\BaoPo\\6\\nsol-s-6-10.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\7\\nsol-d-7-1.lis", @"Resources\\BaoPo\\7\\nsol-s-7-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-2.lis", @"Resources\\BaoPo\\7\\nsol-s-7-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-3.lis", @"Resources\\BaoPo\\7\\nsol-s-7-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-4.lis", @"Resources\\BaoPo\\7\\nsol-s-7-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-5.lis", @"Resources\\BaoPo\\7\\nsol-s-7-5.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-6.lis", @"Resources\\BaoPo\\7\\nsol-s-7-6.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-7.lis", @"Resources\\BaoPo\\7\\nsol-s-7-7.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-8.lis", @"Resources\\BaoPo\\7\\nsol-s-7-8.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-9.lis", @"Resources\\BaoPo\\7\\nsol-s-7-9.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-10.lis", @"Resources\\BaoPo\\7\\nsol-s-7-10.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-11.lis", @"Resources\\BaoPo\\7\\nsol-s-7-11.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-12.lis", @"Resources\\BaoPo\\7\\nsol-s-7-12.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-13.lis", @"Resources\\BaoPo\\7\\nsol-s-7-13.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\8\\nsol-d-8-1.lis", @"Resources\\BaoPo\\8\\nsol-s-8-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-2.lis", @"Resources\\BaoPo\\8\\nsol-s-8-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-3.lis", @"Resources\\BaoPo\\8\\nsol-s-8-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-4.lis", @"Resources\\BaoPo\\8\\nsol-s-8-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-5.lis", @"Resources\\BaoPo\\8\\nsol-s-8-5.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-6.lis", @"Resources\\BaoPo\\8\\nsol-s-8-6.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-7.lis", @"Resources\\BaoPo\\8\\nsol-s-8-7.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-8.lis", @"Resources\\BaoPo\\8\\nsol-s-8-8.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-9.lis", @"Resources\\BaoPo\\8\\nsol-s-8-9.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-10.lis", @"Resources\\BaoPo\\8\\nsol-s-8-10.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-11.lis", @"Resources\\BaoPo\\8\\nsol-s-8-11.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-12.lis", @"Resources\\BaoPo\\8\\nsol-s-8-12.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-13.lis", @"Resources\\BaoPo\\8\\nsol-s-8-13.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-14.lis", @"Resources\\BaoPo\\8\\nsol-s-8-14.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\9\\nsol-d-9-1.lis", @"Resources\\BaoPo\\9\\nsol-s-9-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-2.lis", @"Resources\\BaoPo\\9\\nsol-s-9-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-3.lis", @"Resources\\BaoPo\\9\\nsol-s-9-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-4.lis", @"Resources\\BaoPo\\9\\nsol-s-9-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-5.lis", @"Resources\\BaoPo\\9\\nsol-s-9-5.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-6.lis", @"Resources\\BaoPo\\9\\nsol-s-9-6.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-7.lis", @"Resources\\BaoPo\\9\\nsol-s-9-7.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-8.lis", @"Resources\\BaoPo\\9\\nsol-s-9-8.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-9.lis", @"Resources\\BaoPo\\9\\nsol-s-9-9.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-10.lis", @"Resources\\BaoPo\\9\\nsol-s-9-10.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-11.lis", @"Resources\\BaoPo\\9\\nsol-s-9-11.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-12.lis", @"Resources\\BaoPo\\9\\nsol-s-9-12.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-13.lis", @"Resources\\BaoPo\\9\\nsol-s-9-13.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-14.lis", @"Resources\\BaoPo\\9\\nsol-s-9-14.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-15.lis", @"Resources\\BaoPo\\9\\nsol-s-9-15.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-16.lis", @"Resources\\BaoPo\\9\\nsol-s-9-16.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-17.lis", @"Resources\\BaoPo\\9\\nsol-s-9-17.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\10\\nsol-d-10-1.lis", @"Resources\\BaoPo\\10\\nsol-s-10-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-2.lis", @"Resources\\BaoPo\\10\\nsol-s-10-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-3.lis", @"Resources\\BaoPo\\10\\nsol-s-10-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-4.lis", @"Resources\\BaoPo\\10\\nsol-s-10-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-5.lis", @"Resources\\BaoPo\\10\\nsol-s-10-5.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-6.lis", @"Resources\\BaoPo\\10\\nsol-s-10-6.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-7.lis", @"Resources\\BaoPo\\10\\nsol-s-10-7.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-8.lis", @"Resources\\BaoPo\\10\\nsol-s-10-8.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-9.lis", @"Resources\\BaoPo\\10\\nsol-s-10-9.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-10.lis", @"Resources\\BaoPo\\10\\nsol-s-10-10.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-11.lis", @"Resources\\BaoPo\\10\\nsol-s-10-11.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-12.lis", @"Resources\\BaoPo\\10\\nsol-s-10-12.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-13.lis", @"Resources\\BaoPo\\10\\nsol-s-10-13.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-14.lis", @"Resources\\BaoPo\\10\\nsol-s-10-14.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-15.lis", @"Resources\\BaoPo\\10\\nsol-s-10-15.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-16.lis", @"Resources\\BaoPo\\10\\nsol-s-10-16.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-17.lis", @"Resources\\BaoPo\\10\\nsol-s-10-17.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-18.lis", @"Resources\\BaoPo\\10\\nsol-s-10-18.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-19.lis", @"Resources\\BaoPo\\10\\nsol-s-10-19.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-20.lis", @"Resources\\BaoPo\\10\\nsol-s-10-20.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-21.lis", @"Resources\\BaoPo\\10\\nsol-s-10-21.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-22.lis", @"Resources\\BaoPo\\10\\nsol-s-10-22.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-23.lis", @"Resources\\BaoPo\\10\\nsol-s-10-23.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-24.lis", @"Resources\\BaoPo\\10\\nsol-s-10-24.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-25.lis", @"Resources\\BaoPo\\10\\nsol-s-10-25.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-26.lis", @"Resources\\BaoPo\\10\\nsol-s-10-26.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-27.lis", @"Resources\\BaoPo\\10\\nsol-s-10-27.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-28.lis", @"Resources\\BaoPo\\10\\nsol-s-10-28.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-29.lis", @"Resources\\BaoPo\\10\\nsol-s-10-29.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-30.lis", @"Resources\\BaoPo\\10\\nsol-s-10-30.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-31.lis", @"Resources\\BaoPo\\10\\nsol-s-10-31.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\11\\nsol-d-11-1.lis", @"Resources\\BaoPo\\11\\nsol-s-11-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-2.lis", @"Resources\\BaoPo\\11\\nsol-s-11-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-3.lis", @"Resources\\BaoPo\\11\\nsol-s-11-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-4.lis", @"Resources\\BaoPo\\11\\nsol-s-11-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-5.lis", @"Resources\\BaoPo\\11\\nsol-s-11-5.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-6.lis", @"Resources\\BaoPo\\11\\nsol-s-11-6.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-7.lis", @"Resources\\BaoPo\\11\\nsol-s-11-7.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-8.lis", @"Resources\\BaoPo\\11\\nsol-s-11-8.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-9.lis", @"Resources\\BaoPo\\11\\nsol-s-11-9.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-10.lis", @"Resources\\BaoPo\\11\\nsol-s-11-10.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-11.lis", @"Resources\\BaoPo\\11\\nsol-s-11-11.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-12.lis", @"Resources\\BaoPo\\11\\nsol-s-11-12.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-13.lis", @"Resources\\BaoPo\\11\\nsol-s-11-13.lis", mymat);
        yield return new WaitForEndOfFrame();
        //Function(@"Resources\\BaoPo\\11\\nsol-d-11-14.lis", @"Resources\\BaoPo\\11\\nsol-s-11-14.lis", mymat);
        //yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-15.lis", @"Resources\\BaoPo\\11\\nsol-s-11-15.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-16.lis", @"Resources\\BaoPo\\11\\nsol-s-11-16.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-17.lis", @"Resources\\BaoPo\\11\\nsol-s-11-17.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-18.lis", @"Resources\\BaoPo\\11\\nsol-s-11-18.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-19.lis", @"Resources\\BaoPo\\11\\nsol-s-11-19.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-20.lis", @"Resources\\BaoPo\\11\\nsol-s-11-20.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-21.lis", @"Resources\\BaoPo\\11\\nsol-s-11-21.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-22.lis", @"Resources\\BaoPo\\11\\nsol-s-11-22.lis", mymat);
        yield return new WaitForEndOfFrame();
        Debug.Log("finish");
    }




    IEnumerator DisplaceDEAL() 
    {
        Function(@"Resources\\BaoPo\\1\\nsol-d-1-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\1\\nsol-d-1-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\1\\nsol-d-1-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\1\\nsol-d-1-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\1\\nsol-d-1-5.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\2\\nsol-d-2-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\2\\nsol-d-2-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\2\\nsol-d-2-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\2\\nsol-d-2-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\2\\nsol-d-2-5.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\3\\nsol-d-3-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\3\\nsol-d-3-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\3\\nsol-d-3-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\3\\nsol-d-3-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\3\\nsol-d-3-5.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\4\\nsol-d-4-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-5.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-6.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-7.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-8.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-9.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-10.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-11.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-12.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-13.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-14.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-15.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-16.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-17.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-18.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-19.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\4\\nsol-d-4-20.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\5\\nsol-d-5-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-5.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-6.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-7.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-8.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-9.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-10.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-11.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-12.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-13.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-14.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-15.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-16.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-17.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-18.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-19.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\5\\nsol-d-5-20.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\6\\nsol-d-6-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-5.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-6.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-7.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-8.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-9.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\6\\nsol-d-6-10.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\7\\nsol-d-7-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-5.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-6.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-7.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-8.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-9.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-10.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-11.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-12.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\7\\nsol-d-7-13.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\8\\nsol-d-8-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-5.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-6.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-7.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-8.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-9.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-10.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-11.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-12.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-13.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\8\\nsol-d-8-14.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\9\\nsol-d-9-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-5.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-6.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-7.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-8.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-9.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-10.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-11.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-12.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-13.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-14.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-15.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-16.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\9\\nsol-d-9-17.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\10\\nsol-d-10-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-5.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-6.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-7.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-8.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-9.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-10.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-11.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-12.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-13.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-14.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-15.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-16.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-17.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-18.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-19.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-20.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-21.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-22.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-23.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-24.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-25.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-26.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-27.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-28.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-29.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-30.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\10\\nsol-d-10-31.lis", mymat);
        yield return new WaitForEndOfFrame();

        Function(@"Resources\\BaoPo\\11\\nsol-d-11-1.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-2.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-3.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-4.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-5.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-6.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-7.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-8.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-9.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-10.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-11.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-12.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-13.lis", mymat);
        yield return new WaitForEndOfFrame();
        //Function(@"Resources\\BaoPo\\11\\nsol-d-11-14.lis", mymat);
        //yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-15.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-16.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-17.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-18.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-19.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-20.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-21.lis", mymat);
        yield return new WaitForEndOfFrame();
        Function(@"Resources\\BaoPo\\11\\nsol-d-11-22.lis", mymat);
        yield return new WaitForEndOfFrame();
        Debug.Log("finish");

    }


    public static List<double[]> ExtractDispData(List<string> ond, int NumCount, ref int[] MaxIndexs, ref double[] MaxValues)
    {
        if (ond == null || ond.Count == 0)
        { 
            return null;
        }

        int cou = ond.Count;
        //int[] inds = new int[4];
        double[] trans = new double[3];
        List<double[]> results = new List<double[]>();
        for (int i = 0; i < ond.Count; i++)
        {
            if (!string.IsNullOrEmpty(ond[i])) 
            {
                if (ond[i].Contains("NODE") && ond[i].Contains("USUM")) 
                {
                    int len = i + 37;
                    i++;
                    for (; i <= len; i++) 
                    {
                        trans = new double[3];
                        double.TryParse(ond[i].Substring(9, 12), out trans[0]);
                        double.TryParse(ond[i].Substring(21, 12), out trans[1]);
                        double.TryParse(ond[i].Substring(33, 12), out trans[2]);
                        //double.TryParse(ond[i].Substring(45, 12), out trans[3]);
                        results.Add(trans);
                        if (ond[i].Substring(0, 9).Contains(NumCount.ToString()))
                        {
                            break;
                        }
                    }
                    //Debug.Log(":" + ond[i]);
                }
                if (ond[i].Contains("MAXIMUM") && ond[i].Contains("ABSOLUTE") && ond[i].Contains("VALUES"))
                {
                    i++;
                    int.TryParse(ond[i].Substring(5, 11), out MaxIndexs[0]);
                    int.TryParse(ond[i].Substring(16, 12), out MaxIndexs[1]);
                    int.TryParse(ond[i].Substring(28, 12), out MaxIndexs[2]);  
                    //int.TryParse(ond[i].Substring(40, 12), out MaxIndexs[3]);
                    i++;
                    double.TryParse(ond[i].Substring(8, 12), out MaxValues[0]);
                    double.TryParse(ond[i].Substring(20, 12), out MaxValues[1]);
                    double.TryParse(ond[i].Substring(32, 12), out MaxValues[2]);
                    //double.TryParse(ond[i].Substring(44, 12), out MaxValues[3]);
                    //Debug.Log(":" + MaxIndexs[0] + "," + MaxIndexs[1] + "," + MaxIndexs[2]);
                    //Debug.Log(":" + MaxValues[0] + "," + MaxValues[1] + "," + MaxValues[2]);
                }

               
            }
        }
        ond.Clear();//清空原始位移数据，释放内存
        return results;
    }

    public static List<double> ExtractSlnData(List<string> ond, int NumCount, ref int[] index, ref double[] values)
    {
        if (ond == null || ond.Count == 0)
        {
            return null;
        }

        int cou = ond.Count;
        double trans = 0;
        List<double> results = new List<double>();
        for (int i = 0; i < ond.Count; i++)
        {
            if (!string.IsNullOrEmpty(ond[i]))
            {
                if (ond[i].Contains("NODE") && ond[i].Contains("EPTOEQV"))
                {
                    int len = i + 37;
                    i++;
                    for (; i <= len; i++)
                    {
                        double.TryParse(ond[i].Substring(57, 12), out trans);
                        //Debug.Log(ond[i].Substring(0, 9) + ": " + trans);
                        results.Add(trans);
                        if (ond[i].Substring(0, 9).Contains(NumCount.ToString()))
                        {
                            break;
                        }
                    }
                    //Debug.Log(":" + ond[i]);
                }
                if (ond[i].Contains("MINIMUM") && ond[i].Contains("VALUES"))
                {
                    i++;
                    int.TryParse(ond[i].Substring(ond[i].Length - 12, 12), out index[0]); 
                    i++;
                    double.TryParse(ond[i].Substring(ond[i].Length - 12, 12), out values[0]);
                    //Debug.Log(index[0] + ": " + values[0]);
                }
                if (ond[i].Contains("MAXIMUM") && ond[i].Contains("VALUES"))
                {
                    i++;
                    int.TryParse(ond[i].Substring(ond[i].Length - 12, 11), out index[1]);
                    i++;
                    double.TryParse(ond[i].Substring(ond[i].Length - 12, 12), out values[1]);
                    //Debug.Log(index[1] + ": " + values[1]);
                }

            }
        }
        ond.Clear();//清空原始位移数据，释放内存
        return results;
    }
}
