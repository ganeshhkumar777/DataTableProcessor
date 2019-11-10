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
            ProcessorConfig config=new ProcessorConfig("Old Name");
            var validator=new ManipulatorWithParamsConfig<string,Employee>(config,(input1,input2)=>{
                return input1.Name;
                },master
            );
            configs.Add(config);
            Processor processor=new Processor();
            var dataTableProcessorResult = processor.Process(configs,dt);
            var temp=dataTableProcessorResult.Result.Rows[0]["Old Name"] as string;
            Assert.Equal(temp,"New Name");
        }
    }

}