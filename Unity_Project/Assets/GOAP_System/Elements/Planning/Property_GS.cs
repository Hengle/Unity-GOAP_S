﻿using UnityEngine;
using GOAP_S.Tools;

namespace GOAP_S.Planning
{
    public class Property_GS //: ISerializationCallbackReceiver
    {
        //Content fields
        [SerializeField] private VariableType _variable_type = VariableType._undefined_var_type; //Variable type we are working with in this condition
        [SerializeField] private string _A_key = null; //A Property key, the key of the first property in the condition
        [SerializeField] private OperatorType _operator = OperatorType._undefined_operator; //Condition operator
        [SerializeField] private string _B_key = null; //B Property key, the key of the second property in the condition
        [SerializeField] private object _value = null; //B value, value that we compare with the A Property value

        //Constructors
        public Property_GS()
        {

        }

        //Used to generate properties form bb variables
        public Property_GS(string new_key, VariableType new_type, OperatorType new_operator, object new_value)
        {
            //Set property fields
            _A_key = new_key;
            _variable_type = new_type;
            _operator = new_operator;
            _value = new_value;
        }

        //Planning Methods ============
        public bool Check(Property_GS property)
        {
            //Accept different operators depending of the variable type
            switch(_variable_type)
            {
                case VariableType._bool:
                case VariableType._string:
                    {

                    }
                    break;
                    //case VariableType.
            }
            //Do different operations depending of the operator type
            switch (_operator)
            {
                case OperatorType._equal: return _value == property.value;
                case OperatorType._different: return _value != property.value;
            }

            //In non valid operator type return false
            return false;
        }

        //Get/Set Methods =============
        public VariableType variable_type
        {
            get
            {
                return _variable_type;
            }
            set
            {
                _variable_type = value;
            }
        }

        public string A_key
        {
            get
            {
                return _A_key;
            }
            set
            {
                _A_key = value;
            }
        }

        public string B_key
        {
            get
            {
                return _B_key;
            }
            set
            {
                _B_key = value;
            }
        }

        public OperatorType operator_type
        {
            get
            {
                return _operator;
            }
            set
            {
                _operator = value;
            }
        }

        public object value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public string display_value
        {
            get
            {
                if (!string.IsNullOrEmpty(_B_key))
                {
                    return _B_key;
                }
                else return _value.ToString();
            }
        }
    }
}