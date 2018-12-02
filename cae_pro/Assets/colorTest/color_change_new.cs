using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class color_change_new {
	
	
	public float[] f_L;
	public List<List<color_tria>> ct_L;

	int _d1,_d2;
	int _a1,_a2;
//	int test_count;
	int add_num=0;
	public void init(int count,float[] f)
	{
		ct_L=new List<List<color_tria>>();
		for(int i=0;i<count;i++)
		{
			List<color_tria> l=new List<color_tria>();
			ct_L.Add(l);
		}
		f_L=f;
//		Debug.Log(f_L.Length);
	}
	
	public void deal_tria(float f1,float f2,float f3,float f4,Vector3 v1,Vector3 v2,Vector3 v3,Vector3 v4)
	{
		add_num=0;
		int a=get_index(f1);
		int b=get_index(f2);
		int c=get_index(f3);
		int d=get_index(f4);
//		Debug.Log(f1+"||"+f2+" "+f3+" "+f4);
//				Debug.Log(a+"||"+b+" "+c+" "+d);
//				Debug.LogError(v1+" "+v2+" "+v3+" "+v4);
		
		List<Vector3> la=new List<Vector3>();
		List<Vector3> lb=new List<Vector3>();
		List<Vector3> lc=new List<Vector3>();
		List<Vector3> ld=new List<Vector3>();
		
		create_l(f1,f2,v1,v2,a,b,la);
		create_l(f2,f3,v2,v3,b,c,lb);
		create_l(f3,f4,v3,v4,c,d,lc);
		create_l(f4,f1,v4,v1,d,a,ld);
		
		//		for(int i=0;i<la.Count;i++)
		//		{
		//			Debug.Log("la:"+la[i].ToString("0.0000"));
		//		}
		//		for(int i=0;i<lb.Count;i++)
		//		{
		//			Debug.Log("lb:"+lb[i].ToString("0.0000"));
		//		}for(int i=0;i<lc.Count;i++)
		//		{
		//			Debug.Log("lc:"+lc[i].ToString("0.0000"));
		//		}
		
		
		if(a<=b&&a<=c&&a<=d)
		{
//			Debug.Log("here0");
			deal(a,b,c,d,la,lb,lc,ld,v1,v2,v3,v4);
		}
		else if(b<=c&&b<=a&&b<=d)
		{
//			Debug.Log("here1");
			deal(b,c,d,a,lb,lc,ld,la,v2,v3,v4,v1);
		}
		else if(c<=a&&c<=b&&c<=d)
		{
//			Debug.Log("here2");
			deal(c,d,a,b,lc,ld,la,lb,v3,v4,v1,v2);
		}
		else
		{
//			Debug.Log("here3");
			deal(d,a,b,c,ld,la,lb,lc,v4,v1,v2,v3);
		}

		
//		test_count++;
	}
	
	
	
	void create_l(float f1,float f2,Vector3 v1,Vector3 v2,int n1,int n2,List<Vector3> l)
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
				
				l.Add(v);
			}
		}
		else
		{
			float f;
			for(int i=n1+1;i<=n2;i++)
			{
				f=f_L[i];
				Vector3 v=cacu_v(f1,f2,v1,v2,f);
				l.Add(v);
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
	void deal(int a,int b,int c,int d,List<Vector3> la,List<Vector3> lb,List<Vector3> lc,List<Vector3> ld,Vector3 va,Vector3 vb,Vector3 vc,Vector3 vd)
	{
//		if (la.Count + lb.Count + lc.Count + ld.Count==4) {
//			Debug.LogWarning (test_count+26955);
//		}

		if(a==b&&c==d&&a==c)
		{
			List<Vector3> l=new List<Vector3>();
			l.Add(va);
			l.Add(vd);
			l.Add(vc);
			l.Add(vb);
			add_tria(l,a);
			return;
		}
		if(ld.Count==la.Count+lb.Count+lc.Count)
		{
//			Debug.Log("EEEEEEEEEEEEEE"+ld.Count);
			Vector3 end=vd;
			List< List<Vector3>> left_l=new List<List<Vector3>>();
			List< List<Vector3>> right_l=new List<List<Vector3>>();
			List<Vector3> r_bf=new List<Vector3>();
			List<Vector3> l_bf=new List<Vector3>();
			List<Vector3> r_af=new List<Vector3>();
			List<Vector3> l_af=new List<Vector3>();
			if(ld.Count==1)
			{
//				Debug.Log("EEEEEEEEEEEEEE");
				List<Vector3> l1=new List<Vector3>();
				l1.Add(ld[0]);
				List<Vector3> l2=new List<Vector3>();
//				for(int i=0;i<la.Count;i++)
//				{
//					l2.Add(la[i]);
//				}
//				l2.Add(vb);
//				for(int i=0;i<lb.Count;i++)
//				{
//					l2.Add (lb[i]);
//				}
//				l2.Add(vc);
//				for(int i=0;i<lc.Count;i++)
//				{
//					l2.Add(lc[i]);
//				}
//				Debug.Log(l2.Count);

				if(la.Count==0)
				{
					if(lb.Count==0)
					{
						r_bf.Add(vb);
						r_bf.Add(vc);
						for(int i=0;i<lc.Count;i++)
						{
							l2.Add(lc[i]);
						}
					}
					else
					{
						r_bf.Add(vb);
						for(int i=0;i<lb.Count;i++)
						{
							l2.Add(lb[i]);
						}
						r_af.Add(vc);
					}
				}
				else
				{
					for(int i=0;i<la.Count;i++)
					{
						l2.Add(la[i]);
					}
					r_af.Add(vb);
					r_af.Add(vc);
				}


				left_l.Add(l1);
				right_l.Add(l2);
				new_deal(va,a,left_l,right_l,end,l_bf,r_bf,l_af,r_af);
			}
			else
			{
				for(int i=ld.Count-1;i>0;i--)
				{
					List<Vector3> l1=new List<Vector3>();
					l1.Add(ld[i]);
					l1.Add(ld[i-1]);
					left_l.Add(l1);
				}
//				Debug.Log(left_l.Count+"!!!!!!!!!!!!"+ld.Count);
//				Debug.Log(la.Count+" "+lb.Count+" "+lc.Count);
				for(int i=0;i<la.Count-1;i++)
				{
					List<Vector3> l2=new List<Vector3>();
					l2.Add(la[i]);
					l2.Add(la[i+1]);
			       	right_l.Add(l2);
				}
//				Debug.Log(right_l.Count+"!!!!!!!!!!!!"+la.Count);
				if(la.Count==0)
				{
					if(lb.Count==0)
					{
						{
//							List<Vector3> l2=new List<Vector3>();
//							l2.Add(va);
//							l2.Add(vb);
//							l2.Add(lc[0]);
//							right_l.Add(l2);
							r_bf.Add(vb);
							r_bf.Add(vc);
						}
						for(int i=0;i<lc.Count-1;i++)
						{
							List<Vector3> l2=new List<Vector3>();
							l2.Add(lc[i]);
							l2.Add(lc[i+1]);
							right_l.Add(l2);

						}
					}
					else
					{
						{
//							List<Vector3> l2=new List<Vector3>();
//							l2.Add(va);
//							l2.Add(lb[0]);
//							right_l.Add(l2);
							r_bf.Add(vb);
						}
						for(int i=0;i<lb.Count-1;i++)
						{
							List<Vector3> l2=new List<Vector3>();
							l2.Add(lb[i]);
							l2.Add(lb[i+1]);
							right_l.Add(l2);	
						}

						if(lc.Count==0)
						{
//							List<Vector3> l2=new List<Vector3>();
//							l2.Add(lb[lb.Count-1]);
//							l2.Add(vc);
//							right_l.Add(l2);

							r_af.Add(vc);
						}

						else
						{
							{
								List<Vector3> l2=new List<Vector3>();
								l2.Add(lb[lb.Count-1]);
								l2.Add(vc);
								l2.Add(lc[0]);
								right_l.Add(l2);
							}

							for(int i=0;i<lc.Count-1;i++)
							{
								List<Vector3> l2=new List<Vector3>();
								l2.Add(lc[i]);
								l2.Add(lc[i+1]);
								right_l.Add(l2);
								
							}
						}

					}
				}
				else
				{
					if(lb.Count==0)
					{
						{
							List<Vector3> l2=new List<Vector3>();
							l2.Add(la[la.Count-1]);
							l2.Add(vb);
							l2.Add(vc);
							if(lc.Count>0)
							l2.Add(lc[0]);
							right_l.Add(l2);
						}

						if (lc.Count == 0) {
							List<Vector3> l2=new List<Vector3>();
							if(ld.Count!=0)
								l2.Add(ld[0]);
							else
								l2.Add(vd);
							l2.Add(vd);
							left_l.Add (l2);
						}


						for(int i=0;i<lc.Count-1;i++)
						{
							List<Vector3> l2=new List<Vector3>();
							l2.Add(lc[i]);
							l2.Add(lc[i+1]);
							right_l.Add(l2);
							
						}
					}
					else
					{
						{
							List<Vector3> l2=new List<Vector3>();
							l2.Add(la[la.Count-1]);
							l2.Add(vb);
							l2.Add(lb[0]);
							right_l.Add(l2);
//							Debug.Log("here");
						}
						for(int i=0;i<lb.Count-1;i++)
						{
							List<Vector3> l2=new List<Vector3>();
							l2.Add(lb[i]);
							l2.Add(lb[i+1]);
							right_l.Add(l2);	
						}
						
						if(lc.Count==0)
						{
//							List<Vector3> l2=new List<Vector3>();
//							l2.Add(lb[lb.Count-1]);
//							l2.Add(vc);
//							right_l.Add(l2);

							r_af.Add(vc);

						}
						
						else
						{
							{
								List<Vector3> l2=new List<Vector3>();
								l2.Add(lb[lb.Count-1]);
								l2.Add(vc);
								l2.Add(lc[0]);
								right_l.Add(l2);
							}
							
							for(int i=0;i<lc.Count-1;i++)
							{
								List<Vector3> l2=new List<Vector3>();
								l2.Add(lc[i]);
								l2.Add(lc[i+1]);
								right_l.Add(l2);
								
							}
						}
					}
				}


			}
			new_deal(va,a,left_l,right_l,end,l_bf,r_bf,l_af,r_af);
		}
		else if(ld.Count+lc.Count==la.Count+lb.Count)
		{
//			Debug.Log("here");
			Vector3 end=vc;
			List< List<Vector3>> left_l=new List<List<Vector3>>();
			List< List<Vector3>> right_l=new List<List<Vector3>>();
			List<Vector3> r_bf=new List<Vector3>();
			List<Vector3> l_bf=new List<Vector3>();
			List<Vector3> r_af=new List<Vector3>();
			List<Vector3> l_af=new List<Vector3>();
			for(int i=ld.Count-1;i>0;i--)
			{
				List<Vector3> l1=new List<Vector3>();
				l1.Add(ld[i]);
				l1.Add(ld[i-1]);
				left_l.Add(l1);
			}
		
			if(ld.Count==0)
			{
				{
//					List<Vector3> l1=new List<Vector3>();
//					l1.Add(vd);
//					l1.Add(lc[lc.Count-1]);
//					left_l.Add(l1);

					l_bf.Add(vd);
				}
				for(int i=lc.Count-1;i>0;i--)
				{
					List<Vector3> l1=new List<Vector3>();
					l1.Add(lc[i]);
					l1.Add(lc[i-1]);
					left_l.Add(l1);

				}
				if(lc.Count==1)
				{
					List<Vector3> l1=new List<Vector3>();
					l1.Add(lc[0]);
					left_l.Add(l1);
				}
			}
			else
			{
				if(lc.Count==0)
				{
//					List<Vector3> l1=new List<Vector3>();
//					l1.Add(ld[0]);
//					l1.Add(vd);
//					left_l.Add(l1);
					l_af.Add(vd);
					if(ld.Count==1)
					{
						List<Vector3> l1=new List<Vector3>();
						l1.Add(ld[0]);					
						left_l.Add(l1);
					}
				}
				else
				{
					{
						List<Vector3> l1=new List<Vector3>();
						l1.Add(ld[0]);
						l1.Add(vd);
						l1.Add(lc[lc.Count-1]);
						left_l.Add(l1);
					}

					for(int i=lc.Count-1;i>0;i--)
					{
						List<Vector3> l1=new List<Vector3>();
						l1.Add(lc[i]);
						l1.Add(lc[i-1]);
						left_l.Add(l1);
						
					}


				}
			}

			for(int i=0;i<la.Count-1;i++)
			{
				List<Vector3> l2=new List<Vector3>();
				l2.Add(la[i]);
				l2.Add(la[i+1]);
				right_l.Add(l2);
			}
			if(la.Count==0)
			{
				{
//					List<Vector3> l2=new List<Vector3>();
//					l2.Add(va);
//					l2.Add(lb[0]);
//					right_l.Add(l2);
					r_bf.Add(vb);
				}
				for(int i=0;i<lb.Count-1;i++)
				{
					List<Vector3> l2=new List<Vector3>();
					l2.Add(lb[i]);
					l2.Add(lb[i+1]);
					right_l.Add(l2);
					
				}
				if(lb.Count==1)
				{
					List<Vector3> l2=new List<Vector3>();
					l2.Add(lb[0]);
					right_l.Add(l2);
				}
			}
			else
			{
				if(lb.Count==0)
				{
//					List<Vector3> l2=new List<Vector3>();
//					l2.Add(la[la.Count-1]);
//					l2.Add(vb);
//					right_l.Add(l2);
					r_af.Add(vb);
					if(la.Count==1)
					{
						List<Vector3> l2=new List<Vector3>();
						l2.Add(la[0]);
	
						right_l.Add(l2);
					}
				}
				else
				{
					{
						List<Vector3> l2=new List<Vector3>();
						l2.Add(la[la.Count-1]);
						l2.Add(vb);
						l2.Add(lb[0]);
						right_l.Add(l2);
					}

					for(int i=0;i<lb.Count-1;i++)
					{
						List<Vector3> l2=new List<Vector3>();
						l2.Add(lb[i]);
						l2.Add(lb[i+1]);
						right_l.Add(l2);
						
					}
				}
			}

			new_deal(va,a,left_l,right_l,end,l_bf,r_bf,l_af,r_af);
		}
		else if(ld.Count+lc.Count+lb.Count==la.Count)
		{
			Vector3 end=vb;
			List< List<Vector3>> left_l=new List<List<Vector3>>();
			List< List<Vector3>> right_l=new List<List<Vector3>>();
			List<Vector3> r_bf=new List<Vector3>();
			List<Vector3> l_bf=new List<Vector3>();
			List<Vector3> r_af=new List<Vector3>();
			List<Vector3> l_af=new List<Vector3>();
			if(la.Count==1)
			{
				List<Vector3> l2=new List<Vector3>();
				l2.Add(la[0]);
				List<Vector3> l1=new List<Vector3>();
//				for(int i=0;i<ld.Count;i++)
//				{
//					l1.Add(ld[i]);
//				}
//				l1.Add(vd);
//				for(int i=0;i<lc.Count;i++)
//				{
//					l1.Add (lc[i]);
//				}
//				l1.Add(vc);
//				for(int i=0;i<lb.Count;i++)
//				{
//					l1.Add(lb[i]);
//				}
//				left_l.Add(l1);
				right_l.Add(l2);

				if(ld.Count==0)
				{
					if(lc.Count==0)
					{
						l_bf.Add(vd);
						l_bf.Add(vc);
						for(int i=0;i<lb.Count;i++)
						{
							l1.Add(lb[i]);
						}
						left_l.Add(l1);
//						Debug.Log("here");
					}
					else
					{
						l_bf.Add(vd);
						for(int i=0;i<lc.Count;i++)
						{
							l1.Add(lc[i]);
						}
						l_af.Add(vc);

					}
				}
				else
				{
					for(int i=0;i<ld.Count;i++)
					{
						l1.Add(ld[i]);
					}
					l_af.Add(vd);
					l_af.Add(vc);
				}

//				Debug.Log(left_l.Count+" "+right_l.Count+" "+right_l[0].Count);)

			}
			else
			{
				for(int i=0;i<la.Count-1;i++)
				{
					List<Vector3> l2=new List<Vector3>();
					l2.Add(la[i]);
					l2.Add(la[i+1]);
					right_l.Add(l2);
				}
				
				for(int i=ld.Count-1;i>0;i--)
				{
					List<Vector3> l1=new List<Vector3>();
					l1.Add(ld[i]);
					l1.Add(ld[i-1]);
					left_l.Add(l1);//right_l
				}

				
				if(ld.Count==0)
				{
					if(lc.Count==0)
					{
						{
//							List<Vector3> l1=new List<Vector3>();
//							l1.Add(vd);
//							l1.Add(vc);
//							l1.Add(lb[lb.Count-1]);
//							left_l.Add(l1);

							l_bf.Add(vd);
							l_bf.Add(vc);
						}
						for(int i=lb.Count-1;i>0;i--)
						{
							List<Vector3> l1=new List<Vector3>();
							l1.Add(lb[i]);
							l1.Add(lb[i-1]);
							left_l.Add(l1);
							
						}
					}
					else
					{
						{
//							List<Vector3> l1=new List<Vector3>();
//							l1.Add(vd);
//							l1.Add(lc[lc.Count-1]);
//							left_l.Add(l1);
							l_bf.Add(vd);
						}
						for(int i=lc.Count-1;i>0;i--)
						{
							List<Vector3> l1=new List<Vector3>();
							l1.Add(lc[i]);
							l1.Add(lc[i+1]);
							left_l.Add(l1);	
						}
						
						if(lb.Count==0)
						{
//							List<Vector3> l1=new List<Vector3>();
//							l1.Add(lc[0]);
//							l1.Add(vc);
//							left_l.Add(l1);

							l_af.Add(vc);
							if(lc.Count==1)
							{
								List<Vector3> l1=new List<Vector3>();
								l1.Add(lc[0]);
								left_l.Add(l1);	
							}
						}
						
						else
						{
							{
								List<Vector3> l1=new List<Vector3>();
								l1.Add(lc[0]);
								l1.Add(vc);
								l1.Add(lb[lb.Count-1]);
								left_l.Add(l1);
							}
							
							for(int i=lb.Count-1;i>0;i--)
							{
								List<Vector3> l1=new List<Vector3>();
								l1.Add(lb[i]);
								l1.Add(lb[i-1]);
								left_l.Add(l1);
								
							}
						}
						
					}
				}
				else
				{
					if(lc.Count==0)
					{
						{
							List<Vector3> l2=new List<Vector3>();
							l2.Add(ld[0]);
							l2.Add(vd);
							l2.Add(vc);
							l2.Add(lb[lb.Count-1]);
							left_l.Add(l2);


						}
						
						for(int i=lb.Count-1;i>0;i--)
						{
							List<Vector3> l2=new List<Vector3>();
							l2.Add(lb[i]);
							l2.Add(lb[i-1]);
							left_l.Add(l2);
							
						}
					}
					else
					{
						{
							List<Vector3> l2=new List<Vector3>();
							l2.Add(ld[0]);
							l2.Add(vd);
							l2.Add(lc[lc.Count-1]);
							left_l.Add(l2);
						}
						for(int i=lc.Count-1;i>0;i--)
						{
							List<Vector3> l2=new List<Vector3>();
							l2.Add(lc[i]);
							l2.Add(lc[i-1]);
							left_l.Add(l2);	
						}
						
						if(lb.Count==0)
						{
//							List<Vector3> l2=new List<Vector3>();
//							l2.Add(lc[0]);
//							l2.Add(vc);
//							left_l.Add(l2);

							l_af.Add(vc);

						}
						
						else
						{
							{
								List<Vector3> l2=new List<Vector3>();
								l2.Add(lc[0]);
								l2.Add(vc);
								l2.Add(lb[lb.Count-1]);
								left_l.Add(l2);
							}
							
							for(int i=lb.Count-1;i>0;i--)
							{
								List<Vector3> l2=new List<Vector3>();
								l2.Add(lb[i]);
								l2.Add(lb[i-1]);
								left_l.Add(l2);
								
							}
						}
					}
				}
				
				
			}
			new_deal(va,a,left_l,right_l,end,l_bf,r_bf,l_af,r_af);
		}
		else
		{
			Debug.LogError("error");
		}
	
	}


	void new_deal(Vector3 va,int a, List< List<Vector3>> ld,List<List<Vector3>> la,Vector3 end,List<Vector3> l1,List<Vector3> l2,List<Vector3> l3,List<Vector3> l4)
	{
		if(la.Count!=ld.Count)
		{
//			Debug.Log(la.Count+" error "+ld.Count+" "+test_count);
			return;
		}
		if(la.Count==0)
		{
//			Debug.Log(la.Count+" error "+ld.Count);
			return;
		}
		int type=a;
//		if(la[0].Count==0||ld[0].Count==0)
//		{
//			Debug.Log(la[0].Count+" "+ld[0].Count);
//		}
		{
			List<Vector3> l=new List<Vector3>();
			for(int i=0;i<l1.Count;i++)
			{
				l.Add(l1[i]);
			}
			l.Add(ld[0][0]);
			l.Add(la[0][0]);
			for(int i=l2.Count-1;i>=0;i--)
			{
				l.Add(l2[i]);
			}

			l.Add(va);
//			Debug.Log(l1.Count+" "+l2.Count);
//			for(int i=0;i<l.Count;i++)
//			{
//				Debug.Log(l[i].ToString("0.000000"));
//			}

//			add_tria(la[0][0],va,ld[0][0],a);
			add_tria(l,type);
		}
		type++;
		for(int i=0;i<ld.Count;i++)
		{
			List<Vector3> l=new List<Vector3>();
			for(int j=0;j<ld[i].Count;j++)
			{
				l.Add(ld[i][j]);
			}
			for(int j=la[i].Count-1;j>=0;j--)
			{
				l.Add(la[i][j]);
//				if(i==2)
//					Debug.Log(la[i][j]);
			}


			add_tria(l,type);
			if((ld[i].Count+la[i].Count)>2)
				type++;
		}
		{
			List<Vector3> l=new List<Vector3>();
			l.Add(ld[ld.Count-1][ld[ld.Count-1].Count-1]);
			for(int i=0;i<l3.Count;i++)
			{
				l.Add(l3[i]);
			}
			l.Add(end);

			for(int i=l4.Count-1;i>=0;i--)
			{
				l.Add(l4[i]);
			}
			l.Add(la[la.Count-1][la[la.Count-1].Count-1]);
			add_tria(l,type);
		}
//		if (add_num == 0)
//			Debug.LogWarning (test_count);
//		add_tria (ld[ld.Count-1][ld[ld.Count-1].Count-1],end,la[la.Count-1][la[la.Count-1].Count-1],type);
		                      
	}
	void add_tria(List<Vector3>l,int color_type)
	{

		for(int i=1;i<l.Count-1;i++)
		{
			add_num++;
			color_tria t=new color_tria(l[i-1],l[i],l[l.Count-1],color_type);
			insert_tria(t);
		}
	}

	//保证a是最小的
//	void deal(int a,int b,int c,int d,List<Vector3> la,List<Vector3> lb,List<Vector3> lc,List<Vector3> ld,Vector3 va,Vector3 vb,Vector3 vc,Vector3 vd)
//	{
//		if(a==b&&b==c)
//		{
//			//#########
//			//add_tria
//			add_tria(va,vb,vc,a);
//			return;
//		}
//		
//		if(la.Count>lc.Count)
//		{
//			if(lc.Count==0)
//			{
//				Vector3 v_bf=va;
//				Vector3 v_af=vc;
//				int type=a;
//				for(int i=0;i<la.Count;i++)
//				{
//					add_tria(v_bf,la[i],v_af,lb[lb.Count-1-i],type);
//					v_bf=la[i];
//					v_af=lb[lb.Count-1];
//					type+=1;
//				}
//				
//				add_tria(v_bf,vb,v_af,type);
//			}
//			else
//			{
//				Vector3 v_bf=va;
//				Vector3 v_af;
//				int type=a;
//				
//				add_tria(v_bf,la[0],lc[lc.Count-1],type);
//				v_bf=la[0];
//				v_af=lc[lc.Count-1];
//				type+=1;
//				for(int i=1;i<lc.Count;i++)
//				{
//					add_tria(v_bf,la[i],v_af,lc[lc.Count-i-1],type);
//					v_bf=la[i];
//					v_af=lc[lc.Count-i];
//					type+=1;
//				}
//				
//				//
//				if(lb.Count==0)
//				{
//					Debug.LogError("error");
//					return;
//				}
//				add_tria(v_bf,la[lc.Count],lb[lb.Count-1],vc,v_af,type);
//				
//				//				Debug.Log(v_bf+" "+la[lc.Count]+" "+lb[lb.Count-1]+" "+vb+" "+v_af);
//				v_bf=la[lc.Count];
//				v_af=lb[lb.Count-1];
//				type+=1;
//				
//				for(int i=1;i<lb.Count;i++)
//				{
//					add_tria(v_bf,la[lc.Count+1],v_af,lb[lb.Count-1-i],type);
//					v_bf=la[lc.Count+1];
//					v_af=lb[lb.Count-1-i];
//					type+=1;
//				}
//				
//				
//				add_tria(v_bf,vb,v_af,type);
//				Debug.Log(v_bf+" "+" "+vc+" "+v_af);
//			}
//		}
//		else if(la.Count==lc.Count)
//		{
//			Vector3 v_bf=va;
//			Vector3 v_af=vc;
//			int type=a;
//			
//			add_tria(v_bf,la[0],lc[lc.Count-1],type);
//			v_bf=la[0];
//			v_af=lc[lc.Count-1];
//			type+=1;
//			for(int i=1;i<lc.Count;i++)
//			{
//				add_tria(v_bf,la[i],v_af,lc[lc.Count-i-1],type);
//				v_bf=la[i];
//				v_af=lc[lc.Count-i];
//				type+=1;
//			}
//			
//			add_tria(v_bf,vb,v_af,vc,type);
//		}
//		else
//		{
//			if(la.Count==0)
//			{
//				Vector3 l_bf,r_bf;
//				l_bf=va;
//				r_bf=vb;
//				int type=a;
//				for(int i=0;i<lb.Count;i++)
//				{
//					add_tria(l_bf,r_bf,lc[lc.Count-1-i],lb[i],type);
//					Debug.Log(l_bf.ToString("0.0000")+" "+r_bf.ToString("0.0000"));
//					
//					l_bf=lc[lc.Count-1-i];
//					r_bf=lb[i];
//					type+=1;
//				}
//				
//				add_tria(l_bf,r_bf,vc,type);
//			}
//			else
//			{
//				//###########################
//				Vector3 v_bf=va;
//				Vector3 v_af;
//				int type=a;
//				
//				add_tria(v_bf,la[0],lc[lc.Count-1],type);
//				v_bf=la[0];
//				v_af=lc[lc.Count-1];
//				type+=1;
//				//$$$$$$$$$$$$$$$$$$$$$$$$$$
//				for(int i=1;i<la.Count;i++)
//				{
//					add_tria(v_bf,la[i],v_af,lc[lc.Count-1-i],type);
//					v_bf=la[i];
//					v_af=lc[lc.Count-1-i];
//					type+=1;
//				}
//				
//				add_tria(v_bf,vb,lb[0],lc[lc.Count-1-la.Count],v_af,type);
//				v_bf=lb[0];
//				v_af=lc[lc.Count-1-la.Count];
//				type+=1;
//				
//				for(int i=1;i<lb.Count;i++)
//				{
//					add_tria(v_bf,lb[i],v_af,lc[lc.Count-1-la.Count-i],type);
//					v_bf=lb[i];
//					v_af=lc[lc.Count-1-la.Count-i];
//					type+=1;
//				}
//				
//				add_tria(v_bf,vc,v_af,vc,type);
//				
//			}
//		}
//	}
	
	void add_tria(Vector3 v1,Vector3 v2,Vector3 v3,Vector3 v4,Vector3 v5,int color_type)
	{
		color_tria t1=new color_tria(v1,v2,v3,color_type);
		insert_tria(t1);
		
		color_tria t2=new color_tria(v1,v3,v5,color_type);
		insert_tria(t2);
		
		color_tria t3=new color_tria(v3,v4,v5,color_type);
		insert_tria(t3);
	}
	
	
	void add_tria(Vector3 v1,Vector3 v2,Vector3 v3,Vector3 v4,int color_type)
	{
		color_tria t1=new color_tria(v1,v2,v4,color_type);
		insert_tria(t1);
		
		color_tria t2=new color_tria(v1,v4,v3,color_type);
		insert_tria(t2);
	}
	
	void add_tria(Vector3 v1,Vector3 v2,Vector3 v3,int color_type)
	{
		color_tria t=new color_tria(v1,v2,v3,color_type);
		insert_tria(t);
	}
	
	void insert_tria(color_tria t)
	{
		ct_L[t.color_type].Add(t);
	}
}
