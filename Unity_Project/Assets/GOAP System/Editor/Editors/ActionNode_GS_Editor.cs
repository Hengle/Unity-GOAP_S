﻿using UnityEngine;
using UnityEditor;
using GOAP_S.PT;

//Class used to draw action nodes in the node editor and handle input
public class ActionNode_GS_Editor {

    //Content fields
    private NodeEditor_GS _target_editor = null;
    private ActionNode_GS _target_node = null;
    private Action_GS _target_action = null;
    //UI fields
    static private int initial_separation = 0; //Used to define the separation between the first ui element and the window title
    static private int parts_separation = 0; //Used to define the separation between the diferent action node main elements (condition,action,reward)
    static private int mark_separation = 0; //Used to define the separation between the elements and the window lateral sides
    private string name_str = ""; //Used in edit state to allocate the new name
    private string description_str = ""; //Used in edit state to allocate the new description

    //Constructor =====================
    public ActionNode_GS_Editor(ActionNode_GS new_target, NodeEditor_GS new_editor)
    {
        //Set targets
        _target_editor = new_editor;
        _target_node = new_target;
        if (_target_node != null)
        {
            _target_action = _target_node.action;
        }

        //Set UI values
        parts_separation = 10;
        initial_separation = 30;
        mark_separation = 10;
    }

    //Loop Methods ====================
    public void DrawUI(int id)
    {
        switch (_target_node.UImode)
        {
            case NodeUIMode.EDIT_STATE:
                //Draw window in edit state
                DrawNodeWindowEditState();
                break;

            case NodeUIMode.SET_STATE:
                //Draw window in set state
                DrawNodeWindowSetState();
                break;
        }
    }

    private void DrawNodeWindowEditState()
    {
        //Node name text field
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name");
        _target_node.name = GUILayout.TextField(_target_node.name, GUILayout.Width(90), GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();

        //Node description text field
        GUILayout.BeginVertical();
        GUILayout.Label("Description");
        _target_node.description = GUILayout.TextArea(_target_node.description, GUILayout.Width(40), GUILayout.Height(100), GUILayout.ExpandWidth(true));

        //Close edit mode
        if (GUILayout.Button(
            "Close",
            _target_editor.UI_configuration.node_modify_button_style,
            GUILayout.Width(120), GUILayout.ExpandWidth(true)))
        {
            _target_node.UImode = NodeUIMode.SET_STATE;
        }

        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    private void DrawNodeWindowSetState()
    { 
        GUILayout.BeginHorizontal();
        //Edit
        if (GUILayout.Button(
            "Edit",
            _target_editor.UI_configuration.node_modify_button_style,
            GUILayout.Width(30),GUILayout.ExpandWidth(true)))
        {
            //Set edit state
            _target_node.UImode = NodeUIMode.EDIT_STATE;
        }
        //Delete
        if (GUILayout.Button(
            "Delete",
            _target_editor.UI_configuration.node_modify_button_style,
            GUILayout.Width(30),GUILayout.ExpandWidth(true)))
        {
            _target_editor.selected_agent.DeleteActionNode(_target_node);
        }
        GUILayout.EndHorizontal();

        //Separation
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        //Separation
        //GUILayout.Space(parts_separation);

        //Condition -------------------
        //Condition null case
        if (GUILayout.Button(
            "Select Condition",
            _target_editor.UI_configuration.node_selection_buttons_style,
            GUILayout.Width(150), GUILayout.Height(20),GUILayout.ExpandWidth(true)))
        {

        }
        //-----------------------------

        //Separation
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        //Action ----------------------
        //Action null case
        if (_target_action == null)
        {

            if (GUILayout.Button(
                "Select Action",
                _target_editor.UI_configuration.node_selection_buttons_style,
                GUILayout.Width(150), GUILayout.Height(20),
                GUILayout.ExpandWidth(true)))
            {
                Vector2 mousePos = Event.current.mousePosition;
                PopupWindow.Show(new Rect(mousePos.x, mousePos.y - 100, 0, 0), new GOAP_S.UI.ActionSelectMenu_GS(this, _target_editor));
            }
        }
        //Action set case
        else
        {
            //Action area
            GUILayout.BeginHorizontal("HelpBox");
            GUILayout.FlexibleSpace();
            GUILayout.Label(_target_node.action.name, _target_editor.UI_configuration.node_elements_style, GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            //Edit / Delete area
            GUILayout.BeginHorizontal();
            //Edit
            if (GUILayout.Button(
                "Edit",
                _target_editor.UI_configuration.node_modify_button_style,
                GUILayout.Width(30), GUILayout.ExpandWidth(true)))
            {
                //IDK what to put here but this can be deleted with no problem :v
            }
            //Delete
            if (GUILayout.Button(
                "Delete",
                _target_editor.UI_configuration.node_modify_button_style,
                GUILayout.Width(30), GUILayout.ExpandWidth(true)))
            {
                _target_node.action = null;
            }
            GUILayout.EndHorizontal();
        }
        //-----------------------------

        //Separation
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        //Reward ----------------------
        //Reward null case
        if (GUILayout.Button(
            "Select Reward",
            _target_editor.UI_configuration.node_selection_buttons_style,
            GUILayout.Width(150), GUILayout.Height(20),
            GUILayout.ExpandWidth(true)))
        {

        }
        //-----------------------------

        GUI.DragWindow();
    }

    //Get/Set Methods =================
    public void SetAction(Action_GS new_action)
    {
        //Set the new action in the target action node
        _target_node.action = new_action;
        //Repaint the node editor to update the UI
        _target_editor.Repaint();
    }
}