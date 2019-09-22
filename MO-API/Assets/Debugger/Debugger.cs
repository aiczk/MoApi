using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Profiling;

namespace Utils
{
    public enum Lev
{
    Root,
    Setting,
    Manager,
    Edit,
}

public class Debugger : MonoBehaviour
{
    #region 雑多
    public KeyCode OpenCloseKey = KeyCode.Escape;

    private string possession;

    private List<string> classData = new List<string>();
    private List<string> valueData = new List<string>();
    private List<string> method = new List<string>();

    private Vector2 scrollPosition;
    private int totalObj;

    private const string HorizontalStick = "────────────────────────────────────────";
    private Lev le;
    private GUIStyle style = new GUIStyle();

    private bool isShowWindow = true;
    private bool isFullScreen;
    private bool isLockWindow;

    private struct Log
    {
        public string message;
        public string stackTrace;
        public LogType type;
    }

    private List<Log> logs = new List<Log>();
    private Vector2 scrollPos;
    private bool isShowLog;
    private bool isCollapse = true;

    private static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>
     {
          { LogType.Assert, Color.white },
          { LogType.Error, Color.red },
          { LogType.Exception, Color.red },
          { LogType.Log, Color.white },
          { LogType.Warning, Color.yellow },
     };
    #endregion

    #region Frame

    private int targetFrame;
    private float fps;
    private int m_frame_counts;
    private float m_start_time;
    #endregion

    #region Resolution
    private string resolution_info;
    private string refleshRate;
    #endregion

    #region Heap
    private const int CHK_FRAME = 30;   //!< ヒープメモリ更新頻度

    private int m_Frame = 0;  //!< フレーム数
    private int m_HeapSizeMax1 = 0;     //!< チェック中の取得ヒープ最大サイズ
    private int m_HeapSizeMax2 = 0;     //!< 前回チェックの取得ヒープ最大サイズ

    private int AcquisitionHeapMemorySizeory;
    private int TotalMemoryCapacity;
    #endregion

    const int margin = 20;
    Rect vScrollRect = new Rect(10, 20, 380, 175);
    Rect backButton = new Rect(5, 3, 20, 13);
    Rect closeButton = new Rect(385, 3, 20, 13);
    Rect fullButton = new Rect(365, 3, 20, 13);
    Rect lockrect = new Rect(345, 3, 20, 13);

    Rect windowRect = new Rect(Screen.width - 420, margin, 400, 200);
    Rect logRect = new Rect(Screen.width - 420,margin, 400, 200);

    Rect titleBarRect = new Rect(0, 0, 10000, 20);

    GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
    GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");

    private void Awake()
    {
        style.normal.textColor = Color.white;
        style.fontStyle = FontStyle.BoldAndItalic;

        le = Lev.Root;
        GetClassStr();
        Resolution();
        TotalObjectCount(out totalObj);
    }

    private void Update()
    {
        if (Input.GetKeyDown(OpenCloseKey))
        {
            isShowWindow = !isShowWindow;
            isShowLog = !isShowLog;
        }

        if (isFullScreen)
        {
            windowRect = new Rect(windowRect.x, windowRect.y, 400, 450 + (valueData.Count * 6));
            logRect = new Rect(windowRect.x - 400, windowRect.y, 400, 600);
        }
        else
        {
            windowRect = new Rect(windowRect.x, windowRect.y, 400, 200);
            logRect = new Rect(windowRect.x - 400, windowRect.y, 400, 210);
        }
        
        Framelate(out targetFrame);
        GetMemorySize(out AcquisitionHeapMemorySizeory, out TotalMemoryCapacity);
    }

    private void OnGUI()
    {
        if (!isShowWindow)
            return;

        windowRect = GUILayout.Window(123456, windowRect, DebuggerWindow, TitleChanger());
        logRect = GUILayout.Window(12345, logRect, Logs, "Log");
    }

    private void Logs(int ID)
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button(clearLabel, GUILayout.ExpandWidth(false)))
            logs.Clear();

        isCollapse = GUILayout.Toggle(isCollapse, collapseLabel, "Button", GUILayout.ExpandWidth(false));

        GUILayout.EndHorizontal();

        GUILayout.Label(HorizontalStick);

        scrollPos = GUILayout.BeginScrollView(scrollPos);

        for (var i = 0; i < logs.Count; i++)
        {
            var log = logs[i];

            if (isCollapse)
            {
                var messageSameAsPrevious = i > 0 && log.message == logs[i - 1].message;

                if (messageSameAsPrevious)
                {
                    continue;
                }
            }

            GUI.contentColor = logTypeColors[log.type];
            GUILayout.Label(log.message);
        }

        GUILayout.EndScrollView();

        GUI.contentColor = Color.white;

        GUILayout.EndVertical();
    }

    private void DebuggerWindow(int ID)
    {
        if (GUI.Button(backButton, "➡")) 
            le = Lev.Root;

        if (GUI.Button(closeButton, "×"))
        {
            isShowWindow = false;
            isShowLog = false;
        }

        if (GUI.Button(fullButton, "■")) 
            isFullScreen = !isFullScreen;

        if (GUI.Button(lockrect, "●")) 
            isLockWindow = !isLockWindow;

        if (!isFullScreen)
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(390), GUILayout.Height(175));
        else
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(390));


        switch (le)
        {
            case Lev.Root:
            {
                GUILayout.Label($"解像度 : {resolution_info.PadLeft(71)}");
                GUILayout.Label($"リフレッシュレート : {refleshRate.PadLeft(55)}");
                if (GUILayout.Button($"デバイス情報 >>", "Label"))
                {
                    le = Lev.Setting;
                }

                GUILayout.Label(HorizontalStick);


                foreach (var classdata in classData)
                {
                    if (GUILayout.Button(classdata + " >>", "Label"))
                    {
                        possession = classdata;
                        le = Lev.Manager;
                    }
                }

                GUILayout.Label(HorizontalStick);

                GUILayout.Label($"ターゲットFPS : {targetFrame.ToString().PadLeft(65)}");
                GUILayout.Label($"FPS : {fps.ToString("F1").PadLeft(79)} ");
                GUILayout.Label($"タイムスケール : {Time.timeScale.ToString().PadLeft(65)}");
                GUILayout.Label(HorizontalStick);

                GUILayout.Label($"全オブジェクト総数 :{totalObj.ToString().PadLeft(58)}");
                GUILayout.Label($"GC Count : {GC.CollectionCount(0).ToString().PadLeft(71)}");
                GUILayout.Label($"ヒープメモリサイズ[MB] : {AcquisitionHeapMemorySizeory.ToString().PadLeft(49)}");
                GUILayout.Label($"メモリサイズ[MB] : {TotalMemoryCapacity.ToString().PadLeft(58)}");
                var Percent = (float)AcquisitionHeapMemorySizeory / TotalMemoryCapacity * 100;
                GUILayout.Label($"使用割合[%] : {Percent.ToString("F1").PadLeft(68)}");
                GUILayout.Label(HorizontalStick);

                if (GUILayout.Button("<Edit>","Box"))
                {
                    le = Lev.Edit;
                }

                break;
            }
            case Lev.Setting:
                GUILayout.Label("-System Information-", style);
                ScreenSetting();
                GUILayout.Label(HorizontalStick);
                GUILayout.Label("-Audio-", style);
                AudioConfiguration();
                break;
            case Lev.Manager:
            {
                foreach(var s in this[possession])
                {
                    GUILayout.Label(s.Remove(0, possession.Length));
                }
                GUILayout.Label(HorizontalStick);
                break;
            }
            case Lev.Edit:
                GUILayout.Label($"Edit");
                break;
        }

        GUILayout.EndScrollView();

        if (!isLockWindow)
            GUI.DragWindow(titleBarRect);
    }

    private string TitleChanger()
    {
        var s = "";
        switch (le)
        {
            case Lev.Root:
                s = "Root".PadRight(86);
                break;
            
            case Lev.Setting:
                s = "Root > System".PadRight(80);
                break;
            
            case Lev.Manager:
                s = "Root > Manager".PadRight(79);
                break;
            
            case Lev.Edit:
                s = "Root > Edit".PadRight(84);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }

        return s;
    }

    void GetClassStr()
    {
        //アセンブリ内のクラス/メソッド/フィールドを列挙する感じのやつ
/*
        var asm = Assembly.GetExecutingAssembly();
        var ts = asm.GetTypes();

        foreach (Type t in ts)
        {
            if (t.Name.Contains(CharacterToCollect))
            {
                var members = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic |BindingFlags.Instance | BindingFlags.Static |BindingFlags.DeclaredOnly);
                object temp = Activator.CreateInstance(t);

                foreach (FieldInfo field in members)
                {
                    if (field.MemberType == MemberTypes.Field)
                    {
                        valueData.Add($"{t.Name}{ObjectNames.NicifyVariableName(field.Name)} => {field.GetValue(temp)}");

                        if (!classData.Contains(t.Name))
                        {
                            classData.Add(t.Name);
                        }

                        //Debug.Log($"Class : {t.Name} Field : {field.Name} => {field.GetValue(temp)}");
                    }

                    

                }

            }
        }
        
*/
    }

    private void OnEnable() => Application.logMessageReceived += HandleLog;
    private void OnDisable() => Application.logMessageReceived += null;

    void HandleLog(string message, string stackTrace, LogType type)
    {
        logs.Add(new Log()
        {
            message = message,
            stackTrace = stackTrace,
            type = type,
        });
    }

    private IEnumerable<string> this[string ClassName] => valueData.FindAll(x => x.Contains(ClassName));

    /*!
 *@param[in]	mem1	:	取得ヒープメモリサイズ (MB)
 *@param[in]	mem2	:	全メモリ容量 (MB)
 */
    void GetMemorySize(out int mem1, out int mem2)
    {
        int mem3;

        // 使用可能メモリサイズ取得
        mem2 = SystemInfo.systemMemorySize;

        // 取得済みヒープサイズ取得
        mem3 = (int)Profiler.usedHeapSizeLong / (1024 * 1024);

        if (m_HeapSizeMax1 < mem3)
        {
            m_HeapSizeMax1 = mem3;
        }

        m_Frame++;

        // 取得ヒープサイズ更新
        if (m_Frame > CHK_FRAME)
        {
            m_Frame = 0;
            m_HeapSizeMax2 = m_HeapSizeMax1;
            m_HeapSizeMax1 = 0;
        }

        mem1 = m_HeapSizeMax2;
    }

    static void TotalObjectCount(out int total)
    {
        total = FindObjectsOfType(typeof(GameObject)).Cast<GameObject>().Count(obj => obj.activeInHierarchy);
    }

    void Framelate(out int targetFramelate)
    {
        targetFramelate = Application.targetFrameRate;

        ++m_frame_counts;
        float currentTime = Time.time;
        float diffTime = currentTime - m_start_time;

        if (diffTime >= 1.0f)
        {
            fps = m_frame_counts / diffTime;
            m_start_time = currentTime;
            m_frame_counts = 0;
            gameObject.transform.SetAsLastSibling(); 
        }
    }

    void Resolution()
    {
        var reso = Screen.currentResolution;
        resolution_info = string.Format($"{reso.width.ToString()} x {reso.height.ToString()}");
        refleshRate = string.Format($"{reso.refreshRate.ToString()} Hz");
    }

    void ScreenSetting()
    {
        GUILayout.Label($"CPU : {SystemInfo.processorType}");
        GUILayout.Label($"OS : {SystemInfo.operatingSystem}");
        GUILayout.Label($"Core : {SystemInfo.processorCount.ToString()}");
        GUILayout.Label(HorizontalStick);
        GUILayout.Label($"GPU : {SystemInfo.graphicsDeviceName}");
        GUILayout.Label($"MemorySize : {SystemInfo.graphicsMemorySize.ToString()} MB");
        GUILayout.Label($"API : {SystemInfo.graphicsDeviceType}");

    }

    void AudioConfiguration()
    {
        AudioSettings.GetDSPBufferSize(out _, out var numBuffers);
        AudioConfiguration config = AudioSettings.GetConfiguration();

        string temp = $"Frequency : {config.sampleRate:#,#}Hz";
        GUILayout.Label(temp);
        GUILayout.Label($"Mode : {config.speakerMode.ToString()}");
        GUILayout.Label($"Samples : {config.dspBufferSize.ToString()}");
        GUILayout.Label($"Buffers : {numBuffers.ToString()}");
    }
}
}