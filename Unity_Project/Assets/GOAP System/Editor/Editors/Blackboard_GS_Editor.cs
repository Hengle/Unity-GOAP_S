﻿using UnityEngine;
using UnityEditor;
using GOAP_S.Blackboard;
using GOAP_S.PT;

namespace GOAP_S.UI
{
    public class Blackboard_GS_Editor
    {
        //UI fields
        private Rect _window = Rect.zero; //Rect used to place bb window
        //Target fileds
        private NodeEditor_GS _target_node_editor = null; //NodeEditor is this editor in
        private Blackboard_GS _target_bb = null; //Bb is this editor showing
        private Variable_GS_Editor[] _variable_editors = null;
        private int _variable_editors_num = 0;

        //Construtors =====================
        public Blackboard_GS_Editor(Blackboard_GS target_bb, NodeEditor_GS target_editor)
        {
            //Set the target bb
            _target_bb = target_bb;
            //Set the target node editor
            _target_node_editor = target_editor;
            //Allocate variable editors array
            _variable_editors = new Variable_GS_Editor[ProTools.INITIAL_ARRAY_SIZE];
            //Generate variable editors with the existing variables
            _variable_editors_num = 0;
            foreach (Variable_GS variable in target_bb.variables.Values)
            {
                //Generate a variable editor
                AddVariableEditor(variable);
            }
        }

        //Loop methods ====================
        public void DrawUI(int id)
        {
            //Update position
            if (window_position.x != _target_node_editor.position.width - ProTools.BLACKBOARD_MARGIN)
            {
                window_position = new Vector2(_target_node_editor.position.width - ProTools.BLACKBOARD_MARGIN, 0);
            }

            //Separaion between title and variables
            GUILayout.BeginVertical();
            GUILayout.Space(15);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.EndVertical();

            //Blit all the variables
            GUILayout.BeginVertical();
            GUILayout.Label("Variables", _target_node_editor.UI_configuration.blackboard_title_style);
            for(int k = 0; k <_variable_editors_num; k++)
            { 
                _variable_editors[k].DrawUI();
            }
            GUILayout.EndVertical();

            //Button to add new variables
            if (GUILayout.Button("Add", GUILayout.Width(50)))
            {
                Vector2 mousePos = Event.current.mousePosition;
                PopupWindow.Show(new Rect(mousePos.x, mousePos.y, 0, 0), new VariableSelectMenu_GS(_target_node_editor));
            }
        }

        //Functionality methods ===========
        public void AddVariableEditor(Variable_GS new_variable)
        {
            //Check if we need to allocate more items in the array
            if(_variable_editors_num == _variable_editors.Length)
            {
                //Double array capacity
                Variable_GS_Editor[] new_array = new Variable_GS_Editor[_variable_editors_num * 2];
                //Copy values
                for(int k = 0; k < _variable_editors_num; k++)
                {
                    new_array[k] = _variable_editors[k];
                }
            }

            //Generate new variable editor
            Variable_GS_Editor new_variable_editor = new Variable_GS_Editor(new_variable, _target_bb,_target_node_editor);
            //Add it to the array
            _variable_editors[_variable_editors_num] = new_variable_editor;
            //Update variable editors num
            _variable_editors_num += 1;
        }

        public void DeleteVariableEditor(Variable_GS_Editor target_variable_editor)
        {
            for (int k = 0; k < _variable_editors_num; k++)
            {
                if (_variable_editors[k] == target_variable_editor)
                {
                    if (k == _variable_editors.Length - 1)
                    {
                        _variable_editors[k] = null;
                    }
                    else
                    {
                        for (int i = k; i < _variable_editors_num - 1; i++)
                        {
                            _variable_editors[i] = _variable_editors[i + 1];
                        }
                    }
                    //Update variable editors count
                    _variable_editors_num -= 1;
                }
            }
        }

        //Get/Set methods =================
        public Rect window
        {
            get
            {
                return _window;
            }
            set
            {
                _window = value;
            }
        }

        public Vector2 window_size
        {
            get
            {
                return new Vector2(_window.width, _window.height);
            }
            set
            {
                _window.width = value.x;
                _window.height = value.y;
            }
        }

        public Vector2 window_position
        {
            get
            {
                return new Vector2(_window.x, _window.y);
            }
            set
            {
                _window.x = value.x;
                _window.y = value.y;
            }
        }

        public Blackboard_GS target_bb
        {
            get
            {
                return _target_bb;
            }
            set
            {
                _target_bb = value;
            }
        }

        public NodeEditor_GS target_editor
        {
            get
            {
                return _target_node_editor;
            }
            set
            {
                _target_node_editor = value;
            }
        }
    }
}
