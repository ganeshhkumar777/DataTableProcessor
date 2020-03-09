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
        public  Func<string,string , bool> validator{get; set;}
        public string ErrorMessage{get; set;}
        public bool continueWhenValidationFails{get; set;}
        public string additionalColumn{get; set;}
            public _Validator(Func<string,string, bool> validator,string additionalColumn,string ErrorMessages,bool continueWhenValidationFails){
           // new Func<string,string,bool>((x,y)=>validator(x));
               this.validator = validator;
            ErrorMessage=ErrorMessages;
            this.continueWhenValidationFails=continueWhenValidationFails;
            this.additionalColumn=additionalColumn;
            }
    }

     class _ValidatorWithParams<T> {
        public Func<T,string, string, bool> validator {get; set;}        
        public string ErrorMessage{get; set;}
        public bool continueWhenValidationFails{get; set;}
        public T MasterData{get;set;}
        public string additionalColumn{get; set;}
            public _ValidatorWithParams (Func<T,string, string,bool> validator,string additionalColumn,string ErrorMessages, T masterData,bool continueWhenValidationFails){
            this.validator=validator;    
            ErrorMessage=ErrorMessages;
            MasterData=masterData;
            this.continueWhenValidationFails=continueWhenValidationFails;
            this.additionalColumn=additionalColumn;
            }
    }
    class _Manipulator<ManipulatorResultType>{
        public  Func<string,string,ManipulatorResultType> Manipulator{get; set;}
        public string ColumnToStoreResult{ get; set; }
           public string additionalColumn{get; set;}
            public _Manipulator(Func<string,string,ManipulatorResultType> Manipulator,string additionalColumn,string ColumnToStoreResult){
            this.Manipulator=Manipulator;
            this.ColumnToStoreResult=ColumnToStoreResult;    
            this.additionalColumn=additionalColumn;
            }
    }

    class _ManipulatorWithParams<ResultType,ParameterType>{
        public Func<ParameterType,string,string,ResultType> Manipulator {get; set;}   
        public string ColumnToStoreResult{ get; set; }     
        public ParameterType MasterData{get;set;}
        public string additionalColumn{get; set;}
            public _ManipulatorWithParams (Func<ParameterType,string,string,ResultType> Manipulator,ParameterType masterData,string additionalColumn,string ColumnToStoreResult){
            this.Manipulator=Manipulator;    
            MasterData=masterData;
            this.ColumnToStoreResult=ColumnToStoreResult; 
            this.additionalColumn=additionalColumn;
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
        public ValidatorConfig(AbstractProcessorConfig input, Func<string,string, bool> validator, string additionalColumn,string errorMessage=null, bool continueWhenValidationFails=false){
            if(errorMessage==null){
                errorMessage=string.Format(ErrorMessages.DefaultInvalidColumn,input.ColumnNameToRefer);
            }
            if(input.Validator==null){
                input.Validator=new Queue<_Validator>();
            }
                input.Validator.Enqueue(new _Validator(validator,additionalColumn,errorMessage,continueWhenValidationFails));
                input.Queue.Enqueue(DataTableOperations.Validator);
        }
        public ValidatorConfig(AbstractProcessorConfig input, Func<string, bool> validator, string errorMessage=null, bool continueWhenValidationFails=false){
            new ValidatorConfig(input,new Func<string,string,bool>((x,y)=>validator(x)),null,errorMessage,continueWhenValidationFails);
        }
    }

    internal class ValidatorWithParamsConfig<T> : AbstractProcessorConfig{
        public ValidatorWithParamsConfig(AbstractProcessorConfig input, Func<T,string, string,bool> validator,T masterData,string additionalColumn, string errorMessage=null,bool continueWhenValidationFails=false){
            if(errorMessage==null){
                errorMessage=string.Format(ErrorMessages.DefaultInvalidColumn,input.ColumnNameToRefer);
            }
            if(input.ValidatorWithParams==null){
                input.ValidatorWithParams=new Queue<_ValidatorWithParams<object>>();
            }
            var temp=new Func<object,string,string,bool>((y,z,a)=>validator((T)y,z,a));
                input.ValidatorWithParams.Enqueue(new _ValidatorWithParams<object>(temp,additionalColumn,errorMessage,masterData,continueWhenValidationFails));
                input.Queue.Enqueue(DataTableOperations.ValidatorWithParams);
        }

        public ValidatorWithParamsConfig(AbstractProcessorConfig input, Func<T,string, bool> validator,T masterData, string errorMessage=null,bool continueWhenValidationFails=false){
            new ValidatorWithParamsConfig<T>(input, new Func<T, string, string, bool> ( (x,y,z) =>validator(x,y)),masterData,null,errorMessage,continueWhenValidationFails);
        }
    }

    internal class ManipulatorConfig<ManipulatorResultType> : AbstractProcessorConfig{
        public ManipulatorConfig(AbstractProcessorConfig input, Func<string,string,ManipulatorResultType> Manipulator,string additionalColumn,string ColumnToStoreResult=null){
            if(input.Manipulators==null){
                input.Manipulators=new Queue<_Manipulator<object>>();
            }
            var temp=new Func<string,string,object>((y,z)=>Manipulator(y,z));
            var ins =new _Manipulator<object>(temp,additionalColumn,ColumnToStoreResult);
            input.Manipulators.Enqueue(ins );
            input.Queue.Enqueue(DataTableOperations.Manipulator);
        }

        public ManipulatorConfig(AbstractProcessorConfig input, Func<string,ManipulatorResultType> Manipulator,string ColumnToStoreResult=null){
           new ManipulatorConfig<ManipulatorResultType>(input, new Func<string,string,ManipulatorResultType>((x,y)=>Manipulator(x)),null,ColumnToStoreResult);
        }
    }
    
    public class ManipulatorWithParamsConfig<ResultType,ParameterType> : AbstractProcessorConfig{
        public ManipulatorWithParamsConfig(AbstractProcessorConfig input, Func<ParameterType,string,string,ResultType> Manipulator, ParameterType MasterData,string additionalColumn, string ColumnToStoreResult){
            

            if(input.ManipulatorWithParams==null){
                input.ManipulatorWithParams=new Queue<_ManipulatorWithParams<object, object>>();
            }
            var temp= new Func<object,string,string,object>((y,z,a)=> Manipulator((ParameterType)y ,z,a));
            var ins = new _ManipulatorWithParams<object,object>(temp,MasterData,additionalColumn,ColumnToStoreResult);
            input.ManipulatorWithParams.Enqueue(ins);
            input.Queue.Enqueue(DataTableOperations.ManipulatorWithParams);

        }

        public ManipulatorWithParamsConfig(AbstractProcessorConfig input, Func<ParameterType,string,ResultType> Manipulator, ParameterType MasterData,string ColumnToStoreResult){
            
            new ManipulatorWithParamsConfig<ResultType,ParameterType>(input, new Func<ParameterType,string,string,ResultType>((x,y,z)=>Manipulator(x,y)),MasterData,null,ColumnToStoreResult);

        }
    }
}
