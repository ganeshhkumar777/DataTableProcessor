using Xunit;
using System.Data;
using DataTableProcessorConfig;
using DataTableProcessor;
using System.Collections.Generic;
namespace DataTableProcessor.UnitTest{
public class DataTableValidatorTests{
    DataTable dt;
        public DataTableValidatorTests(){
            dt=SampleData.getSampleDataTable();
        }
        [Fact]
        public void ShouldReturnErrorWhenValidationCheckFails()
        {
            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            ProcessorConfig config=new ProcessorConfig("Old Name");
            var validator=new ValidatorConfig(config,(input)=>{
                return false;
            });
            configs.Add(config);
            Processor processor=new Processor();
            var renamedDt=processor.Process(configs,dt);
            Assert.Equal(renamedDt.Error.Rows[0][0],string.Format(ErrorMessages.DefaultInvalidColumn,"Old Name"));
            Assert.Equal(renamedDt.Error.Rows.Count==1,true);
        }

        [Fact]
        public void ShouldNotReturnErrorWhenValidationCheckFails()
        {
            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            ProcessorConfig config=new ProcessorConfig("Old Name");
            var validator=new ValidatorConfig(config,(input)=>{
                return true;
            });
            configs.Add(config);
            Processor processor=new Processor();
            var renamedDt=processor.Process(configs,dt);
            Assert.Equal(renamedDt.Error.Rows.Count==0,true);
        }       
}
}