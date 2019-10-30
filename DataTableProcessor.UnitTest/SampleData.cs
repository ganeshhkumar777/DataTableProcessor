using System.Data;

namespace DataTableProcessor.UnitTest{
    public static class SampleData {
        public static DataTable getSampleDataTable(){
            DataTable dataTable=new DataTable();
            dataTable.Columns.Add("Old Name");
            DataRow firstRow=dataTable.NewRow();
            firstRow[0]="Ganesh";
            DataRow secondRow=dataTable.NewRow();
            secondRow[0]="Hari";
            
            dataTable.Rows.Add(firstRow);
            dataTable.Rows.Add(secondRow);
            //dataTable.AcceptChanges();
            return dataTable;
        }

    }
}