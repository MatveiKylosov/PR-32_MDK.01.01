using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace VinylRecordsApplication.Classes
{
    public class Record
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int Format { get; set; }
        public int Size { get; set; }
        public int IdManufacturer { get; set; }
        public float Price { get; set; }
        public int IdState { get; set; }
        public string Description { get; set; }

        public static IEnumerable<Record> AllRecords()
        {
            List<Record> records = new List<Record>();
            DataTable recordQuery = Classes.DBConnection.Connection("SELECT * FROM [dbo].[Record]");
            foreach (DataRow row in recordQuery.Rows)
            {
                records.Add(new Record()
                {
                    Id = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    Year = Convert.ToInt32(row[2]),
                    Format = Convert.ToInt32(row[3]),
                    Size = Convert.ToInt32(row[4]),
                    IdManufacturer = Convert.ToInt32(row[5]),
                    Price = float.Parse(row[6].ToString()),
                    IdState = Convert.ToInt32(row[7]),
                    Description = row[8].ToString()
                });
            }
            return records;
        }

        public void Save(bool Update = false)
        {
            string CorrectPrice = this.Price.ToString().Replace(",", ".");
            if (Update == false)
            {
                Classes.DBConnection.Connection(
                    "INSERT INTO " +
                        "[dbo].Record " +
                    "VALUES(" +
                        $"N'{this.Name}', " +
                        $"{this.Year}, " + // исправлены кавычки
                        $"{this.Format}, " +
                        $"{this.Size}," +
                        $"{this.IdManufacturer}," +
                        $"{CorrectPrice}, " +
                        $"{this.IdState}, " +
                        $"N'{this.Description}');");

                this.Id = Record.AllRecords().Where(
                    x => x.Name == this.Name &&
                    x.Year == this.Year &&
                    x.Format == this.Format &&
                    x.Size == this.Size &&
                    x.IdManufacturer == this.IdManufacturer &&
                    x.IdState == this.IdState &&
                    x.Description == this.Description).First().Id;
            }
            else
                Classes.DBConnection.Connection(
                    "UPDATE [dbo].[Record] " +
                        $"SET [Name] = N'{this.Name}'," +
                        $"[Year] = {this.Year}, " +
                        $"[Format] = {this.Format}, " +
                        $"[Size] = {this.Size}, " +
                        $"[IdManufacturer] = {this.IdManufacturer}, " +
                        $"[Price] = {CorrectPrice}, " +
                        $"[IdState] = {this.IdState}, " +
                        $"[Description] = N'{this.Description}' " +
                        $"WHERE [Id] = {this.Id}");
        }


        public void Delete()=>
            Classes.DBConnection.Connection($"DELETE FROM [dbo].[Record] WHERE [Id] = {this.Id};");

        public void ExportToExcel(string path)
        {
            var records = AllRecords().ToList();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Records");

                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Year";
                worksheet.Cells[1, 4].Value = "Format";
                worksheet.Cells[1, 5].Value = "Size";
                worksheet.Cells[1, 6].Value = "IdManufacturer";
                worksheet.Cells[1, 7].Value = "Price";
                worksheet.Cells[1, 8].Value = "IdState";
                worksheet.Cells[1, 9].Value = "Description";

                for (int i = 0; i < records.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = records[i].Id;
                    worksheet.Cells[i + 2, 2].Value = records[i].Name;
                    worksheet.Cells[i + 2, 3].Value = records[i].Year;
                    worksheet.Cells[i + 2, 4].Value = records[i].Format;
                    worksheet.Cells[i + 2, 5].Value = records[i].Size;
                    worksheet.Cells[i + 2, 6].Value = records[i].IdManufacturer;
                    worksheet.Cells[i + 2, 7].Value = records[i].Price;
                    worksheet.Cells[i + 2, 8].Value = records[i].IdState;
                    worksheet.Cells[i + 2, 9].Value = records[i].Description;
                }

                // Сохранение файла
                var file = new FileInfo(path);
                package.SaveAs(file);
            }
        }
    }
}
