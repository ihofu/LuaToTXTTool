using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LuaCopyEditor : Editor
{
    [MenuItem("XLua/自动生成.txt的lua文件")]
    public static void CopyLuaToTxt()
    {
        // 得到所有的lua文件
        string path = Application.dataPath + "/Lua/";

        if (!Directory.Exists(path))
            return;
        
        string[] luaFiles = Directory.GetFiles(path, "*.lua");

        // 拷贝lua文件到指定路径
        string newPath = Application.dataPath + "/LuaTxt/";

        if(!Directory.Exists(newPath))
        {
            Directory.CreateDirectory(newPath);
        }
        else 
        {
            // 如果路径存在 先清空文件夹 保证所有拷贝的文件都是最新的
            string[] oldFiles = Directory.GetFiles(newPath);
            for(int i = 0; i < oldFiles.Length; i++)
            {
                File.Delete(oldFiles[i]);
            }
        }
            

        string newFilePath;
        List<string> newFileNames = new List<string>();
        for(int i = 0; i < luaFiles.Length; i++)
        {
            newFilePath = newPath + luaFiles[i].Substring(luaFiles[i].LastIndexOf("/") + 1) + ".txt";
            File.Copy(luaFiles[i], newFilePath);
            newFileNames.Add(newFilePath);
        }

        // 刷新目录
        AssetDatabase.Refresh();

        for(int i = 0; i < newFileNames.Count; i++)
        {
            AssetImporter importer = AssetImporter.GetAtPath(newFileNames[i].Substring(newFileNames[i].IndexOf("Asset")));
            if(importer != null)
                importer.assetBundleName = "lua";
        }
    }
}
