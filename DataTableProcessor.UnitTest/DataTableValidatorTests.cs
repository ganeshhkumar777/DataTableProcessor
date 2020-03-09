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
            var config=DataTableProcessorConfiguration.CreateConfig("Old Name").AddValidator((input)=>{
                return false;
            }).GetConfiguration();
            
            configs.Add(config);
            var renamedDt=configs.ProcessConfigs(dt);
            Assert.Equal(renamedDt.Error.Rows[0][0],string.Format(ErrorMessages.DefaultInvalidColumn,"Old Name"));
            Assert.Equal(renamedDt.Error.Rows.Count==1,true);
        }

        [Fact]
        public void ShouldNotReturnErrorWhenValidationCheckSucceeds()
        {
            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            var config=DataTableProcessorConfiguration.CreateConfig("Old Name")
                    .AddValidator((input)=>{
                return true;
            }).GetConfiguration();
            
            configs.Add(config);
            var renamedDt=configs.ProcessConfigs(dt);
            Assert.Equal(renamedDt.Error.Rows.Count==0,true);
        } 

         [Fact]
        public void ShouldReturnErrorWhenValidationCheckFailsAfterPassingAdditionalColumnToValidator()
        {
            List<AbstractProcessorConfig> configs=new List<AbstractProcessorConfig>();
            var config=DataTableProcessorConfiguration.CreateConfig("Old Name")
            .AddValidator((input,additionalColumn)=>{
                return additionalColumn == null;
            },"additional Name").GetConfiguration();
            
            configs.Add(config);
            var renamedDt=configs.ProcessConfigs(dt);
            Assert.Equal(renamedDt.Error.Rows[0][0],string.Format(ErrorMessages.DefaultInvalidColumn,"Old Name"));
            Assert.Equal(renamedDt.Error.Rows.Count==1,true);
        }      
}
}