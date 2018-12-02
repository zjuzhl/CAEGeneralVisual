using UnityEngine;
using System.Collections;
using System;  
using System.Text;  
using System.Runtime.InteropServices;  

public class OpenFileDialog {  

	public static string GetFilePath(string path)
	{
		OpenFileName ofn = new OpenFileName();  
		ofn.structSize = Marshal.SizeOf(ofn);  
		//三菱(*.gxw)\0*.gxw\0西门子(*.mwp)\0*.mwp\0All Files\0*.*\0\0  
//		ofn.filter = "All Files\0*.*\0\0";  
//		ofn.filter = "图片文件(*.jpg*.png)\0*.jpg;*.png";
		ofn.filter = "数据文件(*.csv)\0*.csv";

		ofn.file = new string(new char[256]);  
		ofn.maxFile = ofn.file.Length;  
		ofn.fileTitle = new string(new char[64]);  
		ofn.maxFileTitle = ofn.fileTitle.Length;  
		ofn.initialDir = path;//默认路径  
		ofn.title = "打开数据文件";  
		ofn.defExt = "JPG";//显示文件的类型  
		//注意 一下项目不一定要全选 但是0x00000008项不要缺少  
		ofn.flags=0x00080000|0x00001000|0x00000800|0x00000200|0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  
		
		if(OpenFile.GetOpenFileName( ofn ))  
		{  
			Debug.Log( "Selected file with full path: {0}"+ofn.file );  
			return ofn.file;
		}  
		return "get error! ";
	}

}  


[StructLayout( LayoutKind.Sequential, CharSet=CharSet.Auto )]    
public class OpenFileName   
{  
	public int      structSize = 0;  
	public IntPtr   dlgOwner = IntPtr.Zero;   
	public IntPtr   instance = IntPtr.Zero;  
	public String   filter = null;  
	public String   customFilter = null;  
	public int      maxCustFilter = 0;  
	public int      filterIndex = 0;  
	public String   file = null;  
	public int      maxFile = 0;  
	public String   fileTitle = null;  
	public int      maxFileTitle = 0;  
	public String   initialDir = null;  
	public String   title = null;     
	public int      flags = 0;   
	public short    fileOffset = 0;  
	public short    fileExtension = 0;  
	public String   defExt = null;   
	public IntPtr   custData = IntPtr.Zero;    
	public IntPtr   hook = IntPtr.Zero;    
	public String   templateName = null;   
	public IntPtr   reservedPtr = IntPtr.Zero;   
	public int      reservedInt = 0;  
	public int      flagsEx = 0;  
	
} 

public class OpenFile  
{  
	[DllImport("Comdlg32.dll",SetLastError=true,ThrowOnUnmappableChar=true, CharSet = CharSet.Auto)]            
	public static extern bool GetOpenFileName([ In, Out ] OpenFileName ofn );     
	public static  bool GetOpenFileName1([ In, Out ] OpenFileName ofn )  	
	{  
		return GetOpenFileName(ofn);  
	}  
}






