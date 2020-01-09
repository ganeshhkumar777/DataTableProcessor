using System.Data;
using System.Collections.Generic;
using System.Text;
using System;
using DataTableProcessorConfig;
namespace DataTableProcessor
{
    internal class Processor
    {

        public DataTableProcessorResult Process(List<AbstractProcessorConfig> configurations, DataTable dt, ErrorConfig errorConfig){
            
            DataTableProcessorResult result=new DataTableProcessorResult();
            
            DataTable errors = CreateErrorDataTable();

            foreach(var config in configurations){

                if(dt.Columns.Contains(config.ExcelColumnName)) {

                    dt = ProcessConfig(config,dt,errors,errorConfig.StartRowNumberForValidationError);
                }
                else {
                    errors=AddErrorRow(errors,errorConfig.ErrorMessageWhenColumnNotPresentKey==null ? config.ExcelColumnName : string.Format(errorConfig.ErrorMessageWhenColumnNotPresentKey,config.ExcelColumnName),errorConfig.ErrorMessageWhenColumnNotPresentValue);
                }
                
            }
            result.Result=dt;
            result.Error=errors;
            return result;
        }
        public DataTableProcessorResult Process(List<AbstractProcessorConfig> configurations, DataTable dt, int StartRowNumberForValidationError=2){
            
            DataTableProcessorResult result=new DataTableProcessorResult();
            
            DataTable errors = CreateErrorDataTable();

            foreach(var config in configurations){

                if(dt.Columns.Contains(config.ExcelColumnName)) {

                    dt = ProcessConfig(config,dt,errors,StartRowNumberForValidationError);
                }
                else {
                    errors=AddErrorRow(errors,config.ExcelColumnName,ErrorMessages.ColumnNotPresent);
                }
                
            }
            result.Result=dt;
            result.Error=errors;
            return result;
        }
        private DataTable ProcessConfig(AbstractProcessorConfig config, DataTable dt, DataTable errors,int StartRowNumberForValidationError){
            while(config.Queue.Count>0) {

                switch(config.Queue.Dequeue()) {
                    case DataTableOperations.Renamer: {
                        var dequeued = config.Renamer.Dequeue();
                        Renamer(config.ColumnNameToRefer,dequeued, dt);
                        config.ColumnNameToRefer=dequeued.ActualColumnName;
                        break;
                    }
                    case DataTableOperations.Validator:{
                        var dequeued = config.Validator.Dequeue();
                        var result = Validator(config.ColumnNameToRefer,dequeued,dt,StartRowNumberForValidationError);
                        if(!string.IsNullOrWhiteSpace(result)){
                            if(!dequeued.continueWhenValidationFails)
                            config.Queue.Clear();
                            errors = AddErrorRow(errors,dequeued.ErrorMessage,result);
                        }
                        break;
                    }
                    case DataTableOperations.ValidatorWithParams: {
                        
                        var dequeued=config.ValidatorWithParams.Dequeue();
                        var result = ValidatorWithParams(config.ColumnNameToRefer,dequeued,dt,dequeued.MasterData,StartRowNumberForValidationError);
                        if(!string.IsNullOrWhiteSpace(result)){
                            if(!dequeued.continueWhenValidationFails)
                            config.Queue.Clear();
                            errors = AddErrorRow(errors,dequeued.ErrorMessage,result);
                        }
                        break;
                    }
                    case DataTableOperations.Manipulator:{
                        var dequeued=config.Manipulators.Dequeue();
                        var result=Manipulator(config.ColumnNameToRefer,dequeued,dt);
                        break;
                    }
                    case DataTableOperations.ManipulatorWithParams:{
                        var dequeued=config.ManipulatorWithParams.Dequeue();
                        var result=ManipulatorWithParams(config.ColumnNameToRefer,dequeued,dt,dequeued.MasterData);
                        break;
                    }
                }
            }
            return dt;
        }

        private void Renamer(string ExcelColumnName, _Renamer renamer,DataTable dataTable){
            dataTable.Columns[ExcelColumnName].ColumnName = renamer.ActualColumnName;
            
        }

        private string Validator(string Column,_Validator validator, DataTable dataTable,int StartRowNumberForValidationError){
                    
                    StringBuilder stringBuilder=new StringBuilder();
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        if (!validator.validator(dataTable.Rows[i][Column].ToString()))
                        {
                            stringBuilder.Append(stringBuilder.Length>0 ? 
                                                                            "," + (i + StartRowNumberForValidationError).ToString()
                                                                        :
                                                                             (i + StartRowNumberForValidationError).ToString() );
                        }
                    }
                    
                    return stringBuilder.ToString();
        }
        private string ValidatorWithParams<T>(string Column,_ValidatorWithParams<T> validator, DataTable dataTable,T masterData,int StartRowNumberForValidationError){
                    
                    StringBuilder stringBuilder=new StringBuilder();
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        if (!validator.validator(masterData,dataTable.Rows[i][Column].ToString()))
                        {
                            stringBuilder.Append(stringBuilder.Length>0 ? 
                                                                            "," + (i + StartRowNumberForValidationError).ToString()
                                                                        :
                                                                             (i + StartRowNumberForValidationError).ToString() );
                        }
                    }
                    
                    return stringBuilder.ToString();
        }

        private string Manipulator<T>(string Column,_Manipulator<T> manipulator, DataTable dataTable){
            StringBuilder stringBuilder=new StringBuilder();
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        if(manipulator.ColumnToStoreResult!=null && !dataTable.Columns.Contains(manipulator.ColumnToStoreResult)){
                            dataTable.Columns.Add(manipulator.ColumnToStoreResult);
                        }
                      dataTable.Rows[i][manipulator.ColumnToStoreResult ?? Column ] = manipulator.Manipulator(dataTable.Rows[i][Column].ToString());
                    }
                    return stringBuilder.ToString();
        }

        private string ManipulatorWithParams<ResultType,ParameterType>(string Column,_ManipulatorWithParams<ResultType,ParameterType> manipulator, DataTable dataTable,ParameterType masterData){
                  StringBuilder stringBuilder=new StringBuilder();
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        if(manipulator.ColumnToStoreResult!=null && !dataTable.Columns.Contains(manipulator.ColumnToStoreResult)){
                             dataTable.Columns.Add(manipulator.ColumnToStoreResult);
                        }
                      dataTable.Rows[i][manipulator.ColumnToStoreResult ?? Column] = manipulator.Manipulator(masterData,dataTable.Rows[i][Column].ToString());

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