using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MJAPI.Controllers
{
    public class FileDownController : Controller
    {

        private static ICellStyle stylecenter = null;
        private static ICellStyle stylecenter2 = null;
        //
        // GET: /FileDown/

        public ActionResult Index()
        {
            return View();
        }

        public string Hehe()
        {
            return "HEHE,你懂的";
        }
        public ActionResult DownLoad(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return File(MakeExcle("4233"), "123", "极客美家预算" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx");
            }
            else
            {
                id = id.Replace(".xlsx", "");
                byte[] arr = MakeExcle(id);
                if (arr.Length > 0)
                {

                    return File(arr, "application/vnd.ms-excel", "极客美家预算" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls");

                }
                else
                {
                    return RedirectToAction("Hehe");
                }
            }


        }
        private static void Sheet2(IWorkbook workbook, ISheet worksheet2, System.Data.DataTable dt)
        {
            IFont font3 = workbook.CreateFont();
            font3.Color = IndexedColors.OliveGreen.Index;
            font3.IsStrikeout = false;
            font3.FontHeightInPoints = 20;
            font3.FontName = "宋体";
            font3.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            ICellStyle style3 = workbook.CreateCellStyle();
            style3.VerticalAlignment = VerticalAlignment.Center;
            style3.Alignment = HorizontalAlignment.Center;
            style3.SetFont(font3);

            worksheet2.SetColumnWidth(0, (int)((5 + 0.72) * 256));
            worksheet2.SetColumnWidth(1, (int)((12 + 0.72) * 256));
            worksheet2.SetColumnWidth(2, (int)((12 + 0.72) * 256));
            worksheet2.SetColumnWidth(3, (int)((12 + 0.72) * 256));
            worksheet2.SetColumnWidth(4, (int)((12 + 0.72) * 256));
            worksheet2.SetColumnWidth(5, (int)((12 + 0.72) * 256));
            worksheet2.SetColumnWidth(6, (int)((12 + 0.72) * 256));
            worksheet2.SetColumnWidth(7, (int)((12 + 0.72) * 256));
            worksheet2.SetColumnWidth(8, (int)((12 + 0.72) * 256));
            worksheet2.SetColumnWidth(9, (int)((12 + 0.72) * 256));
            worksheet2.SetColumnWidth(10, (int)((12 + 0.72) * 256));
            IRow s2_0 = worksheet2.CreateRow(0);
            s2_0.HeightInPoints = 30;
            ICell scell0 = s2_0.CreateCell(0);

            scell0.SetCellValue("主材清单");
            scell0.CellStyle = style3;
            worksheet2.AddMergedRegion(new CellRangeAddress(0, 0, 0, 11));

            IRow s2_1 = worksheet2.CreateRow(1);


            s2_1.CreateCell(0).SetCellValue("序号");
            s2_1.CreateCell(1).SetCellValue("施工阶段");
            s2_1.CreateCell(2).SetCellValue("主材类型");
            s2_1.CreateCell(3).SetCellValue("主材名称");
            s2_1.CreateCell(4).SetCellValue("品牌");
            s2_1.CreateCell(5).SetCellValue("型号");

            s2_1.CreateCell(6).SetCellValue("数量");
            s2_1.CreateCell(7).SetCellValue("单位");
            s2_1.CreateCell(8).SetCellValue("小计");
            s2_1.CreateCell(9).SetCellValue("地区");
            s2_1.CreateCell(10).SetCellValue("采购日期");
            s2_1.CreateCell(11).SetCellValue("备注");

            IFont font4 = workbook.CreateFont();
            font4.Color = IndexedColors.OliveGreen.Index;
            font4.IsStrikeout = false;
            font4.FontHeightInPoints = 11;
            font4.FontName = "宋体";
            // font4.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

            ICellStyle style4 = workbook.CreateCellStyle();

            style4.VerticalAlignment = VerticalAlignment.Center;
            style4.Alignment = HorizontalAlignment.Center;
            style4.SetFont(font4);
            style4.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style4.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style4.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style4.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            style4.BorderDiagonalLineStyle = NPOI.SS.UserModel.BorderStyle.Thin;

            foreach (var item in s2_1.Cells)
            {

                item.CellStyle = style4;

            }
            int m = 2;

            m = Product(worksheet2, dt, style4, m);



        }

        private static int Product(ISheet worksheet2, System.Data.DataTable dt, ICellStyle style4, int m)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                IRow s2_m = worksheet2.CreateRow(m);
                // 序号	施工阶段	主材类型	主材名称	品牌	型号	数量	单位	小计	地区	采购日期	备注
                //Evaluation	Extension4	Extension6	Num	Netprice	Price	dyZlName	Pname	Pmodel	Unit
                //北京市	房间	NULL	18	13.2	237.6	NULL	多乐士乳胶漆竹炭清新居五合一A895+A931	竹炭清新居五合一A895+A931	m&sup2;

                string ddww = row["Unit"].ToSafeString();
                if (ddww.IsEmpty())
                {
                    ddww = row["Extension4"].ToSafeString() + ddww;
                }
                var obj = new { 
                    sgjd = "",
                    zclx = row["Extension6"].ToSafeString(), 
                    zcmc = row["Pname"].ToSafeString(),
                    pp = row["Bname"].ToSafeString(),
                    xh = row["pmodel"].ToSafeString(), 
                    num = row["num"].ToSafeString().Todouble(),
                    dw = ddww.Replace("m&sup2;", "㎡").Replace("平米", "㎡"),
                    xj = row["price"].ToSafeString().Todouble(),
                    adress = row["Evaluation"].ToSafeString(), 
                    rq = "",
                    bz = "" };
                

                s2_m.CreateCell(0).SetCellValue(i + 1);
                s2_m.CreateCell(1).SetCellValue(obj.sgjd);
                s2_m.CreateCell(2).SetCellValue(obj.zclx);
                s2_m.CreateCell(3).SetCellValue(obj.zcmc);
                s2_m.CreateCell(4).SetCellValue(obj.pp);
                s2_m.CreateCell(5).SetCellValue(obj.xh);

                s2_m.CreateCell(6).SetCellValue(obj.num);
                s2_m.CreateCell(7).SetCellValue(obj.dw);
                s2_m.CreateCell(8).SetCellValue(obj.xj);
                s2_m.CreateCell(9).SetCellValue(obj.adress);
                s2_m.CreateCell(10).SetCellValue(obj.rq);
                s2_m.CreateCell(11).SetCellValue(obj.bz);
                foreach (var item in s2_m.Cells)
                {

                    item.CellStyle = style4;

                }
                m += 1;
            }
            return m;
        }
        private byte[] MakeExcle(string projectid)
        {

            DataTable dtdemand = BLL.UserRoom.GetDemnad(projectid);
            if (dtdemand.Rows.Count == 0)
            {
                return new byte[] { };
            }

            DataTable dtuserrooms = BLL.UserRoom.GetUserRomms(projectid);
            DataTable dtzc = BLL.UserRoom.GetZc(projectid);

            HSSFWorkbook workbook = new HSSFWorkbook();

            #region MyRegion
            IFont font41 = workbook.CreateFont();
            font41.Color = IndexedColors.OliveGreen.Index;
            font41.IsStrikeout = false;
            font41.FontHeightInPoints = 11;
            font41.FontName = "宋体";


            stylecenter = workbook.CreateCellStyle();

            stylecenter.VerticalAlignment = VerticalAlignment.Center;
            stylecenter.Alignment = HorizontalAlignment.Center;
            stylecenter.SetFont(font41); 


            #endregion

            #region 头部标题

            IFont font2 = workbook.CreateFont();
            font2.Color = IndexedColors.OliveGreen.Index;
            font2.IsStrikeout = false;
            font2.FontHeightInPoints = 20;
            font2.FontName = "宋体";
            font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            ICellStyle style2 = workbook.CreateCellStyle();
            style2.VerticalAlignment = VerticalAlignment.Center;
            style2.Alignment = HorizontalAlignment.Center;
            style2.SetFont(font2);


            ISheet worksheet = workbook.CreateSheet("极客美家预算清单");
            ISheet worksheet2 = workbook.CreateSheet("极客美家主材清单");
            worksheet.SetColumnWidth(0, (int)((12 + 0.72) * 256));
            worksheet.SetColumnWidth(1, (int)((12 + 0.72) * 256));
            worksheet.SetColumnWidth(2, (int)((12 + 0.72) * 256));
            worksheet.SetColumnWidth(3, (int)((12 + 0.72) * 256));
            worksheet.SetColumnWidth(4, (int)((12 + 0.72) * 256));
            worksheet.SetColumnWidth(5, (int)((12 + 0.72) * 256));
            worksheet.SetColumnWidth(6, (int)((12 + 0.72) * 256));
            worksheet.SetColumnWidth(7, (int)((12 + 0.72) * 256));
            worksheet.SetColumnWidth(8, (int)((12 + 0.72) * 256));
            worksheet.SetColumnWidth(9, (int)((12 + 0.72) * 256));
            worksheet.SetColumnWidth(10, (int)((12 + 0.72) * 256));
            IRow row = worksheet.CreateRow(0);
            ICell cell = row.CreateCell(0);
            cell.SetCellValue("极客美家预算清单");
            cell.CellStyle = style2;


            worksheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));

            //worksheet.SetColumnWidth(0, 3766); 
            #endregion



            #region 字体样式
            IFont fontb = workbook.CreateFont();
            fontb.Color = IndexedColors.OliveGreen.Index;
            fontb.IsStrikeout = false;
            fontb.FontHeightInPoints = 11;
            fontb.FontName = "宋体";
            fontb.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            ICellStyle styleb = workbook.CreateCellStyle();
            styleb.VerticalAlignment = VerticalAlignment.Center;
            styleb.Alignment = HorizontalAlignment.Center;
            styleb.SetFont(fontb);
            #endregion



            ProjectDetail(worksheet, styleb, dtdemand);

            #region 风格
            IFont fontc = workbook.CreateFont();
            fontc.Color = IndexedColors.OliveGreen.Index;
            fontc.IsStrikeout = false;
            fontc.FontHeightInPoints = 11;
            fontc.FontName = "宋体";
            fontc.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

            ICellStyle stylec = workbook.CreateCellStyle();
            stylec.VerticalAlignment = VerticalAlignment.Center;
            stylec.Alignment = HorizontalAlignment.Center;
            stylec.SetFont(fontc);

            stylec.FillForegroundColor = GetXLColour(workbook, Color.Silver); ;

            stylec.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            stylec.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            stylec.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            stylec.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            stylec.BorderDiagonalLineStyle = NPOI.SS.UserModel.BorderStyle.Thin;

            stylec.FillPattern = FillPattern.SolidForeground;//-------------------------------------------


            #endregion

            int n = 6;

            #region 具体的某个房间



            for (int i = 0; i < dtuserrooms.Rows.Count; i++)
            {
                //循环每一个房间
                n = MakeRoom(worksheet, styleb, stylec, n, dtuserrooms.Rows[i]);//某一个房间的详情

            }


            //
            #region 水电即其它
           
            IRow row6 = worksheet.CreateRow(n);
            IRow row7 = worksheet.CreateRow(n+1);
            for (int i = 0; i < 11; i++)
            {
                row6.CreateCell(i);
                row7.CreateCell(i);
            }
            ICell row6cell = row6.CreateCell(0);
            row6cell.CellStyle = stylec;
            row6cell.SetCellValue("水电和其它");
            worksheet.AddMergedRegion(new CellRangeAddress(n, n + 1, 0, 10));

            n += 2;

            IRow row8 = worksheet.CreateRow(n);
            ICell row8cell0 = row8.CreateCell(0);
            row8cell0.CellStyle = styleb;
            row8cell0.SetCellValue("施工位置");
            ICell row8cell1 = row8.CreateCell(1);
            row8cell1.CellStyle = styleb;
            row8cell1.SetCellValue("序号");
            ICell row8cell2 = row8.CreateCell(2);
            row8cell2.CellStyle = styleb;
            row8cell2.SetCellValue("");
            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 2, 3));

            ICell row8cell4 = row8.CreateCell(4);
            row8cell4.CellStyle = styleb;
            row8cell4.SetCellValue("单位");

            ICell row8cell5 = row8.CreateCell(5);
            row8cell5.CellStyle = styleb;
            row8cell5.SetCellValue("数量");

            ICell row8cell6 = row8.CreateCell(6);
            row8cell6.CellStyle = styleb;
            row8cell6.SetCellValue("单价");

            ICell row8cell7 = row8.CreateCell(7);
            row8cell7.CellStyle = styleb;
            row8cell7.SetCellValue("小计");


            ICell row8cell8 = row8.CreateCell(8);
            row8cell8.CellStyle = styleb;
            row8cell8.SetCellValue("工艺描述");

            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 8, 10));
            n += 1;
            foreach (var item in row6.Cells)
            {
                item.CellStyle = stylec;
            }
            foreach (var item in row7.Cells)
            {
                item.CellStyle = stylec;
            }

            DataTable dt = BLL.UserRoom.GetOther(projectid);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                n = Other(worksheet,n, dt.Rows[i],i+1);
            }
            #endregion


            

            #endregion

            IRow row216 = worksheet.CreateRow(n);
            row216.HeightInPoints = 15;
            for (int i = 0; i < 11; i++)
            {
                row216.CreateCell(i).CellStyle =stylec ; ;
            }
            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 0, 10));
            n += 1;



            #region MyRegion
            IFont font412 = workbook.CreateFont();
            font412.Color=  IndexedColors.OliveGreen.Index;
          
            font412.IsStrikeout = false;
            font412.FontHeightInPoints = 20;
            font412.FontName = "宋体";


            stylecenter2 = workbook.CreateCellStyle();
            stylecenter2.SetFont(font412);
            //stylecenter2.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.PaleBlue.Index;
            stylecenter2.VerticalAlignment = VerticalAlignment.Center;

            stylecenter2.Alignment = HorizontalAlignment.Center;
          //  stylecenter2.FillPattern = FillPattern.SolidForeground;
           


            #endregion
            #region MyRegion
            n = Fotter(worksheet, n, "工程直接费用");
            n = Fotter(worksheet, n, "管理费");
            n = Fotter(worksheet, n, "税金");
            n = Fotter(worksheet, n, "装修合同总价");
            #endregion



            IFont fontbb = workbook.CreateFont();
            fontbb.Color = IndexedColors.OliveGreen.Index;
            fontbb.IsStrikeout = false;
            fontbb.FontHeightInPoints = 11;
            fontbb.FontName = "宋体";
            fontbb.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            ICellStyle stylebb = workbook.CreateCellStyle();
            stylebb.WrapText = true;
            stylebb.VerticalAlignment = VerticalAlignment.Top;
            stylebb.Alignment = HorizontalAlignment.Left;
            stylebb.SetFont(fontbb);



       
         
            IRow row217 = worksheet.CreateRow(n);
            row217.HeightInPoints = 250;
            for (int i = 0; i < 11; i++)
            {
                row217.CreateCell(i).CellStyle = stylebb;


            }


        


            #region 尾部
            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 0, 10));
            row217.GetCell(0).SetCellValue(@"预算说明:                                                                                                    1、本预算书所列项目及涉及的图纸、施工中增减项目的文字、表格等资料，经双方签字确认后，均作为甲、乙方施工、付款、验收、保修的依据。                                                                                                     2、预算书中的工程量与现场施工的工程量有误差时，以现场实际工程量为准，误差由甲、乙双方共同办理增减项调整工程造价。增减项费用在中期款计算、支付。                                                                                        3.计算乳胶漆面积时，如包套，门窗部分按实际面积的50%减除。如遇特殊墙面（如：沙灰墙、保温墙、防水腻子等）需要找平或贴布处理，详见相关项目报价。                                                                                        4、基于水电改造项目的相关细节，在甲乙双方签订家装合同、本工程预算书时无法确定，本预算书所列示的为乙方水电改造项目的收款标准及计算规则，水电改造的实际工程造价，应在工程开工后在现场依据甲方要求确定工程项目签定水电改造工程确认单最后依据本预算书单价进行水电改造工程的结算。水电改造工程不计算为增项，单独结算。改造完工后一周内付款。                               5、物业收取的装修押金（可退）、装修管理费、垃圾外运费由甲方交纳，如因我公司施工人员违反物业规定造成罚款，由我公司承担.施工期间的水费，电费由客户承担。                                                                                  6.以上报价不含地砖、地板、橱柜、灯具、五金件、洁具、开关面板、艺术玻璃等主材，需乙方提供主材，另签代购协议。            7.请不要接受任何口头承若，所承诺的内容必须在合同或报价中注明。                                                     8.交工程款特别提示:甲方交工程款时，应直接到公司财务处交纳或转账，如有特殊情况本人不能亲自到公司交纳者，事先电话通知，乙方派专人到工地收款，收款时以盖有“北京首标装饰设计有限公司财务专用章” 
	#endregion的收据为凭证，白条无效。                           9.本预算书为甲乙双方签订的《北京市家庭居室装饰装修工程施工合同》的附件，甲方及乙方指定代理人签字有效。  "); 
            #endregion


            Sheet2(workbook, worksheet2, dtzc);


            MemoryStream fs = new MemoryStream();

            workbook.Write(fs);


            try
            {

                return fs.ToArray();
            }
            catch
            {
                return new byte[] { };
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }


        }

        private static int Fotter(ISheet worksheet, int n,string str)
        {
            IRow row217 = worksheet.CreateRow(n);
            row217.HeightInPoints = 45;
            for (int i = 0; i < 11; i++)
            {
                row217.CreateCell(i).CellStyle=stylecenter2;
            }

            row217.GetCell(0).SetCellValue(str);

            worksheet.AddMergedRegion(new  CellRangeAddress(n, n, 0, 1));
            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 0, 1));
            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 2, 7));
            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 8, 10));
            n += 1;
            return n;
        }

        private static int Other(ISheet worksheet, int n,System.Data.DataRow row,int index)
        {
            var obj = new { gy = row["extension"].ToSafeString(), dw = row["Unit"].ToSafeString().Replace("m&sup2;", "㎡").Replace("平米", "㎡"), number = row["extension1"].ToSafeString().Todouble(), dj = row["price"].ToSafeString().Todouble(), xj = row["extension2"].ToSafeString().Todouble(), gyms = row["desc"].ToSafeString() };

            IRow row8 = worksheet.CreateRow(n);
            row8.HeightInPoints = 30;
            ICell row8cell0 = row8.CreateCell(0);
            row8cell0.CellStyle = stylecenter;
            row8cell0.SetCellValue("");
            ICell row8cell1 = row8.CreateCell(1);
            row8cell1.CellStyle = stylecenter;
            row8cell1.SetCellValue(index);
            ICell row8cell2 = row8.CreateCell(2);
            row8cell2.CellStyle = stylecenter;
            row8cell2.SetCellValue(obj.gy);
            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 2, 3));

            ICell row8cell4 = row8.CreateCell(4);
            row8cell4.CellStyle = stylecenter;
            row8cell4.SetCellValue(obj.dw);

            ICell row8cell5 = row8.CreateCell(5);
            row8cell5.CellStyle = stylecenter;
            row8cell5.SetCellValue(obj.number);

            ICell row8cell6 = row8.CreateCell(6);
            row8cell6.CellStyle = stylecenter;
            row8cell6.SetCellValue(obj.dj);

            ICell row8cell7 = row8.CreateCell(7);
            row8cell7.CellStyle = stylecenter;
            row8cell7.SetCellValue(obj.xj);


            ICell row8cell8 = row8.CreateCell(8);
            row8cell8.CellStyle = stylecenter;
            row8cell8.SetCellValue(obj.gyms);

            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 8, 10));
            n += 1;
            return n;
        }

        private static void ProjectDetail(ISheet worksheet, ICellStyle styleb, System.Data.DataTable dt)
        {
            var row = dt.Rows[0];


            double t1 = 0;

            double t2 = 0;

            double.TryParse(row["ProductId"].ToSafeString(), out t1);
            double.TryParse(row["ProjectId"].ToSafeString(), out t2);
            var obj = new
            {
                username = row["Extension12"].ToSafeString(),//UnitId
                userphone = row["Extension14"].ToSafeString(),
                hx = row["Extension13"].ToSafeString(),
                fx = "新房",
                address = row["Extension2"].ToSafeString(),
                area = row["UnitId"].ToSafeString(),
                zj = t1 + t2,
                desingername = "极客美家",
                desingerphone = "",
                zxfg = row["Description"].ToSafeString(),
                qdrq = ""
            };


            #region 第二行
            IRow row2 = worksheet.CreateRow(2);
            row2.HeightInPoints = 30;
            ICell row2cell = row2.CreateCell(0);
            row2cell.CellStyle = styleb;

            row2cell.SetCellValue("客  户：");

            ICell kfxm = row2.CreateCell(2);
            kfxm.CellStyle = styleb;
            kfxm.SetCellValue(obj.username);

            ICell kfdh = row2.CreateCell(3);
            kfdh.CellStyle = styleb;
            kfdh.SetCellValue("电 话：");
            ICell kfdhv = row2.CreateCell(4);
            kfdhv.CellStyle = styleb;
            kfdhv.SetCellValue(obj.userphone);


            ICell hx7 = row2.CreateCell(7);
            hx7.CellStyle = styleb;
            hx7.SetCellValue("户 型：");

            ICell hx7v = row2.CreateCell(8);
            hx7v.CellStyle = styleb;
            hx7v.SetCellValue(obj.hx);



            ICell fx7 = row2.CreateCell(9);
            fx7.CellStyle = styleb;
            fx7.SetCellValue("房 型：");

            ICell fxv = row2.CreateCell(10);
            fxv.CellStyle = styleb;
            fxv.SetCellValue(obj.fx);



            worksheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 1));
            worksheet.AddMergedRegion(new CellRangeAddress(2, 2, 4, 6));
            #endregion



            #region 第三行
            IRow row3 = worksheet.CreateRow(3);
            row3.HeightInPoints = 30;
            ICell row3cell = row3.CreateCell(0);
            row3cell.CellStyle = styleb;
            row3cell.SetCellValue("工程地址：");

            worksheet.AddMergedRegion(new CellRangeAddress(3, 3, 0, 1));

            ICell row3cellv = row3.CreateCell(2);
            row3cellv.CellStyle = styleb;
            row3cellv.SetCellValue(obj.address);
            worksheet.AddMergedRegion(new CellRangeAddress(3, 3, 2, 6));


            ICell row3mj = row3.CreateCell(7);
            row3mj.CellStyle = styleb;
            row3mj.SetCellValue("面 积：");

            ICell row3mjv = row3.CreateCell(8);
            row3mjv.CellStyle = styleb;
            row3mjv.SetCellValue(obj.area);


            ICell row3zj = row3.CreateCell(9);
            row3zj.CellStyle = styleb;
            row3zj.SetCellValue("造 价：");

            ICell row3zjv = row3.CreateCell(10);
            row3zjv.CellStyle = styleb;
            row3zjv.SetCellValue(obj.zj);
            #endregion



            #region 第四行
            IRow row4 = worksheet.CreateRow(4);
            row4.HeightInPoints = 30;
            ICell row4cell = row4.CreateCell(0);
            row4cell.CellStyle = styleb;

            row4cell.SetCellValue("设计师：");

            ICell kfxm4 = row4.CreateCell(2);
            kfxm4.CellStyle = styleb;
            kfxm4.SetCellValue(obj.desingername);

            ICell kfdh4 = row4.CreateCell(3);
            kfdh4.CellStyle = styleb;
            kfdh4.SetCellValue("电 话：");
            ICell kfdhv4 = row4.CreateCell(4);
            kfdhv4.CellStyle = styleb;
            kfdhv4.SetCellValue(obj.desingerphone);


            ICell hx74 = row4.CreateCell(7);
            hx74.CellStyle = styleb;
            hx74.SetCellValue("装修风格：");

            ICell hx7v4 = row4.CreateCell(8);
            hx7v4.CellStyle = styleb;
            hx7v4.SetCellValue(obj.zxfg);



            ICell fx74 = row4.CreateCell(9);
            fx74.CellStyle = styleb;
            fx74.SetCellValue("签单日期：");

            ICell fxv4 = row4.CreateCell(10);
            fxv4.CellStyle = styleb;
            fxv4.SetCellValue(obj.qdrq);



            worksheet.AddMergedRegion(new CellRangeAddress(4, 4, 0, 1));
            worksheet.AddMergedRegion(new CellRangeAddress(4, 4, 4, 6));
            #endregion
        }

        /// <summary>
        /// 某一个房间
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="styleb"></param>
        /// <param name="stylec"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static int MakeRoom(ISheet worksheet, ICellStyle styleb, ICellStyle stylec, int n, System.Data.DataRow row)
        {
            //地 墙  顶  周
            var qmj = "";
            var dmj = "";
            var tmj = "";
            var zc = "";
            var o = row["extension2"].ToSafeString();

            string[] arr = o.Split(',');
            if (arr.Length == 4)
            {
                dmj = arr[0];
                qmj = arr[1];
                tmj = arr[2];
                zc = arr[3];
            }




            var obj = new { roomid = row["id"].ToSafeString(), rommname = row["roomName"].ToSafeString(), qmj = qmj, dmj = dmj, gyzj = "", tmj = tmj, zc = zc, zczj = "" };

            DataTable gytable = BLL.UserRoom.GetGyTable(obj.roomid);

            #region 房间顶部
            IRow row6 = worksheet.CreateRow(n);
            ICell row6cell = row6.CreateCell(0);
            row6.CreateCell(1);
            row6cell.CellStyle = stylec;
            row6cell.SetCellValue(obj.rommname);

            ICell row6qmj = row6.CreateCell(2);
            row6qmj.CellStyle = stylec;
            row6qmj.SetCellValue("墙面积：");

            ICell row6qmjv = row6.CreateCell(3);
            row6qmjv.CellStyle = stylec;
            row6qmjv.SetCellValue(obj.qmj.Todouble());


            ICell row6dmj = row6.CreateCell(4);
            row6dmj.CellStyle = stylec;
            row6dmj.SetCellValue("地面积：");

            row6.CreateCell(5);

            ICell row6dmjv = row6.CreateCell(6);
            row6dmjv.CellStyle = stylec;
            row6dmjv.SetCellValue( obj.dmj.Todouble());

            row6.CreateCell(7);

            ICell row6gyzj = row6.CreateCell(8);
            row6gyzj.CellStyle = stylec;
            row6gyzj.SetCellValue("工艺总价：");


            ICell row6gyzjv = row6.CreateCell(9);
            row6gyzjv.CellStyle = stylec;
            row6gyzjv.SetCellValue( obj.gyzj.Todouble());
            row6.CreateCell(10);

            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 4, 5));
            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 6, 7));
            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 9, 10));
            worksheet.AddMergedRegion(new CellRangeAddress(n, n + 1, 0, 1));
            n += 1;
            IRow row7 = worksheet.CreateRow(n);

            for (int i = 0; i < 11; i++)
            {
                row7.CreateCell(i);
            }

            ICell row7qmj = row7.CreateCell(2);
            row7qmj.CellStyle = stylec;
            row7qmj.SetCellValue("顶面积：");

            ICell row7qmjv = row7.CreateCell(3);
            row7qmjv.CellStyle = stylec;
            row7qmjv.SetCellValue(obj.tmj.Todouble());


            ICell row7dmj = row7.CreateCell(4);
            row7dmj.CellStyle = stylec;
            row7dmj.SetCellValue("周长：");

            ICell row7dmjv = row7.CreateCell(6);
            row7dmjv.CellStyle = stylec;
            row7dmjv.SetCellValue( obj.zc.Todouble());


            ICell row7gyzj = row7.CreateCell(8);
            row7gyzj.CellStyle = stylec;
            row7gyzj.SetCellValue("主材总价:");


            ICell row7gyzjv = row7.CreateCell(9);
            row7gyzjv.CellStyle = stylec;
            row7gyzjv.SetCellValue(obj.zczj.Todouble());
            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 4, 5));
            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 6, 7));
            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 9, 10));
            #endregion

            foreach (var item in row6.Cells)
            {
                item.CellStyle = stylec;
            }
            foreach (var item in row7.Cells)
            {
                item.CellStyle = stylec;
            }
            #region 顶部
            n += 1;
            IRow row8 = worksheet.CreateRow(n);
            ICell row8cell0 = row8.CreateCell(0);
            row8cell0.CellStyle = styleb;
            row8cell0.SetCellValue("施工位置");
            ICell row8cell1 = row8.CreateCell(1);
            row8cell1.CellStyle = styleb;
            row8cell1.SetCellValue("序号");
            ICell row8cell2 = row8.CreateCell(2);
            row8cell2.CellStyle = styleb;
            row8cell2.SetCellValue("工艺");
            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 2, 3));

            ICell row8cell4 = row8.CreateCell(4);
            row8cell4.CellStyle = styleb;
            row8cell4.SetCellValue("单位");

            ICell row8cell5 = row8.CreateCell(5);
            row8cell5.CellStyle = styleb;
            row8cell5.SetCellValue("数量");

            ICell row8cell6 = row8.CreateCell(6);
            row8cell6.CellStyle = styleb;
            row8cell6.SetCellValue("单价");

            ICell row8cell7 = row8.CreateCell(7);
            row8cell7.CellStyle = styleb;
            row8cell7.SetCellValue("小计");


            ICell row8cell8 = row8.CreateCell(8);
            row8cell8.CellStyle = styleb;
            row8cell8.SetCellValue("工艺描述");

            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 8, 10));

            n += 1;
            #endregion

            //墙  循环墙   长度

            #region MyRegion

            n = NewMethod(worksheet, n, "墙面", gytable.Select("types='qm'"));
            n = NewMethod(worksheet, n, "顶面", gytable.Select("types='dm'"));
            n = NewMethod(worksheet, n, "地面", gytable.Select("types='ld'"));

            #endregion

            return n;
        }

        private static int NewMethod(ISheet worksheet, int n, string o, System.Data.DataRow[] rows)
        {
            if (rows.Count() > 0)
            {

                for (int i = 0; i < rows.Count(); i++)
                {
                    var row = rows[i];
                    NewMethod(worksheet, n, i, o, row);
                    n += 1;
                }



                worksheet.AddMergedRegion(new CellRangeAddress(n - rows.Count(), n - 1, 0, 0));

            }
            return n;
        }

        private static void NewMethod(ISheet worksheet, int n, int i, string o, System.Data.DataRow row)
        {
            var obj = new { gy = row["extension"].ToSafeString(), dw = row["Unit"].ToSafeString(), number = row["extension1"].ToSafeString(), dj = row["price"].ToSafeString(), xj = row["extension2"].ToSafeString(), gyms = row["desc"].ToSafeString() };
            IRow row9 = worksheet.CreateRow(n);
            row9.HeightInPoints = 30;
            ICell row9cell0 = row9.CreateCell(0);
            row9cell0.SetCellValue(o);

            ICell row9cell1 = row9.CreateCell(1);
            row9cell1.SetCellValue(i + 1);

            ICell row9cell2 = row9.CreateCell(2);
            row9cell2.SetCellValue(obj.gy);//
            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 2, 3));


            ICell row9cell4 = row9.CreateCell(4);
            row9cell4.SetCellValue(obj.dw.Replace("m&sup2;", "㎡").Replace("平米", "㎡"));//


            ICell row9cell5 = row9.CreateCell(5);
            row9cell5.SetCellValue(obj.number.Todouble());//


            ICell row9cell6 = row9.CreateCell(6);
            row9cell6.SetCellValue(obj.dj.Todouble());//

            ICell row9cell7 = row9.CreateCell(7);
            row9cell7.SetCellValue(obj.xj.Todouble());//

            ICell row9cell8 = row9.CreateCell(8);
            row9cell8.SetCellValue(obj.gyms);//


            //ICellStyle style4 = workbook.CreateCellStyle();

            //style4.VerticalAlignment = VerticalAlignment.Center;
            //style4.Alignment = HorizontalAlignment.Center;
            //style4.SetFont(font4);
            foreach (var item in row9.Cells)
            {

                item.CellStyle = stylecenter;

            }
            worksheet.AddMergedRegion(new CellRangeAddress(n, n, 8, 10));
        }


        private static short GetXLColour(HSSFWorkbook workbook, Color SystemColour)
        {
            short s = 0;
            HSSFPalette XlPalette = workbook.GetCustomPalette();
            HSSFColor XlColour = XlPalette.FindColor(SystemColour.R, SystemColour.G, SystemColour.B);
            if (XlColour == null)
            {
                if (NPOI.HSSF.Record.PaletteRecord.STANDARD_PALETTE_SIZE < 255)
                {
                    if (NPOI.HSSF.Record.PaletteRecord.STANDARD_PALETTE_SIZE < 64)
                    {
                        //NPOI.HSSF.Record.PaletteRecord.STANDARD_PALETTE_SIZE = 64;
                        //NPOI.HSSF.Record.PaletteRecord.STANDARD_PALETTE_SIZE += 1;
                        XlColour = XlPalette.AddColor(SystemColour.R, SystemColour.G, SystemColour.B);
                    }
                    else
                    {
                        XlColour = XlPalette.FindSimilarColor(SystemColour.R, SystemColour.G, SystemColour.B);
                    }

                    s = XlColour.Indexed;
                }

            }
            else
                s = XlColour.Indexed;

            return s;
        }  
    }
}
