using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.BehaviorTrees.Editor
{
    public class BehTreeGraphView : GraphView
    {
        public BehTreeGraphView()
        {
            AddManupalators();
            AddGridBackground();
            AddStyles();
        }
        
        void AddManupalators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale,ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
        }

        void AddStyles()
        {
            StyleSheet styleSheet = EditorGUIUtility.Load("BehaviorTree/BTGraphViewStyles.uss") as StyleSheet;
            styleSheets.Add(styleSheet);
        }
        
        void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            
            Insert(0,gridBackground);
        }
    }
}


