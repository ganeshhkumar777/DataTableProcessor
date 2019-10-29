using System.Data;
using System.Collections.Generic;
using System.Text;
namespace DataTableProcessor
{
    public class Processor
    {
        public DataTable Process(List<ProcessorConfig> configurations, DataTable dt){
            
            foreach(var config in configurations){

                if(dt.Columns.Contains(config.ExcelColumnName)) {

                    dt = ProcessConfig(config,dt);
                }
                else {
                    // error has to be handled
                }
                
            }
            return dt;
        }
        private DataTable ProcessConfig(ProcessorConfig config, DataTable dt){
            while(config.Queue.Count>0) {

                switch(config.Queue.Dequeue()) {
                    case "Renamer": {
                        var dequeued = config.Renamer.Dequeue();
                        Renamer(config.ColumnNameToRefer,dequeued, dt);
                        config.ColumnNameToRefer=dequeued.ActualColumnName;
                        break;
                    }
                    case "Validator":{
                        Validator(config.ColumnNameToRefer,config.Validator.Dequeue(),dt);
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

    }
}