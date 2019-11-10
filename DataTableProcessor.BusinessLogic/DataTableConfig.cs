using System;
using System.Collections.Generic;
namespace DataTableProcessorConfig
{
    public abstract class AbstractProcessorConfig
    {
        public string ExcelColumnName{get; set;}
        public string ColumnNameToRefer{get;set;}
        public Queue<string> Queue{get; set;}
        public Queue<_Renamer> Renamer{get; set;}
        public Queue<_Validator> Validator{get;set;}
        public dynamic ValidatorWithParams{get;set;}

    }
    public class ProcessorConfig: AbstractProcessorConfig{

        public ProcessorConfig(string ExcelColumnName){
            this.ExcelColumnName=ExcelColumnName;
            this.ColumnNameToRefer=ExcelColumnName;
            this.Queue=new Queue<string>();
        }

    }

    public class _Renamer {
            public string ActualColumnName{get; set;}
            public _Renamer(string name){
                ActualColumnName = name;
            }
    }

    public class _Validator {
        public Func<string, bool> validator{get; set;}
        public string ErrorMessage{get; set;}
            public _Validator(Func<string, bool> validator,string ErrorMessages){
            this.validator=validator;    
            ErrorMessage=ErrorMessages;
            }
    }

public class _ValidatorWithParams<T> {
        public Func<T,string, bool> validator {get; set;}
        public string ErrorMessage{get; set;}

        public T MasterData{get;set;}
            public _ValidatorWithParams (Func<T,string, bool> validator,string ErrorMessages, T masterData){
            this.validator=validator;    
            ErrorMessage=ErrorMessages;
            MasterData=masterData;
            }
    }
    public class RenamerConfig:AbstractProcessorConfig {
        public RenamerConfig(AbstractProcessorConfig input, string ActualColumnName) {
            if(input.Renamer==null){
                input.Renamer=new Queue<_Renamer>();
            }
                input.Renamer.Enqueue(new _Renamer(ActualColumnName));
                input.Queue.Enqueue("Renamer");
        }
    }
    
    public class ValidatorConfig : AbstractProcessorConfig{
        public ValidatorConfig(AbstractProcessorConfig input, Func<string, bool> validator, string errorMessage=null){
            if(errorMessage==null){
                errorMessage=string.Format(ErrorMessages.DefaultInvalidColumn,input.ColumnNameToRefer);
            }
            if(input.Validator==null){
                input.Validator=new Queue<_Validator>();
            }
                input.Validator.Enqueue(new _Validator(validator,errorMessage));
                input.Queue.Enqueue("Validator");
        }
    }

    public class ValidatorWithParamsConfig<T> : AbstractProcessorConfig{
        public ValidatorWithParamsConfig(AbstractProcessorConfig input, Func<T,string, bool> validator,T masterData, string errorMessage=null){
            if(errorMessage==null){
                errorMessage=string.Format(ErrorMessages.DefaultInvalidColumn,input.ColumnNameToRefer);
            }
            if(input.Validator==null){
                input.ValidatorWithParams=new Queue<_ValidatorWithParams<T>>();
            }
                input.ValidatorWithParams.Enqueue(new _ValidatorWithParams<T>(validator,errorMessage,masterData));
                input.Queue.Enqueue("ValidatorWithParams");
        }
    }

    
    

}
