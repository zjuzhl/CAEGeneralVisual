using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mxl_sample : MonoBehaviour {
	float[] real_midp = {
		
		156.94f, 164.22f, 90.026f, 66.818f, 57.148f, 53.939f, 51.905f, 51.109f, 52.825f, 66.089f, 94.058f, 132.89f,139.57f, 102.70f, 87.273f, 77.865f, 57.390f, 79.623f, 56.446f, 65.543f, 58.370f,0.39925f,103.11f, 81.019f, 66.707f, 53.606f, 39.538f, 25.964f, 14.444f, 6.3206f, 2.1425f, 2.1269f, 6.1560f, 13.957f, 25.058f, 38.534f, 53.477f, 70.208f, 89.402f, 130.55f, 86.344f, 78.077f, 64.126f, 51.861f, 
		40.744f, 30.522f, 21.052f, 12.268f, 5.4855f, 67.333f, 70.561f, 65.684f, 58.457f, 50.576f, 41.960f, 32.420f, 22.210f, 12.311f, 61.147f, 67.080f, 68.035f, 64.897f, 59.335f, 51.855f, 42.703f, 32.325f, 
		21.104f, 58.382f, 65.755f, 69.514f, 69.278f, 65.702f, 59.503f, 51.205f, 41.286f, 30.293f, 56.610f, 
		64.746f, 69.881f, 71.379f, 69.414f, 64.495f, 57.261f, 48.444f, 39.557f, 55.495f, 63.534f, 68.882f, 
		71.247f, 70.548f, 66.804f, 60.693f, 53.691f, 48.526f, 56.523f, 62.982f, 66.353f, 68.584f, 69.556f,67.277f, 61.813f, 56.556f, 56.395f, 65.069f, 67.475f, 63.576f, 62.107f, 66.685f, 69.063f, 63.279f, 
		54.592f, 59.613f, 91.197f, 82.590f, 69.120f, 56.732f, 54.202f, 79.302f, 67.107f, 54.080f, 49.435f
	}; 
	Color [] c_L;
	float[] _midPointT;
	public GameObject ob;
	float [] f_area;

	public Material red;
	public Material red1;
	color_change_new cc_script;
	Mesh mesh;
	List<GameObject> g_L;

	color_change_cxl cxl_script;
	// Use this for initialization
	void Start () {
		c_L=new Color[9];
		c_L[0]=new Color(0,0,255/255.0f);
		c_L[1]=new Color(0,180/255.0f,255/255.0f);
		c_L[2]=new Color(0,255/255.0f,255/255.0f);
		c_L[3]=new Color(0,255/255.0f,180/255.0f);
		c_L[4]=new Color(0,255/255.0f,0);
		c_L[5]=new Color(180/255.0f,255/255.0f,0);
		
		c_L[6]=new Color(255/255.0f,255/255.0f,0);
		c_L[7]=new Color(255/255.0f,180/255.0f,0);
		c_L[8]=new Color(255/255.0f,0,0);
		_midPointT=new float[121];
		fetch_real_array();
		 mesh = ob.GetComponent<MeshFilter>().mesh;

		f_area=new float[10];
		float min,max;
		min=-1;max=-1;
		for(int i=0;i<_midPointT.Length;i++)
		{
			if(i==0)
			{
				min=_midPointT[0];
				max=_midPointT[0];
			}
			else
			{
				if(_midPointT[i]<min)
				{
					min=_midPointT[i];
				}
				else if(_midPointT[i]>max)
				{
					max=_midPointT[i];
				}
			}
		}
		
		float size=(max-min)/9.0f;
		f_area[0]=min;
		f_area[9]=max;

		for(int i=1;i<9;i++)
		{
			f_area[i]=min+size*i;
			Debug.Log(i+" "+f_area[i].ToString("0.000000"));
		}

		cc_script=new color_change_new();
		g_L=new List<GameObject>();
		cxl_script = new color_change_cxl ();

//		for(int i=0;i<mesh.triangles.Length/3;i++)//mesh.triangles.Length/3
//		{
//			cc_script.deal_tria(_midPointT[mesh.triangles[3*i+0]],_midPointT[mesh.triangles[3*i+1]],_midPointT[mesh.triangles[3*i+2]],mesh.vertices[mesh.triangles[3*i+0]],mesh.vertices[mesh.triangles[3*i+1]],mesh.vertices[mesh.triangles[3*i+2]]);
//		}







	}

	public void clear()
	{
		if(g_L.Count>0)
		for(int i=0;i<g_L.Count;i++)
		{
			GameObject.DestroyImmediate(g_L[i]);
		}
	}

	void four_p(Vector3 v1,Vector3 v2,Vector3 v3,Vector3 v4,Material m,mesh_create mc,Vector3 p,int i)
	{
		List<Vector3> vl=new List<Vector3>();
		List<Vector3> nl=new List<Vector3>();
		vl.Add(v3);
		vl.Add(v2);
		vl.Add(v1);
		vl.Add(v4);
		vl.Add(v3);
		vl.Add(v1);
		nl.Add(new Vector3(0,-1,0));
		nl.Add(new Vector3(0,-1,0));
		nl.Add(new Vector3(0,-1,0));
      	nl.Add(new Vector3(0,-1,0));
      	nl.Add(new Vector3(0,-1,0));
		nl.Add(new Vector3(0,-1,0));
		GameObject g=new GameObject("mark"+i.ToString(), typeof(MeshFilter), typeof(MeshRenderer));
		mc.create(vl,nl,g,m);
		g.transform.position=p;
		g_L=new List<GameObject>();
	}
	
	// Update is called once per frame
	void OnGUI()
	{
		if(GUI.Button(new Rect(100,20,100,20),"case1"))
		{
			Debug.Log("!!!!!!!!!!!!!!!!!!!!!");
			clear();
//			this.transform.GetComponent<new_date_input>().clear();
//			cc_script.init(9,f_area);
			cxl_script.init(9,f_area);

			Vector3 []vv=new Vector3[121];
			
			for(int i=0;i<11;i++)
			{
				for(int j=0;j<11;j++)
				{

					vv[11*i+j]=new Vector3(2*(j-1)+2,0,2*(i-1)+2);
				}
			}
			
			for(int i=0;i<10;i++)
			{
				for(int j=0;j<10;j++)
				{
//					if(i==0&&j==9)
					{
//					cxl_script.deal_tria(_midPointT[11*i+j],_midPointT[11*i+j+1],_midPointT[11*i+j+12],vv[11*i+j],vv[11*i+j+1],vv[11*i+j+12]);
//					cxl_script.deal_tria(_midPointT[11*i+j],_midPointT[11*i+j+12],_midPointT[11*i+j+11],vv[11*i+j],vv[11*i+j+12],vv[11*i+j+11]);
					cxl_script.deal_tria(_midPointT[11*i+j],_midPointT[11*i+j+1],_midPointT[11*i+j+12],_midPointT[11*i+j+11],vv[11*i+j],vv[11*i+j+1],vv[11*i+j+12],vv[11*i+j+11]);
//					cc_script.deal_tria(_midPointT[11*i+j],_midPointT[11*i+j+1],_midPointT[11*i+j+12],_midPointT[11*i+j+11],vv[11*i+j],vv[11*i+j+1],vv[11*i+j+12],vv[11*i+j+11]);
					}
				}
			}
			
			g_L=new List<GameObject>();
			for(int i=0;i<9;i++)
			{
				GameObject ob1=new GameObject(ob.name+i.ToString(), typeof(MeshFilter), typeof(MeshRenderer));
				g_L.Add(ob1);
				ob1.transform.position=ob.transform.position;
			}
			mesh_create mc=new mesh_create();
			Shader ss1=red.shader;
			mesh.Clear();
//			for(int i=0;i<9;i++)
//			{
//				List<Vector3> vl=new List<Vector3>();
//				List<Vector3> nl=new List<Vector3>();
////				Debug.Log(i+"@@"+cc_script.ct_L[i].Count);
//				for(int j=0;j<cc_script.ct_L[i].Count;j++)
//				{
//					vl.Add(cc_script.ct_L[i][j].v1);
//					vl.Add(cc_script.ct_L[i][j].v2);
//					vl.Add(cc_script.ct_L[i][j].v3);
//					nl.Add(new Vector3(0,1,0));
//					nl.Add(new Vector3(0,1,0));
//					nl.Add(new Vector3(0,1,0));
//				}
//			
//				Material m=new Material(ss1);
//				m.color=c_L[i];
//				mc.create(vl,nl,g_L[i],m);
//			}

			List<Vector3> vl1=new List<Vector3>();
			List<Vector3> nl1=new List<Vector3>();
			for(int i=0;i<9;i++)
			{
			for(int j=0;j<cxl_script.ct_L[i].Count;j++)
			{
				vl1.Add(cxl_script.ct_L[i][j].v1);
				vl1.Add(cxl_script.ct_L[i][j].v2);
				vl1.Add(cxl_script.ct_L[i][j].v3);
				nl1.Add(new Vector3(0,1,0));
				nl1.Add(new Vector3(0,1,0));
				nl1.Add(new Vector3(0,1,0));
			}
			}
			Shader ss=red.shader;
			Material m1=new Material(ss);
			m1.color=c_L[1];
			GameObject ob11=new GameObject("line", typeof(MeshFilter), typeof(MeshRenderer));
			mc.create(vl1,nl1,ob11,m1);
		}
	}
	void fetch_real_array()
	{
		int count=0;
		//		int mod=0;
		int s=0;
		for(int k=11;k>=9;k--)
		{
			if(k==11)
			{
				//				if(mod%2==0)
				{
					
					s=(11-k)/2;
					if(k==7)
					{
						Debug.Log(count+" cc "+s);
					}
					
					_midPointT[s+s*11]=real_midp[count];
					count++;
					_midPointT[s+s*11+k-1]=real_midp[count];
					count++;
					for(int i=1;i<k-1;i++)
					{
						_midPointT[s+s*11+i]=real_midp[count];
						count++;
					}
					
					_midPointT[(s+k-1)+(s+k-1)*11]=real_midp[count];
					count++;
					
					for(int i=1;i<k-1;i++)
					{
						_midPointT[(s+k-1)+(s+i)*11]=real_midp[count];
						count++;
					}
					
					_midPointT[(s)+(s+k-1)*11]=real_midp[count];
					count++;
					for(int i=k-2;i>=1;i--)
					{
						_midPointT[(s+i)+(s+k-1)*11]=real_midp[count];
						count++;
					}
					
					for(int i=k-2;i>=1;i--)
					{
						_midPointT[(s)+(s+i)*11]=real_midp[count];
						count++;
					}
					k--;
					//					mod++;
				}
				//				else
				//				{
				//					s=(11-k)/2;
				//					if(k==9)
				//					{
				//						Debug.Log(count+" cc "+s);
				//					}
				//					_midPointT[s+s*11]=real_midp[count];
				//					count++;
				//					_midPointT[s+(s+k-1)*11]=real_midp[count];
				//					count++;
				//
				//					for(int i=1;i<k-1;i++)
				//					{
				//						_midPointT[s+(s+i)*11]=real_midp[count];
				//						count++;
				//					}
				//					if(k==9)
				//					{
				//						Debug.Log(count+" cc "+s);
				//					}
				//
				//					_midPointT[(s+k-1)+(s+k-1)*11]=real_midp[count];
				//					count++;
				//					for(int i=1;i<k-1;i++)
				//					{
				//						_midPointT[(s+i)+(s+k-1)*11]=real_midp[count];
				//						count++;
				//					}
				//					if(k==9)
				//					{
				//						Debug.Log(count+" cc "+s);
				//					}
				//
				//					_midPointT[s+s*11+k-1]=real_midp[count];
				//					count++;
				//					for(int i=k-2;i>=1;i--)
				//					{
				//						_midPointT[(s+k-1)+(s+i)*11]=real_midp[count];
				//						count++;
				//					}
				//					if(k==9)
				//					{
				//						Debug.Log(count+" cc "+s);
				//					}
				//
				//					for(int i=k-2;i>=1;i--)
				//					{
				//						_midPointT[s+s*11+i]=real_midp[count];
				//						count++;
				//					}
				//
				//					mod++;
				//				}
			}
			else
			{
				for(int n=1;n<=9;n++)
				{
					for(int m=1;m<=9;m++)
					{
						_midPointT[n+m*11]=real_midp[count];
						count++;
					}
				}
				
				Debug.Log(count+"||||||||||||||||||||||||");
			}
			//			else
			//			{
			//				s=(11-k)/2;
			//				Debug.Log(count+"||||||||||||||||||||||||");
			//				_midPointT[s+s*11]=real_midp[count];
			//			}
			
			//			k--;
		}
	}
}
