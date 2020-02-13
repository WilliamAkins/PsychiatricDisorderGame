using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtEnvironmentNum;
    [SerializeField] private TextMeshProUGUI txtGeneticNum;
    [SerializeField] private TextMeshProUGUI txtCombinedNum;

    [SerializeField] private Button _btnContinue;

    [SerializeField] private Button _btnSubmitScore;
    [SerializeField] private GameObject _submitScorePopup;

    private int _randEnvironmentNum = 0;
    private int _randGeneticNum = 0;

    private bool _scoreSubmitted = false;

    // Start is called before the first frame update
    private void Start()
    {
        transform.localPosition = new Vector3(0.0f, -1000.0f, 0.0f);
        
        StartCoroutine(LerpGameObjectY(gameObject, 0.0f, 1.0f, false));

        StartCoroutine(RandomlySwitchNumbers(txtEnvironmentNum, -10, 10, 20, true));
        StartCoroutine(RandomlySwitchNumbers(txtGeneticNum, -10, 10, 20, false));

        _btnContinue.onClick.AddListener(BtnContinuePressed);
        _btnSubmitScore.onClick.AddListener(BtnSubmitScorePressed);
    }

    // Update is called once per frame
    private void Update()
    {
        txtCombinedNum.text = (_randEnvironmentNum + _randGeneticNum).ToString();
    }

    private void BtnContinuePressed()
    {
        StopAllCoroutines();
        StartCoroutine(LerpGameObjectY(gameObject, -1000.0f, 1.0f, true));
    }

    private void BtnSubmitScorePressed()
    {
        Instantiate(_submitScorePopup, Vector3.zero, Quaternion.identity);
    }

    private IEnumerator LerpGameObjectY(GameObject c, float finalY, float speed, bool endGame)
    {
        float lerpTimer = 0.0f;
        Vector3 pos = c.transform.localPosition;

        while (lerpTimer < speed)
        {
            lerpTimer += Time.deltaTime;

            pos = Vector3.Lerp(pos, new Vector3(pos.x, finalY, pos.z), lerpTimer / speed);
            c.transform.localPosition = pos;

            yield return null;
        }

        //end the game by returning to the main menu
        if (endGame)
            GameData.State.Set(GameData.State.GameState.ReturnToMenu);
    }

    private IEnumerator RandomlySwitchNumbers(TextMeshProUGUI textObj, int minNum, int maxNum, int numSwitches, bool isEnvironment)
    {
        int curSwitchCount = numSwitches;
        
        //run a counter for each time the value switches
        float timer = 0.0f;
        while (timer < 1.0f / curSwitchCount)
        {
            timer += Time.deltaTime;
            
            yield return null;
        }

        int num = Random.Range(minNum, maxNum);

        //store the random result externally for the combined number
        if (isEnvironment)
            _randEnvironmentNum = num;
        else
            _randGeneticNum = num;

        //display the result
        textObj.text = num.ToString();

        curSwitchCount--;

        //either show the final result or keep recurring with random numbers
        if (curSwitchCount > 0)
        {
            StartCoroutine(RandomlySwitchNumbers(textObj, minNum, maxNum, curSwitchCount, isEnvironment));
        }
        else
        {
            int actualNum = 0;
            if (isEnvironment)
            {
                actualNum = GameData.Points.Environment.Get();
                _randEnvironmentNum = actualNum;
            }
            else
            {
                actualNum = GameData.Points.Genetic.Get();
                _randGeneticNum = actualNum;
               
            }
            textObj.text = actualNum.ToString();

            _btnContinue.interactable = true;
            _btnSubmitScore.interactable = true;
        }
    }

    public void SetScoreSubmitted(bool newScoreSubmitted) => _scoreSubmitted = newScoreSubmitted;
}