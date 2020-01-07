using System.Data;
using Xunit;
using DataTableProcessorConfig;
using System.Collections.Generic;
namespace DataTableProcessor.UnitTest{
    public class  DataTableManipulatorWithParamsTests{
        DataTable dt;
        public DataTableManipulatorWithParamsTests(){
            dt=SampleData.getSampleDataTable();
        }
        [Fact]
        public void ManipulatorWithParamsCheck(){
            Employee master=new Employee();
            master.Name="New Name";
            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            var config = DataTableProcessorConfiguration.CreateConfig("Old Name")
                                        .AddManipulatorWithParams((input1,input2)=>{
                return input1.Name;
                },master).GetConfiguration();
            
            configs.Add(config);
            var dataTableProcessorResult= configs.ProcessConfigs(dt);
            var temp=dataTableProcessorResult.Result.Rows[0]["Old Name"] as string;
            Assert.Equal(temp,"New Name");
        }

         [Fact]
        public void StoreManipulatedValuesToAnotherColumn(){
            Employee master=new Employee();
            master.Name="New Name";
            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            var config = DataTableProcessorConfiguration.CreateConfig("Old Name")
                                        .AddManipulatorWithParams((input1,input2)=>{
                return input1.Name;
                },master,"New Column").GetConfiguration();
            
            configs.Add(config);
            var dataTableProcessorResult= configs.ProcessConfigs(dt);
            var temp=dataTableProcessorResult.Result.Rows[0]["New Column"] as string;
            Assert.Equal(temp,"New Name");
        }

        [Fact]
        public void StoreManipulatedValuesToAnotherColumnAndColumnShouldContainSameValue(){
            dt=SampleData.getSampleDataTable();
            Employee master=new Employee();
            master.Name="New Name";
            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            var config = DataTableProcessorConfiguration.CreateConfig("Old Name")
                                        .AddManipulatorWithParams((input1,input2)=>{
                return "New Name";
                },master,"New Column").GetConfiguration();
            
            configs.Add(config);
            var dataTableProcessorResult= configs.ProcessConfigs(dt);
            var temp=dataTableProcessorResult.Result.Rows[0]["Old Name"] as string;
            Assert.Equal(temp,"Ganesh");
        }
    }

}