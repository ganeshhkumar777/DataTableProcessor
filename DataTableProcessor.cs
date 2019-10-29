using System;
using System.Collections.Generic;
namespace DataTableProcessor
{
    public abstract class ProcessorConfig
    {
        public string ExcelColumnName{get; set;}
        public string ColumnNameToRefer{get;set;}
        public Queue<string> Queue{get; set;}
        public Queue<_Renamer> Renamer{get; set;}
        public Queue<_Validator> Validator{get;set;}
    }
    public abstract class DataTableProcessor: ProcessorConfig{

        public DataTableProcessor(string ExcelColumnName){
            this.ExcelColumnName=ExcelColumnName;
            this.ColumnNameToRefer=ExcelColumnName;
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
            public _Validator(Func<string, bool> validator){
            this.validator=validator;    
            }
    }

    public class Renamer:ProcessorConfig {
        public Renamer(ProcessorConfig input, string ActualColumnName) {
            if(input.Renamer==null){
                input.Renamer=new Queue<_Renamer>();
            }
                input.Renamer.Enqueue(new _Renamer(ActualColumnName));
                input.Queue.Enqueue("Renamer");
        }
    }
    public class Validator : ProcessorConfig{
        public Validator(ProcessorConfig input, Func<string, bool> validator){
            if(input.Validator==null){
                input.Validator=new Queue<_Validator>();
            }
                input.Validator.Enqueue(new _Validator(validator));
                input.Queue.Enqueue("Validator");
        }
    }

    

}
