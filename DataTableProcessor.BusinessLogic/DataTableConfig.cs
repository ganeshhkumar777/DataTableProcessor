using System;
using System.Collections.Generic;
using DataTableProcessor;
namespace DataTableProcessorConfig
{
    public abstract class AbstractProcessorConfig
    {
        internal string ExcelColumnName{get; set;}
        internal string ColumnNameToRefer{get;set;}
        internal Queue<string> Queue{get; set;}
        internal Queue<_Renamer> Renamer{get; set;}
        internal Queue<_Validator> Validator{get;set;}
        internal Queue<_ValidatorWithParams<object>> ValidatorWithParams{get;set;}

        internal Queue<_Manipulator<object>> Manipulators{get; set;}

        internal Queue<_ManipulatorWithParams<object,object>> ManipulatorWithParams{get; set;}

        
    }
    internal class ProcessorConfig: AbstractProcessorConfig{

        public ProcessorConfig(string ExcelColumnName){
            this.ExcelColumnName=ExcelColumnName;
            this.ColumnNameToRefer=ExcelColumnName;
            this.Queue=new Queue<string>();
        }

    }

     class _Renamer {
            public string ActualColumnName{get; set;}
            public _Renamer(string name){
                ActualColumnName = name;
            }
    }

     class _Validator {
        public  Func<string, bool> validator{get; set;}
        public string ErrorMessage{get; set;}
        public bool continueWhenValidationFails{get; set;}
            public _Validator(Func<string, bool> validator,string ErrorMessages,bool continueWhenValidationFails){
            this.validator=validator;    
            ErrorMessage=ErrorMessages;
            this.continueWhenValidationFails=continueWhenValidationFails;
            }
    }

     class _ValidatorWithParams<T> {
        public Func<T,string, bool> validator {get; set;}        
        public string ErrorMessage{get; set;}
        public bool continueWhenValidationFails{get; set;}
        public T MasterData{get;set;}
            public _ValidatorWithParams (Func<T,string, bool> validator,string ErrorMessages, T masterData,bool continueWhenValidationFails){
            this.validator=validator;    
            ErrorMessage=ErrorMessages;
            MasterData=masterData;
            this.continueWhenValidationFails=continueWhenValidationFails;
            }
    }
    class _Manipulator<ManipulatorResultType>{
        public  Func<string,ManipulatorResultType> Manipulator{get; set;}
            public _Manipulator(Func<string,ManipulatorResultType> Manipulator){
            this.Manipulator=Manipulator;
            }
    }

    class _ManipulatorWithParams<ResultType,ParameterType>{
        public Func<ParameterType,string,ResultType> Manipulator {get; set;}        
        public ParameterType MasterData{get;set;}
            public _ManipulatorWithParams (Func<ParameterType,string,ResultType> Manipulator,ParameterType masterData){
            this.Manipulator=Manipulator;    
            MasterData=masterData;
            }
    }
    internal class RenamerConfig:AbstractProcessorConfig {
        public RenamerConfig(AbstractProcessorConfig input, string ActualColumnName) {
            if(input.Renamer==null){
                input.Renamer=new Queue<_Renamer>();
            }
                input.Renamer.Enqueue(new _Renamer(ActualColumnName));
                input.Queue.Enqueue(DataTableOperations.Renamer);
        }
    }
    
    internal class ValidatorConfig : AbstractProcessorConfig{
        public ValidatorConfig(AbstractProcessorConfig input, Func<string, bool> validator, string errorMessage=null, bool continueWhenValidationFails=false){
            if(errorMessage==null){
                errorMessage=string.Format(ErrorMessages.DefaultInvalidColumn,input.ColumnNameToRefer);
            }
            if(input.Validator==null){
                input.Validator=new Queue<_Validator>();
            }
                input.Validator.Enqueue(new _Validator(validator,errorMessage,continueWhenValidationFails));
                input.Queue.Enqueue(DataTableOperations.Validator);
        }
    }

    internal class ValidatorWithParamsConfig<T> : AbstractProcessorConfig{
        public ValidatorWithParamsConfig(AbstractProcessorConfig input, Func<T,string, bool> validator,T masterData, string errorMessage=null,bool continueWhenValidationFails=false){
            if(errorMessage==null){
                errorMessage=string.Format(ErrorMessages.DefaultInvalidColumn,input.ColumnNameToRefer);
            }
            if(input.ValidatorWithParams==null){
                input.ValidatorWithParams=new Queue<_ValidatorWithParams<object>>();
            }
            var temp=new Func<object,string,bool>((y,z)=>validator((T)y,z));
                input.ValidatorWithParams.Enqueue(new _ValidatorWithParams<object>(temp,errorMessage,masterData,continueWhenValidationFails));
                input.Queue.Enqueue(DataTableOperations.ValidatorWithParams);
        }
    }

    internal class ManipulatorConfig<ManipulatorResultType> : AbstractProcessorConfig{
        public ManipulatorConfig(AbstractProcessorConfig input, Func<string,ManipulatorResultType> Manipulator){
            if(input.Manipulators==null){
                input.Manipulators=new Queue<_Manipulator<object>>();
            }
            var temp=new Func<string,object>(y=>Manipulator(y));
            var ins =new _Manipulator<object>(temp);
            input.Manipulators.Enqueue(ins );
            input.Queue.Enqueue(DataTableOperations.Manipulator);
        }
    }
    
    public class ManipulatorWithParamsConfig<ResultType,ParameterType> : AbstractProcessorConfig{
        public ManipulatorWithParamsConfig(AbstractProcessorConfig input, Func<ParameterType,string,ResultType> Manipulator, ParameterType MasterData){
            

            if(input.ManipulatorWithParams==null){
                input.ManipulatorWithParams=new Queue<_ManipulatorWithParams<object, object>>();
            }
            var temp= new Func<object,string,object>((y,z)=> Manipulator((ParameterType)y ,z));
            var ins = new _ManipulatorWithParams<object,object>(temp,MasterData);
            input.ManipulatorWithParams.Enqueue(ins);
            input.Queue.Enqueue(DataTableOperations.ManipulatorWithParams);

        }
    }
}
