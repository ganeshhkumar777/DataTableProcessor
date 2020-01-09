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

         [Fact]
        public void ValidatorWithParamsFailureCheck(){
            Employee master=new Employee();
            master.Name="Ganesh";

            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            var config= DataTableProcessorConfiguration.CreateConfig("Old Name").AddValidatorWithParams((input1,input2)=>{
                return master.Name=="Hari";
                },master)
                .GetConfiguration();
            
            configs.Add(config);
            var dataTableProcessorResult = configs.ProcessConfigs(dt);
            Assert.Equal(dataTableProcessorResult.Error.Rows.Count,1);
            Assert.Equal("2,3",dataTableProcessorResult.Error.Rows[0].ItemArray[1]);
        }

        [Fact]
        public void ValidatorWithParamsFailureCheckWithStartRowSet(){
            Employee master=new Employee();
            master.Name="Ganesh";

            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            var config= DataTableProcessorConfiguration.CreateConfig("Old Name").AddValidatorWithParams((input1,input2)=>{
                return master.Name=="Hari";
                },master)
                .GetConfiguration();
            
            configs.Add(config);
            var dataTableProcessorResult = configs.ProcessConfigs(dt,3);
            Assert.Equal(dataTableProcessorResult.Error.Rows.Count,1);
            Assert.Equal("3,4",dataTableProcessorResult.Error.Rows[0].ItemArray[1]);
        }

        [Fact]
        public void ValidatorWithParamsFailureCheckWithStartRowSetThroughErrorConfig(){
            Employee master=new Employee();
            master.Name="Ganesh";

            ErrorConfig errorConfig=new ErrorConfig();
            errorConfig.ErrorMessageWhenColumnNotPresentKey="Column {0} is not present ";
            errorConfig.ErrorMessageWhenColumnNotPresentValue="Header Row";
            errorConfig.StartRowNumberForValidationError=3;

            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            var config= DataTableProcessorConfiguration.CreateConfig("Old Name").AddValidatorWithParams((input1,input2)=>{
                return master.Name=="Hari";
                },master)
                .GetConfiguration();
            
            configs.Add(config);
            var dataTableProcessorResult = configs.ProcessConfigs(dt,errorConfig);
            Assert.Equal(dataTableProcessorResult.Error.Rows.Count,1);
            Assert.Equal("3,4",dataTableProcessorResult.Error.Rows[0].ItemArray[1]);
        }

        [Fact]
        public void ColumnItselfIsNotThere(){
            Employee master=new Employee();
            master.Name="Ganesh";

            ErrorConfig errorConfig=new ErrorConfig();
            errorConfig.ErrorMessageWhenColumnNotPresentKey="Column {0} is not present ";
            errorConfig.ErrorMessageWhenColumnNotPresentValue="Header Row";
            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            var config= DataTableProcessorConfiguration.CreateConfig("Old Name1").AddValidatorWithParams((input1,input2)=>{
                return master.Name=="Hari";
                },master)
                .GetConfiguration();
            
            configs.Add(config);
            var dataTableProcessorResult = configs.ProcessConfigs(dt,errorConfig);
            Assert.Equal(dataTableProcessorResult.Error.Rows.Count,1);
            Assert.Equal("Column Old Name1 is not present ",dataTableProcessorResult.Error.Rows[0].ItemArray[0]);
            Assert.Equal("Header Row",dataTableProcessorResult.Error.Rows[0].ItemArray[1]);
        }

        [Fact]
        public void ColumnItselfIsNotThereButErrorConfigIsNotPassed(){
            Employee master=new Employee();
            master.Name="Ganesh";

            ErrorConfig errorConfig=new ErrorConfig();
            errorConfig.ErrorMessageWhenColumnNotPresentKey="Column {0} is not present ";
            errorConfig.ErrorMessageWhenColumnNotPresentValue="Header Row";
            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            var config= DataTableProcessorConfiguration.CreateConfig("Old Name1").AddValidatorWithParams((input1,input2)=>{
                return master.Name=="Hari";
                },master)
                .GetConfiguration();
            
            configs.Add(config);
            var dataTableProcessorResult = configs.ProcessConfigs(dt,2);
            Assert.Equal(dataTableProcessorResult.Error.Rows.Count,1);
            Assert.Equal("Old Name1",dataTableProcessorResult.Error.Rows[0].ItemArray[0]);
            Assert.Equal("Column is not present",dataTableProcessorResult.Error.Rows[0].ItemArray[1]);
        }
    }

    public class Employee{
        public string Name{get; set;}
    }
}