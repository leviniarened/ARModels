using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using UnityEngine;

[Serializable]
public class ConfigurationFile
{
    public List<string> models = new List<string>();
    public List<int> markers = new List<int>();
}

public class IIS_Core : MonoBehaviour
{
    [SerializeField] string host = "localhost";
    [SerializeField] string port = "8000";
    [SerializeField] string scripts_directory = "cgi-bin";

    string loginPage = "login.py";
    string updatePage = "update.py";
    string registerPage = "registration.py";
    string uploadPage = "upload.py";
    string downloadPage = "download.py";
    string fetchAllPage = "fetch_all.py";
    private string update_configPage = "update_config.py";

    string username;
    string accesskey;

    static IIS_Core _instance;

    [ContextMenu("TestUpload")]
    public void TestMeshUpload()
    {
        string filename = "C://Users/levin/Desktop/Mesh.obj";
        UploadFile(filename);
    }

    [ContextMenu("TestRegister")]
    public void TestRegister()
    {
        StartCoroutine(Register(username, accesskey));
    }

    [ContextMenu("TestLogin")]
    public void TestLogin()
    {
        StartCoroutine(Login(username, accesskey));
    }

    [ContextMenu("TestDownloadFile")]
    public void TestDownloadFile()
    {
        StartCoroutine(DownloadFile("Mesh.obj"));
    }

    public void SetUsername(string newUsername)
    {
        username = newUsername;
    }
    public void SetAccesskey(string newAccesskey)
    {
        accesskey = newAccesskey;
    }

    public static void SetUpProfile(string username, string accesskey)
    {
        IIS_Core.Instance.SetUsername(username);
        IIS_Core.Instance.SetAccesskey(accesskey);
    }

    public static void FetchAllFiles(Action OnSuccess = null, Action<string> OnFailed = null)
    {
        IIS_Core.Instance.DoFetchAll(OnSuccess, OnFailed);
    }

    public static void LoginIn(string username, string accesskey, Action<string> OnSuccess = null, Action<string> OnFailed=null)
    {
        IIS_Core.Instance.DoLogin(username, accesskey, OnSuccess, OnFailed);
    }

    public static void RegisterUser(string username, string accesskey, Action OnSuccess = null, Action<string> OnFailed = null)
    {
        IIS_Core.Instance.DoRegister(username, accesskey, OnSuccess, OnFailed);
    }

    protected void DoRegister(string username, string accesskey, Action OnSuccess = null, Action<string> OnFailed = null)
    {
        StartCoroutine(Register(username, accesskey, OnSuccess, OnFailed));
    }

    protected void DoLogin(string username, string accesskey, Action<string> OnSuccess = null, Action<string> OnFailed = null)
    {
        StartCoroutine(Login(username, accesskey, OnSuccess, OnFailed));
    }

    protected void DoFetchAll(Action OnSuccess = null, Action<string> OnFailed = null)
    {
        StartCoroutine(FetchAll(OnSuccess, OnFailed));
    }

    public static IIS_Core Instance
    {
        get
        {
            if (_instance) return _instance;
            _instance = FindObjectOfType<IIS_Core>();
            if (_instance) return _instance;
            var container = new GameObject { name = "IIS_Core" };
            _instance = container.AddComponent<IIS_Core>();
            return _instance;
        }
    }

    public void OpenFileWindow()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Models", ".obj"));
        FileBrowser.ShowLoadDialog(OnFileSelected, OnFileSelectionCanceled);
    }

    void OnFileSelected(string filename)
    {
        UploadFile(filename);
    }

    void OnFileSelectionCanceled()
    {
        Debug.Log("Cancel");
    }

    IEnumerator Register(string username, string accesskey, Action OnSuccess = null, Action<string> OnFailed = null)
    {
        var registerWWW = string.Format("http://{0}:{1}/{2}/{3}", host, port, scripts_directory, registerPage);

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("accesskey", accesskey);

        var result = new WWW(registerWWW, form);

        yield return result;

        if (!string.IsNullOrEmpty(result.error))
        {
            if (OnFailed != null)
            {
                OnFailed(result.error);
            }
            print(result.error);
        }
        else
        {
            if (OnSuccess != null)
            {
                OnSuccess();
            }
            //DecodeSave(result.text);
            print(result.text);
        }
    }

    IEnumerator FetchAll(Action OnSuccess = null, Action<string> OnFailed = null)
    {
        var loginWWW = string.Format("http://{0}:{1}/{2}/{3}", host, port, scripts_directory, fetchAllPage);

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("accesskey", accesskey);

        var result = new WWW(loginWWW, form);

        yield return result;

        if (!string.IsNullOrEmpty(result.error))
        {
            if (OnFailed != null)
            {
                OnFailed(result.error);
            }
            print(result.error);
        }
        else
        {
            if (OnSuccess != null)
            {
                OnSuccess();
            }
            print(result.text);
        }
    }

    public static void UpdateCfg(string filename, int index, Action OnSuccess = null, Action<string> OnFailed = null)
    {
        IIS_Core.Instance.DoUpdateConfig(filename, index, OnSuccess, OnFailed);
    }

    protected void DoUpdateConfig(string filename, int index, Action OnSuccess = null, Action<string> OnFailed = null)
    {
        StartCoroutine(UpdateConfig(filename, index, OnSuccess, OnFailed));
    }

    IEnumerator UpdateConfig(string filename, int index,  Action OnSuccess = null, Action<string> OnFailed = null)
    {
        var loginWWW = string.Format("http://{0}:{1}/{2}/{3}", host, port, scripts_directory, update_configPage);

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("accesskey", accesskey);
        form.AddField("filename", filename);
        form.AddField("markerindex", index);
        var result = new WWW(loginWWW, form);

        yield return result;

        if (!string.IsNullOrEmpty(result.error))
        {
            if (OnFailed != null)
            {
                OnFailed(result.error);
            }
            print(result.error);
        }
        else
        {
            if (OnSuccess != null)
            {
                OnSuccess();
            }
            print(result.text);
        }
    }

    IEnumerator Login(string username, string accesskey, Action<string> OnSuccess= null, Action<string> OnFailed = null)
    {
        var loginWWW = string.Format("http://{0}:{1}/{2}/{3}", host, port, scripts_directory, loginPage);

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("accesskey", accesskey);

        var result = new WWW(loginWWW, form);

        yield return result;

        if (!string.IsNullOrEmpty(result.error))
        {
            if (OnFailed != null)
            {
                OnFailed(result.error);
            }

            print(result.error);
        }
        else
        {
            if (OnSuccess != null)
            {
                OnSuccess(result.text);
            }
            //DecodeSave(result.text);
            print(result.text);
        }
    }

    public static void DownloadModel(string name, Action<string> OnDone = null, Action OnFailed = null)
    {
        IIS_Core.Instance.DoDownloadModel(name, OnDone, OnFailed);
    }

    protected void DoDownloadModel(string name, Action<string> OnDone = null, Action OnFailed = null)
    {
        StartCoroutine(DownloadFile(name, OnDone, OnFailed));
    }

    IEnumerator DownloadFile(string filename, Action<string> OnDone = null, Action OnFailed = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("file", filename);

        var downloadWWW = string.Format("http://{0}:{1}/{2}/{3}", host, port, scripts_directory, downloadPage);

        var result = new WWW(downloadWWW, form);

        yield return result;

        if (!string.IsNullOrEmpty(result.error))
        {
            print(result.error);
            if (OnFailed != null)
                OnFailed();
        }
        else
        {
            if (OnDone != null)
                OnDone(result.text);
            //print(result.text);
        }
    }


    IEnumerator UploadFileCoroutine(string localFileName)
    {
        WWW localFile = new WWW("file:///" + localFileName);
        yield return localFile;
        if (localFile.error == null)
            Debug.Log("Loaded file successfully");
        else
        {
            Debug.Log("Open file error: " + localFile.error);
            yield break; // stop the coroutine here
        }
        WWWForm postForm = new WWWForm();
        
        postForm.AddBinaryData("file", localFile.bytes, Path.GetFileName(localFileName), "text/plain");
        postForm.AddField("username", username);

        var uploadWWW = string.Format("http://{0}:{1}/{2}/{3}", host, port, scripts_directory, uploadPage);
        WWW upload = new WWW(uploadWWW, postForm);
        yield return upload;
        if (upload.error == null)
        {
            Debug.Log(upload.text);
            DecodeSave(upload.text, FindObjectOfType<UIView>());

        }
        else
            Debug.Log("Error during upload: " + upload.error);
    }

    void UploadFile(string localFileName)
    {
        StartCoroutine(UploadFileCoroutine(localFileName));
    }

    public IEnumerator Save(string save)
    {
        WWWForm form = new WWWForm();
        form.AddField("cfg", save);
        form.AddField("username", username);

        var UpdateWWW = string.Format("http://{0}:{1}/{2}/{3}", host, port, scripts_directory, updatePage);

        var result = new WWW(UpdateWWW, form);

        yield return result;

        if (!string.IsNullOrEmpty(result.error))
        {
            print(result.error);
        }
        else
        {
            ///DecodeSave(result.text);
            print(result.text);
        }
    }

    public void Save(Save s)
    {
        StartCoroutine(Save(JsonUtility.ToJson(s)));
    }
    
    public void DecodeSave(string save, IDrawItems r)
    {
        if(r == null) return;
        var obj = JsonUtility.FromJson<ConfigurationFile>(save);
        if (obj == null) return;//exception
        r.DrawItems(obj);
    }

    public static void DecodeAndApplySaveToView(string save, IDrawItems r)
    {
        IIS_Core.Instance.DecodeSave(save, r);
    }
}
