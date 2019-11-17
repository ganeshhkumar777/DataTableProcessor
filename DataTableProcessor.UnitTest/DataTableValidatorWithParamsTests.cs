using Xunit;
using System.Collections.Generic;
using DataTableProcessorConfig;
using System.Data;
using System.Diagnostics;
namespace DataTableProcessor.UnitTest{

    public class  DataTableValidatorWithParamsTests{
        DataTable dt;
        public DataTableValidatorWithParamsTests(){
            dt=SampleData.getSampleDataTable();
        }
        [Fact]
        public void ValidatorWithParamsCheck(){
            Employee master=new Employee();
            master.Name="Ganesh";
            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            ProcessorConfig config=new ProcessorConfig("Old Name");
            var validator=new ValidatorWithParamsConfig<Employee>(config,(input1,input2)=>{
                return master.Name=="Ganesh";
                },master
            );
            configs.Add(config);
            Processor processor=new Processor();
            var dataTableProcessorResult = processor.Process(configs,dt);
            Assert.Equal(dataTableProcessorResult.Error.Rows.Count,0);
        }
    }

    public class Employee{
        public string Name{get; set;}
    }
}