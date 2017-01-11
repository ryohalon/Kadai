using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RecipeManager : MonoBehaviour
{
    public List<List<int>> recipes = new List<List<int>>();
    public List<bool> isDiscovery = new List<bool>();

    void Start()
    {
        LoadRecipe();
        LoadDiscovery();
    }

    private void LoadRecipe()
    {
        TextAsset csvFile = Resources.Load("Data/Recipe") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        List<string[]> recipeDatas = new List<string[]>();
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            recipeDatas.Add(line.Split(','));
        }

        foreach (var recipe in recipeDatas)
        {
            List<int> recipe_ = new List<int>();
            for (int i = 0; i < 4; i++)
                recipe_.Add(int.Parse(recipe[i]));
            
            recipes.Add(recipe_);
        }
    }

    private void LoadDiscovery()
    {
        TextAsset csvFile = Resources.Load("Data/RecipeDiscovery") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        string line = reader.ReadLine();
        string[] discoveryDatas = line.Split(',');

        foreach (var discovery in discoveryDatas)
        {
            isDiscovery.Add(bool.Parse(discovery));
        }
    }

    public void OnApplicationQuit()
    {
        StreamWriter sw = new StreamWriter(Application.dataPath + "/Resources/Data/RecipeDiscovery.csv", false);
        string txt = "";
        for (int i = 0; i < isDiscovery.Count; i++)
        {
            txt += isDiscovery[i].ToString();
            if (i != isDiscovery.Count - 1)
                txt += ',';
        }
        sw.WriteLine(txt);
        sw.Flush();
        sw.Close();
    }
}
