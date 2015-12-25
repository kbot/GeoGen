using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

public class KBFileManager : Singleton<KBFileManager> {

	public enum KBFileDirectory 
	{
		gameData = 0,
		persistentData
	}
	
	private string gameDataPath;
	private string persistentDataPath;
	
	public void Initialize()
	{
		gameDataPath = Application.dataPath;
		persistentDataPath = Application.persistentDataPath;
	}

	
	// ---------------------------------------------------------------------------------------------------
	// checkFile()
	// --------------------------------------------------------------------------------------------------- 
	// checks to see whether a file exists
	// ---------------------------------------------------------------------------------------------------
	public string checkFile(string filePath,KBFileDirectory fileDirectory = KBFileDirectory.gameData)
	{
		string absoluteFilePath = pathForEnumeratedFileDirectory (fileDirectory) + "/" + filePath;
		if(File.Exists (absoluteFilePath)) {
			return absoluteFilePath;
		}	
		return null;
	}

	public string pathForEnumeratedFileDirectory(KBFileDirectory fileDirectory) {
		string returnPath;
		switch (fileDirectory) {
			case KBFileDirectory.gameData: { returnPath = gameDataPath;}break;
			case KBFileDirectory.persistentData: { returnPath = persistentDataPath;}break;
			default: {
				returnPath = null;
				Debug.LogWarning("pathForEnumeratedFileDirectory no file directory for enum " + fileDirectory);
			}
				break;
		}
		return returnPath;
	}

	public XmlDocument loadXMLDoc(string filename, KBFileDirectory fileDirectory = KBFileDirectory.gameData) {
		XmlDocument xmlDoc = null;
		string absoluteFilePath = null; 
		if ((absoluteFilePath = checkFile(filename,fileDirectory)) != null) {
			xmlDoc = new XmlDocument();
			xmlDoc.Load (absoluteFilePath);
		} else {
			Debug.LogError("readXMLDoc - No File found named " + filename + " in " + pathForEnumeratedFileDirectory (fileDirectory));
		}
		return xmlDoc;
	}

	public XmlReader readerForXMLDoc(string filename, KBFileDirectory fileDirectory = KBFileDirectory.gameData) {
		XmlReader xmlReader = null;
		string absoluteFilePath = null; 
		if ((absoluteFilePath = checkFile(filename,fileDirectory)) != null) {
			xmlReader = XmlReader.Create(absoluteFilePath);
		} else {
			Debug.LogError("readerForXMLDoc - No File found named " + filename + " in " + pathForEnumeratedFileDirectory (fileDirectory));
		}
		return xmlReader;
	}
	public string readElementFromXMLDoc(XmlReader xmlReader, string elementName) {

		if (xmlReader != null && elementName != null) {
			xmlReader.MoveToContent();
			xmlReader.Read();
			return xmlReader.ReadElementString(elementName);
		} else {
			Debug.LogError("readElementFromXMLDoc - No reader or elementName");
		}
		return null;
	}
}
