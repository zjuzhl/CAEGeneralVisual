using UnityEngine;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 文件头判断文件类型
/// </summary>
//zhuhonglin
//20170619

public enum FileTypeHeader
{
	Other = -1,
	TXT,
	PDF,
	JPG,
	PNG,
	GIF,
	BMP16,
	BMP24,
	BMP256,
	HTML,
	HTM,
	MP3,
	MP4,
	AVI,
	EXE,
	ZIP,
	RAR
}

public class FileTypeBase
{
	public Dictionary<string, FileTypeHeader> TypeBase = new Dictionary<string, FileTypeHeader>();
	public FileTypeBase(){
		TypeBase.Add ("test",FileTypeHeader.Other);
		TypeBase.Add ("313233313233",FileTypeHeader.TXT);
		TypeBase.Add ("255044462d312e350a25",FileTypeHeader.PDF);
		TypeBase.Add ("ffd8ffe000104a464946",FileTypeHeader.JPG);
		TypeBase.Add ("89504e470d0a1a0a0000",FileTypeHeader.PNG);
		TypeBase.Add ("47494638396126026f01",FileTypeHeader.GIF);
		TypeBase.Add ("424d228c010000000000",FileTypeHeader.BMP16);
		TypeBase.Add ("424d8240090000000000",FileTypeHeader.BMP24);
		TypeBase.Add ("424d8e1b030000000000",FileTypeHeader.BMP256);
		TypeBase.Add ("3c21444f435459504520",FileTypeHeader.HTML);
		TypeBase.Add ("3c21646f637479706520",FileTypeHeader.HTM);
		TypeBase.Add ("49443303000000002176",FileTypeHeader.MP3);
		TypeBase.Add ("00000020667479706d70",FileTypeHeader.MP4);
		TypeBase.Add ("52494646d07d60074156",FileTypeHeader.AVI);
		TypeBase.Add ("4d5a9000030000000400",FileTypeHeader.EXE);
		TypeBase.Add ("504b0304140000000800",FileTypeHeader.ZIP);
		TypeBase.Add ("526172211a0700cf9073",FileTypeHeader.RAR);
	}
}

public class iFileType : FileTypeBase{

	string filepath;

	public iFileType(string path){
		filepath = path;
	}

	string GetStreamHeader(byte[] src )
	{
		if (src == null)
			return null;
		if (src.Length < 2)
			return null;
		StringBuilder sb = new StringBuilder ();
		for (int i = 0; i < src.Length; i++) {
			int v = src [i] & 0xFF;
			string hv = v.ToString ("X");
			if (hv.Length < 2) {
				sb.Append (0);
			}
			sb.Append (hv);
		}
		Debug.Log (sb.ToString());
		return sb.ToString ();
	}

	FileTypeHeader FindFileTypeHeader(string key)
	{
		if (string.IsNullOrEmpty (key))
			return FileTypeHeader.Other;
		FileTypeHeader fth;
//		Debug.Log (key.ToLower());
		if (TypeBase.ContainsKey (key.ToLower ())) {
			TypeBase.TryGetValue (key.ToLower (), out fth);
			return fth;
		} else if (TypeBase.ContainsKey (key.ToUpper ())) {
			TypeBase.TryGetValue (key.ToLower (), out fth);
			return fth;
		}
		return FileTypeHeader.Other;
	}


	public FileTypeHeader GetFileTypeHeader()
	{
		if (string.IsNullOrEmpty (filepath)) {
			return FileTypeHeader.Other;
		}
		try{
			byte[] test = new byte[10];
			try
			{
				BinaryReader br = new BinaryReader(new FileStream(filepath,FileMode.Open));
				test = br.ReadBytes(10);
				string StreamHeader = GetStreamHeader(test);
				return FindFileTypeHeader(StreamHeader);
			}
			catch (IOException e)
			{
				Debug.Log(e.Message + ". Cannot open file.");
			}
		}catch(IOException ioe){
			Debug.Log (ioe.ToString());
		}
		return FileTypeHeader.Other;
	}
}
//iFileType type = new iFileType ("desk.jpg");
//Debug.Log(type.GetFileTypeHeader ().ToString());
//
//iFileType type0 = new iFileType ("12.pdf");
//Debug.Log(type0.GetFileTypeHeader ().ToString());
//
//iFileType type1 = new iFileType ("mydata");
//Debug.Log(type1.GetFileTypeHeader ().ToString());
//
//iFileType type2 = new iFileType ("mydata.dat");
//Debug.Log(type2.GetFileTypeHeader ().ToString());
//
//iFileType type3 = new iFileType ("mydata.txt");
//Debug.Log(type3.GetFileTypeHeader ().ToString());
//
//iFileType type4 = new iFileType ("34.zip");
//Debug.Log(type4.GetFileTypeHeader ().ToString());
