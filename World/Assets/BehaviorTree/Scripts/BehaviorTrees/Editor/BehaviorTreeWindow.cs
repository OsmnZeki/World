using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace Scripts.BehaviorTrees.Editor
{
    public class BehaviorTreeWindow : EditorWindow
    {

        [MenuItem("Window/Behavior Tree/Graph")]
        public static void Open()
        {
            GetWindow<BehaviorTreeWindow>("Behavior Tree Graph");
        }

        void OnEnable()
        {
            AddGraphView();
            AddStyles();
        }
        void AddStyles()
        {
            StyleSheet styleSheet = EditorGUIUtility.Load("BehaviorTree/BTVariables.uss") as StyleSheet;
            rootVisualElement.styleSheets.Add(styleSheet);
        }

        void AddGraphView()
        {
            BehTreeGraphView graphView = new BehTreeGraphView();
            graphView.StretchToParentSize();
            
            rootVisualElement.Add(graphView);
        }

    }

}

