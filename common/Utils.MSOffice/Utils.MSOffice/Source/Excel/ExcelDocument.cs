using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Extensions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Fasterflect;

namespace NetSteps.Common.Utility
{
    public class ExcelDocument
    {
        private const string DefaultSheetName = "Sheet1";

        public static List<string[]> GetExcelData(string fileName)
        {
            List<string[]> fileData = new List<string[]>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                //WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                //WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                //SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                var workbookPart = spreadsheetDocument.WorkbookPart;
                var workbook = workbookPart.Workbook;
                var sheet = workbook.Descendants<Sheet>().First();

                IEnumerable<Row> rows = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet.Descendants<Row>();
                string text;
                foreach (Row r in rows)
                {
                    string[] row = new string[r.Descendants<Cell>().Count()];
                    int count = 0;
                    foreach (Cell c in r.Descendants<Cell>())
                    {
                        //text = c.CellValue.Text;
                        if (c.DataType == null || c.DataType != CellValues.SharedString)
                        {
                            text = c.CellValue.Text;
                        }
                        else
                        {
                            text = GetSharedStringItemById(workbookPart, Convert.ToInt32(c.CellValue.Text)).InnerText;
                        }

                        row[count] = text;
                        count++;
                    }
                    fileData.Add(row);
                }
            }
            return fileData;

        }

        public static SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }

        public static MemoryStream Create(Dictionary<string, IEnumerable> rowData, IEnumerable<string> columnHeaders, IEnumerable<string> properties, Dictionary<string, MemberGetter> overridenValueGetters = null)
        {
            return CreateSpreadSheet(rowData, columnHeaders, properties, null, overridenValueGetters);
        }

        public static MemoryStream Create(string sheetName, IEnumerable rowData, IEnumerable<string> columnHeaders, IEnumerable<string> properties, Dictionary<string, MemberGetter> overridenValueGetters = null)
        {
            if (string.IsNullOrEmpty(sheetName))
                sheetName = DefaultSheetName;

            Dictionary<string, IEnumerable> data = new Dictionary<string, IEnumerable>();
            data.Add(string.IsNullOrEmpty(sheetName) ? DefaultSheetName : sheetName, rowData);
            return CreateSpreadSheet(data, columnHeaders, properties, null, overridenValueGetters);
        }

        private static MemoryStream CreateSpreadSheet(Dictionary<string, IEnumerable> rowData, IEnumerable<string> columnHeaders, IEnumerable<string> properties, Stylesheet styleSheet, Dictionary<string, MemberGetter> overridenValueGetters = null)
        {
            MemoryStream xmlStream = SpreadsheetReader.Create();
            using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(xmlStream, true))
            {
                spreadSheet.WorkbookPart.Workbook.Sheets.RemoveAllChildren();
                foreach (KeyValuePair<string, IEnumerable> data in rowData)
                {
                    InsertSheet(spreadSheet, data.Key, data.Value, columnHeaders, properties, styleSheet, overridenValueGetters);
                }
            }

            return xmlStream;
        }

        private static void InsertSheet(SpreadsheetDocument spreadSheet, string sheetName, IEnumerable rowData, IEnumerable<string> columnHeaders, IEnumerable<string> properties,
            Stylesheet styleSheet, Dictionary<string, MemberGetter> overridenValueGetters = null)
        {
            uint rowNum = 0;
            int colNum = 0, maxWidth = 0, minCol = 1, maxCol = properties == null ? minCol : properties.Count();
            maxCol = maxCol == 1 && columnHeaders == null ? 1 : columnHeaders.Count();

            if (string.IsNullOrEmpty(sheetName))
                sheetName = DefaultSheetName;

            SetSheetName(sheetName, spreadSheet);

            //if (styleSheet == null)
            //{
            //    SetStyleSheet(spreadSheet);
            //}
            //else
            //{
            //    spreadSheet.WorkbookPart.WorkbookStylesPart.Stylesheet = styleSheet;
            //    spreadSheet.WorkbookPart.WorkbookStylesPart.Stylesheet.Save();
            //}

            WorksheetPart worksheetPart = SpreadsheetReader.GetWorksheetPartByName(spreadSheet, sheetName);

            if (worksheetPart == null)
                worksheetPart = SpreadsheetWriter.InsertWorksheet(spreadSheet, sheetName);

            WriteHeaders(columnHeaders, out rowNum, out colNum, out maxWidth, spreadSheet, worksheetPart);

            AddCellWidthStyles(Convert.ToUInt32(minCol), Convert.ToUInt32(maxCol), maxWidth, spreadSheet, worksheetPart);

            if (properties == null || properties.Count() == 0)
                WriteRowsFromProperties(rowData, columnHeaders, rowNum, out maxWidth, spreadSheet, worksheetPart, overridenValueGetters);
            else
                WriteRowsFromProperties(rowData, properties, rowNum, out maxWidth, spreadSheet, worksheetPart, overridenValueGetters);

            worksheetPart.Worksheet.Save();
        }

        private static void SetSheetName(string excelSpreadSheetName, SpreadsheetDocument spreadSheet)
        {
            excelSpreadSheetName = excelSpreadSheetName ?? DefaultSheetName;
            Sheet ss = spreadSheet.WorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == excelSpreadSheetName);

            if (ss == null)
            {// Add a blank WorksheetPart.
                WorksheetPart newWorksheetPart = spreadSheet.WorkbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = spreadSheet.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                string relationshipId = spreadSheet.WorkbookPart.GetIdOfPart(newWorksheetPart);

                // Get a unique ID for the new worksheet.
                uint sheetId = 1;
                if (sheets.Elements<Sheet>().Count() > 0)
                {
                    sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }

                // Give the new worksheet a name.
                //string sheetName = "Sheet" + sheetId;

                // Append the new worksheet and associate it with the workbook.
                Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = excelSpreadSheetName };
                sheets.Append(sheet);

                ss = sheet;
            }

            ss.Name = excelSpreadSheetName;
        }

        private static void AddCellWidthStyles(uint minColumn, uint maxColumn, int maxWidth, SpreadsheetDocument spreadSheet,
        WorksheetPart workSheetPart)
        {
            Columns cols = new Columns(new Column() { CustomWidth = true, Min = minColumn, Max = maxColumn, Width = maxWidth, BestFit = false });
            workSheetPart.Worksheet.InsertBefore<Columns>(cols, workSheetPart.Worksheet.GetFirstChild<SheetData>());
        }

        private static void SetStyleSheet(SpreadsheetDocument spreadSheet)
        {
            Stylesheet styleSheet = spreadSheet.WorkbookPart.WorkbookStylesPart.Stylesheet;
            styleSheet.Fonts.AppendChild(new Font(new FontSize() { Val = 11 }, GetColor(System.Drawing.Color.White), new FontName() { Val = "Arial" }));
            styleSheet.Fills.AppendChild(new Fill()
            {
                PatternFill = new PatternFill()
                {
                    PatternType = PatternValues.Solid,
                    BackgroundColor = GetBackGroundColor(System.Drawing.Color.Black)
                }
            });
            spreadSheet.WorkbookPart.WorkbookStylesPart.Stylesheet.Save();
        }

        private static BackgroundColor GetBackGroundColor(System.Drawing.Color color)
        {
            return new BackgroundColor()
            {
                Rgb = new HexBinaryValue()
                {
                    Value =
                        System.Drawing.ColorTranslator.ToHtml(
                            System.Drawing.Color.FromArgb(
                                color.A,
                                color.R,
                                color.G,
                                color.B)).Replace("#", "")
                }
            };
        }

        private static Color GetColor(System.Drawing.Color color)
        {
            return new Color()
            {
                Rgb = new HexBinaryValue()
                {
                    Value =
                        System.Drawing.ColorTranslator.ToHtml(
                            System.Drawing.Color.FromArgb(
                                color.A,
                                color.R,
                                color.G,
                                color.B)).Replace("#", "")
                }
            };
        }

        private static void SetHeaderStyle(string cellLocation, SpreadsheetDocument spreadSheet, WorksheetPart workSheetPart)
        {
            Stylesheet styleSheet = spreadSheet.WorkbookPart.WorkbookStylesPart.Stylesheet;
            Cell cell = workSheetPart.Worksheet.Descendants<Cell>().FirstOrDefault(c => c.CellReference == cellLocation);
            if (cell == null)
            {
                throw new ArgumentNullException("Cell not found");
            }

            cell.SetAttribute(new OpenXmlAttribute("", "s", "", "1"));
            OpenXmlAttribute cellStyleAttribute = cell.GetAttribute("s", "");
            CellFormats cellFormats = spreadSheet.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats;

            CellFormat cellFormat = (CellFormat)cellFormats.First();
            CellFormat cf = new CellFormat(cellFormat.OuterXml);
            cf.FontId = styleSheet.Fonts.Count;
            cf.FillId = styleSheet.Fills.Count;
            cellFormats.AppendChild(cf);

            int a = (int)styleSheet.CellFormats.Count.Value;
            cell.SetAttribute(cellStyleAttribute);
            cell.StyleIndex = styleSheet.CellFormats.Count;

            workSheetPart.Worksheet.Save();
        }

        private static string ReplaceSpecialCharacters(string value)
        {
            return value.Replace("’", "'").Replace("“", "\"").Replace("”", "\"").Replace("–", "-").Replace("…", "...");
        }

        private static void WriteValues(string column, uint row, string value, SpreadsheetDocument spreadSheet, WorksheetPart worksheetPart)
        {
            if (value.Contains("$"))
            {
                value = value.Replace("$", "").Replace(",", "");
            }

            Cell cell = CreateNewCell(column, row, spreadSheet, worksheetPart);
            cell.CellValue = new CellValue(value);
            double test;
			DateTime dateTest;
            CellValues type;
            if (double.TryParse(value, out test))
                type = CellValues.Number;
            else if (DateTime.TryParse(value, out dateTest))
                type = CellValues.String;
            else
                type = CellValues.String;
            cell.DataType = new EnumValue<CellValues>(type);
        }

        private static Cell CreateNewCell(string column, uint row, SpreadsheetDocument spreadSheet, WorksheetPart worksheetPart)
        {
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            string cellReference = column + row.ToString();
            int columnIndex = SpreadsheetReader.GetColumnIndex(column);

            Row r = GetRow(sheetData, row);

            var existingCell = r.Elements<Cell>().FirstOrDefault(c => c.CellReference.Value == cellReference);
            if (existingCell != default(Cell))
                return existingCell;

            Cell refChild = null;
            foreach (Cell cell2 in r.Elements<Cell>())
            {
                if (SpreadsheetReader.GetColumnIndex(SpreadsheetReader.ColumnFromReference(cell2.CellReference.Value)) > columnIndex)
                {
                    refChild = cell2;
                    break;
                }
            }
            Cell newChild = new Cell();
            newChild.CellReference = cellReference;
            r.InsertBefore<Cell>(newChild, refChild);
            return newChild;
        }

        private static Row GetRow(SheetData sheetData, uint row)
        {
            var existingRow = sheetData.Elements<Row>().FirstOrDefault<Row>(r => r.RowIndex.Value == row);
            if (existingRow != default(Row))
                return existingRow;

            Row newChild = new Row();
            newChild.RowIndex = row;
            int count = 0;
            foreach (Row row2 in sheetData.Elements<Row>())
            {
                if (row2.RowIndex.Value > row)
                {
                    sheetData.InsertAt<Row>(newChild, count);
                    return newChild;
                }
                count++;
            }
            sheetData.Append(new OpenXmlElement[] { newChild });
            return newChild;
        }

        private static string GetColumnLetter(int column)
        {
            //All columns are at a 0 index. So starting second column at -1 to increment to 0.
            StringBuilder builder = new StringBuilder();
            int secondColumn = -1;
            //25 == z
            while (column > 25)
            {
                //Increment second column.
                secondColumn++;
                //Reset back to start at A 26 would be down to 0 which is A
                column -= 26;
            }
            //Print Second Column first.
            if (secondColumn > -1)
                builder.Append(Convert.ToChar(secondColumn + 65));
            //Printing out the incremented column
            builder.Append(Convert.ToChar(column + 65));
            return builder.ToString();
        }

        private static void WriteRowsFromProperties(IEnumerable rowData, IEnumerable<string> properties, uint row, out int maxWidth, SpreadsheetDocument spreadSheet, WorksheetPart workSheet, Dictionary<string, MemberGetter> overridenValueGetters = null)
        {
            Dictionary<string, MemberGetter> getters = new Dictionary<string, MemberGetter>();
            Type type = null;
            var e = rowData.GetEnumerator();
            maxWidth = 0;

            SheetData sheetData = workSheet.Worksheet.GetFirstChild<SheetData>();
            if (e.MoveNext())
            {
                type = e.Current.GetType();
                foreach (string property in properties)
                {
                    if (overridenValueGetters != null && overridenValueGetters.ContainsKey(property))
                        getters.Add(property, overridenValueGetters[property]);
                    else
                        getters.Add(property, type.DelegateForGetPropertyValue(property));
                }
                foreach (var obj in rowData)
                {
                    Row newRow = new Row() { RowIndex = row };
                    var spans = new ListValue<StringValue>();
                    spans.Items.Add(new StringValue(string.Format("1:{0}", properties.Count())));
                    newRow.Spans = spans;
                    sheetData.Append(newRow);
                    int column = 0;
                    foreach (string property in properties)
                    {
                        var value = getters[property](obj);
                        string strValue = value == null ? string.Empty : ReplaceSpecialCharacters(value.ToString());
                        if (strValue.Contains("$"))
                            strValue = strValue.Replace("$", string.Empty).Replace(",", string.Empty);
                        maxWidth = strValue.Length > maxWidth ? strValue.Length : maxWidth;

                        Cell cell = new Cell();
                        cell.CellReference = string.Format("{0}{1}", GetColumnLetter(column), row);
                        cell.CellValue = new CellValue(strValue);
                        double test;
						DateTime dateTest;
                        CellValues valueType;
                        if (double.TryParse(strValue, out test))
                            valueType = CellValues.Number;
						else if (DateTime.TryParse(strValue, out dateTest))
                            valueType = CellValues.String;
                        else
                            valueType = CellValues.String;
                        cell.DataType = new EnumValue<CellValues>(valueType);

                        newRow.Append(cell);

                        column++;
                    }
                    row++;
                }
            }
        }

        private static void WriteHeaders(IEnumerable<string> columnHeaders, out uint row, out int column, out int maxWidth, SpreadsheetDocument spreadSheet, WorksheetPart workSheet)
        {
            row = 1;
            column = 0;
            maxWidth = 0;
            SheetData sheetData = workSheet.Worksheet.GetFirstChild<SheetData>();
            Row newRow = new Row() { RowIndex = row };
            var spans = new ListValue<StringValue>();
            spans.Items.Add(new StringValue(string.Format("1:{0}", columnHeaders.Count())));
            sheetData.Append(newRow);
            foreach (string header in columnHeaders)
            {
                string headerText = ReplaceSpecialCharacters(header);
                maxWidth = headerText.Length > maxWidth ? headerText.Length : maxWidth;

                string cellReference = string.Format("{0}{1}", GetColumnLetter(column), row);

                Cell cell = new Cell();
                cell.CellReference = cellReference;
                cell.CellValue = new CellValue(headerText);
                cell.DataType = new EnumValue<CellValues>(CellValues.String);

                newRow.Append(cell);

                //SetHeaderStyle(cellReference, spreadSheet, workSheet);

                column++;
            }
            row++;
        }

    }
}
