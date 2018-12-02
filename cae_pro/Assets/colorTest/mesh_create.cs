using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mesh_create 
{
	GameObject a_test;
	public void create(List<Vector3> all_result,List<Vector3> normol,GameObject obj,Material red)
	{
		//		Vector3 po=target.transform.position;
		//		red=target.renderer.material;
		
		a_test=obj;//target;
		int obj_num=1;
		
		if(all_result.Count>64998)
		{
			obj_num=all_result.Count/64998;
			if(all_result.Count%64998==0)
			{
				obj_num+=1;
			}
			else
			{
				obj_num+=1;
			}
		}
		
		
		
		for(int i=0;i<obj_num;i++)
		{
			if(i==0)
			{
				Vector3 [] v;
				Vector3 [] n;
				if(obj_num>1)
				{
					v=new Vector3[64998];
					n=new Vector3[64998];
					for(int j=0;j<64998;j++)
					{
						v[j]=all_result[i*64998+j];
						n[j]=normol[i*64998+j];
						
					}
				}
				else
				{
					v=all_result.ToArray();
					n=normol.ToArray();
				}
				
				
				
				Vector2 [] uv=new Vector2[v.Length];
				
				int[] index=new int[v.Length];
				
				for(int ii=0;ii<v.Length;ii++)
				{
					index[ii]=ii;
				}
				

				
				Mesh _mymesh1;
				
				_mymesh1=a_test.GetComponent<MeshFilter>().mesh;
				_mymesh1.Clear();
				_mymesh1.vertices=v;
				_mymesh1.normals=n;
				_mymesh1.uv=uv;
				
				_mymesh1.triangles=index;
				a_test.GetComponent<Renderer>().material=red;	
				a_test.transform.position=obj.transform.position;//target.transform.position;
				//				a_test.GetComponent<BoxCollider>().size=new Vector3(1,1,1);
			}
			else
			{
				Vector3 []v1;
				Vector3 []n1;
				if(i<(obj_num-1))
				{
					v1=new Vector3[64998];
					n1=new Vector3[64998];
					
					for(int j=0;j<64998;j++)
					{
						v1[j]=all_result[i*64998+j];
						n1[j]=normol[i*64998+j];
					}
				}
				else
				{
					
					int co=all_result.Count%64998;
					v1=new Vector3[co];
					n1=new Vector3[co];
					for(int j=0;j<co;j++)
					{
						v1[j]=all_result[i*64998+j];
						n1[j]=normol[i*64998+j];
					}
				}
				
				//				Vector3 [] n1=new Vector3[v1.Length];
				Vector2 [] uv1=new Vector2[v1.Length];
				int[] index1=new int[v1.Length];
				for(int jj=0;jj<v1.Length;jj++)
				{
					index1[jj]=jj;
				}
				
				
				GameObject b=GameObject.Find((obj.name+i.ToString()));
				if(b==null)
				{
					b=new GameObject(obj.name+i.ToString(), typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));
				}
				
				Mesh _mymesh2;
				
				_mymesh2=b.GetComponent<MeshFilter>().mesh;
				_mymesh2.Clear();
				_mymesh2.vertices=v1;
				_mymesh2.normals=n1;
				_mymesh2.uv=uv1;
				
				_mymesh2.triangles=index1;
				b.GetComponent<Renderer>().material=red;	
				b.transform.position=obj.transform.position;//target.transform.position;
				//				b.GetComponent<BoxCollider>().size=new Vector3(1,1,1);
				b.transform.parent=a_test.transform;
				
			}
		}	
	}
}
