using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Screen object variables
    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject saveLoadTestUI;
    public GameObject cardListScreen;
    public GameObject createNewCardScreen;
    void Awake()
    {
        //check that this instance exists
        if (instance == null)
        {
            //if it doesnt, make it exist
            instance = this;
        }
        else if (instance != this)
        {
            //destroy duplicate instances
            Destroy(gameObject);
        }
        //set this instance as protected
        DontDestroyOnLoad(gameObject);
        LoginScreen();
    }

    //Functions to change the login screen UI
    public void LoginScreen() //Back button
    {
        loginUI.SetActive(true);
        registerUI.SetActive(false);
        saveLoadTestUI.SetActive(false);
        cardListScreen.SetActive(false);
        createNewCardScreen.SetActive(false);

    }
    public void RegisterScreen() // Regester button
    {
        loginUI.SetActive(false);
        registerUI.SetActive(true);
        saveLoadTestUI.SetActive(false);
        cardListScreen.SetActive(false);
        createNewCardScreen.SetActive(false);

    }

    public void CardListScreen()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        saveLoadTestUI.SetActive(false);
        cardListScreen.SetActive(true);
        createNewCardScreen.SetActive(false);

    }

    public void SaveLoadTestScreen()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        saveLoadTestUI.SetActive(true);
        cardListScreen.SetActive(false);
        createNewCardScreen.SetActive(false);


    }

    public void CardCreationScreen()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        saveLoadTestUI.SetActive(false);
        cardListScreen.SetActive(false);
        createNewCardScreen.SetActive(true);
    }
}
