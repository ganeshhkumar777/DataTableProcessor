using System.Data;
using Xunit;
using DataTableProcessorConfig;
using System.Collections.Generic;
namespace DataTableProcessor.UnitTest{
    public class  DataTableManipulatorTests{
        DataTable dt;
        public DataTableManipulatorTests(){
            dt=SampleData.getSampleDataTable();
        }
        [Fact]
        public void ManipulatorCheck(){
            Employee master=new Employee();
            master.Name="New Name";
            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            var config=DataTableProcessorConfiguration.CreateConfig("Old Name").AddManipulator<string>((input1)=>{
                return master.Name;
                }
            ).GetConfiguration();
            
            configs.Add(config);
            var dataTableProcessorResult = configs.ProcessConfigs(dt);
            var temp=dataTableProcessorResult.Result.Rows[0]["Old Name"] as string;
            Assert.Equal(temp,"New Name");
        }

          [Fact]
        public void StoreManipulatedValuesToAnotherColumn(){
            dt=SampleData.getSampleDataTable();
            Employee master=new Employee();
            master.Name="New Name";
            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            var config=DataTableProcessorConfiguration.CreateConfig("Old Name").AddManipulator<string>((input1)=>{
                return master.Name;
                },"New Column"
            ).GetConfiguration();
            
            configs.Add(config);
            var dataTableProcessorResult = configs.ProcessConfigs(dt);
            var temp=dataTableProcessorResult.Result.Rows[0]["New Column"] as string;
            Assert.Equal(temp,"New Name");
        }

         [Fact]
        public void StoreValuesFromAdditionalColumnToAnotherColumn(){
            dt=SampleData.getSampleDataTable();
            Employee master=new Employee();
            master.Name="New Name";
            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            var config=DataTableProcessorConfiguration.CreateConfig("Old Name").AddManipulator<string>((input1,input2)=>{
                return input2;
                },"additional Name","New Column"
            ).GetConfiguration();
            
            configs.Add(config);
            var dataTableProcessorResult = configs.ProcessConfigs(dt);
            var temp=dataTableProcessorResult.Result.Rows[0]["New Column"] as string;
            Assert.Equal(temp,"new Ganesh");
        }

         [Fact]
        public void StoreValuesFromAdditionalColumnToSameColumn(){
            dt=SampleData.getSampleDataTable();
            Employee master=new Employee();
            master.Name="New Name";
            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            var config=DataTableProcessorConfiguration.CreateConfig("Old Name").AddManipulator<string>((input1,input2)=>{
                return input2;
                },"additional Name"
            ).GetConfiguration();
            
            configs.Add(config);
            var dataTableProcessorResult = configs.ProcessConfigs(dt);
            var temp=dataTableProcessorResult.Result.Rows[0]["Old Name"] as string;
            Assert.Equal(temp,"new Ganesh");
        }
    }

}