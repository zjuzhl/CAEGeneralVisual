Shader "Custom/temp1" { 
Properties { 
	_MainTex ("Base (RGB)", 2D) = "white" {} 
	_Point1("Temperature1",Range(0.39925,164.22)) = 156.94 
	_Point2("Temperature2",Range(0.39925,164.22)) = 164.22 
	_Point3("Temperature3",Range(0.39925,164.22)) = 132.89 
	_Point4("Temperature4",Range(0.39925,164.22)) = 0.33925 
//	_Point5("TEST",Range(0, int.max)) = 100
	
} 
SubShader { 
	AlphaTest Greater 0.1 
	pass 
	{ 
		CGPROGRAM 
		// Upgrade NOTE: excluded shader from DX11, Xbox360, OpenGL ES 2.0 because it uses unsized arrays 
		//#pragma exclude_renderers d3d11 xbox360 gles 
		#pragma target 3.0 
		#pragma vertex vert 
		#pragma fragment frag 
		#include "UnityCG.cginc" 


		sampler2D _MainTex; 
		float4 _MainTex_ST; 
		float _Point1; 
		float _Point2; 
		float _Point3; 
		float _Point4; 
		bool computer = false; 


		struct v2f { 
		float4 pos:SV_POSITION; 
		float2 uv:TEXCOORD0; 
		}; 

		v2f vert(appdata_base v) 
		{ 
			v2f o; 
			o.pos=mul(UNITY_MATRIX_MVP,v.vertex); 
			o.uv = TRANSFORM_TEX(v.texcoord,_MainTex); 
			return o; 
		} 

		float computerTemperature(float2 uv) 
		{ 
			float _midPointX[121] = {
			0, 
			1, 
			0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 
			1, 
			1, 1, 1, 1, 1, 1, 1, 1, 1, 
			0, 
			0.9, 0.8, 0.7, 0.6, 0.5, 0.4, 0.3, 0.2, 0.1, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0.1, 0.1, 0.1, 0.1, 
			0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 
			0.2, 0.2, 0.2, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 0.3, 
			0.3, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.5, 
			0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.6, 0.6, 0.6, 
			0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.7, 0.7, 0.7, 0.7, 0.7, 
			0.7, 0.7, 0.7, 0.7, 0.8, 0.8, 0.8, 0.8, 0.8, 0.8, 0.8, 
			0.8, 0.8, 0.9, 0.9, 0.9, 0.9, 0.9, 0.9, 0.9, 0.9, 0.9
			}; 
			float _midPointY[121] = {
			0, 
			0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 
			1, 
			0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 
			1, 
			1, 1, 1, 1, 1, 1, 1, 1, 1, 0.9, 0.8, 
			0.7,0.6, 0.5, 0.4, 0.3, 0.2, 0.1, 0.1, 0.2, 0.3, 0.4, 
			0.5, 0.6, 0.7, 0.8, 0.9, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 
			0.7, 0.8, 0.9, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 
			0.9, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.1, 
			0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.1, 0.2, 0.3, 
			0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.1, 0.2, 0.3, 0.4, 0.5, 
			0.6, 0.7, 0.8, 0.9, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 
			0.8, 0.9, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9
			}; 
			float _midPointT[121] = {
			156.94, 
			164.22, 
			90.026, 66.818, 57.148, 53.939, 51.905, 51.109, 52.825, 66.089, 94.058, 
			132.89, 
			139.57, 102.70, 87.273, 77.865, 57.390, 79.623, 56.446, 65.543, 58.370, 
			0.39925, 
			103.11, 81.019, 66.707, 53.606, 39.538, 25.964, 14.444, 6.3206, 2.1425, 2.1269, 6.1560, 
			13.957, 25.058, 38.534, 53.477, 70.208, 89.402, 130.55, 86.344, 78.077, 64.126, 51.861, 
			40.744, 30.522, 21.052, 12.268, 5.4855, 67.333, 70.561, 65.684, 58.457, 50.576, 41.960, 
			32.420, 22.210, 12.311, 61.147, 67.080, 68.035, 64.897, 59.335, 51.855, 42.703, 32.325, 
			21.104, 58.382, 65.755, 69.514, 69.278, 65.702, 59.503, 51.205, 41.286, 30.293, 56.610, 
			64.746, 69.881, 71.379, 69.414, 64.495, 57.261, 48.444, 39.557, 55.495, 63.534, 68.882, 
			71.247, 70.548, 66.804, 60.693, 53.691, 48.526, 56.523, 62.982, 66.353, 68.584, 69.556, 
			67.277, 61.813, 56.556, 56.395, 65.069, 67.475, 63.576, 62.107, 66.685, 69.063, 63.279, 
			54.592, 59.613, 91.197, 82.590, 69.120, 56.732, 54.202, 79.302, 67.107, 54.080, 49.435
			}; 

			float d1 = sqrt(uv.x*uv.x+uv.y*uv.y); 
			float d2 = sqrt((1-uv.x)*(1-uv.x)+(1-uv.y)*(1-uv.y)); 
			float d3 = sqrt(uv.x*uv.x+(1-uv.y)*(1-uv.y)); 
			float d4 = sqrt((1-uv.x)*(1-uv.x)+uv.y*uv.y); 


			float m = 1/d1+1/d2+1/d3+1/d4; 
			float n = 1/d1*_Point1+1/d2*_Point4+1/d3*_Point3+1/d4*_Point2; 
			for (int i = 0 ; i < _midPointT.Length ; i++) 
			{ 
				float dp = sqrt((uv.x-_midPointX[i])*(uv.x-_midPointX[i])+(uv.y-_midPointY[i])*(uv.y-_midPointY[i])); 
				m = m + 1/dp; 
				n = n + 1/dp*_midPointT[i]; 
			} 
			return n/m; 
		} 

		float4 frag(v2f i):COLOR 
		{ 

			float4 outp; 
			float4 texCol = tex2D(_MainTex,i.uv); 
			float temp = computerTemperature(i.uv); 

			//图像区域，判定设置为颜色的A > 0.5,输出为材质颜色+光亮值 
			if(texCol.w>0.5) 
			{ 
				if(temp >= 60) 
					outp = float4(1,0,0,1)*(temp-60)/40+float4(1,1,0,1)*(1-(temp-60)/40); 
					else if(temp >= 30) 
					outp = float4(1,1,0,1)*(temp-30)/30+float4(0,1,0,1)*(1-(temp-30)/30); 
					else 
					outp = float4(0,1,0,1)*(temp)/30+float4(0,0,1,1)*(1-(temp)/30); 
			} 
			else 
				outp = float4(0,0,0,0); 
			return outp; 
		} 
	ENDCG 
	} 
} 
FallBack "Diffuse" 
} 
//其中_Point1到4是平面4个顶点上的温度值 
//_midPointX,_midPointY,_midPointT给出了平面内三个点的位置和温度值，实际应用中可以相应修改和增删 