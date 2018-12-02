using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class color_change_cxl  {

	public float[] f_L;
	public List<List<color_tria>> ct_L;

	List<List<Vector3>> line_l;
	public float scale=0.05f;

	public void init(int count,float[] f)
	{
		ct_L=new List<List<color_tria>>();
		for(int i=0;i<count;i++)
		{
			List<color_tria> l=new List<color_tria>();
			ct_L.Add(l);
		}
		f_L=f;
//		line_l = new List<List<Vector3>> ();
//		for (int i=0; i<f.Length; i++) {
//			List<Vector3> l=new List<Vector3>();
//			line_l.Add(l);
//		}
//		Debug.Log(f_L.Length);
	}



	public void deal_tria(float f1,float f2,float f3,Vector3 v1,Vector3 v2,Vector3 v3)
	{
		line_l = new List<List<Vector3>> ();
		for (int i=0; i<f_L.Length; i++) {
			List<Vector3> l=new List<Vector3>();
			line_l.Add(l);
		}
		int a = get_index (f1);
		int b = get_index (f2);
		int c = get_index (f3);
//						Debug.Log(a+"||"+b+" "+c+" ");
//						Debug.LogError(v1+" "+v2+" "+v3);
		create_l(f1,f2,v1,v2,a,b);
		create_l(f2,f3,v2,v3,b,c);
		create_l(f3,f1,v3,v1,c,a);

		int min = a;
		if (min > b)
			min = b;
		if(min>c)
			min = c;

		int max = a;
		if (max < b)
			max = b;
		if(max<c)
			max = c;

		for (int i=min; i<max; i++) 
		{
			if(line_l[i].Count!=4)
			{
//				Debug.LogError("error "+line_l[i].Count);
			}
			else
			{
				add_tria(line_l[i][0],line_l[i][1],line_l[i][2],line_l[i][3],i);
			}
		}


	}

	public void deal_tria(float f1,float f2,float f3,float f4,Vector3 v1,Vector3 v2,Vector3 v3,Vector3 v4)
	{
		line_l = new List<List<Vector3>> ();
		for (int i=0; i<f_L.Length; i++) {
			List<Vector3> l=new List<Vector3>();
			line_l.Add(l);
		}
		int a = get_index (f1);
		int b = get_index (f2);
		int c = get_index (f3);
		int d = get_index (f4);
//								Debug.Log(a+"||"+b+" "+c+" "+d);
//								Debug.LogError(v1+" "+v2+" "+v3);
		create_l(f1,f2,v1,v2,a,b);
		create_l(f2,f3,v2,v3,b,c);
		create_l(f3,f4,v3,v4,c,d);
		create_l(f4,f1,v4,v1,d,a);
		
		int min = a;
		if (min > b)
			min = b;
		if(min>c)
			min = c;
		if(min>d)
			min = d;
		
		int max = a;
		if (max < b)
			max = b;
		if(max<c)
			max = c;
		if(max<d)
			max = d;
		
		for (int i=min; i<=max; i++) 
		{
			if(line_l[i].Count!=4)
			{
//								Debug.LogError("error "+line_l[i].Count+" "+i);
			}
			else
			{
//				Debug.LogError(i);
				add_tria(line_l[i][0],line_l[i][1],line_l[i][2],line_l[i][3],i);
			}
		}
		
		
	}

	void create_l(float f1,float f2,Vector3 v1,Vector3 v2,int n1,int n2)
	{
		if(n1==n2)
		{
			return;
		}
		
		if(n1>n2)
		{
			float f;
			for(int i=n1;i>n2;i--)
			{
				f=f_L[i];
				Vector3 v=cacu_v(f1,f2,v1,v2,f);
				
				Vector3 vv=v-scale*(v2-v1);

				line_l[i].Add(vv);
				line_l[i].Add(v);
			}
		}
		else
		{
			float f;
			for(int i=n1+1;i<=n2;i++)
			{
				f=f_L[i];
				Vector3 v=cacu_v(f1,f2,v1,v2,f);
				Vector3 vv=v+scale*(v2-v1);
				
				line_l[i].Add(v);
				line_l[i].Add(vv);
			}
		}
	}
	
	Vector3 cacu_v(float f1,float f2,Vector3 v1,Vector3 v2,float f)
	{
		Vector3 v;
		if(f1>f2)
		{
			float t=	(f-f2)/(f1-f2);
			v=t*v1+(1-t)*v2;
			return v;
		}
		else
		{
			float t=	(f-f1)/(f2-f1);
			v=t*v2+(1-t)*v1;
			return v;
		}
	}

	void add_tria(Vector3 v1,Vector3 v2,Vector3 v3,Vector3 v4,int color_type)
	{
//		Debug.Log ("!!!!!!!!!!!!!!!!!!");
//		Debug.Log(v1.ToString("0.00000000"));
//		Debug.Log(v2.ToString("0.00000000"));
//		Debug.Log(v3.ToString("0.00000000"));
//		Debug.Log(v4.ToString("0.00000000"));
		color_tria t1=new color_tria(v3,v2,v1,color_type);
		insert_tria(t1);
		
		color_tria t2=new color_tria(v1,v4,v3,color_type);
		insert_tria(t2);
	}

	void insert_tria(color_tria t)
	{
		ct_L[t.color_type].Add(t);
	}

	int get_index(float f1)
	{
		for(int i=0;i<f_L.Length-1;i++)
		{
			if(i!=f_L.Length-2)
			{
				if(f1>=f_L[i]&&f1<f_L[i+1])
					return i;
			}
			else
			{
				if(f1>=f_L[i])
					return i;
			}
		}
		return 0;
	}
}
