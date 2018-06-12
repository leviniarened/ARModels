using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IView
{
    void Show();
    void Hide();
}

public interface IDrawItems
{
    void DrawItems(ConfigurationFile s);
}

public interface IAuthForm
{
    void OnAuthSuccessCallback(string username, string accesskey);
    void OnAuthLoginClick();
    void OnAuthRegisterClick();
}

public class UIView : MonoBehaviour, IAuthForm, IDrawItems
{
    [SerializeField] private ViewWidget AuthWindow;
    [SerializeField] private ViewWidget AssetViewWindow;

    [SerializeField] private InputField username;
    [SerializeField] private InputField passwrod;

    [SerializeField] private Text loginStatusText;


    [SerializeField]
    private Transform arItemListViewRoot;

    [SerializeField]
    private ARItem ARElement;

    void Start()
    {
        AssetViewWindow.Hide();
        AuthWindow.Show();
    }

    public void DrawItems(ConfigurationFile s)
    {
        //clean collection
        foreach (Transform t in arItemListViewRoot)
        {
            if (t.GetComponent<ARItem>() != null)
            {
                Destroy(t.gameObject);
            }
        }

        for (var i = 0; i < s.models.Count; i++)
        {
            var t = Instantiate(ARElement, arItemListViewRoot);
            t.transform.SetAsFirstSibling();
            t.SetUp(s.models[i], s.markers[i]);
        }
    }

    public void OnAuthSuccessCallback(string username, string accesskey)
    {
        IIS_Core.SetUpProfile(username, accesskey);
        IIS_Core.FetchAllFiles();
    }

    public void OnAuthLoginClick()
    {
        IIS_Core.LoginIn(username.text, passwrod.text, 
        (v) =>
        {
            AuthWindow.Hide();
            AssetViewWindow.Show();
            OnAuthSuccessCallback(username.text, passwrod.text);
            IIS_Core.DecodeAndApplySaveToView(v, this);
        },
            s => { loginStatusText.text = s; } );
    }

    public void OnAuthRegisterClick()
    {
        IIS_Core.RegisterUser(username.text, passwrod.text,
            () =>
            {
                AuthWindow.Hide();
                AssetViewWindow.Show();
                OnAuthSuccessCallback(username.text, passwrod.text);
            },
            s => { loginStatusText.text = s; });
    }
}
