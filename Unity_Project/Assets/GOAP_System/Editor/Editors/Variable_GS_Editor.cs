﻿using UnityEngine;
using UnityEditor;
using GOAP_S.Blackboard;
using GOAP_S.PT;
using System.Reflection;

namespace GOAP_S.UI
{
    public class Variable_GS_Editor
    {
        //Content fields
        private  Variable_GS _target_variable = null;
        private Blackboard_GS _target_blackboard = null;
        //State fields
        private int _selected_property_index = -1;
        PropertyInfo[] _properties_info = null;
        string[] _properties_paths = null;
        private EditorUIMode _UI_mode = EditorUIMode.SET_STATE;
        private string new_name = null;

        //Constructors ====================
        public Variable_GS_Editor(Variable_GS new_variable, Blackboard_GS new_bb)
        {
            //Set target variable
            _target_variable = new_variable;
            //Set target bb
            _target_blackboard = new_bb;
        }

        //Loop Methods ====================
        public void DrawUI()
        {
            //Draw variable in edit mode
            if (_UI_mode == EditorUIMode.EDIT_STATE)
            {
                DrawInEditMode();
            }
            //Draw variable in show mode
            else
            {
                DrawInShow();
            }
        }

        private void DrawInShow()
        {
            GUILayout.BeginHorizontal();

            //Edit button, swap between edit and show state
            if (!Application.isPlaying)
            {
                if (GUILayout.Button("O", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    //Change state
                    _UI_mode = EditorUIMode.EDIT_STATE;
                    //Set input focus to null
                    GUI.FocusControl("null");
                    //Set new name string
                    new_name = _target_variable.name;
                }
            }

            //Show variable type
            GUILayout.Label(_target_variable.type.ToString().Replace('_',' '), UIConfig_GS.Instance.left_bold_style, GUILayout.MaxWidth(60.0f));

            //Show variable name
            GUILayout.Label(_target_variable.name, GUILayout.MaxWidth(80.0f));

            //Show variable value
            //Non binded variable case
            if (!_target_variable.is_binded)
            {
                //Get variable value
                object value = _target_variable.object_value;
                //Generate UI field from type
                ProTools.ValueFieldByVariableType(_target_variable.type, ref value);
                //If value is different we update the variable value
                if (value != _target_variable.object_value)
                {
                    _target_variable.object_value = value;
                };
            }
            //Binded variable case
            else
            {
                //If variables is binded we show the field inheritance path
                GUILayout.Label(_target_variable.display_field_short_path, GUILayout.MaxWidth(90.0f));
            }

            //Free space margin
            GUILayout.FlexibleSpace();

            //Remove button
            if (!Application.isPlaying)
            {
                if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    //Add remove the current var method to accept menu delegates callback
                    SecurityAcceptMenu_GS.on_accept_delegate += () => _target_blackboard.RemoveVariable(_target_variable.name);
                    //Add remove current var editor from blackboard editor to accept menu delegates calback
                    SecurityAcceptMenu_GS.on_accept_delegate += () => NodeEditor_GS.Instance.blackboard_editor.DeleteVariableEditor(_target_variable.name);
                    //Get mouse current position
                    Vector2 mousePos = Event.current.mousePosition;
                    //Open security accept menu on mouse position
                    PopupWindow.Show(new Rect(mousePos.x, mousePos.y, 0, 0), new SecurityAcceptMenu_GS());
                }
            }

            GUILayout.EndHorizontal();
        }

        private void DrawInEditMode()
        {
            GUILayout.BeginHorizontal();

            //Edit button, swap between edit and show state
            if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20)) || (Application.isPlaying && _UI_mode == EditorUIMode.EDIT_STATE))
            {
                //Change state
                _UI_mode = EditorUIMode.SET_STATE;
                //Set input focus to null
                GUI.FocusControl("null");
                //Check name change
                if(string.Compare(new_name,_target_variable.name) != 0)
                {
                    //Repeated name case
                    if (_target_blackboard.variables.ContainsKey(new_name))
                    {
                        Debug.LogWarning("The name '" + new_name + "' already exists!");
                    }
                    //New name case
                    else
                    {
                        //Change key in the dictionary, the variable is now stored with the new key
                        _target_blackboard.variables.RenameKey(_target_variable.name, new_name);
                        //Change variable name
                        _target_variable.name = new_name;
                    }
                }
            }

            //Show variable type
            GUILayout.Label(_target_variable.type.ToString().Replace('_', ' '), UIConfig_GS.Instance.left_bold_style, GUILayout.MaxWidth(60.0f));

            //Edit variable name
            new_name = EditorGUILayout.TextField(new_name, GUILayout.MaxWidth(80.0f));

            //Show variable value
            //Non binded variable case
            if (!_target_variable.is_binded)
            {
                //Get variable value
                object value = _target_variable.object_value;
                //Generate UI field from type
                ProTools.ValueFieldByVariableType(_target_variable.type, ref value);
                //If value is different we update the variable value
                if (value != _target_variable.object_value)
                {
                    _target_variable.object_value = value;
                }
            }

            ShowBindOptions();
            

            GUILayout.EndHorizontal();
        }

        private void ShowBindOptions()
        {
            //Bind variable
            GUILayout.BeginHorizontal();

            if (!_target_variable.is_binded)
            {
                //Free space margin
                GUILayout.FlexibleSpace();

                //Show bind selection dropdown
                if (GUILayout.Button("Bind",GUILayout.MaxWidth(40.0f)))
                {
                    //Get all the fields in the agent gameobject
                    _properties_info = ProTools.FindConcretePropertiesInGameObject(NodeEditor_GS.Instance.selected_agent.gameObject, ProTools.VariableTypeToSystemType(target_variable.type));
                    //Get all the paths of the fields found
                    _properties_paths = new string[_properties_info.Length];
                    int index = 0;
                    foreach (PropertyInfo field_info in _properties_info)
                    {
                        //_properties_paths[index] = _properties_info[index].DeclaringType + "/" + _properties_info[index].PropertyType + "/" +  _properties_info[index].Name;// _properties_info[index].Name;
                        _properties_paths[index] = string.Format("{0}.{1}", field_info.ReflectedType.FullName, field_info.Name);
                        // _properties_paths[index] = _properties_paths[index].Replace('.', '/');
                        index += 1;
                    }

                    GenericMenu dropdown = new GenericMenu();
                    for (int k = 0; k < _properties_paths.Length; k++)
                    {
                        //First adapt path to the dropdown format
                        string ui_adapted_path = _properties_paths[k].Replace('.', '/');

                        dropdown.AddItem(
                            //Generate gui content from property path strin
                            new GUIContent(ui_adapted_path),
                            //show the currently selected item as selected
                            k == _selected_property_index,
                            //lambda to set the selected item to the one being clicked
                            selectedIndex => _selected_property_index = (int)selectedIndex,
                            //index of this menu item, passed on to the lambda when pressed.
                            k
                       );
                    }
                    dropdown.ShowAsContext(); //finally show the dropdown
                }
            }
            else
            {
                //Show current bind
                GUILayout.Label(_target_variable.display_field_short_path,GUILayout.MaxWidth(90.0f));

                //Free space margin
                GUILayout.FlexibleSpace();

                //UnBind button
                if (GUILayout.Button("UnBind",GUILayout.MaxWidth(50.0f)))
                {
                    //Unbind varaible resetting  getter/setter and field path
                    _target_variable.UnbindField();
                    //Reset path index in the dropdown
                    _selected_property_index = -1;
                }
            }
            GUILayout.EndHorizontal();

            //Check if the variable has been binded, so a path index has been set
            if (!target_variable.is_binded && _selected_property_index != -1)
            {
                //Bind variable to the new field using the selected path
                EditorApplication.delayCall += () => _target_variable.BindField(_properties_paths[_selected_property_index], null);
            }
        }

        //Get/Set Methods =================
        public Variable_GS target_variable
        {
            get
            {
                return _target_variable;
            }
            set
            {
                _target_variable = value;
            }
        }

        public Blackboard_GS target_blackboard
        {
            get
            {
                return _target_blackboard;
            }
            set
            {
                _target_blackboard = value;
            }
        }
    }
}
