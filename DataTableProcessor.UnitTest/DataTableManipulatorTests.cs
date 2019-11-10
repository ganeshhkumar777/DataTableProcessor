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
            ProcessorConfig config=new ProcessorConfig("Old Name");
            var validator=new ManipulatorConfig<string>(config,(input1)=>{
                return master.Name;
                }
            );
            configs.Add(config);
            Processor processor=new Processor();
            var dataTableProcessorResult = processor.Process(configs,dt);
            var temp=dataTableProcessorResult.Result.Rows[0]["Old Name"] as string;
            Assert.Equal(temp,"New Name");
        }
    }

}