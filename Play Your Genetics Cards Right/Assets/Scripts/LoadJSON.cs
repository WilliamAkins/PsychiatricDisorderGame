using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class Card
{
    //says whether the card has been marked as positive or negative
    public bool IsPositive { get; set; } = false;

    public string Heading { get; set; }
    public string SubHeading { get; set; }
    public string Description { get; set; }
    public string Gender { get; set; }
    public string References { get; set; }
    public int Points { get; set; }
}

public class GeneticCards
{
    public List<Card> GoodCards { get; set; }
    public List<Card> BadCards { get; set; }
}

public class EnvironmentCards
{
    public List<Card> GoodCards { get; set; }
    public List<Card> BadCards { get; set; }
}

public class Outcome
{
    public GeneticCards GeneticCards { get; set; }
    public EnvironmentCards EnvironmentCards { get; set; }
}

public class LoadJSON : MonoBehaviour
{
    //on start, load all the data from the json file into the c# data structures
    private void Start()
    {
        string filePath = "outcomes.json";

        string path = Path.Combine(Application.streamingAssetsPath, filePath);
        Debug.Log(path);

        string jsonString = File.ReadAllText(path);

        Outcome outcome = JsonConvert.DeserializeObject<Outcome>(jsonString);

        //store the outcome data within the gameData script for use within external scripts
        GameData.CardOutcomes.Set(outcome);
    }
}
