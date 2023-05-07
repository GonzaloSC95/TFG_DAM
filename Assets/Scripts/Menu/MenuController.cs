using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    /* Atributos */
    [SerializeField] private GameObject loadGamePanel;
    [SerializeField] private GameObject newGamePanel;
    [SerializeField] private InputField inputStartEmail;
    [SerializeField] private InputField inputStartPwd;
    [SerializeField] private InputField inputLoadEmail;
    [SerializeField] private InputField inputLoadPwd;
    [SerializeField] private GameObject AlertImage;
    [SerializeField] private Text AlertText;
    [SerializeField] private Color RedColor;
    [SerializeField] private Color GreenColor;
    private string patternEmail;
    private string patternPsw;

    /* Métodos */
    /* Método OpenNewGamePanel */
    private void Start()
    {
        //Patrones
        patternEmail = @"([\w\.\-_]+)?\w+@[\w-_]+(\.\w+){1,}";
        patternPsw = @"^(?=.*[A-Z])(?=.*\d).{10,}$";
        //Eventos
        inputStartEmail.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(inputStartEmail.text, inputStartEmail, "N"); });
        inputStartPwd.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(inputStartPwd.text, inputStartPwd, "N"); });
        inputLoadEmail.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(inputLoadEmail.text, inputLoadEmail, "L"); });
        inputLoadPwd.onValueChanged.AddListener(delegate { OnInputFieldValueChanged(inputLoadPwd.text, inputLoadPwd, "L"); });
    }
    /* Método OpenNewGamePanel */
    public void OpenNewGamePanel()
    {
        newGamePanel.gameObject.SetActive(true);
    }
    /* Método StartNewGame */
    public void StartNewGame()
    {
        if (!ValDatosInput("N"))
        {
            AlertImage.gameObject.SetActive(true);
            return;
        }
        else
        {
            AlertImage.gameObject.SetActive(false);
            //TODO: Persistencia de datos
            SceneManager.LoadScene("Nivel_1");
        }
    }
    /* Método ValDatosInput */
    public bool ValDatosInput(string panelType)
    {
        InputField email = (panelType == "N") ? inputStartEmail : inputLoadEmail;
        InputField psw = (panelType == "N") ? inputStartPwd : inputLoadPwd;
        bool validation = true;
        AlertText.text = "";

        if((email.text == "") || (psw.text == ""))
        {
            AlertText.text = "- You must fill in your email and password";
            validation = false;
        }
        else
        {
            if (!Regex.IsMatch(email.text, patternEmail))
            {
                AlertText.text += "- Not valid Email\n\n";
                ChangeInputColorNormal(email, RedColor);
                validation = false;
            }
            if (!Regex.IsMatch(psw.text, patternPsw))
            {
                AlertText.text += "- The password must contain at least one capital letter and one number.";
                ChangeInputColorNormal(psw, RedColor);
                validation = false;
            }
        }
        
        return validation;
    }
    /* Método BackMainMenu */
    public void BackMainMenu()
    {
        if(newGamePanel.gameObject.activeSelf)
        {
            newGamePanel.gameObject.SetActive(false);
        }
        if(loadGamePanel.gameObject.activeSelf)
        {
            loadGamePanel.gameObject.SetActive(false);
        }
        CleanInputFields();
        AlertImage.gameObject.SetActive(false);
    }
    /* Método OpenLoadGamePanel */
    public void OpenLoadGamePanel()
    {
        loadGamePanel.gameObject.SetActive(true);
    }
    /* Método LoadNewGame */
    public void LoadNewGame()
    {
        //string currentLoadScene;
        if (!ValDatosInput("L"))
        {
            AlertImage.gameObject.SetActive(true);
            return;
        }
        else
        {
            AlertImage.gameObject.SetActive(false);
            //TODO: Recuperacion de datos
            //SceneManager.LoadScene(currentLoadScene);
        }
    }
    /* Método ChangeInputColorNormal */
    public void ChangeInputColorNormal(InputField input,Color color)
    {
        ColorBlock cb = input.colors;
        cb.normalColor = color;
        input.colors = cb;
    }
    /* Método ChangeInputColorNormal */
    public void OnInputFieldValueChanged(string value,InputField input, string panelType)
    {
        AlertText.text = "";
        if(panelType == "N")
        {
            if (!ValDatos(panelType))
            {
                AlertImage.gameObject.SetActive(true);
            }
            else
            {
                ChangeInputColorNormal(input, GreenColor);
                AlertImage.gameObject.SetActive(false);
            }
        }
        else 
        {
            if (!ValDatos(panelType))
            {
                AlertImage.gameObject.SetActive(true);
            }
            else
            {
                ChangeInputColorNormal(input, GreenColor);
                AlertImage.gameObject.SetActive(false);
            }
        }
    }
    /* Método ValDatosNewGameOnChangeText */
    public bool ValDatos(string panelType)
    {
        InputField email = (panelType == "N") ? inputStartEmail : inputLoadEmail;
        InputField psw = (panelType == "N") ? inputStartPwd : inputLoadPwd;
        bool validation = true;

        if ((!Regex.IsMatch(email.text, patternEmail)) && (email.text != ""))
        {
            AlertText.text += "- Not valid Email\n\n";
            ChangeInputColorNormal(email, RedColor);
            validation = false;
        }
        else
        {
            ChangeInputColorNormal(email, GreenColor);
        }
        if (!Regex.IsMatch(psw.text, patternPsw) && (psw.text != ""))
        {
            AlertText.text += "- The password must contain at least one capital letter and one number.";
            ChangeInputColorNormal(psw, RedColor);
            validation = false;
        }
        else
        {
            ChangeInputColorNormal(psw, GreenColor);
        }

        return validation;
    }
    /* Método CleanInputFields */
    public void CleanInputFields()
    {
        inputLoadEmail.text = "";
        inputLoadPwd.text = "";
        inputStartEmail.text = "";
        inputStartPwd.text = "";
        AlertText.text = "";
    }
}
