using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class FixCSProjectLangVersion : AssetPostprocessor
{
    public static void OnGeneratedCSProject(string path, string content)
    {
        // 修改所有.csproj文件，确保使用C# 9.0
        string updatedContent = Regex.Replace(
            content,
            @"<LangVersion>\d+\.\d+</LangVersion>",
            "<LangVersion>9.0</LangVersion>",
            RegexOptions.IgnoreCase
        );

        // 如果文件中没有LangVersion标签，添加它
        if (!Regex.IsMatch(updatedContent, @"<LangVersion>9.0</LangVersion>"))
        {
            updatedContent = Regex.Replace(
                updatedContent,
                @"<PropertyGroup>",
                "<PropertyGroup>\n    <LangVersion>9.0</LangVersion>",
                RegexOptions.IgnoreCase
            );
        }

        File.WriteAllText(path, updatedContent);
        UnityEngine.Debug.Log("Updated C# language version to 9.0 for: " + path);
    }
}