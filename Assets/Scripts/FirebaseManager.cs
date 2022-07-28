using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using Firebase.Database;
using System.Linq;
using Firebase.Storage;
using Firebase.Extensions;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;


[System.Serializable]
public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;


    //test image for firebase storage
    public Image testimage;
    public Material testmaterial;
    //old hardcoded function - to be implemented later
    //public void LoadTestImage()
    //{
    //    Debug.Log("Starting to do the thing");
    //    FirebaseStorage storage = FirebaseStorage.DefaultInstance;
    //    StorageReference storageReference = storage.GetReferenceFromUrl("gs://hit381-card-game.appspot.com");
    //    //StorageReference cardTest = storageReference.Child("sdfghsfdghsfg.JPG");
    //    storageReference.Child("sdfghsfdghsfg.JPG").GetBytesAsync(1024 * 1024).ContinueWithOnMainThread(task =>
    //      {
    //          if (task.IsCompleted)
    //          {
    //              var texture = new Texture2D(2, 2);
    //              byte[] fileContent = task.Result;

    //              texture.LoadImage(fileContent);
    //              var newRect = new Rect(0f, 0f, texture.width, texture.height);
    //              var sprite = Sprite.Create(texture, newRect, Vector2.zero);
    //              testimage.sprite = sprite;
    //          }
    //          else
    //          {
    //              Debug.Log("broke");

    //          }
    //      });

        
    //}

    public IEnumerable LoadTestImage(int NumberWithinSet, Image image, string SetCode)
    {
        Debug.Log("Loading Image");
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference storageReference = storage.GetReferenceFromUrl("gs://hit381-card-game.appspot.com");
        //StorageReference cardTest = storageReference.Child("sdfghsfdghsfg.JPG");
        var imageTask = storageReference.Child(SetCode).Child(SetCode + "-" + NumberWithinSet.ToString()).GetBytesAsync(1024 * 1024);
        yield return new WaitUntil(predicate: () => imageTask.IsCompleted);
        {
            if (imageTask.IsCompleted)
            {
                var texture = new Texture2D(2, 2);
                byte[] fileContent = imageTask.Result;

                texture.LoadImage(fileContent);
                var newRect = new Rect(0f, 0f, texture.width, texture.height);
                var sprite = Sprite.Create(texture, newRect, Vector2.zero);
                testimage.sprite = sprite;
            }
            else
            {
                Debug.Log("broke");

            }
        }


    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    { 
    
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }


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

        StartCoroutine(FireBaseCheck().GetEnumerator());
        
    }

    public IEnumerable FireBaseCheck()
    {
        var task = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(predicate: () => task.IsCompleted);

        dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Firebase didn't resolve everything: " + dependencyStatus);
            }
        
    }



    //variables for firebase
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public DatabaseReference dBReference;

    //user registry variables
    [Header("Variables for Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField password1RegisterField;
    public TMP_InputField password2RegisterField;
    public TMP_Text registryWarningText;

    //Login variables
    [Header("Variables for Login")]
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TMP_Text loginWarningText;
    public TMP_Text loginConfirmText;

    [Header("CardData")]
    public GameObject cardElement;
    public Transform cardContent;




    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Authenticaion");

        auth = FirebaseAuth.DefaultInstance;
        dBReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void LoginButton()
    {
        StartCoroutine(Login(emailField.text, passwordField.text));
    }

    public void RegisterButton()
    {
        StartCoroutine(Register(emailRegisterField.text, password1RegisterField.text, usernameRegisterField.text));
    }
    public void SignOutButton()
    {
        auth.SignOut();
        deckSelector.options.Clear();
        UIManager.instance.LoginScreen();
        ClearLogInFields();
        ClearRegisterFields();
    }

    public void SaveDataButton()
    {
        //StartCoroutine(UpdateUsernameAuth(usernameScoreField.text));
        //StartCoroutine(UpdateUsernameDatabase(usernameScoreField.text));

        //StartCoroutine(UpdateXP(int.Parse(xpField.text)));
        //StartCoroutine(UpdateKills(int.Parse(killsField.text)));
        //StartCoroutine(UpdateDeaths(int.Parse(deathsField.text)));
    }

    public void ScoreboardButton()
    {
        UIManager.instance.CardListScreen();
        //StartCoroutine(LoadScoreboardData());
        StartCoroutine(LoadDeckPreviewData(deckSelector.GetComponentInChildren<TMP_Text>().text.ToString().Substring(0, 5)).GetEnumerator());
    }




    private IEnumerator Login(string _email, string _password)
    {
        //calls firebase auth signi function passing the email and password from LoginButton()
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //wait till it's done
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);


        if(LoginTask.Exception != null)
        {
            //error handling
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Something was wrong there...";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "You didn't put in your email...";
                    break;

                case AuthError.MissingPassword:
                    message = "You didn't put in your password...";

                    break;

                case AuthError.WrongPassword:
                    message = "Wrong password, buddy...";

                    break;

                case AuthError.InvalidEmail:
                    message = "That's not a real email address...";

                    break;

                case AuthError.UserNotFound:
                    message = "We don't know who that is...";

                    break;
            }
            loginWarningText.text = message;
        }
        else
        {
            //user has successfully authenticated
            user = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.Email);
            loginWarningText.text = "";
            loginConfirmText.text = "*hacker voice* You're In.";
            StartCoroutine(CheckForAdminPermissions().GetEnumerator());
            StartCoroutine(RefreshKeywordList().GetEnumerator());


            //adding a listener to the user's decks changing
            dBReference.Child("users").Child(user.UserId).Child("decks").ValueChanged += HandleUserDecksChanged;
            

            //StartCoroutine(LoadUserData().GetEnumerator());
            //trying to be funny to myself is my way of coping right now, as of time of writing i nearly lost the entire project up to this point, i deserve a chuckle - 30/05/22

            yield return new WaitForSeconds(2);
            //usernameScoreField.text = user.DisplayName;
            UIManager.instance.SaveLoadTestScreen();
            loginConfirmText.text = "";
            ClearLogInFields();
            ClearRegisterFields();
            StartCoroutine(LoadSetNames().GetEnumerator());
            
        }

    }
    public GameObject cardCreationScreenButton;

    public IEnumerable CheckForAdminPermissions()
    {
        var task = dBReference.Child("users").Child(user.UserId).Child("role").GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        DataSnapshot snapshot = task.Result;
        //string key = "";
        //Debug.Log(snapshot.Value.ToString());
        if (snapshot.Value.ToString() == "admin")
        {
            cardCreationScreenButton.GetComponent<Button>().interactable = true;
            Debug.Log("button turn on please");

        }
        else if (snapshot.Value.ToString() == "player")
        {
            cardCreationScreenButton.GetComponent<Button>().interactable = false;
            Debug.Log("button turn off please");

            //cardCreationScreenButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("shits fucked");
        }
        //int.Parse(item.Child("Code").Value.ToString()).ToString("D5")
        Debug.Log("shits fucked2");



    }

    void HandleUserDecksChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.Log(args.DatabaseError.Message);
            return;
        }
        StartCoroutine(LoadUserData().GetEnumerator());
    }

    public void HandleUserSetsChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.Log(args.DatabaseError.Message);
            return;
        }
        StartCoroutine(LoadCardData(setSelector.GetComponentInChildren<TMP_Text>().text.ToString()).GetEnumerator());
    }

    public void ManualUserSetsChanged()
    {
        StartCoroutine(LoadCardData(setSelector.GetComponentInChildren<TMP_Text>().text.ToString()).GetEnumerator());
    }


    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            registryWarningText.text = "Missing Username";
        }
        else if (password1RegisterField.text != password2RegisterField.text)
        {
            //If the password does not match show a warning
            registryWarningText.text = "Password Does Not Match!";
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Registry Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "You didn't put in your email...";
                        break;
                    case AuthError.MissingPassword:
                        message = "You didn't put in your password...";
                        break;
                    case AuthError.WeakPassword:
                        message = "Bro, that password sucks, I'm not taking that.";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Uhhh we already have that email address in our system";
                        break;
                }
                registryWarningText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                user = RegisterTask.Result;

                if (user != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = user.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        registryWarningText.text = "Username Set Failed!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen



                        CreateDefaultDeck();
                        CreateDefaultCardCollection();
                        UIManager.instance.LoginScreen();
                        registryWarningText.text = "";
                        
                        ClearLogInFields();
                        ClearRegisterFields();
                    }
                }
            }
        }
    }

    public void CreateDefaultCardCollection()
    {
        //populating deck#00001
        var DBTask = dBReference.Child("decks").Child("00001");
        for (int i = 1; i <= array.Length; i++)
        {
            var cardIDNumber = DBTask.Child("Card" + i.ToString("D3")).Child("SetNumber").SetValueAsync(array[i-1].ToString("D3"));
            var cardIDSet = DBTask.Child("Card" + i.ToString("D3")).Child("SetCode").SetValueAsync("Alpha");
        }
        var DBTask2 = dBReference.Child("users").Child(user.UserId).Child("cards");
        //giving the brand new account some generic cards to add to a collection
        for (int i = 1; i <= 10; i++)
        {
            DBTask2.Child("Alpha").Child(i.ToString("D3")).Child("amountOwned").SetValueAsync(3);
        }
       

    }


    public int[] array = new int[] { 001, 001, 001, 002, 002, 002, 003, 003, 003, 004, 004, 004, 005, 005, 005, 006, 006, 006, 007, 007, 007, 008, 008, 008, 009, 009, 009, 010, 010, 010};
    public void CreateDefaultDeck()
    {
        var DBTask = dBReference.Child("users").Child(user.UserId).Child("decks").Child("001").Child("Code").SetValueAsync("00001");
        var DBTask2 = dBReference.Child("users").Child(user.UserId).Child("decks").Child("001").Child("CustomName").SetValueAsync("DefaultDeck");

    }

    //make them ienumerators and add yields
    public void CreateRandomDeck()
    {
        StartCoroutine(GenerateRandomDecks());
    }
    public IEnumerator GenerateRandomDecks()
    {
        //find the lowest empty deck ID
        var DBTask = dBReference.Child("decks").OrderByKey().GetValueAsync();
        
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            Debug.Log("this should never happen, why does the user not have a deck");
        }
        else
        {
            int number = 0;
            //data retrieved and exists
            DataSnapshot snapshot = DBTask.Result;

            foreach (var item in snapshot.Children)
            {
                Debug.Log(item.ToString());
                number = int.Parse(item.Key) + 1;
                Debug.Log(number);

            }
            //DeckData deckData = new DeckData();
            //points.Clear();
            deck.Clear();
            #region hardcoded deck codes
            //deckData.Card001.SetCode = "Alpha";
            //deckData.Card001.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card002.SetCode = "Alpha";
            //deckData.Card002.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card003.SetCode = "Alpha";
            //deckData.Card003.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card004.SetCode = "Alpha";
            //deckData.Card004.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card005.SetCode = "Alpha";
            //deckData.Card005.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card006.SetCode = "Alpha";
            //deckData.Card006.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card007.SetCode = "Alpha";
            //deckData.Card007.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card008.SetCode = "Alpha";
            //deckData.Card008.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card009.SetCode = "Alpha";
            //deckData.Card009.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card010.SetCode = "Alpha";
            //deckData.Card010.SetID = (Random.Range(1, 9).ToString("D3"));

            //deckData.Card011.SetCode = "Alpha";
            //deckData.Card011.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card012.SetCode = "Alpha";
            //deckData.Card012.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card013.SetCode = "Alpha";
            //deckData.Card013.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card014.SetCode = "Alpha";
            //deckData.Card014.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card015.SetCode = "Alpha";
            //deckData.Card015.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card016.SetCode = "Alpha";
            //deckData.Card016.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card017.SetCode = "Alpha";
            //deckData.Card017.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card018.SetCode = "Alpha";
            //deckData.Card018.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card019.SetCode = "Alpha";
            //deckData.Card019.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card020.SetCode = "Alpha";
            //deckData.Card020.SetID = (Random.Range(1, 9).ToString("D3"));

            //deckData.Card021.SetCode = "Alpha";
            //deckData.Card021.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card022.SetCode = "Alpha";
            //deckData.Card022.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card023.SetCode = "Alpha";
            //deckData.Card023.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card024.SetCode = "Alpha";
            //deckData.Card024.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card025.SetCode = "Alpha";
            //deckData.Card025.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card026.SetCode = "Alpha";
            //deckData.Card026.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card027.SetCode = "Alpha";
            //deckData.Card027.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card028.SetCode = "Alpha";
            //deckData.Card028.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card029.SetCode = "Alpha";
            //deckData.Card029.SetID = (Random.Range(1, 9).ToString("D3"));
            //deckData.Card030.SetCode = "Alpha";
            //deckData.Card030.SetID = (Random.Range(1, 9).ToString("D3"));
            #endregion
            var DBTask2 = dBReference.Child("decks").Child(number.ToString("D5"));
            string setCode = "Alpha";

            //Dictionary<string, Dictionary<string, string>> deck = new Dictionary<string, Dictionary<string, string>> ();            //List<CardData> deckList = new List<CardData>();
            for (int i = 1; i <= 30; i++)
            {
                
                string setIDno = (Random.Range(1, 9).ToString("D3"));


                deck.Add(
                        "Card" + (i.ToString("D3")),
                        new Dictionary<string, string>
                        {
                        { "SetCode", setCode},
                        { "SetID", setIDno}
                        }
                );
                //string test = JsonUtility.ToJson(deck, true);
                //Debug.Log(test);
                             
                
               //cardValuePair.Add(setCode, setIDno);
                //deckList.Add(card);



                //CardData cardData = new CardData("Alpha", (Random.Range(1, 9).ToString("D3")));
                //var DBTask3 = DBTask2.Child("Card" + (i.ToString("D3")));
                //DBTask3.Child("SetCode").SetValueAsync("Alpha");
                //DBTask3.Child("SetID").SetValueAsync((Random.Range(1, 9).ToString("D3")));
            }
            string json = JsonConvert.SerializeObject(deck, Formatting.Indented);
            Debug.Log(json);
            //string jsonString = JsonUtility.ToJson(deck);
            //string jsonString2 = JsonUtility.ToJson(points);
            //Debug.Log(jsonString);
            //Debug.Log(jsonString2);

            DBTask2.SetRawJsonValueAsync(json);
            //Debug.Log(deckList);
            //string jsonString = JsonUtility.ToJson(deckList);
            //Debug.Log(jsonString);
            //DBTask2.SetRawJsonValueAsync(jsonString);

        }

        //create folder with that name
        //generate card001 - card030, add SetCode Alpha
        //for each child - add SetID rand(001 - 009)
    }
    //public List<CardData> deckData = new List<CardData>();
    //public DeckData deckData;
    public Dictionary<string, Dictionary<string, string>> deck = new Dictionary<string, Dictionary<string, string>>();

    public void CreateCard(string name, string cost, string power, string toughness, string speed, string set, List<string> keywords)
    {
        StartCoroutine(CreateNewCard(name, cost, power,toughness,speed,set,keywords));
    }
    public IEnumerator CreateNewCard(string name, string cost, string power, string toughness, string speed, string set, List<string> keywords)
    {
        var DBTask = dBReference.Child("cards").Child("sets").Child(set).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            Debug.Log("this should never happen, why does the user not have a deck");
        }
        else
        {
            int number = 0;
            //data retrieved and exists
            DataSnapshot snapshot = DBTask.Result;
            bool DoTheThing = true;
            foreach (var item in snapshot.Children)
            {
                Debug.Log(item.ToString());
                number = int.Parse(item.Key) + 1;
                Debug.Log(number);
                if (item.Child("cardName").ToString() == name)
                {
                    DoTheThing = false;
                }
            }

            if (DoTheThing == true)
            {
                var DBTask2 = dBReference.Child("cards").Child("sets").Child(set).Child(number.ToString("D3"));
                var cardDetails = new Dictionary<string, string>();
                cardDetails.Add("cardName", name);
                cardDetails.Add("cost", cost);
                cardDetails.Add("power", power);
                cardDetails.Add("toughness", toughness);
                cardDetails.Add("speed", speed);

                for (int i = 0; i < keywords.Count; i++)
                {
                    cardDetails.Add($"keyword{i + 1}", keywords[i]);
                }
                string json = JsonConvert.SerializeObject(cardDetails, Formatting.Indented);
                Debug.Log(json);

                DBTask2.SetRawJsonValueAsync(json);
            }
        }
        

    }

    public TMP_Dropdown deckSelector;
    public TMP_Dropdown deckSelector2;

    public IEnumerable LoadUserData()
    {
        //Debug.Log("loading Decks");
        //yield return new WaitForSeconds(1);
        var DBTask = dBReference.Child("users").Child(user.UserId).Child("decks").OrderByChild("CustomName").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            Debug.Log("this should never happen, why does the user not have a deck");
        }
        else
        {
            //data retrieved and exists
            DataSnapshot snapshot = DBTask.Result;
            deckSelector.options.Clear();
            deckSelector2.options.Clear();
            foreach (var childSnapshot in snapshot.Children)
            {
                Debug.Log(childSnapshot.Child("CustomName").Value.ToString());

                //deckSelector.options.Add(new TMP_Dropdown.OptionData(childSnapshot.Child("CustomName").Value.ToString()));
                deckSelector.options.Add(new TMP_Dropdown.OptionData(childSnapshot.Child("Code").Value.ToString() + " - " + childSnapshot.Child("CustomName").Value.ToString()));
                deckSelector2.options.Add(new TMP_Dropdown.OptionData(childSnapshot.Child("Code").Value.ToString() + " - " + childSnapshot.Child("CustomName").Value.ToString()));


            }
            //Debug.Log("decks added");
        }
        deckSelector.RefreshShownValue();
    }

    public TMP_Dropdown setSelector;
    public IEnumerable LoadSetNames()
    {
        var DBTask = dBReference.Child("cards").Child("sets").OrderByKey().GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            Debug.Log("this should never happen, why does the user not have a deck");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            setSelector.options.Clear();

            setSelector.options.Add(new TMP_Dropdown.OptionData(""));
            foreach (var childSnapshot in snapshot.Children)
            {
                Debug.Log(childSnapshot.Key.ToString());

                //deckSelector.options.Add(new TMP_Dropdown.OptionData(childSnapshot.Child("CustomName").Value.ToString()));
                setSelector.options.Add(new TMP_Dropdown.OptionData(childSnapshot.Key.ToString()));
                


            }
            setSelector.RefreshShownValue();
        }
    }



    public void ClearList()
    {
        deckSelector.options.Clear();
        Debug.Log("Deleted");
    }

    public void RefreshDammit()
    {

        StartCoroutine(LoadUserData().GetEnumerator());
    }



    public TMP_InputField importDeckField;
    public void ImportDeck()
    {
        string code = int.Parse(importDeckField.text.ToString()).ToString("D5");
        Debug.Log("before main task");
        dBReference.Child("decks").Child(code).GetValueAsync().ContinueWith(task =>
        {

            if (task.IsFaulted)
            {
                Debug.Log("in fault");
                //error handling
            }
            else if (task.IsCompleted)
            {
                Debug.Log("task completed but:");
                if (task.Result != null)
                {
                    Debug.Log("is not null");
                    var userDeckCounter = dBReference.Child("users").Child(user.UserId).Child("decks").OrderByKey().GetValueAsync();
                    Debug.Log("after userdeckcounter");
                    int number = 0;
                    DataSnapshot snapshot = userDeckCounter.Result;
                    bool DoTheThing = true;

                    string duplicateNameProtection = "";
                    foreach (var item in snapshot.Children)
                    {
                        //Debug.Log(int.Parse(item.Child("Code").Value.ToString()).ToString("D5") + "and the other number is" + code);
                        if (int.Parse(item.Child("Code").Value.ToString()).ToString("D5") == code)
                        {

                            DoTheThing = false;
                        }
                        if(item.Child("CustomName").Value.ToString() == code) //if the name already exists - e.g 00001
                        {
                            duplicateNameProtection = $"{code}(2)";
                            #region //extra unused code for more complicated names
                            //the purpose of this function was to create (2), (3), etc. after duplicate deck names
                            //in the end it was just easier to make the deck renaming rules alphanumeric excluding punctuation
                            //if I change the rules to allow punctuation in deck names, this code will be needed for fringe cases
                            //for (int i = 0; i < snapshot.ChildrenCount; i++) //for each deck in the user's list
                            //{
                            //    bool BreakLoop = true;
                            //    foreach (var name in snapshot.Children) //-keep in mind that 00001(0) and 00001(1) make no sense
                            //    {
                            //        if (item.Child("CustomName").Value.ToString() == $"{code}({i + 2})") //check if '00001(2)' exists 
                            //        {
                            //            BreakLoop = false;
                            //            break; //if one of the names matches already we don't need to check the rest
                            //        }
                            //    }
                            //    if (BreakLoop == true) //we only get here if none of the names have the name 00001(X) 
                            //    {
                            //        duplicateNameProtection = $"{code}({i + 2})";
                            //        break;//once we find the first 00001(X) that isn't a duplicate, we can be done with the namechecking side of the function
                            //        //and it's a mathematical certainty that the loop will end at or before (childrencount) iterations
                            //    }

                            //}
                            #endregion 
                        }
                        else
                        {
                            duplicateNameProtection = code;
                        }
                        //number++;
                        if (int.Parse(item.Key) >= number)
                        {
                            number = int.Parse(item.Key) + 1;
                            Debug.Log(number);

                        }
                    }




                    if (DoTheThing)
                    {
                        Debug.Log("before DBTasks");
                        //needs to be incremented on the user's decks
                        var DBTask2 = dBReference.Child("users").Child(user.UserId).Child("decks").Child(number.ToString("D3")).Child("Code").SetValueAsync(code);
                        var DBTask = dBReference.Child("users").Child(user.UserId).Child("decks").Child(number.ToString("D3")).Child("CustomName").SetValueAsync(duplicateNameProtection);
                        Debug.Log("just before coroutine");
                        //StartCoroutine(LoadUserData().GetEnumerator());
                        //RefreshDammit();
                        Debug.Log("just after coroutine");

                    }
                    //StartCoroutine(LoadUserData().GetEnumerator());

                }
                //StartCoroutine(LoadUserData().GetEnumerator());

            }
            //StartCoroutine(LoadUserData().GetEnumerator());

        });
        StartCoroutine(LoadUserData().GetEnumerator());

    }

    private IEnumerator RefreshDeckList()
    {
        Debug.Log("loading Decks");

        var DBTask = dBReference.Child("users").Child(user.UserId).Child("decks").OrderByChild("CustomName").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            Debug.Log("this should never happen, why does the user not have a deck");
        }
        else
        {
            //data retrieved and exists
            DataSnapshot snapshot = DBTask.Result;
            deckSelector.options.Clear();
            foreach (var childSnapshot in snapshot.Children)
            {
                deckSelector.options.Add(new TMP_Dropdown.OptionData( childSnapshot.Child("Code").Value.ToString() +" - "+ childSnapshot.Child("CustomName").Value.ToString()));

            }
            Debug.Log("decks added");
        }
    }

    public TMP_Dropdown keyword1;
    public TMP_Dropdown keyword2;
    public TMP_Dropdown keyword3;
    public TMP_Dropdown keyword4;
    private IEnumerable RefreshKeywordList()
    {
        Debug.Log("Loading monster keywords into cardMaker");

        var DBTask = dBReference.Child("cards").Child("keywords").OrderByKey().GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            Debug.Log("this should never happen, why are there no keywords");
        }
        else
        {
            //data retrieved and exists
            DataSnapshot snapshot = DBTask.Result;
            keyword1.options.Clear();
            keyword2.options.Clear();
            keyword3.options.Clear();
            keyword4.options.Clear();

            keyword1.options.Add(new TMP_Dropdown.OptionData(""));
            keyword2.options.Add(new TMP_Dropdown.OptionData(""));
            keyword3.options.Add(new TMP_Dropdown.OptionData(""));
            keyword4.options.Add(new TMP_Dropdown.OptionData(""));
            foreach (var childSnapshot in snapshot.Children)
            {
                keyword1.options.Add(new TMP_Dropdown.OptionData(childSnapshot.Key.ToString()));
                keyword2.options.Add(new TMP_Dropdown.OptionData(childSnapshot.Key.ToString()));
                keyword3.options.Add(new TMP_Dropdown.OptionData(childSnapshot.Key.ToString()));
                keyword4.options.Add(new TMP_Dropdown.OptionData(childSnapshot.Key.ToString()));

            }
            Debug.Log("decks added");
        }
    }




    public void DeleteDeck()
    {
        string customName = deckSelector.GetComponentInChildren<TMP_Text>().text.ToString().Substring(0, 5);
        dBReference.Child("users").Child(user.UserId).Child("decks").OrderByChild("CustomName").EqualTo(customName).GetValueAsync().ContinueWith(task =>
        {

            if (task.IsFaulted)
            {
                //error handling
            }
            else if (task.IsCompleted)
            {
                if (task.Result != null)
                {
                    DataSnapshot snapshot = task.Result;
                    string key = "";

                    if(snapshot.ChildrenCount == 1)
                    {
                        foreach (var item in snapshot.Children)
                        {
                            key = int.Parse(item.Key.ToString()).ToString("D3");
                            Debug.Log(key);

                        }
                        _ = dBReference.Child("users").Child(user.UserId).Child("decks").Child(key).RemoveValueAsync();

                    }

                    //int.Parse(item.Child("Code").Value.ToString()).ToString("D5")


                }
            }
        });
        StartCoroutine(LoadUserData().GetEnumerator());
    }

    public TMP_InputField newDeckName;
    public void RenameDeck()
    {
        string newName = newDeckName.text;
        string oldName = deckSelector.GetComponentInChildren<TMP_Text>().text.ToString().Substring(0,5);
        dBReference.Child("users").Child(user.UserId).Child("decks").OrderByChild("CustomName").EqualTo(oldName).GetValueAsync().ContinueWith(task =>
        {

            if (task.IsFaulted)
            {
                //error handling
            }
            else if (task.IsCompleted)
            {
                if (task.Result != null)
                {
                    DataSnapshot snapshot = task.Result;
                    int number = 0;
                    var nameRef = dBReference.Child("users").Child(user.UserId).Child("decks");
                    dBReference.Child("users").Child(user.UserId).Child("decks").OrderByChild("CustomName").EqualTo(newName).GetValueAsync().ContinueWith(task2 =>
                    {
                        DataSnapshot newNameCheck = task2.Result;
                        if (newNameCheck.ChildrenCount == 0)
                        {
                            foreach (var item in snapshot.Children)
                            {
                                number = int.Parse(item.Key);
                            }
                            if (number != 0)
                            {
                                nameRef.Child(int.Parse(number.ToString()).ToString("D3")).Child("CustomName").SetValueAsync(newName);
                            }
                        }
                        else
                        {
                            //return 'No duplicate names please.'
                            Debug.Log("duplicate name detected");

                        }

                    });

                   
                    //int.Parse(item.Child("Code").Value.ToString()).ToString("D5")

                    StartCoroutine(LoadUserData().GetEnumerator());
                }
            }
        });
    }

    public void ClearLogInFields()
    {
        emailField.text = "";
        passwordField.text = "";
    }

    public void ClearRegisterFields()
    {
        usernameRegisterField.text = "";
        password1RegisterField.text = "";
        password2RegisterField.text = "";
        emailRegisterField.text = "";
    }


    private IEnumerator UpdateUsernameAuth(string _username)
    {
        //create a userprofile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        //call the firebase authentiation update-user-profile function, passing the profile with the username
        var ProfileTask = user.UpdateUserProfileAsync(profile);
        //wait for completion
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        //check it works
        if(ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //auth username is now updated
        }
    }

    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        var DBTask = dBReference.Child("users").Child(user.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if(DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //database deaths is updated
        }
    }


    public void GetFixedDeckButton(string opponentDeck)
    {

        StartCoroutine(LoadDecksForGame(opponentDeck));

    }
    public void Load00001DeckTest()
    {
        Debug.Log("button clicked");

        //StartCoroutine(LoadDecksForGame(00001.ToString("D5")).GetEnumerator());
        Debug.Log("coroutine maybe started?");

    }
    public void GetRandomDeckButton()
    {
        StartCoroutine(GetRandomDeck().GetEnumerator());
    }
    public IEnumerable GetRandomDeck()
    {
        var DBTask = dBReference.Child("decks").OrderByKey().GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            string opponentDeck =  Random.Range(1, int.Parse(snapshot.ChildrenCount.ToString())).ToString("D5");
            Debug.Log(opponentDeck);
            StartCoroutine(LoadDecksForGame(opponentDeck));
        }
    }


    public List<Card> playerDecklist;
    public List<Card> opponentDecklist;

    //public TextMeshProUGUI timer;
    //public float timerCount;
    //public bool timerBool = false;
    public IEnumerator LoadDecksForGame(string opponentDeck)
    {
        Debug.Log("coroutine definitely started");

        playerDecklist.Clear();
        opponentDecklist.Clear();
        bool opponentDeckReady = false;
        bool playerDeckReady = false;

        //this was supposed to be a timer to show how long the deck loading process had been going for but it just freezes the game and i dont know why
        //Debug.Log("Timer Started");
        //timerBool = true;
        //timerCount = 0;
        //while (timerBool == true)
        //{
        //    timerCount += Time.deltaTime;
        //    timer.text = timerCount.ToString();
        //}

        var DBTask = dBReference.Child("decks").Child(opponentDeck).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                Debug.Log(dBReference.Child("cards").Child("sets").Child(childSnapshot.Child("SetCode").Value.ToString()).Child(childSnapshot.Child("SetID").Value.ToString()));
                var DBTask2 = dBReference.Child("cards").Child("sets").Child(childSnapshot.Child("SetCode").Value.ToString()).Child(childSnapshot.Child("SetID").Value.ToString()).GetValueAsync();
                //string cardName = childSnapshot.Child("cardName").Value.ToString();
                yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

                DataSnapshot dataSnapshot2 = DBTask2.Result;
                Card newCard = new Card();
                opponentDecklist.Add(newCard);

                newCard.setCode = childSnapshot.Child("SetCode").Value.ToString();
                newCard.id = int.Parse(childSnapshot.Child("SetID").Value.ToString());
                newCard.cardName = dataSnapshot2.Child("cardName").Value.ToString();
                newCard.cost = int.Parse(dataSnapshot2.Child("cost").Value.ToString());
                newCard.power = int.Parse(dataSnapshot2.Child("power").Value.ToString());
                newCard.toughness = int.Parse(dataSnapshot2.Child("toughness").Value.ToString());
                newCard.speed = int.Parse(dataSnapshot2.Child("speed").Value.ToString());
                newCard.colour = Color.white; //placeholder
                newCard.cardImage = Resources.Load<Sprite>("VOID"); //placeholder
                if (dataSnapshot2.Child("keyword1").Exists)
                {
                    newCard.cardKeywords.Add(dataSnapshot2.Child("keyword1").Value.ToString());
                    if (dataSnapshot2.Child("keyword2").Exists)
                    {
                        newCard.cardKeywords.Add(dataSnapshot2.Child("keyword2").Value.ToString());

                        if (dataSnapshot2.Child("keyword3").Exists)
                        {
                           newCard.cardKeywords.Add(dataSnapshot2.Child("keyword3").Value.ToString());

                            if (dataSnapshot2.Child("keyword4").Exists)
                            {
                                newCard.cardKeywords.Add(dataSnapshot2.Child("keyword4").Value.ToString());
                            }
                        }
                    }

                }
            }
            if(opponentDecklist.Count == 30)
            {
                opponentDeckReady = true;
            }
        }

        var DBTask3 = dBReference.Child("decks").Child(deckSelector.GetComponentInChildren<TMP_Text>().text.ToString().Substring(0, 5)).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask3.IsCompleted);
        if (DBTask3.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask3.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask3.Result;

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                Debug.Log(dBReference.Child("cards").Child("sets").Child(childSnapshot.Child("SetCode").Value.ToString()).Child(childSnapshot.Child("SetID").Value.ToString()));
                var DBTask4 = dBReference.Child("cards").Child("sets").Child(childSnapshot.Child("SetCode").Value.ToString()).Child(childSnapshot.Child("SetID").Value.ToString()).GetValueAsync();
                //string cardName = childSnapshot.Child("cardName").Value.ToString();
                yield return new WaitUntil(predicate: () => DBTask4.IsCompleted);

                DataSnapshot dataSnapshot2 = DBTask4.Result;
                Card newCard = new Card();
                newCard.setCode = childSnapshot.Child("SetCode").Value.ToString();
                newCard.id = int.Parse(childSnapshot.Child("SetID").Value.ToString());
                newCard.cardName = dataSnapshot2.Child("cardName").Value.ToString();
                newCard.cost = int.Parse(dataSnapshot2.Child("cost").Value.ToString());
                newCard.power = int.Parse(dataSnapshot2.Child("power").Value.ToString());
                newCard.toughness = int.Parse(dataSnapshot2.Child("toughness").Value.ToString());
                newCard.speed = int.Parse(dataSnapshot2.Child("speed").Value.ToString());
                newCard.colour = Color.white; //placeholder
                newCard.cardImage = Resources.Load<Sprite>("VOID"); //placeholder
                if (dataSnapshot2.Child("keyword1").Exists)
                {
                    newCard.cardKeywords.Add(dataSnapshot2.Child("keyword1").Value.ToString());
                    if (dataSnapshot2.Child("keyword2").Exists)
                    {
                        newCard.cardKeywords.Add(dataSnapshot2.Child("keyword2").Value.ToString());

                        if (dataSnapshot2.Child("keyword3").Exists)
                        {
                            newCard.cardKeywords.Add(dataSnapshot2.Child("keyword3").Value.ToString());

                            if (dataSnapshot2.Child("keyword4").Exists)
                            {
                                newCard.cardKeywords.Add(dataSnapshot2.Child("keyword4").Value.ToString());
                            }
                        }
                    }

                }
                playerDecklist.Add(newCard);
            }
            if (playerDecklist.Count == 30)
            {
                playerDeckReady = true;
            }
        }

        if(playerDeckReady && opponentDeckReady)
        {
            Debug.Log("It's time to duel!");
            LoadScene("CardGameBoard");
        }

        //loadscene
    }
         

    #region OldCodeFromTutorials
    //private IEnumerator UpdateKills(int _kills)
    //{
    //    var DBTask = dBReference.Child("users").Child(user.UserId).Child("kills").SetValueAsync(_kills);

    //    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

    //    if (DBTask.Exception != null)
    //    {
    //        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    //    }
    //    else
    //    {
    //        //database kills is updated
    //    }
    //}

    //private IEnumerator UpdateXP(int _xp)
    //{
    //    var DBTask = dBReference.Child("users").Child(user.UserId).Child("xp").SetValueAsync(_xp);

    //    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

    //    if (DBTask.Exception != null)
    //    {
    //        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    //    }
    //    else
    //    {
    //        //database xp amount is updated
    //    }
    //}

    //private IEnumerator UpdateDeaths(int _deaths)
    //{
    //    var DBTask = dBReference.Child("users").Child(user.UserId).Child("deaths").SetValueAsync(_deaths);

    //    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

    //    if (DBTask.Exception != null)
    //    {
    //        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    //    }
    //    else
    //    {
    //        //database username is updated
    //    }
    //}


    //private IEnumerator LoadUserData()
    //{
    //    var DBTask = dBReference.Child("users").Child(user.UserId).GetValueAsync();

    //    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

    //    if (DBTask.Exception != null)
    //    {
    //        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    //    }
    //    else if (DBTask.Result.Value == null)
    //    {
    //        //data retrieved but does not exist - create 'default' entry
    //        xpField.text = "0";
    //        killsField.text = "0";
    //        deathsField.text = "0";
    //    }
    //    else
    //    {
    //        //data retrieved and exists
    //        DataSnapshot snapshot = DBTask.Result;

    //        xpField.text = snapshot.Child("xp").Value.ToString();
    //        killsField.text = snapshot.Child("kills").Value.ToString();
    //        deathsField.text = snapshot.Child("deaths").Value.ToString();
    //    }
    //}
    #endregion
    public IEnumerable LoadDeckPreviewData(string deck)
    {
        var DBTask = dBReference.Child("decks").Child(deck).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            foreach (Transform child in cardContent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                Debug.Log(dBReference.Child("cards").Child("sets").Child(childSnapshot.Child("SetCode").Value.ToString()).Child(childSnapshot.Child("SetID").Value.ToString()));
                var DBTask2 = dBReference.Child("cards").Child("sets").Child(childSnapshot.Child("SetCode").Value.ToString()).Child(childSnapshot.Child("SetID").Value.ToString()).GetValueAsync();
                //string cardName = childSnapshot.Child("cardName").Value.ToString();
                yield return new WaitUntil(predicate: () => DBTask2.IsCompleted);

                DataSnapshot dataSnapshot2 = DBTask2.Result;
                string cardName = dataSnapshot2.Child("cardName").Value.ToString();
                string cost = dataSnapshot2.Child("cost").Value.ToString();
                string power = dataSnapshot2.Child("power").Value.ToString();
                string toughness = dataSnapshot2.Child("toughness").Value.ToString();
                string speed = dataSnapshot2.Child("speed").Value.ToString();
                string keywords = "";

                if (dataSnapshot2.Child("keyword1").Exists)
                {
                    keywords += dataSnapshot2.Child("keyword1").Value.ToString();
                    if (dataSnapshot2.Child("keyword2").Exists)
                    {
                        keywords += ", " + dataSnapshot2.Child("keyword2").Value.ToString();

                        if (dataSnapshot2.Child("keyword3").Exists)
                        {
                            keywords += ", " + dataSnapshot2.Child("keyword3").Value.ToString();

                            if (dataSnapshot2.Child("keyword4").Exists)
                            {
                                keywords += ", " + dataSnapshot2.Child("keyword4").Value.ToString();
                            }
                        }
                    }
                    
                }

                GameObject scoreboardElement = Instantiate(cardElement, cardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(cardName, power, toughness, speed, keywords);
            }
        
        }
    }

    public IEnumerable LoadCardData(string set)
    {
        var DBTask = dBReference.Child("cards").Child("sets").Child(set).OrderByKey().GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //data has been retreived
            DataSnapshot snapshot = DBTask.Result;

            foreach (Transform child in cardContent.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string cardName = childSnapshot.Child("cardName").Value.ToString();
                string power = (childSnapshot.Child("power").Value.ToString());
                string toughness = (childSnapshot.Child("toughness").Value.ToString());
                string speed = (childSnapshot.Child("speed").Value.ToString());
                string keywords = "";

                if (childSnapshot.Child("keyword1").Exists)
                {
                    keywords += childSnapshot.Child("keyword1").Value.ToString();
                    if (childSnapshot.Child("keyword2").Exists)
                    {
                        keywords +=", " + childSnapshot.Child("keyword2").Value.ToString();
                        if (childSnapshot.Child("keyword3").Exists)
                        {
                            keywords += ", " + childSnapshot.Child("keyword3").Value.ToString();
                            if (childSnapshot.Child("keyword4").Exists)
                            {
                                keywords += ", " + childSnapshot.Child("keyword4").Value.ToString();
                            }
                        }
                    }
                    
                }


                //instantiate new scoreboard elements
                GameObject scoreboardElement = Instantiate(cardElement, cardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(cardName, power, toughness, speed, keywords);
            }

            UIManager.instance.CardListScreen();
        }
    }
    


}
