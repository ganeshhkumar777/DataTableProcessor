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
            var config= DataTableProcessorConfiguration.CreateConfig("Old Name").AddValidatorWithParams((input1,input2)=>{
                return master.Name=="Ganesh";
                },master)
                .GetConfiguration();
            
            configs.Add(config);
            var dataTableProcessorResult = configs.ProcessConfigs(dt);
            Assert.Equal(dataTableProcessorResult.Error.Rows.Count,0);
        }
    }

    public class Employee{
        public string Name{get; set;}
    }
}