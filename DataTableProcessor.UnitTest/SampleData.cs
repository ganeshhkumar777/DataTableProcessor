using System.Data;

namespace DataTableProcessor.UnitTest{
    public static class SampleData {
        public static DataTable getSampleDataTable(){
            DataTable dataTable=new DataTable();
            dataTable.Columns.Add("Old Name");
            dataTable.Columns.Add("additional Name");
            DataRow firstRow=dataTable.NewRow();
            firstRow[0]="Ganesh";
            firstRow[1]="new Ganesh";
            DataRow secondRow=dataTable.NewRow();
            secondRow[0]="Hari";
            secondRow[1]="new Hari";
            
            dataTable.Rows.Add(firstRow);
            dataTable.Rows.Add(secondRow);
            //dataTable.AcceptChanges();
            return dataTable;
        }

    }
}