using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class FlavourTextDictionary : MonoBehaviour
{
    public static FlavourTextDictionary instance;
    private static Dictionary<FlavourTextType, FlavourTextCollection> typedCollections = new Dictionary<FlavourTextType, FlavourTextCollection>();

    [SerializeField] private List<FlavourTextCollectionType> flavourTextCollectionTypes = new List<FlavourTextCollectionType>();
    private string baseExtension = "/GameFiles/TextFiles/";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            UnpackCollections();
        }
    }

    private void UnpackCollections()
    {
        foreach (FlavourTextCollectionType collectionType in flavourTextCollectionTypes)
        {
            typedCollections.Add(collectionType.type, collectionType.collection);

            FlavourTextCollection collection = collectionType.collection;

            string filePathExtension = collection.fileName;
            string filePath = Application.dataPath + baseExtension + filePathExtension + ".txt";
            collection.text = File.ReadAllLines(filePath).ToList();
        }
    }

    public static string GetRandomText(FlavourTextType type)
    {
        if (!typedCollections.ContainsKey(type)) return "";

        List<string> words = typedCollections[type].text;
        if (words.Count <= 0) return "";

        int randomIndex = Random.Range(0, words.Count);
        return words[randomIndex];
    }

    public static int GetRandomNumberInRange(int lower, int upper)
    { 
        return Random.Range(lower, upper);
    }
}

[Serializable]
public struct FlavourTextCollectionType
{ 
    public FlavourTextType type;
    public FlavourTextCollection collection;

    public FlavourTextCollectionType(FlavourTextType type, FlavourTextCollection collection)
    { 
        this.type = type;
        this.collection = collection;
    }
}

[Serializable]
public class FlavourTextCollection
{
    public string fileName;
    public List<string> text { get; set; }

    public FlavourTextCollection(string fileName, List<string> text)
    { 
        this.fileName = fileName;
        this.text = text;
    }
}

public enum FlavourTextType
{ 
    Trait,
    Personality,
    Class,
    Background,
    Allignment
}
