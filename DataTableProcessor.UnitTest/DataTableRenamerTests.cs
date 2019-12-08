using System;
using Xunit;
using System.Data;
using System.Collections.Generic;
using DataTableProcessorConfig;
using DataTableProcessor;
namespace DataTableProcessor.UnitTest{
    public class DataTableRenamerTests
    {
        DataTable dt;
        public DataTableRenamerTests(){
            dt=SampleData.getSampleDataTable();
        }
        [Fact]
        public void SampleDataCheck()
        {
            Assert.Equal(dt.Columns["Old Name"].ColumnName,"Old Name");
        }
        
        [Fact]
        public void ColumnNameAfterRenamingUsingBuilder(){
         
            List<AbstractProcessorConfig> ldp=new List<AbstractProcessorConfig>();

            var dp=DataTableProcessorConfiguration.CreateConfig("Old Name")
                                 .AddRenamer("new Name")
                                 .GetConfiguration();
            
            ldp.Add(dp);
            var renamedDt = ldp.ProcessConfigs(dt);
            Assert.Equal(renamedDt.Result.Columns.Contains("new Name"),true);
            Assert.Equal(renamedDt.Result.Columns.Contains("Old Name"),false);
            Assert.Equal(renamedDt.Error.Rows.Count,0);
        }

        [Fact]
        public void CheckWhetherErrorIsReturnedIfColumnIsNotPresent(){
                        
            List<AbstractProcessorConfig> ldp=new List<AbstractProcessorConfig>();

            var dp= DataTableProcessorConfiguration.CreateConfig("My Name").AddRenamer("new Name").GetConfiguration();
            ldp.Add(dp);
            var renamedDt=ldp.ProcessConfigs(dt);
            
            Assert.Equal(renamedDt.Error.Rows[0][0],"My Name");
            Assert.Equal(renamedDt.Error.Rows[0][1],ErrorMessages.ColumnNotPresent);
        }
    }
}
