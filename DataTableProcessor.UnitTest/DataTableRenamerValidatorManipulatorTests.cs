using Xunit;
using System.Data;
using DataTableProcessorConfig;
using System.Collections.Generic;
namespace DataTableProcessor.UnitTest{
    public class DataTableRenamerValidatorManipulatorTests{
        DataTable dt;
        public DataTableRenamerValidatorManipulatorTests(){
            
            dt=SampleData.getSampleDataTable();        
        }
        [Fact]
        public void ShouldNotRunManipulationIfValidationFails(){
                List<AbstractProcessorConfig> ldp=new List<AbstractProcessorConfig>();

            var dp=DataTableProcessorConfiguration.CreateConfig("Old Name")
                                 .AddRenamer("new Name")
                                 .AddValidator((y)=>{
                                     return false;
                                 },null).AddManipulator<int>((y)=>{
                                     return 1;
                                 })
                                 .GetConfiguration();
            
            ldp.Add(dp);
            var renamedDt = ldp.ProcessConfigs(dt);
            Assert.Equal(renamedDt.Result.Columns.Contains("new Name"),true);
            Assert.Equal(renamedDt.Result.Columns.Contains("Old Name"),false);
            Assert.Equal(renamedDt.Error.Rows.Count,1);
               var temp=renamedDt.Result.Rows[0]["new Name"] as string;
            Assert.Equal("Ganesh",temp);
        }

[Fact]
        public void ShouldRunManipulationIfValidationSucceeds(){
                List<AbstractProcessorConfig> ldp=new List<AbstractProcessorConfig>();

            var dp=DataTableProcessorConfiguration.CreateConfig("Old Name")
                                 .AddRenamer("new Name")
                                 .AddValidator((y)=>{
                                     return true;
                                 },null).AddManipulator<int>((y)=>{
                                     return 1;
                                 })
                                 .GetConfiguration();
            
            ldp.Add(dp);
            var renamedDt = ldp.ProcessConfigs(dt);
            Assert.Equal(renamedDt.Result.Columns.Contains("new Name"),true);
            Assert.Equal(renamedDt.Result.Columns.Contains("Old Name"),false);
            Assert.Equal(renamedDt.Error.Rows.Count,0);
               var temp=renamedDt.Result.Rows[0]["new Name"] as string;
            Assert.Equal("1",temp);
        }

            [Fact]
        public void ShouldRunManipulationIfValidationFailsButContinueSetToTrue(){
                List<AbstractProcessorConfig> ldp=new List<AbstractProcessorConfig>();

            var dp=DataTableProcessorConfiguration.CreateConfig("Old Name")
                                 .AddRenamer("new Name")
                                 .AddValidator((y)=>{
                                     return true;
                                 },null).AddManipulator<int>((y)=>{
                                     return 1;
                                 })
                                 .GetConfiguration();
           
            ldp.Add(dp);
                
            var renamedDt = ldp.ProcessConfigs(dt);
            Assert.Equal(renamedDt.Result.Columns.Contains("new Name"),true);
            Assert.Equal(renamedDt.Result.Columns.Contains("Old Name"),false);
            Assert.Equal(renamedDt.Error.Rows.Count,0);
            var temp=renamedDt.Result.Rows[0]["new Name"] as string;
            Assert.Equal("1",temp);
        }
    }


    }

