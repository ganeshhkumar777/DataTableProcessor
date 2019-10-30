using System;
using DataTableProcessor;
using System.Data;
using System.Collections.Generic;
using DataTableProcessorConfig;
namespace DataTableProcessorSampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable dataTable=new DataTable();
            dataTable.Columns.Add("Old Name");
            DataRow firstRow=dataTable.NewRow();
            firstRow.ItemArray[0]="Ganesh";
            DataRow secondRow=dataTable.NewRow();
            secondRow.ItemArray[0]="Hari";
            
            dataTable.Rows.Add(firstRow);
            dataTable.Rows.Add(secondRow);
            dataTable.AcceptChanges();
            List<AbstractProcessorConfig> ldp=new List<AbstractProcessorConfig>();
            ProcessorConfig dp=new ProcessorConfig("Old Name");
            var rename = new RenamerConfig(dp,"new Name");
            var validate=new ValidatorConfig(dp,(x)=>{
                return false;
            });
            ldp.Add(dp);
            Processor p=new Processor();
            p.Process(ldp,dataTable);

        }
    }
}
