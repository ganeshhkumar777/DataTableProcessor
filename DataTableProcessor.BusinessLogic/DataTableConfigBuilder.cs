using System;
using System.Data;
using System.Collections.Generic;
using DataTableProcessor;
namespace DataTableProcessorConfig
{
    public static class ExtensionMethods{
        public static DataTableProcessorResult ProcessConfigs(this List<AbstractProcessorConfig> configs, DataTable dt){
            Processor obj=new Processor();
           return obj.Process(configs,dt);
        }
    }

    public class DataTableProcessorConfiguration{
        internal AbstractProcessorConfig config;
        private DataTableProcessorConfiguration(){
            
        }        

        // make excel column name a string array
        public static ConfigurationBuilder CreateConfig(string ExcelColumnName){
            
            return new ConfigurationBuilder(new DataTableProcessorConfiguration(),ExcelColumnName);
        }
        
        public class ConfigurationBuilder{            
            DataTableProcessorConfiguration _config;
            public ConfigurationBuilder(DataTableProcessorConfiguration instance,string ExcelColumnName) {
            _config=instance;
            _config.config=new ProcessorConfig(ExcelColumnName);
            }
            public ConfigurationBuilder AddRenamer(string ActualColumnName){
            new RenamerConfig(_config.config,ActualColumnName);
            return this;
            }
            public ConfigurationBuilder AddValidator(Func<string, bool> validator, string errorMessage=null,bool continueWhenValidationFails=false){
            new ValidatorConfig(_config.config,validator,errorMessage,continueWhenValidationFails);
            return this;
            }
            
            public ConfigurationBuilder AddValidatorWithParams<T>(Func<T,string, bool> validator,T masterData, string errorMessage=null,bool continueWhenValidationFails=false){
            new ValidatorWithParamsConfig<T>(_config.config,validator,masterData,errorMessage,continueWhenValidationFails);
            return this;
            }
            
            public ConfigurationBuilder AddManipulator<ManipulatorResultType>(Func<string,ManipulatorResultType> Manipulator){
            new ManipulatorConfig<ManipulatorResultType>(_config.config,Manipulator);
            return this;
            }
            
            public ConfigurationBuilder AddManipulatorWithParams<ParameterType,ResultType>(Func<ParameterType,string,ResultType> Manipulator, ParameterType MasterData){
            new ManipulatorWithParamsConfig<ResultType,ParameterType>(_config.config,Manipulator,MasterData);
            return this;
            }
            public AbstractProcessorConfig GetConfiguration(){
                return _config.config;
            }
        }

        
    }

     

}
