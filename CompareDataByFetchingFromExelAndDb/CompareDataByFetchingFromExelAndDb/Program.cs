using CompareDataByFetchingFromExelAndDb.Context;
using CompareDataByFetchingFromExelAndDb.Entity;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompareDataByFetchingFromExelAndDb
{
	class Program
	{
		static List<VpResourceTranslationEntitiy> _vpResourceTranslationEntitiys = new();
		static int _valueOfNotMatch = 0;
		static void Main(string[] args)
		{
			
			bool isContinue = true;
			string processType = null;
			do
			{
				Console.Write("Please enter R/r for read from exel and E/e for end to process:");
				processType = Console.ReadLine();
				if (processType.ToLower() == "r")
				{
					_vpResourceTranslationEntitiys = GetValueListFromVpTranslationTable();
					FindDifferenceValueBetweenExelValueAndDb();
				}
				else if (processType.ToLower() == "e")
				{
					isContinue = false;
				}
				else
				{
					Console.WriteLine("Invalid key entered");
				}

			} while (isContinue);
		}
		static void FindDifferenceValueBetweenExelValueAndDb()
		{
			Console.Write("Please enter the file path of the exel= ");
			string excelFilePath = Console.ReadLine() + ".xlsx";
			using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(excelFilePath, false))
			{
				WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
				WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
				SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
				foreach (Row row in sheetData.Elements<Row>())
				{
					Cell idCell = row.Elements<Cell>().ElementAt(1);
					Cell valueCell = row.Elements<Cell>().ElementAt(5);
					string id = GetCellValue(workbookPart, idCell);
					string value = GetCellValue(workbookPart, valueCell);
					if (!CompareValuesBetweenDbAndExel(id, value))
					{
						Console.WriteLine(id);
						_valueOfNotMatch++;
					}
				}
			}
			Console.WriteLine("**********************Total Number ************************");
			Console.WriteLine(_valueOfNotMatch);
		}

		private static bool CompareValuesBetweenDbAndExel(string id, string value)
		{
			try
			{
				long entityId = long.Parse(id);
				var dbEntity = _vpResourceTranslationEntitiys.FirstOrDefault(x => x.Id == entityId);

				if (dbEntity != null)
				{
					// Veritabanındaki değeri Excel değeriyle karşılaştır
					if (dbEntity.Value == value || value.Trim() == "eyp")
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		private static string GetCellValue(WorkbookPart workbookPart, Cell cell)
		{
			SharedStringTablePart stringTablePart = workbookPart.SharedStringTablePart;

			if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
			{
				return stringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(int.Parse(cell.InnerText)).InnerText;
			}
			else
			{
				return cell.InnerText;
			}
		}
		private static List<VpResourceTranslationEntitiy> GetValueListFromVpTranslationTable()
		{
			using (var dbContext = new VpResourceTranslationDbContext())
			{
				var result = dbContext.VpResourceTranslation.Where(x => x.CultureCode == "de-DE").ToList();
				return result;
			}
		}
	}
}
