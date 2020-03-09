using System;
using System.Data;
using System.Collections.Generic;
using DataTableProcessor;
namespace DataTableProcessorConfig
{
    public static class ExtensionMethods{
        public static DataTableProcessorResult ProcessConfigs(this List<AbstractProcessorConfig> configs, DataTable dt,int StartRowNumberForValidationError=2){
            Processor obj=new Processor();
           return obj.Process(configs,dt,StartRowNumberForValidationError);
        }

        public static DataTableProcessorResult ProcessConfigs(this List<AbstractProcessorConfig> configs, DataTable dt, ErrorConfig errorConfig){
            Processor obj=new Processor();
           return obj.Process(configs,dt,errorConfig);
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

            public ConfigurationBuilder AddValidator(Func<string,string, bool> validator,string additionalColumn, string errorMessage=null,bool continueWhenValidationFails=false){
            new ValidatorConfig(_config.config,validator,additionalColumn , errorMessage,continueWhenValidationFails);
            return this;
            }
            
            public ConfigurationBuilder AddValidatorWithParams<T>(Func<T,string, bool> validator,T masterData, string errorMessage=null,bool continueWhenValidationFails=false){
            new ValidatorWithParamsConfig<T>(_config.config,validator,masterData,errorMessage,continueWhenValidationFails);
            return this;
            }
            public ConfigurationBuilder AddValidatorWithParams<T>(Func<T,string,string, bool> validator,T masterData,string additionalColumn, string errorMessage=null,bool continueWhenValidationFails=false){
            new ValidatorWithParamsConfig<T>(_config.config,validator,masterData,additionalColumn,errorMessage,continueWhenValidationFails);
            return this;
            }
              /// <summary>
        /// Method to Add Manipulators
        /// </summary>
        /// <param name="Manipulator"></param>
        /// <param name="ColumnToStoreResult">Nullable column.if value is null, manipulated data will be stored to the same column</param>
        /// <returns> ConfigurationBuilder </returns>
            public ConfigurationBuilder AddManipulator<ManipulatorResultType>(Func<string,ManipulatorResultType> Manipulator,string ColumnToStoreResult=null){
            new ManipulatorConfig<ManipulatorResultType>(_config.config,Manipulator,ColumnToStoreResult);
            return this;
            }
             public ConfigurationBuilder AddManipulator<ManipulatorResultType>(Func<string,string,ManipulatorResultType> Manipulator,string additionalColumn,string ColumnToStoreResult=null){
            new ManipulatorConfig<ManipulatorResultType>(_config.config,Manipulator,additionalColumn,ColumnToStoreResult);
            return this;
            }
            
            public ConfigurationBuilder AddManipulatorWithParams<ParameterType,ResultType>(Func<ParameterType,string,ResultType> Manipulator, ParameterType MasterData,string ColumnToStoreResult=null){
            new ManipulatorWithParamsConfig<ResultType,ParameterType>(_config.config,Manipulator,MasterData,ColumnToStoreResult);
            return this;
            }
            public ConfigurationBuilder AddManipulatorWithParams<ParameterType,ResultType>(Func<ParameterType,string,string,ResultType> Manipulator, ParameterType MasterData,string additionalColumn, string ColumnToStoreResult=null){
            new ManipulatorWithParamsConfig<ResultType,ParameterType>(_config.config,Manipulator,MasterData,additionalColumn,ColumnToStoreResult);
            return this;
            }
            public AbstractProcessorConfig GetConfiguration(){
                return _config.config;
            }
        }

       public static ConfigurationBuilder CopyConfig(AbstractProcessorConfig abstractProcessorConfig,string ExcelColumnName){
            
            return new ConfigurationBuilder(new DataTableProcessorConfiguration(),ExcelColumnName);
        }

        
    }

     

}
