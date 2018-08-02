using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace MXJ.Core.Utility.File
{
    public static class ExcelHelper
    {
        static ExcelHelper()
        {
        }

        #region 保存为文件
        public static void SaveFile(string fileName, string sheetName, string header, List<string> titleList, List<List<string>> dataList)
        {
            if (!string.IsNullOrEmpty(sheetName) && !string.IsNullOrWhiteSpace(sheetName))
            {
                if (null != titleList && titleList.Count > 0)
                {
                    #region 创建excel sheet
                    IWorkbook workBook = new HSSFWorkbook();
                    ISheet sheet = workBook.CreateSheet(sheetName);
                    #endregion

                    #region 创建标头
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, titleList.Count - 1));
                    IRow firstRow = sheet.CreateRow(0);
                    //设置单元格的高度
                    firstRow.Height = 30 * 20;
                    ICell firstCell = firstRow.CreateCell(0);
                    firstCell.CellStyle = GetFirstRowStyle(workBook);
                    firstCell.SetCellValue(header);
                    #endregion

                    #region 创建字段
                    IRow titleRow = sheet.CreateRow(1);
                    titleRow.Height = 20 * 20;
                    var titleCellStyle = GetTitleStyle(workBook);
                    for (int i = 0; i < titleList.Count; i++)
                    {
                        var item = titleList[i];
                        ICell cell = titleRow.CreateCell(i);
                        //设置单元格的宽度
                        sheet.SetColumnWidth(i, 50 * 100);
                        cell.CellStyle = titleCellStyle;
                        cell.SetCellValue(item);
                    }
                    #endregion

                    #region 创建数据
                    var cellStyle = GetCellStyle(workBook);
                    for (int j = 0; j < dataList.Count; j++)
                    {
                        var list = dataList[j];
                        IRow row = sheet.CreateRow(j + 2);
                        row.Height = 20 * 20;
                        for (int c = 0; c < list.Count; c++)
                        {
                            var item = list[c];
                            ICell cell = row.CreateCell(c);
                            cell.CellStyle = cellStyle;
                            cell.SetCellValue(item);
                        }
                    }
                    #endregion

                    #region 进行导出
                    try
                    {
                        using (FileStream stream = System.IO.File.OpenWrite(fileName))
                        {

                            workBook.Write(stream);

                            stream.Close();

                        }


                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                    finally
                    {
                        workBook = null;
                    }
                    #endregion
                }
            }

        }

        #endregion

        #region 导出excle
        public static MemoryStream ExportFile(string sheetName, string header, List<string> titleList, List<List<string>> dataList)
        {
            if (!string.IsNullOrEmpty(sheetName) && !string.IsNullOrWhiteSpace(sheetName))
            {
                if (null != titleList && titleList.Count > 0)
                {
                    #region 创建excel sheet
                    IWorkbook workBook = new HSSFWorkbook();
                    ISheet sheet = workBook.CreateSheet(sheetName);
                    #endregion

                    #region 创建标头
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, titleList.Count - 1));
                    IRow firstRow = sheet.CreateRow(0);
                    //设置单元格的高度
                    firstRow.Height = 30 * 20;
                    ICell firstCell = firstRow.CreateCell(0);
                    firstCell.CellStyle = GetFirstRowStyle(workBook);
                    firstCell.SetCellValue(header);
                    #endregion

                    #region 创建字段
                    IRow titleRow = sheet.CreateRow(1);
                    titleRow.Height = 20 * 20;
                    var titleCellStyle = GetTitleStyle(workBook);
                    for (int i = 0; i < titleList.Count; i++)
                    {
                        var item = titleList[i];
                        ICell cell = titleRow.CreateCell(i);
                        //设置单元格的宽度
                        sheet.SetColumnWidth(i, 50 * 100);
                        cell.CellStyle = titleCellStyle;
                        cell.SetCellValue(item);
                    }
                    #endregion

                    #region 创建数据
                    var cellStyle = GetCellStyle(workBook);
                    for (int j = 0; j < dataList.Count; j++)
                    {
                        var list = dataList[j];
                        IRow row = sheet.CreateRow(j + 2);
                        row.Height = 20 * 20;
                        for (int c = 0; c < list.Count; c++)
                        {
                            var item = list[c];
                            ICell cell = row.CreateCell(c);
                            cell.CellStyle = cellStyle;
                            cell.SetCellValue(item);
                        }
                    }
                    #endregion

                    #region 进行导出
                    try
                    {
                        MemoryStream ms = new MemoryStream();
                        workBook.Write(ms);
                        return ms;
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                    finally
                    {
                        workBook = null;
                    }
                    #endregion
                }
            }
            return null;
        }

        public static MemoryStream ExportFile(string sheetName, string header, List<string> titleList, List<List<object>> dataList, object[] totalList = null, string totalTile = null, int endColumn = 1)
        {
            if (!string.IsNullOrEmpty(sheetName) && !string.IsNullOrWhiteSpace(sheetName))
            {
                if (null != titleList && titleList.Count > 0)
                {
                    #region 创建excel sheet
                    IWorkbook workBook = new HSSFWorkbook();
                    ISheet sheet = workBook.CreateSheet(sheetName);
                    #endregion

                    #region 创建标头
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, titleList.Count - 1));
                    IRow firstRow = sheet.CreateRow(0);
                    //设置单元格的高度
                    firstRow.Height = 30 * 20;
                    ICell firstCell = firstRow.CreateCell(0);
                    firstCell.CellStyle = GetFirstRowStyle(workBook);
                    firstCell.SetCellValue(header);
                    #endregion

                    #region 创建字段
                    IRow titleRow = sheet.CreateRow(1);
                    titleRow.Height = 20 * 20;
                    var titleCellStyle = GetTitleStyle(workBook);
                    for (int i = 0; i < titleList.Count; i++)
                    {
                        var item = titleList[i];
                        ICell cell = titleRow.CreateCell(i);
                        //设置单元格的宽度
                        sheet.SetColumnWidth(i, 50 * 100);
                        cell.CellStyle = titleCellStyle;
                        cell.CellStyle.Alignment = HorizontalAlignment.Center;
                        cell.SetCellValue(item);
                    }
                    #endregion

                    #region 创建数据
                    var cellStyle = GetCellStyle(workBook);
                    for (int j = 0; j < dataList.Count; j++)
                    {
                        var list = dataList[j];
                        IRow row = sheet.CreateRow(j + 2);
                        row.Height = 20 * 20;
                        for (int c = 0; c < list.Count; c++)
                        {
                            var item = list[c];
                            ICell cell = row.CreateCell(c);
                            cell.CellStyle = cellStyle;
                            cell.CellStyle.Alignment = HorizontalAlignment.Center;
                            if (item != null)
                            {
                                if (item is decimal)
                                {
                                    cell.SetCellType(CellType.Numeric);
                                    cell.SetCellValue(double.Parse(item.ToString()));
                                }
                                else
                                {
                                    cell.SetCellValue((item.ToString()));
                                }
                            }
                        }
                    }
                    #endregion

                    #region 统计数据
                    if (totalList != null)
                    {
                        IRow totalRow = sheet.CreateRow(dataList.Count + 2);
                        totalRow.Height = 20 * 20;
                        var totalCellStyle = GetTitleStyle(workBook);
                        for (int i = 0; i < totalList.Length; i++)
                        {
                            var item = totalList[i];
                            ICell cell = totalRow.CreateCell(i);
                            //设置单元格的宽度
                            sheet.SetColumnWidth(i, 50 * 100);
                            cell.CellStyle = titleCellStyle;
                            cell.CellStyle.Alignment = HorizontalAlignment.Center;
                            if (totalTile != null && i == 0)
                                cell.SetCellValue(totalTile);
                            if (item != null)
                                cell.SetCellValue(item.ToString());
                        }

                        //合并单元格
                        CellRangeAddress cellRangeAddress = new CellRangeAddress(dataList.Count + 2, dataList.Count + 2, 0, endColumn - 1);
                        sheet.AddMergedRegion(cellRangeAddress);
                    }
                    #endregion

                    #region 进行导出
                    try
                    {
                        MemoryStream ms = new MemoryStream();
                        workBook.Write(ms);
                        return ms;
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                    finally
                    {
                        workBook = null;
                    }
                    #endregion
                }
            }
            return null;
        }

        private static ICellStyle GetFirstRowStyle(IWorkbook book)
        {
            ICellStyle cellStyle = GetBaseCellStyle(book);
            IFont font = GetBaseFont(book);
            font.FontHeight = 25 * 10;
            font.Boldweight = short.MaxValue;
            cellStyle.SetFont(font);
            return cellStyle;
        }

        private static ICellStyle GetTitleStyle(IWorkbook book)
        {
            ICellStyle cellStyle = GetBaseCellStyle(book);
            IFont font = GetBaseFont(book);
            font.Boldweight = short.MaxValue;
            cellStyle.SetFont(font);
            return cellStyle;
        }

        private static ICellStyle GetCellStyle(IWorkbook book)
        {
            ICellStyle cellStyle = GetBaseCellStyle(book);
            IFont font = GetBaseFont(book);
            cellStyle.SetFont(font);
            return cellStyle;
        }

        private static ICellStyle GetBaseCellStyle(IWorkbook book)
        {
            ICellStyle cellStyle = book.CreateCellStyle();
            //设置单元格的样式：水平对齐居中
            cellStyle.Alignment = HorizontalAlignment.CenterSelection;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            return cellStyle;
        }

        private static IFont GetBaseFont(IWorkbook book)
        {
            IFont baseFont = book.CreateFont();
            baseFont.FontHeight = 20 * 10;
            return baseFont;
        }

        public static EexcleDictionaryResult TestReadExcelDictionary(Stream stream, List<List<string>> dataList)
        {
            EexcleDictionaryResult result = new EexcleDictionaryResult();
            result.IsLoadSuccess = false;
            try
            {
                IWorkbook workbook = GetWorkBook(stream);
                if (null != workbook)
                {
                    ISheet sheet = workbook.GetSheetAt(0);//读取到指定的sheet
                    result.SheetName = sheet.SheetName;
                    IRow headerRow = sheet.GetRow(0);//获取第一行，一般为表头
                    ICell headerCell = headerRow.GetCell(0);
                    if (null != headerCell)
                        result.Header = FilterTitle(headerCell + string.Empty);
                    IRow titleRow = sheet.GetRow(1);//得到属性名字行
                    result.TitleList = new List<string>();
                    if (null != titleRow)
                    {
                        foreach (ICell item in titleRow.Cells)
                        {
                            string title = item + string.Empty;
                            if (!string.IsNullOrEmpty(title) && !string.IsNullOrWhiteSpace(title))
                                result.TitleList.Add(FilterTitle(title));
                        }
                    }
                    result.ExcelDataList = new List<IDictionary<string, object>>();
                    #region 创建数据
                    var cellStyle = GetCellStyle(workbook);
                    for (int j = 0; j < dataList.Count; j++)
                    {
                        var list = dataList[j];
                        IRow row = sheet.CreateRow(j + 2);
                        row.Height = 20 * 20;
                        for (int c = 0; c < list.Count; c++)
                        {
                            var item = list[c];
                            ICell cell = row.CreateCell(c);
                            cell.CellStyle = cellStyle;
                            cell.SetCellValue(item);
                        }
                    }
                    #endregion

                    #region 进行导出
                    try
                    {
                        MemoryStream ms = new MemoryStream();
                        workbook.Write(ms);
                        //return ms;
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                    finally
                    {
                        workbook = null;
                    }
                    #endregion
                    result.IsLoadSuccess = (result.ExcelDataList.Count > 0);
                }
            }
            catch (Exception exc)
            {
                result.IsLoadSuccess = false;
                throw exc;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
                stream = null;
            }
            return result;
        }
        #endregion

        #region 读取excel数据

        private static IWorkbook GetWorkBook(Stream stream)
        {
            IWorkbook workbook = null;
            workbook = WorkbookFactory.Create(stream);
            return workbook;
        }

        /// <summary>
        /// 读取Excel文件流
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="header">表头</param>
        /// <param name="titleList">属性列名字</param>
        /// <param name="excelDataList">数据</param>
        /// <returns></returns>
        public static EexcleListResult ReadExcelList(Stream stream)
        {
            EexcleListResult result = new EexcleListResult();
            result.IsLoadSuccess = false;
            try
            {
                IWorkbook workbook = GetWorkBook(stream);
                if (null != workbook)
                {
                    ISheet sheet = workbook.GetSheetAt(0);//读取到指定的sheet
                    result.SheetName = sheet.SheetName;
                    IRow headerRow = sheet.GetRow(0);//获取第一行，一般为表头
                    ICell headerCell = headerRow.GetCell(0);
                    if (null != headerCell)
                        result.Header = FilterTitle(headerCell + string.Empty);
                    IRow titleRow = sheet.GetRow(1);//得到属性名字行
                    result.TitleList = new List<string>();
                    if (null != titleRow)
                    {
                        foreach (ICell item in titleRow.Cells)
                        {
                            string title = item + string.Empty;
                            if (!string.IsNullOrEmpty(title) && !string.IsNullOrWhiteSpace(title))
                                result.TitleList.Add(FilterTitle(title));
                        }
                    }
                    result.ExcelDataList = new List<List<string>>();
                    var firstRowCount = sheet.FirstRowNum + 2;
                    for (int i = firstRowCount; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);//得到一行
                        if (null == row) { break; }
                        if (row.LastCellNum < result.TitleList.Count) { break; }
                        List<string> item = new List<string>();
                        int emptyIndex = 0;
                        for (int j = row.FirstCellNum; j < result.TitleList.Count; j++)
                        {
                            ICell cell = row.GetCell(j);//得到cell
                            if (null != cell)
                            {
                                switch (cell.CellType)
                                {
                                    case CellType.Unknown: item.Add((cell + string.Empty).Trim()); break;
                                    case CellType.Blank: item.Add((cell + string.Empty).Trim()); emptyIndex++; continue;
                                    case CellType.Boolean: item.Add((cell.BooleanCellValue + string.Empty).Trim()); break;
                                    case CellType.String: item.Add((cell.StringCellValue + string.Empty).Trim()); break;
                                    case CellType.Error: item.Add(ErrorEval.GetText(cell.ErrorCellValue).Trim()); break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(cell))
                                        {
                                            var dt = cell.DateCellValue;
                                            if (null != dt && DateTime.MinValue != dt)
                                                item.Add(dt.ToString("yyyy-MM-dd hh:mm:ss").Trim());
                                        }
                                        else { item.Add((cell.NumericCellValue + string.Empty).Trim()); }
                                        break;
                                    case CellType.Formula:
                                        switch (cell.CachedFormulaResultType)
                                        {
                                            case CellType.Unknown: item.Add((cell.CellFormula + string.Empty).Trim()); break;
                                            case CellType.Blank: item.Add((cell + string.Empty).Trim()); continue;
                                            case CellType.Boolean: item.Add((cell.BooleanCellValue + string.Empty).Trim()); break;
                                            case CellType.String: item.Add((cell.StringCellValue + string.Empty).Trim()); break;
                                            case CellType.Error: item.Add(ErrorEval.GetText(cell.ErrorCellValue).Trim()); break;
                                            case CellType.Numeric:
                                                if (DateUtil.IsCellDateFormatted(cell))
                                                {
                                                    var dt = cell.DateCellValue;
                                                    if (null != dt && DateTime.MinValue != dt)
                                                        item.Add(dt.ToString("yyyy-MM-dd hh:mm:ss").Trim());
                                                }
                                                else { item.Add((cell.NumericCellValue + string.Empty).Trim()); }
                                                break;
                                        }
                                        break;
                                }
                            }
                        }
                        if (emptyIndex < result.TitleList.Count)
                        {
                            if (item.Count >= result.TitleList.Count)
                            {
                                if (item.Count(s => string.IsNullOrEmpty(s + string.Empty)) < result.TitleList.Count)
                                {
                                    result.ExcelDataList.Add(item);
                                }
                            }
                        }
                    }
                    result.IsLoadSuccess = (result.ExcelDataList.Count > 0);
                }
            }
            catch (Exception exc)
            {
                result.IsLoadSuccess = false;
                throw exc;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
                stream = null;
            }
            return result;
        }

        /// <summary>
        /// 读取Excel文件流
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="header">表头</param>
        /// <param name="titleList">属性列名字</param>
        /// <param name="excelDataList">数据</param>
        /// <returns></returns>
        public static EexcleDictionaryResult ReadExcelDictionaryNoTitle(Stream stream)
        {
            EexcleDictionaryResult result = new EexcleDictionaryResult();
            result.IsLoadSuccess = false;
            try
            {
                IWorkbook workbook = GetWorkBook(stream);
                if (null != workbook)
                {
                    ISheet sheet = workbook.GetSheetAt(0);//读取到指定的sheet
                    result.SheetName = sheet.SheetName;
                    IRow titleRow = sheet.GetRow(0);//得到属性名字行
                    result.TitleList = new List<string>();
                    if (null != titleRow)
                    {
                        foreach (ICell item in titleRow.Cells)
                        {
                            string title = item + string.Empty;
                            if (!string.IsNullOrEmpty(title) && !string.IsNullOrWhiteSpace(title))
                                result.TitleList.Add(FilterTitle(title));
                        }
                    }
                    result.ExcelDataList = new List<IDictionary<string, object>>();
                    //遍历读取cell(sheet.FirstRowNum + 1)
                    var firstRowCount = sheet.FirstRowNum + 1;
                    for (int i = firstRowCount; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);//得到一行
                        if (null == row)
                        {
                            continue;
                        }

                        IDictionary<string, object> item = new Dictionary<string, object>();
                        int emptyIndex = 0;
                        for (int j = row.FirstCellNum; j < result.TitleList.Count; j++)
                        {
                            ICell cell = row.GetCell(j);//得到cell
                            string title = FilterTitle(result.TitleList[j]);//得到标题cell
                            if (cell != null && !string.IsNullOrEmpty(title))
                            {
                                switch (cell.CellType)
                                {
                                    case CellType.Unknown: item.Add(title, (cell + string.Empty).Trim()); break;
                                    case CellType.Blank: item.Add(title, (cell + string.Empty).Trim()); emptyIndex++; continue;//
                                    case CellType.Boolean: item.Add(title, (cell.BooleanCellValue + string.Empty).Trim()); break;
                                    case CellType.String: item.Add(title, (cell.StringCellValue + string.Empty).Trim()); break;
                                    case CellType.Error: item.Add(title, ErrorEval.GetText(cell.ErrorCellValue).Trim()); break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(cell))
                                        {
                                            var dt = cell.DateCellValue;
                                            if (null != dt && DateTime.MinValue != dt)
                                                item.Add(title, dt);
                                        }
                                        else { item.Add(title, (cell.NumericCellValue + string.Empty).Trim()); }
                                        break;
                                    case CellType.Formula:
                                        switch (cell.CachedFormulaResultType)
                                        {
                                            case CellType.Unknown: item.Add(title, (cell.CellFormula + string.Empty).Trim()); break;
                                            case CellType.Blank: item.Add(title, (cell + string.Empty).Trim()); continue;//
                                            case CellType.Boolean: item.Add(title, (cell.BooleanCellValue + string.Empty).Trim()); break;
                                            case CellType.String: item.Add(title, (cell.StringCellValue + string.Empty).Trim()); break;
                                            case CellType.Error: item.Add(title, ErrorEval.GetText(cell.ErrorCellValue).Trim()); break;
                                            case CellType.Numeric:
                                                if (DateUtil.IsCellDateFormatted(cell))
                                                {
                                                    var dt = cell.DateCellValue;
                                                    if (null != dt && DateTime.MinValue != dt)
                                                        item.Add(title, (dt.ToString("yyyy-MM-dd hh:mm:ss")).Trim());
                                                }
                                                else { item.Add(title, (cell.NumericCellValue + string.Empty).Trim()); }
                                                break;
                                        }
                                        break;
                                }
                            }
                        }
                        if (emptyIndex < result.TitleList.Count)
                        {
                            result.ExcelDataList.Add(item);
                        }

                    }
                    result.IsLoadSuccess = (result.ExcelDataList.Count > 0);
                }
            }
            catch (Exception exc)
            {
                result.IsLoadSuccess = false;
                throw exc;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
                stream = null;
            }
            return result;
        }


        /// <summary>
        /// 读取Excel文件流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="prams"></param>
        /// <returns></returns>
        public static Dictionary<string, EexcleDictionaryResult> ReadExcelDictionaryForManySheets(Stream stream, params string[] prams)
        {
            Dictionary<string, EexcleDictionaryResult> results = new Dictionary<string, EexcleDictionaryResult>();
            if (prams != null)
            {
                IWorkbook workbook = GetWorkBook(stream);
                try
                {
                    if (null != workbook)
                    {
                        foreach (var key in prams)
                        {
                            EexcleDictionaryResult result = new EexcleDictionaryResult();
                            result.IsLoadSuccess = false;
                            ISheet sheet = workbook.GetSheet(key);//读取到指定的sheet
                            result.SheetName = sheet.SheetName;
                            IRow headerRow = sheet.GetRow(0);//获取第一行，一般为表头
                            ICell headerCell = headerRow.GetCell(0);
                            if (null != headerCell)
                            {
                                result.Header = FilterTitle(headerCell + string.Empty);
                            }

                            IRow titleRow = sheet.GetRow(1);//得到属性名字行
                            result.TitleList = new List<string>();
                            if (null != titleRow)
                            {
                                foreach (ICell item in titleRow.Cells)
                                {
                                    string title = item + string.Empty;
                                    if (!string.IsNullOrEmpty(title) && !string.IsNullOrWhiteSpace(title))
                                    {
                                        result.TitleList.Add(FilterTitle(title));
                                    }
                                }
                            }

                            result.ExcelDataList = new List<IDictionary<string, object>>();
                            var firstRowCount = sheet.FirstRowNum + 2;
                            for (int i = firstRowCount; i <= sheet.LastRowNum; i++)
                            {
                                IRow row = sheet.GetRow(i);//得到一行
                                if (null == row) { break; }
                                IDictionary<string, object> item = new Dictionary<string, object>();
                                for (int j = row.FirstCellNum; j < result.TitleList.Count; j++)
                                {
                                    ICell cell = row.GetCell(j);//得到cell
                                    string title = FilterTitle(result.TitleList[j]);//得到标题cell
                                    if (cell != null && !string.IsNullOrEmpty(title))
                                    {
                                        switch (cell.CellType)
                                        {
                                            case CellType.Unknown: item.Add(title, (cell + string.Empty).Trim()); break;
                                            case CellType.Blank: item.Add(title, (cell + string.Empty).Trim()); continue;//emptyIndex++;
                                            case CellType.Boolean: item.Add(title, (cell.BooleanCellValue + string.Empty).Trim()); break;
                                            case CellType.String: item.Add(title, (cell.StringCellValue + string.Empty).Trim()); break;
                                            case CellType.Error: item.Add(title, ErrorEval.GetText(cell.ErrorCellValue).Trim()); break;
                                            case CellType.Numeric:
                                                if (DateUtil.IsCellDateFormatted(cell))
                                                {
                                                    var dt = cell.DateCellValue;
                                                    if (null != dt && DateTime.MinValue != dt)
                                                    {
                                                        item.Add(title, (dt.ToString("yyyy-MM-dd hh:mm:ss")).Trim());
                                                    }
                                                }
                                                else
                                                {
                                                    item.Add(title, (cell.NumericCellValue + string.Empty).Trim());
                                                }

                                                break;
                                            case CellType.Formula:
                                                switch (cell.CachedFormulaResultType)
                                                {
                                                    case CellType.Unknown: item.Add(title, (cell.CellFormula + string.Empty).Trim()); break;
                                                    case CellType.Blank: item.Add(title, (cell + string.Empty).Trim()); continue;//
                                                    case CellType.Boolean: item.Add(title, (cell.BooleanCellValue + string.Empty).Trim()); break;
                                                    case CellType.String: item.Add(title, (cell.StringCellValue + string.Empty).Trim()); break;
                                                    case CellType.Error: item.Add(title, ErrorEval.GetText(cell.ErrorCellValue).Trim()); break;
                                                    case CellType.Numeric:
                                                        if (DateUtil.IsCellDateFormatted(cell))
                                                        {
                                                            var dt = cell.DateCellValue;
                                                            if (null != dt && DateTime.MinValue != dt)
                                                            {
                                                                item.Add(title, (dt.ToString("yyyy-MM-dd hh:mm:ss")).Trim());
                                                            }
                                                        }

                                                        else
                                                        {
                                                            item.Add(title, (cell.NumericCellValue + string.Empty).Trim());
                                                        }

                                                        break;
                                                }
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        item.Add(title, (string.Empty + string.Empty).Trim());
                                    }
                                }

                                if (item.Values.Count >= result.TitleList.Count)
                                {
                                    if (item.Values.Count(s => string.IsNullOrEmpty(s + string.Empty)) < result.TitleList.Count)
                                    {
                                        result.ExcelDataList.Add(item);
                                    }
                                }
                            }

                            result.IsLoadSuccess = (result.ExcelDataList.Count > 0);

                            results.Add(key, result);
                        }
                    }
                }
                catch (Exception exc)
                {
                    throw exc;
                }
                finally
                {
                    stream.Close();
                    stream.Dispose();
                    stream = null;
                }
            }

            return results;
        }

        /// <summary>
        /// 读取Excel文件流
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="header">表头</param>
        /// <param name="titleList">属性列名字</param>
        /// <param name="excelDataList">数据</param>
        /// <returns></returns>
        public static EexcleDictionaryResult ReadExcelDictionary(Stream stream)
        {
            EexcleDictionaryResult result = new EexcleDictionaryResult();
            result.IsLoadSuccess = false;
            try
            {
                IWorkbook workbook = GetWorkBook(stream);
                if (null != workbook)
                {
                    ISheet sheet = workbook.GetSheetAt(0);//读取到指定的sheet
                    result.SheetName = sheet.SheetName;
                    IRow headerRow = sheet.GetRow(0);//获取第一行，一般为表头
                    ICell headerCell = headerRow.GetCell(0);
                    if (null != headerCell)
                        result.Header = FilterTitle(headerCell + string.Empty);
                    IRow titleRow = sheet.GetRow(1);//得到属性名字行
                    result.TitleList = new List<string>();
                    if (null != titleRow)
                    {
                        foreach (ICell item in titleRow.Cells)
                        {
                            string title = item + string.Empty;
                            if (!string.IsNullOrEmpty(title) && !string.IsNullOrWhiteSpace(title))
                                result.TitleList.Add(FilterTitle(title));
                        }
                    }
                    result.ExcelDataList = new List<IDictionary<string, object>>();
                    //遍历读取cell(sheet.FirstRowNum + 1)
                    var firstRowCount = sheet.FirstRowNum + 2;
                    for (int i = firstRowCount; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);//得到一行
                        if (null == row) { break; }
                        //if (row.LastCellNum < result.TitleList.Count) { break; }
                        IDictionary<string, object> item = new Dictionary<string, object>();
                        //int emptyIndex = 0;
                        for (int j = row.FirstCellNum; j < result.TitleList.Count; j++)
                        {
                            ICell cell = row.GetCell(j);//得到cell
                            string title = FilterTitle(result.TitleList[j]);//得到标题cell
                            if (cell != null && !string.IsNullOrEmpty(title))
                            {
                                switch (cell.CellType)
                                {
                                    case CellType.Unknown: item.Add(title, (cell + string.Empty).Trim()); break;
                                    case CellType.Blank: item.Add(title, (cell + string.Empty).Trim()); continue;//emptyIndex++;
                                    case CellType.Boolean: item.Add(title, (cell.BooleanCellValue + string.Empty).Trim()); break;
                                    case CellType.String: item.Add(title, (cell.StringCellValue + string.Empty).Trim()); break;
                                    case CellType.Error: item.Add(title, ErrorEval.GetText(cell.ErrorCellValue).Trim()); break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(cell))
                                        {
                                            var dt = cell.DateCellValue;
                                            if (null != dt && DateTime.MinValue != dt)
                                                item.Add(title, (dt.ToString("yyyy-MM-dd hh:mm:ss")).Trim());
                                        }
                                        else { item.Add(title, (cell.NumericCellValue + string.Empty).Trim()); }
                                        break;
                                    case CellType.Formula:
                                        switch (cell.CachedFormulaResultType)
                                        {
                                            case CellType.Unknown: item.Add(title, (cell.CellFormula + string.Empty).Trim()); break;
                                            case CellType.Blank: item.Add(title, (cell + string.Empty).Trim()); continue;//
                                            case CellType.Boolean: item.Add(title, (cell.BooleanCellValue + string.Empty).Trim()); break;
                                            case CellType.String: item.Add(title, (cell.StringCellValue + string.Empty).Trim()); break;
                                            case CellType.Error: item.Add(title, ErrorEval.GetText(cell.ErrorCellValue).Trim()); break;
                                            case CellType.Numeric:
                                                if (DateUtil.IsCellDateFormatted(cell))
                                                {
                                                    var dt = cell.DateCellValue;
                                                    if (null != dt && DateTime.MinValue != dt)
                                                        item.Add(title, (dt.ToString("yyyy-MM-dd hh:mm:ss")).Trim());
                                                }
                                                else { item.Add(title, (cell.NumericCellValue + string.Empty).Trim()); }
                                                break;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                item.Add(title, (string.Empty + string.Empty).Trim());
                            }
                        }
                        //if (emptyIndex < result.TitleList.Count) 
                        //{
                        if (item.Values.Count >= result.TitleList.Count)
                        {
                            if (item.Values.Count(s => string.IsNullOrEmpty(s + string.Empty)) < result.TitleList.Count)
                            {
                                result.ExcelDataList.Add(item);
                            }
                        }
                        // }

                    }
                    result.IsLoadSuccess = (result.ExcelDataList.Count > 0);
                }
            }
            catch (Exception exc)
            {
                result.IsLoadSuccess = false;
                throw exc;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
                stream = null;
            }
            return result;
        }

        /// <summary>
        /// 读取Excel文件流
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="titleNum">数据行列头所属行(索引从0开始)</param>
        /// <param name="lineNum">数据表头所占行数</param>
        /// <returns></returns>
        public static EexcleDictionaryResult ReadExcelDictionary(Stream stream, int titleNum = 1, int lineNum = 1)
        {
            EexcleDictionaryResult result = new EexcleDictionaryResult();
            result.IsLoadSuccess = false;
            try
            {
                IWorkbook workbook = GetWorkBook(stream);
                if (null != workbook)
                {
                    ISheet sheet = workbook.GetSheetAt(0);//读取到指定的sheet
                    result.SheetName = sheet.SheetName;

                    #region 标题行
                    if (titleNum > 0)
                    {
                        IRow headerRow = sheet.GetRow(0);//获取第一行，一般为表头
                        ICell headerCell = headerRow.GetCell(0);
                        if (null != headerCell)
                            result.Header = FilterTitle(headerCell + string.Empty);
                    }
                    #endregion

                    #region 参数行
                    if (titleNum > 1)
                    {
                        IRow titleRow1;
                        string title, value;
                        result.Params = new Dictionary<string, object>();
                        for (int i = 1; i < titleNum; i++)
                        {
                            titleRow1 = sheet.GetRow(i);//得到参数行
                            if (null != titleRow1)
                            {
                                for (int r = 0; r < titleRow1.Cells.Count; r = r + 2)
                                {
                                    title = titleRow1.Cells[r] + string.Empty;
                                    //参数标题为空的，均不收集相关数据
                                    if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
                                        continue;
                                    value = titleRow1.Cells[r + 1] + string.Empty;
                                    result.Params.Add(title, value);
                                }
                            }
                        }

                    }
                    #endregion

                    #region 列头
                    result.TitleList = new List<string>();
                    result.TitleList = GetDataColumnsHeader(sheet, titleNum, lineNum);                    
                    #endregion

                    result.ExcelDataList = new List<IDictionary<string, object>>();
                    //遍历读取cell(sheet.FirstRowNum + 1)
                    var firstRowCount = sheet.FirstRowNum + (2 + titleNum - 1 + lineNum - 1);//设置数据行的开始索引
                    for (int i = firstRowCount; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);//得到一行
                        if (null == row) { break; }
                        //if (row.LastCellNum < result.TitleList.Count) { break; }
                        IDictionary<string, object> item = new Dictionary<string, object>();
                        //int emptyIndex = 0;
                        for (int j = row.FirstCellNum; j < result.TitleList.Count; j++)
                        {
                            ICell cell = row.GetCell(j);//得到cell
                            string title = FilterTitle(result.TitleList[j]);//得到标题cell
                            if (cell != null && !string.IsNullOrEmpty(title))
                            {
                                switch (cell.CellType)
                                {
                                    case CellType.Unknown: item.Add(title, (cell + string.Empty).Trim()); break;
                                    case CellType.Blank: item.Add(title, (cell + string.Empty).Trim()); continue;//emptyIndex++;
                                    case CellType.Boolean: item.Add(title, (cell.BooleanCellValue + string.Empty).Trim()); break;
                                    case CellType.String: item.Add(title, (cell.StringCellValue + string.Empty).Trim()); break;
                                    case CellType.Error: item.Add(title, ErrorEval.GetText(cell.ErrorCellValue).Trim()); break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(cell))
                                        {
                                            var dt = cell.DateCellValue;
                                            if (null != dt && DateTime.MinValue != dt)
                                                item.Add(title, (dt.ToString("yyyy-MM-dd HH:mm:ss")).Trim());
                                        }
                                        else { item.Add(title, (cell.NumericCellValue + string.Empty).Trim()); }
                                        break;
                                    case CellType.Formula:
                                        switch (cell.CachedFormulaResultType)
                                        {
                                            case CellType.Unknown: item.Add(title, (cell.CellFormula + string.Empty).Trim()); break;
                                            case CellType.Blank: item.Add(title, (cell + string.Empty).Trim()); continue;//
                                            case CellType.Boolean: item.Add(title, (cell.BooleanCellValue + string.Empty).Trim()); break;
                                            case CellType.String: item.Add(title, (cell.StringCellValue + string.Empty).Trim()); break;
                                            case CellType.Error: item.Add(title, ErrorEval.GetText(cell.ErrorCellValue).Trim()); break;
                                            case CellType.Numeric:
                                                if (DateUtil.IsCellDateFormatted(cell))
                                                {
                                                    var dt = cell.DateCellValue;
                                                    if (null != dt && DateTime.MinValue != dt)
                                                        item.Add(title, (dt.ToString("yyyy-MM-dd HH:mm:ss")).Trim());
                                                }
                                                else { item.Add(title, (cell.NumericCellValue + string.Empty).Trim()); }
                                                break;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                item.Add(title, (string.Empty + string.Empty).Trim());
                            }
                        }
                        //if (emptyIndex < result.TitleList.Count) 
                        //{
                        if (item.Values.Count >= result.TitleList.Count)
                        {
                            if (item.Values.Count(s => string.IsNullOrEmpty(s + string.Empty)) < result.TitleList.Count)
                            {
                                result.ExcelDataList.Add(item);
                            }
                        }
                        // }

                    }
                    result.IsLoadSuccess = (result.ExcelDataList.Count > 0);
                }
            }
            catch (Exception exc)
            {
                result.IsLoadSuccess = false;
                throw exc;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
                stream = null;
            }
            return result;
        }

        /// <summary>
        /// 获取excel数据列列头
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="titleNum">数据表头索引行</param>
        /// <param name="lineNum">数据表头所占行数</param>
        /// <returns></returns>
        private static List<string> GetDataColumnsHeader(ISheet sheet, int titleNum = 1, int lineNum = 1)
        {
            List<string> headers = new List<string>();
            Dictionary<int, string> dic = new Dictionary<int, string>();
            IRow titleRow;//得到属性名字行
            int num = 0;
            for (int i = 0; i < lineNum; i++)
            {
                titleRow = sheet.GetRow(titleNum + i);//得到属性名字行
                if (null != titleRow)
                {
                    num = 0;
                    foreach (ICell item in titleRow.Cells)
                    {
                        string title = item + string.Empty;
                        if (!string.IsNullOrEmpty(title) && !string.IsNullOrWhiteSpace(title))
                        {
                            dic.Remove(num);
                            dic.Add(num, title);
                        }
                        num++;
                    }
                }
            }
            foreach (var item in dic.OrderBy(d => d.Key))
            {
                headers.Add(item.Value);
            }
            return headers;
        }

        /// <summary>
        /// 根据文件路径读取Excel文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="header">表头</param>
        /// <param name="titleList">>属性列名字</param>
        /// <param name="excelDataList">数据</param>
        /// <returns></returns>
        public static EexcleDictionaryResult ReadExcelDictionary(string path)
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fs.Position = 0;
            using (fs)
            {
                return ReadExcelDictionary(fs);
            }
        }

        /// <summary>
        /// 判断合并单元格重载，调用时要在输出变量前加 out
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNum"></param>
        /// <param name="colNum"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colSpan"></param>
        /// <returns></returns>
        public static bool IsMergeCell(ISheet sheet, int rowNum, int colNum, out int rowSpan, out int colSpan)
        {
            bool result = false;
            rowSpan = 0;
            colSpan = 0;
            if ((rowNum < 1) || (colNum < 1)) return result;
            int rowIndex = rowNum - 1;
            int colIndex = colNum - 1;
            int regionsCount = sheet.NumMergedRegions;
            rowSpan = 1;
            colSpan = 1;
            for (int i = 0; i < regionsCount; i++)
            {
                CellRangeAddress range = sheet.GetMergedRegion(i);
                sheet.IsMergedRegion(range);
                if (range.FirstRow == rowIndex && range.FirstColumn == colIndex)
                {
                    rowSpan = range.LastRow - range.FirstRow + 1;
                    colSpan = range.LastColumn - range.FirstColumn + 1;
                    break;
                }
            }
            try
            {
                result = sheet.GetRow(rowIndex).GetCell(colIndex).IsMergedCell;
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return result;
        }

        /// <summary>
        /// 过滤标题列符号
        /// </summary>
        /// <param name="title">标题</param>
        /// <returns></returns>
        public static string FilterTitle(string title)
        {
            if (!string.IsNullOrEmpty(title) && !string.IsNullOrWhiteSpace(title))
            {
                title = title.Replace("*", string.Empty).Trim();
                title = Regex.Replace(title, @"\s{1,}", string.Empty, RegexOptions.IgnoreCase).Trim();
                title = Regex.Replace(title, @"\n\s*\r\n", string.Empty, RegexOptions.IgnoreCase).Trim();
                return title.Trim();
            }
            return string.Empty;
        }

        /// <summary>
        /// 读取用户数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static List<List<string>> ReadUserExcelList(Stream stream)
        {
            List<List<string>> list = null;
            try
            {
                IWorkbook workbook = new XSSFWorkbook(stream);
                if (workbook != null)
                {
                    ISheet sheet = workbook.GetSheetAt(0);//读取到指定的sheet
                    IRow titleRow = sheet.GetRow(1);//得到属性名字行
                    List<string> titleList = new List<string>();
                    if (null != titleRow)
                    {
                        foreach (ICell item in titleRow.Cells)
                        {
                            string title = item + string.Empty;
                            if (!string.IsNullOrEmpty(title) && !string.IsNullOrWhiteSpace(title))
                                titleList.Add(FilterTitle(title));
                        }
                    }
                    list = new List<List<string>>();
                    var firstRowCount = sheet.FirstRowNum + 2;
                    for (int i = firstRowCount; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);//得到一行
                        if (null == row) { break; }
                        if (row.LastCellNum < titleList.Count) { break; }
                        List<string> item = new List<string>();
                        int emptyIndex = 0;
                        for (int j = row.FirstCellNum; j < titleList.Count; j++)
                        {
                            ICell cell = row.GetCell(j);//得到cell
                            if (null != cell)
                            {
                                switch (cell.CellType)
                                {
                                    case CellType.Unknown: item.Add(cell + string.Empty); break;
                                    case CellType.Blank: item.Add(cell + string.Empty); emptyIndex++; continue;
                                    case CellType.Boolean: item.Add(cell.BooleanCellValue + string.Empty); break;
                                    case CellType.String: item.Add(cell.StringCellValue + string.Empty); break;
                                    case CellType.Error: item.Add(ErrorEval.GetText(cell.ErrorCellValue)); break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(cell))
                                        {
                                            var dt = cell.DateCellValue;
                                            if (null != dt && DateTime.MinValue != dt)
                                                item.Add(dt.ToString("yyyy-MM-dd hh:mm:ss"));
                                        }
                                        else { item.Add(cell.NumericCellValue + string.Empty); }
                                        break;
                                    case CellType.Formula:
                                        switch (cell.CachedFormulaResultType)
                                        {
                                            case CellType.Unknown: item.Add(cell.CellFormula + string.Empty); break;
                                            case CellType.Blank: item.Add(cell + string.Empty); continue;
                                            case CellType.Boolean: item.Add(cell.BooleanCellValue + string.Empty); break;
                                            case CellType.String: item.Add(cell.StringCellValue + string.Empty); break;
                                            case CellType.Error: item.Add(ErrorEval.GetText(cell.ErrorCellValue)); break;
                                            case CellType.Numeric:
                                                if (DateUtil.IsCellDateFormatted(cell)) { item.Add(cell.DateCellValue.ToString("yyyy-MM-dd hh:mm:ss") + string.Empty); }
                                                else { item.Add(cell.NumericCellValue + string.Empty); }
                                                break;
                                        }
                                        break;
                                }
                            }
                        }
                        if (emptyIndex < titleList.Count)
                            list.Add(item);
                    }
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                stream.Flush();
                stream.Close();
                stream.Dispose();
                stream = null;
            }
            return list;
        }
        #endregion

        public class EexcleListResult : ExcelResultBase
        {
            public List<List<string>> ExcelDataList { get; set; }
        }

        public class EexcleDictionaryResult : ExcelResultBase
        {
            public List<IDictionary<string, object>> ExcelDataList { get; set; }
        }

        public class ExcelResultBase
        {
            public bool IsLoadSuccess { get; set; }

            public string SheetName { get; set; }

            public string Header { get; set; }

            public List<string> TitleList { get; set; }

            public Dictionary<string, object> Params { get; set; }
        }
    }
}
