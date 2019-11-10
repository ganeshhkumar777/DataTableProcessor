using System.Data;
using System.Collections.Generic;
using System.Text;
using System;
using DataTableProcessorConfig;
namespace DataTableProcessor
{
    public class Processor
    {
        public DataTableProcessorResult Process(List<AbstractProcessorConfig> configurations, DataTable dt){
            DataTableProcessorResult result=new DataTableProcessorResult();
            
            DataTable errors = CreateErrorDataTable();

            foreach(var config in configurations){

                if(dt.Columns.Contains(config.ExcelColumnName)) {

                    dt = ProcessConfig(config,dt,errors);
                }
                else {
                    errors=AddErrorRow(errors,config.ExcelColumnName,ErrorMessages.ColumnNotPresent);
                }
                
            }
            result.Result=dt;
            result.Error=errors;
            return result;
        }
        private DataTable ProcessConfig(AbstractProcessorConfig config, DataTable dt, DataTable errors){
            while(config.Queue.Count>0) {

                switch(config.Queue.Dequeue()) {
                    case "Renamer": {
                        var dequeued = config.Renamer.Dequeue();
                        Renamer(config.ColumnNameToRefer,dequeued, dt);
                        config.ColumnNameToRefer=dequeued.ActualColumnName;
                        break;
                    }
                    case "Validator":{
                        var dequeued = config.Validator.Dequeue();
                        var result = Validator(config.ColumnNameToRefer,dequeued,dt);
                        if(!string.IsNullOrWhiteSpace(result)){
                            errors = AddErrorRow(errors,dequeued.ErrorMessage,result);
                        }
                        break;
                    }
                    case "ValidatorWithParams": {
                        Type type=typeof(List<string>);
                        var dequeued=config.ValidatorWithParams.Dequeue();
                        var result = ValidatorWithParams(config.ColumnNameToRefer,dequeued,dt,dequeued.MasterData);
                        if(!string.IsNullOrWhiteSpace(result)){
                            errors = AddErrorRow(errors,dequeued.ErrorMessage,result);
                        }
                        break;
                    }
                }
            }
            return dt;
        }

        private void Renamer(string ExcelColumnName, _Renamer renamer,DataTable dataTable){
            dataTable.Columns[ExcelColumnName].ColumnName = renamer.ActualColumnName;
            
        }

        private string Validator(string Column,_Validator validator, DataTable dataTable){
                    
                    StringBuilder stringBuilder=new StringBuilder();
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        if (!validator.validator(dataTable.Rows[i][Column].ToString()))
                        {
                            stringBuilder.Append("," + (i + 2).ToString());
                        }
                    }
                    return stringBuilder.ToString();
        }
        private string ValidatorWithParams<T>(string Column,_ValidatorWithParams<T> validator, DataTable dataTable,T masterData){
                    
                    StringBuilder stringBuilder=new StringBuilder();
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        if (!validator.validator(masterData,dataTable.Rows[i][Column].ToString()))
                        {
                            stringBuilder.Append("," + (i + 2).ToString());
                        }
                    }
                    return stringBuilder.ToString();
        }
        private DataTable CreateErrorDataTable(){
            DataTable dataTable=new DataTable();
            dataTable.Columns.Add("Column Name");
            dataTable.Columns.Add("ErrorDetails");
            return dataTable;
        }

        private DataTable AddErrorRow(DataTable errors, string key,string value){

                   DataRow dataRow = errors.NewRow();
                   dataRow[0]=key;
                   dataRow[1]=value;
                   errors.Rows.Add(dataRow);
                   //errors.AcceptChanges();
            return errors;
        }

    }
}