using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    /* Atributos */
    //About UI
    [SerializeField] private GameObject credits;
    [SerializeField] private Animator creditsAnim;
    [SerializeField] private GameObject thanks;
    [SerializeField] private Animator thanksAnim;
    [SerializeField] private GameObject panelPartidas;
    //About Colors
    [SerializeField] private Color[] colorsTwo;
    [SerializeField] private Color[] colorsThree;
    //About Text
    [SerializeField] private Text title;
    [SerializeField] private Text text;
    [SerializeField] private Text userText;
    [SerializeField] private Text partidasText;

    /* Métodos */
    /* Método Start */
    public void Start()
    {
        SetUserPartidasText();
        StartCoroutine(ChangeCreditsText());
    }

    /* Método Update */
    public IEnumerator ChangeCreditsText()
    {
        for(var i = 1; i <= 4; i++)
        {
            if(i == 1)
            {
                yield return new WaitForSeconds(creditsAnim.GetCurrentAnimatorStateInfo(0).length);
            }
            else if(i == 2)
            {
                title.color = colorsTwo[0];
                text.color = colorsTwo[1];
                title.text = "Music By".ToUpper();
                text.text = "Crash Bandicoot PS2".ToUpper();
                yield return new WaitForSeconds(creditsAnim.GetCurrentAnimatorStateInfo(0).length);
            }
            else if(i == 3)
            {
                title.color = colorsThree[0];
                text.color = colorsThree[1];
                title.text = "Assets By".ToUpper();
                text.text = "Unity Free Store".ToUpper();
                yield return new WaitForSeconds(creditsAnim.GetCurrentAnimatorStateInfo(0).length);
            }
            else
            {
                credits.SetActive(false);
                thanks.SetActive(true);
                yield return new WaitForSeconds(thanksAnim.GetCurrentAnimatorStateInfo(0).length*1.25f);
            }
        }
        panelPartidas.SetActive(true);
        
    }

    /* Método SetPartidasText */
    public void SetUserPartidasText()
    {
        if (SessionManager.Instance != null)
        {
            var user = SessionManager.Instance.User;
            if (user != null)
            {
                userText.text = string.Format(userText.text, user.Name);
                var partidas = SessionManager.Instance.Db.GetPartidasOrderBy(user.Id, "Id");
                if (partidas != null)
                {
                    partidasText.text = "";
                    string text = "Level: {0} | Points: {1} | Life: {2} | Date: {3}\n";
                    string separator = "--------------------------------------------------------\n";
                    foreach (Partida p in partidas)
                    {
                        partidasText.text += string.Format(text, p.Level, p.Points, p.Life, p.Date);
                        partidasText.text += separator;
                    }
                }
            } 
        }
    }

    /* Método BackToMenu */
    public void BackToMenu()
    {
        CreateNewPartidaForCurrentUser();
        SceneManager.LoadScene("Menu");
    }

    /* Método BackToMenu */
    public void CreateNewPartidaForCurrentUser()
    {
        try 
        {
            var user = SessionManager.Instance.User;
            if (user != null)
            {
                var Level = SessionManager.Instance.Db.CreatePartida(user.Id, "Nivel_1", 0, 3, System.DateTime.Now);
                if (Level != null) SceneManager.LoadScene("Menu");
            }
        } catch(Exception e)
        {
            Debug.LogError(" SessionManager.Instance.User = null -- " + e.Message);
        }
        
    }
}
