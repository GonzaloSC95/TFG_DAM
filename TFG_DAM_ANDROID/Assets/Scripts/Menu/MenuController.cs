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
    private string WrongFormtEmailMsg;
    private string WrongFormtPasswordMsg;
    private string EmptyFieldMsg;
    private string WrongEmailOrPaswMsg;
    private string UserAlreadyExistMsg;
    //Usuario de BBDD
    private Usuario User;
    private Partida Level;


    /* Métodos */
    /* Método OpenNewGamePanel */
    private void Start()
    {
        //Patrones
        patternEmail = @"([\w\.\-_]+)?\w+@[\w-_]+(\.\w+){1,}";
        patternPsw = @"^(?=.*[A-Z])(?=.*\d).{10,}$";
        //Mensajes
        WrongFormtEmailMsg = "- Not valid Email.\n\n";
        WrongFormtPasswordMsg = "- The password must contain at least 10 characters, one capital letter and one number.";
        EmptyFieldMsg = "- You must fill in your email and password.";
        WrongEmailOrPaswMsg = "- Wrong Email or Password. Maybe your user has not been created yet.";
        UserAlreadyExistMsg = "- That user already exists. Try another email or log in.";
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
        else if(!ValDatosInputDb("N"))
        {
            AlertImage.gameObject.SetActive(true);
            return;
        }
        else
        {
            AlertImage.gameObject.SetActive(false);
            User = SessionManager.Instance.Db.CreateUsuario(inputStartEmail.text, CryptController.Encrypt(inputStartPwd.text));
            Level = SessionManager.Instance.Db.CreatePartida(User.Id, "Nivel_1", 0, 3, System.DateTime.Now);
            if((User != null) && (Level != null))
            {
                SessionManager.Instance.User = User;
                SceneManager.LoadScene(Level.Level);
            }
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
            AlertText.text = EmptyFieldMsg;
            validation = false;
        }
        else
        {
            if (!Regex.IsMatch(email.text, patternEmail))
            {
                AlertText.text += WrongFormtEmailMsg;
                ChangeInputColorNormal(email, RedColor);
                validation = false;
            }
            if (!Regex.IsMatch(psw.text, patternPsw))
            {
                AlertText.text += WrongFormtPasswordMsg;
                ChangeInputColorNormal(psw, RedColor);
                validation = false;
            }
        }
        
        return validation;
    }
    /* Método ValDatosInputDb */
    public bool ValDatosInputDb(string panelType)
    {
        InputField email = (panelType == "N") ? inputStartEmail : inputLoadEmail;
        InputField psw = (panelType == "N") ? inputStartPwd : inputLoadPwd;
        User = SessionManager.Instance.Db.GetFirstUsuario(email.text);
        bool validation = true;
        AlertText.text = "";

        if ((panelType == "N") && (User != null))
        {
            AlertText.text += UserAlreadyExistMsg;
            validation = false;
        }
        if ((panelType == "L") && ((User == null) || CryptController.Decrypt(User.Psw) != psw.text))
        {
            AlertText.text += WrongEmailOrPaswMsg;
            validation = false;
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
        else if (!ValDatosInputDb("L"))
        {
            AlertImage.gameObject.SetActive(true);
            return;
        }
        else
        {
            AlertImage.gameObject.SetActive(false);
            //Recuperacion de datos y carga de la escena
            Level = SessionManager.Instance.Db.GetLastPartida(User.Id);
            if ((User != null) && (Level != null))
            {
                SessionManager.Instance.User = User;
                SceneManager.LoadScene(Level.Level);
            }

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
            AlertText.text += WrongFormtEmailMsg;
            ChangeInputColorNormal(email, RedColor);
            validation = false;
        }
        else
        {
            ChangeInputColorNormal(email, GreenColor);
        }
        if (!Regex.IsMatch(psw.text, patternPsw) && (psw.text != ""))
        {
            AlertText.text += WrongFormtPasswordMsg;
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
    /* Método ExitApplication */
    public void ExitApplication()
    {
        Application.Quit();
    }
}
