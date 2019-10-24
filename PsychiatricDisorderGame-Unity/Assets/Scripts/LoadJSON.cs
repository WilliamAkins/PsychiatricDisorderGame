using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class PositiveOutcome
{
    public string Heading { get; set; }
    public string SubHeading { get; set; }
    public string Description { get; set; }
    public int Points { get; set; }
}

public class NegetiveOutcome
{
    public string Heading { get; set; }
    public string SubHeading { get; set; }
    public string Description { get; set; }
    public int Points { get; set; }
}

public class Outcome
{
    public List<PositiveOutcome> positiveOutcomes { get; set; }
    public List<NegetiveOutcome> negetiveOutcomes { get; set; }
}

public class LoadJSON : MonoBehaviour
{
    //on start, load all the data from the json file into the c# data structures
    private void Start()
    {
        string path = "outcomes.json";

        path = Application.dataPath + "\\resources\\outcomes\\" + path;
        string jsonString = File.ReadAllText(path);

        Outcome outcome = JsonConvert.DeserializeObject<Outcome>(jsonString);

        //store the outcome data within the gameData script for use within external scripts
        GameData.setOutcomes(outcome);
    }
}
