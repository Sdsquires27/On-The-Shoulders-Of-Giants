using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class NodeEditor : EditorWindow
{

    List<Rect> windows = new List<Rect>();
    List<string> windowSetting = new List<string>();
    List<int> windowsToAttach = new List<int>();
    List<int> attachedWindows = new List<int>();

    [MenuItem("Window/Node Editor")]
    static void ShowEditor()
    {
        NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
    }


    void OnGUI()
    {
        if (windowsToAttach.Count == 2)
        {
            attachedWindows.Add(windowsToAttach[0]);
            attachedWindows.Add(windowsToAttach[1]);
            windowsToAttach = new List<int>();
        }

        if (attachedWindows.Count >= 2)
        {
            for (int i = 0; i < attachedWindows.Count; i += 2)
            {
                DrawNodeCurve(windows[attachedWindows[i]], windows[attachedWindows[i + 1]]);
            }
        }

        BeginWindows();

        if (GUILayout.Button("Create Node"))
        {
            windows.Add(new Rect(10, 10, 100, 100));
            windowSetting.Add("Dialogue");
        }

        if (GUILayout.Button("Create if node"))
        {
            windows.Add(new Rect(10, 10, 100, 100));
            windowSetting.Add("If");
        }

        if (GUILayout.Button("Save Tree"))
        {

        }



        for (int i = 0; i < windows.Count; i++)
        {
            if (windowSetting[i] == "Dialogue")
            {
                windows[i] = GUI.Window(i, windows[i], DrawNodeWindow, "Window " + i);
            }
            else if (windowSetting[i] == "If")
            {
                windows[i] = GUI.Window(i, windows[i], DrawIfNodeWindow, "Window " + i);
            }
            else if (windowSetting[i] == "Else if")
            {
                windows[i] = GUI.Window(i, windows[i], DrawElseIfWindows, "Window " + i);
            }
            else if (windowSetting[i] == "Else")
            {
                windows[i] = GUI.Window(i, windows[i], DrawNodeWindow, "Window " + i);
            }
        }


        EndWindows();
    }

    void DrawNodeWindow(int id)
    {
        if (GUILayout.Button("Attach"))
        {
            windowsToAttach.Add(id);
        }

        if (GUILayout.Button("Remove"))
        {
            while (attachedWindows.Contains(id))
            {
                int index = attachedWindows.IndexOf(id);
                if (index % 2 == 0)
                {
                    attachedWindows.RemoveAt(index + 1);
                }
                else
                {
                    attachedWindows.RemoveAt(index - 1);
                }
                attachedWindows.Remove(id);
            }
        }

        GUI.DragWindow();
    }


    void DrawIfNodeWindow(int id)
    {
        if (GUILayout.Button("Attach"))
        {
            windowsToAttach.Add(id);
        }
        if (GUILayout.Button("Add else if"))
        {
            windowSetting.Add("Else if");
            windows.Add(new Rect(10, 10, 100, 100));
            windowsToAttach.Add(id);
            windowsToAttach.Add(windows.Count - 1);
        }
        if (GUILayout.Button("Add else"))
        {
            windowSetting.Add("Else");
            windows.Add(new Rect(10, 10, 100, 100));
            windowsToAttach.Add(id);
            windowsToAttach.Add(windows.Count - 1);
        }

        GUI.DragWindow();
    }

    void DrawElseIfWindows(int id)
    {


        GUI.DragWindow();
    }

    void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);

        for (int i = 0; i < 3; i++)
        {// Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }
}