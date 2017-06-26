using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
#endif

public class UMBCrossPromoBase : MonoBehaviour {
    protected static string listenerObjectName="~UMBCrossPromo";
    private static GameObject _instance=null;
    protected static UMBCrossPromoBase instance {
            get {  if (_instance==null) {
                    _instance=new GameObject(UMBCrossPromoBase.listenerObjectName);
                    _instance.AddComponent<UMBCrossPromo>();
                    DontDestroyOnLoad( _instance );
                }
                return _instance.GetComponent<UMBCrossPromoBase>();
            }
        }

    public delegate void ParameterlessDelegate();
    public delegate void MessageDelegate(string message);

    public static event ParameterlessDelegate OnDidLoad;
    public static event MessageDelegate OnDidFailToLoad;
    public static event ParameterlessDelegate OnDidClose;
    public static event MessageDelegate OnDidOpenStoreForAppWithId;
    public static event ParameterlessDelegate OnDidCloseStore;
    public static event MessageDelegate OnDidCallActionWithUrl;
    public static event ParameterlessDelegate OnDidTrackInstall;
    public static event MessageDelegate OnDidFailToTrackInstall;

    protected void callbackDidLoadCrossPromo(string message) {
        if (OnDidLoad!=null) {
            OnDidLoad();
        }
    }
    protected void callbackDidFailToLoadWithError(string error) {
        if (OnDidFailToLoad!=null) {
            OnDidFailToLoad(error);
        }
    }

    protected void callbackDidClose(string message) {
        if (OnDidClose!=null) {
            OnDidClose();
        }
    }
    protected void callbackDidOpenStoreForAppWithId(string appId) {
        if (OnDidOpenStoreForAppWithId!=null) {
            OnDidOpenStoreForAppWithId(appId);
        }
    }
    protected void callbackDidCloseStore(string message) {
        if (OnDidCloseStore!=null) {
            OnDidCloseStore();
        }
    }
    protected void callbackDidCallAction(string url) {
        if (OnDidCallActionWithUrl!=null) {
            OnDidCallActionWithUrl(url);
        }
    }

    protected void callbackDidTrackInstall(string message) {
        if (OnDidTrackInstall!=null) {
            OnDidTrackInstall();
        }
    }

    protected void callbackDidFailToTrackInstall(string error) {
        if (OnDidFailToTrackInstall!=null) {
            OnDidFailToTrackInstall(error);
        }
    }

    public static void Show(string bundleId) {
        if (UMBCrossPromoBase.instance!=null) 
        {
             instance.StartCoroutine(ShowCorotine(bundleId));
            //  instance.show(bundleId);
        }
    }

    private static IEnumerator ShowCorotine(string bundleId) {
         yield return 0;
         instance.show(bundleId);
     }
    
    public static void Track(string bundleId) {
        if (UMBCrossPromoBase.instance!=null) 
            instance.StartCoroutine(TrackCorotine(bundleId));
    }

    private static IEnumerator TrackCorotine(string bundleId) {
         yield return 0;
         instance.track(bundleId);
     }

    public static void Hide() { 
        if (UMBCrossPromoBase.instance!=null) 
            instance.StartCoroutine(HideCorotine());
    }

    private static IEnumerator HideCorotine() {
         yield return 0;
         instance.hide();
     }
    
    protected virtual void show(string bundleId) { }
    protected virtual void track(string bundleId) { }
    protected virtual void hide() { }
}

#if UNITY_IOS && !UNITY_EDITOR

public class UMBCrossPromo : UMBCrossPromoBase {
	[DllImport("__Internal")]
    private static extern void _UMBCrossSetListenerObjectName(string objectName);
    [DllImport("__Internal")]
    private static extern void _UMBCrossPromoShow(string bundleId);
    [DllImport("__Internal")]
    private static extern void _UMBCrossPromoTrack(string bundleId);
	[DllImport("__Internal")]
    private static extern void _UMBCrossPromoHide();    

	protected override void show(string bundleId) {
        if (Application.platform == RuntimePlatform.IPhonePlayer) {
            _UMBCrossSetListenerObjectName(UMBCrossPromoBase.listenerObjectName);
            _UMBCrossPromoShow(bundleId);
        }
    }

	protected override void hide() {
        if (Application.platform == RuntimePlatform.IPhonePlayer) {
            _UMBCrossSetListenerObjectName(UMBCrossPromoBase.listenerObjectName);
            _UMBCrossPromoHide();
        }
    }

    protected override void track(string bundleId) {
        if (Application.platform == RuntimePlatform.IPhonePlayer) {
            _UMBCrossSetListenerObjectName(UMBCrossPromoBase.listenerObjectName);
            _UMBCrossPromoTrack(bundleId);
        }
    }
}
#endif

#if UNITY_ANDROID && !UNITY_EDITOR

public class UMBCrossPromo : UMBCrossPromoBase {
    private AndroidJavaClass androidCrossPromo;
    private AndroidJavaClass AndroidCrossPromo {
        get {
            if (androidCrossPromo==null)
            {
                androidCrossPromo=new AndroidJavaClass("com.umbrella.umbcrosspromounity.UMBCrossPromoUnity"); 
            }
            return androidCrossPromo;
        }
    }

    private AndroidJavaObject androidUnityActivity;
    private AndroidJavaObject AndroidUnityActivity
    {
        get {
            if (androidUnityActivity==null)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
                androidUnityActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return androidUnityActivity;
        }
    }

    private void CallStatic(string methodName, params object[] args)
    {
        try
        {
            this.AndroidUnityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                this.AndroidCrossPromo.CallStatic("SetListenerObjectName",UMBCrossPromoBase.listenerObjectName);
                this.AndroidCrossPromo.CallStatic(methodName, args);
            }));
        } catch (System.Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }
    }

	protected override void show(string bundleId) {
        if (Application.platform != RuntimePlatform.Android) return;
        this.CallStatic("Show", this.AndroidUnityActivity, bundleId);
    }

	protected override void hide() {
        if (Application.platform != RuntimePlatform.Android) return;
        this.CallStatic("Hide");
    }

    protected override void track(string bundleId) {
        if (Application.platform != RuntimePlatform.Android) return;
        this.CallStatic("Track", bundleId);
    }
}
#elif UNITY_EDITOR

public class UMBCrossPromo : UMBCrossPromoBase {
	
}
#endif

/*#if UNITY_EDITOR

public class UMBCrossPromoXcodeProjectMod : MonoBehaviour
{
    internal static void DeleteDirectory(string target_dir)
    {
        string[] files = Directory.GetFiles(target_dir);
        string[] dirs = Directory.GetDirectories(target_dir);

        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        foreach (string dir in dirs)
        {
            DeleteDirectory(dir);
        }

        Directory.Delete(target_dir, false);
    }

	internal static void CopyAndReplaceDirectory(string srcPath, string dstPath)
    {
        if (Directory.Exists(dstPath))
            DeleteDirectory(dstPath);
        if (File.Exists(dstPath))
            File.Delete(dstPath);
 
        Directory.CreateDirectory(dstPath);
 
        foreach (var file in Directory.GetFiles(srcPath))
            File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));
 
        foreach (var dir in Directory.GetDirectories(srcPath))
            CopyAndReplaceDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
    }
 
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            string projPath = PBXProject.GetPBXProjectPath(path);
            PBXProject proj = new PBXProject();
     
            proj.ReadFromString(File.ReadAllText(projPath));
            string target = proj.TargetGuidByName("Unity-iPhone");
      
            CopyAndReplaceDirectory("Assets/Plugins/iOS/UMBCrossPromo.framework", Path.Combine(path, "Frameworks/UMBCrossPromo.framework"));
            proj.AddFileToBuild(target, proj.AddFile("Frameworks/UMBCrossPromo.framework", "Frameworks/UMBCrossPromo.framework", PBXSourceTree.Source));
  
            proj.SetBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
            proj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Frameworks");
     
            File.WriteAllText(projPath, proj.WriteToString());
        }
    }
}
#endif*/