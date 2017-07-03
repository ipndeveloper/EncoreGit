using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.Configuration;

namespace NetSteps.Data.Entities
{
    public class Pdf
    {
        private static PdfPCell GetCelda(CeldaPDF Celda)
        {

            PdfPCell lblCelda = new PdfPCell(new Phrase(Celda.Text, FontFactory.GetFont(Celda.FontName, Celda.FontSize, Celda.esBold ? Font.BOLD : Font.NORMAL))) { Colspan = Celda.Colspan };
            lblCelda.Border = 0;
            lblCelda.PaddingBottom = Celda.PaddingBottom == 0f ? 7f : Celda.PaddingBottom;
            lblCelda.HorizontalAlignment = Celda.HorizontalAlignment;
            if (Celda.UseDescender) lblCelda.UseDescender = true;
            if (Celda.VerticalAlignment > 0) lblCelda.VerticalAlignment = Celda.VerticalAlignment;
            if (Celda.fixedLeading > 0 && Celda.multipliedLeading > 0) lblCelda.SetLeading(Celda.fixedLeading, Celda.multipliedLeading);
            if (Celda.FixedHeight > 0) lblCelda.FixedHeight = Celda.FixedHeight;
            if (Celda.BorderWidthBottom > 0) lblCelda.BorderWidthBottom = Celda.BorderWidthBottom;
            lblCelda.BackgroundColor = iTextSharp.text.BaseColor.WHITE;

            return lblCelda;
        }

        public static MemoryStream GeneratePDFMemoryStream(List<string> OrderList)
        {
            string orderNumber = "";
            MemoryStream ms = new MemoryStream();

            Document document = new Document(iTextSharp.text.PageSize.A4);

            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            writer.CloseStream = false;
            document.Open();

            foreach (var List in OrderList)
            {
                orderNumber = List.ToString();

                #region Functions

                DataTable dtFirstLine = DataAccess.GetDataSet(DataAccess.GetCommand("InvoiceFirstLine", new Dictionary<string, object>() { { "@orderNumber", orderNumber } }, "Core")).Tables[0];
                DataTable dtSecondLine = DataAccess.GetDataSet(DataAccess.GetCommand("InvoiceSecondLine", new Dictionary<string, object>() { { "@orderNumber", orderNumber } }, "Core")).Tables[0];
                DataTable dtSecondLineBill = DataAccess.GetDataSet(DataAccess.GetCommand("InvoiceSecondLineBill", new Dictionary<string, object>() { { "@orderNumber", orderNumber } }, "Core")).Tables[0];
                DataTable dtThirdLine = DataAccess.GetDataSet(DataAccess.GetCommand("InvoiceTirdLine", new Dictionary<string, object>() { { "@orderNumber", orderNumber } }, "Core")).Tables[0];
                DataTable dtFourthLine = DataAccess.GetDataSet(DataAccess.GetCommand("InvoiceFourthLine", new Dictionary<string, object>() { { "@orderNumber", orderNumber } }, "Core")).Tables[0];
                DataTable dtPaymentLine = DataAccess.GetDataSet(DataAccess.GetCommand("InvoicePaymentLine", new Dictionary<string, object>() { { "@orderNumber", orderNumber } }, "Core")).Tables[0];
                DataTable dtTotalLine = DataAccess.GetDataSet(DataAccess.GetCommand("InvoiceTotalLine", new Dictionary<string, object>() { { "@orderNumber", orderNumber } }, "Core")).Tables[0];

                #endregion

                #region Grupo1

                PdfPTable tableL1 = new PdfPTable(3);
                PdfPCell[] CellsLinea1 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "Dist. No", FontName = "Verdana-Bold", FontSize = 10, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.1f, multipliedLeading = 0.1f}),
                                                GetCelda(new CeldaPDF(){Text = "Date", FontName = "Verdana-Bold", FontSize = 10, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.1f, multipliedLeading = 0.1f}),
                                                GetCelda(new CeldaPDF(){Text = "Invoice No.", FontName = "Verdana-Bold", FontSize = 10, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.1f, multipliedLeading = 0.1f}),
                                        };
                PdfPRow RowLinea1 = new PdfPRow(CellsLinea1);
                tableL1.Rows.Add(RowLinea1);

                if (dtFirstLine.Rows.Count > 0)
                {
                    PdfPCell[] CellsLinea2 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = (dtFirstLine.Rows[0]["AccountID"] == DBNull.Value ? "" : dtFirstLine.Rows[0]["AccountID"].ToString()), FontName = "ArialUnicodeMS", FontSize = 10, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.1f, multipliedLeading = 0.1f}),
                                                GetCelda(new CeldaPDF(){Text = (dtFirstLine.Rows[0]["Date"] == DBNull.Value ? "" : dtFirstLine.Rows[0]["Date"].ToString()), FontName = "ArialUnicodeMS", FontSize = 10, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.1f, multipliedLeading = 0.1f}),
                                                GetCelda(new CeldaPDF(){Text = (dtFirstLine.Rows[0]["InvoiceNro"] == DBNull.Value ? "" : dtFirstLine.Rows[0]["InvoiceNro"].ToString()), FontName = "ArialUnicodeMS", FontSize = 10, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.1f, multipliedLeading = 0.1f}),
                                        };

                    PdfPRow RowLinea2 = new PdfPRow(CellsLinea2);
                    tableL1.Rows.Add(RowLinea2);
                }

                tableL1.TotalWidth = 200f;
                tableL1.LockedWidth = true;

                float[] WithCol1 = new float[] { 70f, 70f, 60f };
                tableL1.SetWidths(WithCol1);
                tableL1.HorizontalAlignment = Element.ALIGN_RIGHT;

                #endregion

                #region GrupoLogo

                //logo
                iTextSharp.text.Image logo;
                logo = iTextSharp.text.Image.GetInstance(ConfigurationManager.AppSettings["FileUploadAbsoluteWebPath"].ToString() + ConfigurationManager.AppSettings["RutaLogo"].ToString());
                logo.SetAbsolutePosition(50, 755);
                logo.ScaleAbsoluteWidth(48);
                logo.ScaleAbsoluteHeight(50);

                #endregion

                #region Grupo2

                PdfPTable tableG2 = new PdfPTable(2);

                PdfPCell[] CellsLinea3 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "      ", FontName = "Verdana-Bold", FontSize = 10, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                                                GetCelda(new CeldaPDF(){Text = "Ship To", FontName = "Verdana-Bold", FontSize = 10, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                                       //        GetCelda(new CeldaPDF(){Text = "Bill To", FontName = "Verdana-Bold", FontSize = 10, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                                        };

                PdfPRow RowLinea3 = new PdfPRow(CellsLinea3);
                tableG2.Rows.Add(RowLinea3);

                string strName = "";
                string strAddress = "";
                string strCity = "";
                string strBillName = "";
                string strBillAddress = "";
                string strBillCity = "";
                if (dtSecondLine.Rows.Count > 0)
                {
                    strName = (dtSecondLine.Rows[0]["Name"] == DBNull.Value ? "" : dtSecondLine.Rows[0]["Name"].ToString());
                    strAddress = (dtSecondLine.Rows[0]["Address"] == DBNull.Value ? "" : dtSecondLine.Rows[0]["Address"].ToString()); ;
                    strCity = (dtSecondLine.Rows[0]["City"] == DBNull.Value ? "" : dtSecondLine.Rows[0]["City"].ToString()); ;
                }
                else
                {
                    strName = "";
                    strAddress = "";
                    strCity = "";
                }
                /*
                if (dtSecondLineBill.Rows.Count > 0)
                {
                    strBillName = (dtSecondLineBill.Rows[0]["Name"] == DBNull.Value ? "" : dtSecondLineBill.Rows[0]["Name"].ToString());
                    strBillAddress = (dtSecondLineBill.Rows[0]["Address"] == DBNull.Value ? "" : dtSecondLineBill.Rows[0]["Address"].ToString());
                    strBillCity = (dtSecondLineBill.Rows[0]["City"] == DBNull.Value ? "" : dtSecondLineBill.Rows[0]["City"].ToString());
                }
                else
                {
                    strBillName = "";
                    strBillAddress = "";
                    strBillCity = "";
                }
                */
                PdfPCell[] CellsLinea4 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "      ", FontName = "ArialUnicodeMS", FontSize = 10, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.4f, multipliedLeading = 0.4f}),
                                                GetCelda(new CeldaPDF(){Text = strName, FontName = "ArialUnicodeMS", FontSize = 10, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.4f, multipliedLeading = 0.4f}),
                 //                               GetCelda(new CeldaPDF(){Text = strBillName, FontName = "ArialUnicodeMS", FontSize = 10, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.4f, multipliedLeading = 0.4f}),
                                        };

                PdfPRow RowLinea4 = new PdfPRow(CellsLinea4);
                tableG2.Rows.Add(RowLinea4);

                PdfPCell[] CellsLinea5 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "      ", FontName = "ArialUnicodeMS", FontSize = 10, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.4f, multipliedLeading = 0.4f}),
                                                GetCelda(new CeldaPDF(){Text = strAddress, FontName = "ArialUnicodeMS", FontSize = 10, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.4f, multipliedLeading = 0.4f}),
                //                                GetCelda(new CeldaPDF(){Text = strBillAddress, FontName = "ArialUnicodeMS", FontSize = 10, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.4f, multipliedLeading = 0.4f}),
                                        };

                PdfPRow RowLinea5 = new PdfPRow(CellsLinea5);
                tableG2.Rows.Add(RowLinea5);


                PdfPCell[] CellsLinea6 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "      ", FontName = "ArialUnicodeMS", FontSize = 10, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.4f, multipliedLeading = 0.4f}),
                                                GetCelda(new CeldaPDF(){Text = strCity, FontName = "ArialUnicodeMS", FontSize = 10, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.4f, multipliedLeading = 0.4f}),
                //                                GetCelda(new CeldaPDF(){Text = strBillCity, FontName = "ArialUnicodeMS", FontSize = 10, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.4f, multipliedLeading = 0.4f}),
                                        };

                PdfPRow RowLinea6 = new PdfPRow(CellsLinea6);
                tableG2.Rows.Add(RowLinea6);

                tableG2.TotalWidth = 465f;
                tableG2.LockedWidth = true;

                float[] WithCol2 = new float[] { 33f, 272f };
                tableG2.SetWidths(WithCol2);
                tableG2.HorizontalAlignment = Element.ALIGN_LEFT;

                #endregion

                #region Grupo3

                PdfPTable tableG3 = new PdfPTable(5);
                PdfPCell[] CellsLinea7 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "Invoice No.", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, VerticalAlignment = Element.ALIGN_BOTTOM, BorderWidthBottom = 0.75f}),
                                                GetCelda(new CeldaPDF(){Text = "Date", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, VerticalAlignment = Element.ALIGN_BOTTOM, BorderWidthBottom = 0.75f}),
                                                GetCelda(new CeldaPDF(){Text = "Type", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, VerticalAlignment = Element.ALIGN_BOTTOM, BorderWidthBottom = 0.75f}),
                                                GetCelda(new CeldaPDF(){Text = "Status", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, VerticalAlignment = Element.ALIGN_BOTTOM, BorderWidthBottom = 0.75f}),
                                                GetCelda(new CeldaPDF(){Text = "Shipping", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, VerticalAlignment = Element.ALIGN_BOTTOM, BorderWidthBottom = 0.75f}),
                                        };
                PdfPRow RowLinea7 = new PdfPRow(CellsLinea7);
                tableG3.Rows.Add(RowLinea7);

                if (dtThirdLine.Rows.Count > 0)
                {
                    PdfPCell[] CellsLinea8 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = (dtThirdLine.Rows[0]["orderNumber"] == DBNull.Value ? "" : dtThirdLine.Rows[0]["orderNumber"].ToString()), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                                                GetCelda(new CeldaPDF(){Text = (dtThirdLine.Rows[0]["Date"] == DBNull.Value ? "" : dtThirdLine.Rows[0]["Date"].ToString()), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                                                GetCelda(new CeldaPDF(){Text = (dtThirdLine.Rows[0]["Type"] == DBNull.Value ? "" : dtThirdLine.Rows[0]["Type"].ToString()), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                                                GetCelda(new CeldaPDF(){Text = (dtThirdLine.Rows[0]["Status"] == DBNull.Value ? "" : dtThirdLine.Rows[0]["Status"].ToString()), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                                                GetCelda(new CeldaPDF(){Text = (dtThirdLine.Rows[0]["Shipping"] == DBNull.Value ? "" : dtThirdLine.Rows[0]["Shipping"].ToString()), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                                        };

                    PdfPRow RowLinea8 = new PdfPRow(CellsLinea8);
                    tableG3.Rows.Add(RowLinea8);
                }

                tableG3.TotalWidth = 550f;
                tableG3.LockedWidth = true;

                float[] WithCol3 = new float[] { 70f, 120f, 70f, 70f, 100f };
                tableG3.SetWidths(WithCol3);

                #endregion

                #region TextPageNext

                PdfPTable tablePagNext = new PdfPTable(1);
                PdfPCell[] CellsPagLinea1 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "- CONTINUED ON NEXT", FontName = "ArialUnicodeMS", FontSize = 10, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, VerticalAlignment = Element.ALIGN_BOTTOM, fixedLeading = 0.2f, multipliedLeading = 0.2f}),
                                        };
                PdfPRow RowPagLinea1 = new PdfPRow(CellsPagLinea1);
                tablePagNext.Rows.Add(RowPagLinea1);

                PdfPCell[] CellsPagLinea2 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "PAGE -", FontName = "ArialUnicodeMS", FontSize = 10, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, VerticalAlignment = Element.ALIGN_BOTTOM, fixedLeading = 0.2f, multipliedLeading = 0.2f}),
                                        };

                PdfPRow RowPagLinea2 = new PdfPRow(CellsPagLinea2);
                tablePagNext.Rows.Add(RowPagLinea2);

                tablePagNext.TotalWidth = 130;
                tablePagNext.LockedWidth = true;

                float[] WithColPagNext = new float[] { 130 };
                tablePagNext.SetWidths(WithColPagNext);

                #endregion

                #region TextTaxes

                PdfPTable tableTextTaxes = new PdfPTable(1);
                PdfPCell[] CellsTextTaxes1 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "Taxes are based", FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, VerticalAlignment = Element.ALIGN_BOTTOM, fixedLeading = 0.1f, multipliedLeading = 0.1f}),
                                        };
                PdfPRow RowTextTaxes1 = new PdfPRow(CellsTextTaxes1);
                tableTextTaxes.Rows.Add(RowTextTaxes1);

                PdfPCell[] CellsTextTaxes2 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "on Full Retail", FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, VerticalAlignment = Element.ALIGN_BOTTOM, fixedLeading = 0.1f, multipliedLeading = 0.1f}),
                                        };

                PdfPRow RowTextTaxes2 = new PdfPRow(CellsTextTaxes2);
                tableTextTaxes.Rows.Add(RowTextTaxes2);

                PdfPCell[] CellsTextTaxes3 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "Price", FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, VerticalAlignment = Element.ALIGN_BOTTOM, fixedLeading = 0.1f, multipliedLeading = 0.1f}),
                                        };

                PdfPRow RowTextTaxes3 = new PdfPRow(CellsTextTaxes3);
                tableTextTaxes.Rows.Add(RowTextTaxes3);

                tableTextTaxes.TotalWidth = 80f;
                tableTextTaxes.LockedWidth = true;

                float[] WithColTextTaxes = new float[] { 80f };
                tableTextTaxes.SetWidths(WithColTextTaxes);

                #endregion

                #region Grupo4

                PdfPCell[] CellsItemCab = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "Quantity", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, BorderWidthBottom = 0.75f}),
                                                GetCelda(new CeldaPDF(){Text = "Item #", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, BorderWidthBottom = 0.75f}),
                                                GetCelda(new CeldaPDF(){Text = "Description", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, BorderWidthBottom = 0.75f}),
                                                GetCelda(new CeldaPDF(){Text = "Unit Price", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, BorderWidthBottom = 0.75f}),
                                                GetCelda(new CeldaPDF(){Text = "Amount", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_RIGHT, BorderWidthBottom = 0.75f}),
                                        };
                PdfPRow RowItemCab = new PdfPRow(CellsItemCab);

                DataView dvPadre = new DataView(dtFourthLine, "ParentKit = ''", "SKU", DataViewRowState.CurrentRows);

                PdfPTable tableItems = new PdfPTable(5);
                tableItems.Rows.Add(RowItemCab);

                string strSKU;
                int intIndex = 0;
                bool blnKit = false;
                int intItems = 0;
                foreach (DataRowView rv in dvPadre)
                {
                    strSKU = rv["SKU"].ToString();

                    if (intIndex == 0 || blnKit)
                    {
                        PdfPCell[] CellsItems = new PdfPCell[] { 
                        GetCelda(new CeldaPDF(){Text = rv["quantity"].ToString(), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                        GetCelda(new CeldaPDF(){Text = rv["SKU"].ToString(), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                        GetCelda(new CeldaPDF(){Text = rv["Name"].ToString(), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                        GetCelda(new CeldaPDF(){Text = String.Format("${0:0.00}", Convert.ToDecimal(rv["UnitPrice"])), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                        GetCelda(new CeldaPDF(){Text = String.Format("${0:0.00}", Convert.ToDecimal(rv["Amount"])), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_RIGHT}),

                        };
                        PdfPRow RowItems = new PdfPRow(CellsItems);
                        tableItems.Rows.Add(RowItems);
                        intItems = intItems + 1;
                        if ((intItems % 25) == 0)
                        {
                            intItems %= 25;
                            //Primera Pagina
                            document.Add(new Paragraph(" "));
                            document.Add(tableL1);
                            document.Add(logo);
                            document.Add(new Paragraph(" "));
                            document.Add(new Paragraph(" "));
                            document.Add(new Paragraph(" "));
                            document.Add(new Paragraph(" "));
                            document.Add(tableG2);
                            document.Add(new Paragraph(" "));
                            document.Add(tableG3);
                            document.Add(new Paragraph(" "));
                            //----------------

                            tableItems.TotalWidth = 550f;
                            tableItems.LockedWidth = true;

                            float[] WithColItems = new float[] { 60f, 60f, 250f, 80f, 50f };
                            tableItems.SetWidths(WithColItems);
                            document.Add(tableItems);

                            tablePagNext.WriteSelectedRows(0, tablePagNext.Rows.Count, 80, 160, writer.DirectContent);
                            tableTextTaxes.WriteSelectedRows(0, tableTextTaxes.Rows.Count, 500, 160, writer.DirectContent);

                            tableItems.DeleteBodyRows();
                            tableItems.Rows.Add(RowItemCab);
                            document.NewPage();
                        }
                    }
                    else
                    {
                        PdfPCell[] CellsItems = new PdfPCell[] { 
                        GetCelda(new CeldaPDF(){Text = rv["quantity"].ToString(), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.3f, multipliedLeading = 0.3f}),
                        GetCelda(new CeldaPDF(){Text = rv["SKU"].ToString(), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.3f, multipliedLeading = 0.3f}),
                        GetCelda(new CeldaPDF(){Text = rv["Name"].ToString(), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.3f, multipliedLeading = 0.3f}),
                        GetCelda(new CeldaPDF(){Text = String.Format("${0:0.00}", Convert.ToDecimal(rv["UnitPrice"])), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.3f, multipliedLeading = 0.3f}),
                        GetCelda(new CeldaPDF(){Text = String.Format("${0:0.00}", Convert.ToDecimal(rv["Amount"])), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_RIGHT, fixedLeading = 0.3f, multipliedLeading = 0.3f}),

                        };
                        PdfPRow RowItems = new PdfPRow(CellsItems);
                        tableItems.Rows.Add(RowItems);
                        intItems = intItems + 1;
                        if ((intItems % 25) == 0)
                        {
                            intItems %= 25;
                            //Primera Pagina
                            document.Add(new Paragraph(" "));
                            document.Add(tableL1);
                            document.Add(logo);
                            document.Add(new Paragraph(" "));
                            document.Add(new Paragraph(" "));
                            document.Add(new Paragraph(" "));
                            document.Add(new Paragraph(" "));
                            document.Add(tableG2);
                            document.Add(new Paragraph(" "));
                            document.Add(tableG3);
                            document.Add(new Paragraph(" "));
                            //----------------

                            tableItems.TotalWidth = 550f;
                            tableItems.LockedWidth = true;

                            float[] WithColItems = new float[] { 60f, 60f, 250f, 80f, 50f };
                            tableItems.SetWidths(WithColItems);
                            document.Add(tableItems);

                            tablePagNext.WriteSelectedRows(0, tablePagNext.Rows.Count, 80, 160, writer.DirectContent);
                            tableTextTaxes.WriteSelectedRows(0, tableTextTaxes.Rows.Count, 500, 160, writer.DirectContent);

                            tableItems.DeleteBodyRows();
                            tableItems.Rows.Add(RowItemCab);
                            document.NewPage();
                        }
                    }

                    DataView dvHijo = new DataView(dtFourthLine, "ParentKit = '" + strSKU + "'", "SKU", DataViewRowState.CurrentRows);
                    blnKit = false;
                    foreach (DataRowView rvHijos in dvHijo)
                    {
                        PdfPCell[] CellsItemsHijos = new PdfPCell[] { 
                        GetCelda(new CeldaPDF(){Text = "   " + rvHijos["quantity"].ToString(), FontName = "ArialUnicodeMS", FontSize = 6, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, BorderWidthBottom = 0.5f, fixedLeading = 1f, multipliedLeading = 0.4f}),
                        GetCelda(new CeldaPDF(){Text = "   " + rvHijos["SKU"].ToString(), FontName = "ArialUnicodeMS", FontSize = 6, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, BorderWidthBottom = 0.5f, fixedLeading = 1f, multipliedLeading = 0.4f}),
                        GetCelda(new CeldaPDF(){Text = "   " + rvHijos["Name"].ToString(), FontName = "ArialUnicodeMS", FontSize = 6, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, BorderWidthBottom = 0.5f, fixedLeading = 1f, multipliedLeading = 0.4f}),
                        GetCelda(new CeldaPDF(){Text = "   ", FontName = "ArialUnicodeMS", FontSize = 6, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, BorderWidthBottom = 0.5f, fixedLeading = 1f, multipliedLeading = 0.4f}),
                        GetCelda(new CeldaPDF(){Text = "   ", FontName = "ArialUnicodeMS", FontSize = 6, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, BorderWidthBottom = 0.5f, fixedLeading = 1f, multipliedLeading = 0.4f}),

                        };
                        PdfPRow RowItemsHijos = new PdfPRow(CellsItemsHijos);
                        tableItems.Rows.Add(RowItemsHijos);
                        intItems = intItems + 1;
                        if ((intItems % 25) == 0)
                        {
                            intItems %= 25;
                            //Primera Pagina
                            document.Add(new Paragraph(" "));
                            document.Add(tableL1);
                            document.Add(logo);
                            document.Add(new Paragraph(" "));
                            document.Add(new Paragraph(" "));
                            document.Add(new Paragraph(" "));
                            document.Add(new Paragraph(" "));
                            document.Add(tableG2);
                            document.Add(new Paragraph(" "));
                            document.Add(tableG3);
                            document.Add(new Paragraph(" "));
                            //----------------

                            tableItems.TotalWidth = 550f;
                            tableItems.LockedWidth = true;

                            float[] WithColItems = new float[] { 60f, 60f, 250f, 80f, 50f };
                            tableItems.SetWidths(WithColItems);
                            document.Add(tableItems);

                            tablePagNext.WriteSelectedRows(0, tablePagNext.Rows.Count, 80, 160, writer.DirectContent);
                            tableTextTaxes.WriteSelectedRows(0, tableTextTaxes.Rows.Count, 500, 160, writer.DirectContent);

                            tableItems.DeleteBodyRows();
                            tableItems.Rows.Add(RowItemCab);
                            document.NewPage();
                        }

                        blnKit = true;
                    }

                    intIndex = intIndex + 1;

                }

                #endregion

                #region Grupo5

                PdfPCell[] CellsGrupo5Cab = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "Payment Date", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, BorderWidthBottom = 1f}),
                                                GetCelda(new CeldaPDF(){Text = "Type", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, BorderWidthBottom = 1f}),
                                                GetCelda(new CeldaPDF(){Text = "Amount", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, BorderWidthBottom = 1f}),
                                        };
                PdfPRow RowGrupo5Cab = new PdfPRow(CellsGrupo5Cab);

                PdfPTable tableGrupo5Cab = new PdfPTable(3);
                tableGrupo5Cab.Rows.Add(RowGrupo5Cab);

                foreach (DataRow row in dtPaymentLine.Rows)
                {
                    int intValores;
                    intValores = dtPaymentLine.Rows.IndexOf(row);
                    if (intValores == 0)
                    {
                        PdfPCell[] CellsItems = new PdfPCell[] { 
                        GetCelda(new CeldaPDF(){Text = row["date"].ToString(), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                        GetCelda(new CeldaPDF(){Text = row["paymentType"].ToString(), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                        GetCelda(new CeldaPDF(){Text = String.Format("${0:0.00}", Convert.ToDecimal(row["amount"])), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),                        
                        };

                        PdfPRow RowItems = new PdfPRow(CellsItems);
                        tableGrupo5Cab.Rows.Add(RowItems);
                    }
                    else
                    {
                        PdfPCell[] CellsItems = new PdfPCell[] { 
                        GetCelda(new CeldaPDF(){Text = row["date"].ToString(), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.1f, multipliedLeading = 0.1f}),
                        GetCelda(new CeldaPDF(){Text = row["paymentType"].ToString(), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.1f, multipliedLeading = 0.1f}),
                        GetCelda(new CeldaPDF(){Text = String.Format("${0:0.00}", Convert.ToDecimal(row["amount"])), FontName = "ArialUnicodeMS", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.1f, multipliedLeading = 0.1f}),                        
                        };

                        PdfPRow RowItems = new PdfPRow(CellsItems);
                        tableGrupo5Cab.Rows.Add(RowItems);
                    }
                }

                tableGrupo5Cab.TotalWidth = 280f;
                tableGrupo5Cab.LockedWidth = true;

                float[] WithColItemsG5 = new float[] { 110f, 130f, 40f };
                tableGrupo5Cab.SetWidths(WithColItemsG5);

                #endregion

                #region Grupo6

                PdfPTable tableGrupo6 = new PdfPTable(2);

                if (dtTotalLine.Rows.Count > 0)
                {
                    PdfPCell[] CellsGrupo6Linea1 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "Subtotal:", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                                                GetCelda(new CeldaPDF(){Text = String.Format("${0:0.00}", Convert.ToDecimal(dtTotalLine.Rows[0]["subtotal"])), FontName = "Verdana-Bold", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                                        };
                    PdfPRow RowGrupo6LInea1 = new PdfPRow(CellsGrupo6Linea1);
                    tableGrupo6.Rows.Add(RowGrupo6LInea1);

                    PdfPCell[] CellsGrupo6Linea2 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "Tax*:", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.2f, multipliedLeading = 0.2f}),
                                                GetCelda(new CeldaPDF(){Text = String.Format("${0:0.00}", Convert.ToDecimal(dtTotalLine.Rows[0]["taxAmount"])), FontName = "Verdana-Bold", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.2f, multipliedLeading = 0.2f}),
                                        };
                    PdfPRow RowGrupo6LInea2 = new PdfPRow(CellsGrupo6Linea2);
                    tableGrupo6.Rows.Add(RowGrupo6LInea2);

                    PdfPCell[] CellsGrupo6Linea3 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "Shipping:", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.2f, multipliedLeading = 0.2f}),
                                                GetCelda(new CeldaPDF(){Text = String.Format("${0:0.00}", Convert.ToDecimal(dtTotalLine.Rows[0]["shippingTotal"])), FontName = "Verdana-Bold", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.2f, multipliedLeading = 0.2f}),
                                        };
                    PdfPRow RowGrupo6LInea3 = new PdfPRow(CellsGrupo6Linea3);
                    tableGrupo6.Rows.Add(RowGrupo6LInea3);

                    PdfPCell[] CellsGrupo6Linea4 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "Handling:", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.2f, multipliedLeading = 0.2f}),
                                                GetCelda(new CeldaPDF(){Text = "", FontName = "Verdana-Bold", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.2f, multipliedLeading = 0.2f}),
                                        };
                    PdfPRow RowGrupo6LInea4 = new PdfPRow(CellsGrupo6Linea4);
                    tableGrupo6.Rows.Add(RowGrupo6LInea4);

                    PdfPCell[] CellsGrupo6Linea5 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "Total:", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 1f, multipliedLeading = 1f}),
                                                GetCelda(new CeldaPDF(){Text = String.Format("${0:0.00}", Convert.ToDecimal(dtTotalLine.Rows[0]["GrandTotal"])), FontName = "Verdana-Bold", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 1f, multipliedLeading = 1f}),
                                        };
                    PdfPRow RowGrupo6LInea5 = new PdfPRow(CellsGrupo6Linea5);
                    tableGrupo6.Rows.Add(RowGrupo6LInea5);

                    PdfPCell[] CellsGrupo6Linea6 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "Payments:", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.8f, multipliedLeading = 0.8f}),
                                                GetCelda(new CeldaPDF(){Text = String.Format("${0:0.00}", Convert.ToDecimal(dtTotalLine.Rows[0]["PaymentTotal"])), FontName = "Verdana-Bold", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.8f, multipliedLeading = 0.8f}),
                                        };
                    PdfPRow RowGrupo6LInea6 = new PdfPRow(CellsGrupo6Linea6);
                    tableGrupo6.Rows.Add(RowGrupo6LInea6);

                    PdfPCell[] CellsGrupo6Linea7 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "Balance Due:", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.1f, multipliedLeading = 0.1f}),
                                                GetCelda(new CeldaPDF(){Text = String.Format("${0:0.00}", Convert.ToDecimal(dtTotalLine.Rows[0]["Balance"])), FontName = "Verdana-Bold", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED, fixedLeading = 0.1f, multipliedLeading = 0.1f}),
                                        };
                    PdfPRow RowGrupo6LInea7 = new PdfPRow(CellsGrupo6Linea7);
                    tableGrupo6.Rows.Add(RowGrupo6LInea7);
                }
                else
                {
                    PdfPCell[] CellsGrupo6Linea1 = new PdfPCell[] { 
                                                GetCelda(new CeldaPDF(){Text = "Subtotal:", FontName = "Verdana-Bold", FontSize = 9, esBold =true, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                                                GetCelda(new CeldaPDF(){Text = "", FontSize = 9, esBold =false, Colspan = 1, HorizontalAlignment = Element.ALIGN_UNDEFINED}),
                                        };
                    PdfPRow RowGrupo6LInea1 = new PdfPRow(CellsGrupo6Linea1);
                    tableGrupo6.Rows.Add(RowGrupo6LInea1);
                }

                tableGrupo6.TotalWidth = 110f;
                tableGrupo6.LockedWidth = true;

                float[] WithColItemsG6 = new float[] { 70f, 40f, };
                tableGrupo6.SetWidths(WithColItemsG6);

                #endregion

                //Cabecera Pagina
                document.Add(new Paragraph(" "));
                document.Add(tableL1);
                document.Add(logo);
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(tableG2);
                document.Add(new Paragraph(" "));
                document.Add(tableG3);
                document.Add(new Paragraph(" "));
                //----------------

                tableItems.TotalWidth = 550f;
                tableItems.LockedWidth = true;

                float[] WithColItems2 = new float[] { 60f, 60f, 250f, 80f, 50f };
                tableItems.SetWidths(WithColItems2);
                document.Add(tableItems);

                tableTextTaxes.WriteSelectedRows(0, tableTextTaxes.Rows.Count, 390, 80, writer.DirectContent);
                tableGrupo5Cab.WriteSelectedRows(0, tableGrupo5Cab.Rows.Count, 20, 180, writer.DirectContent);
                tableGrupo6.WriteSelectedRows(0, tableGrupo6.Rows.Count, 390, 190, writer.DirectContent);

                document.NewPage();
            }

            document.Close();

            return ms;
        }

        public static byte[] MergePDFs(List<byte[]> pdfFiles)
        {
            if (pdfFiles.Count > 1)
            {
                PdfReader finalPdf;
                Document pdfContainer;
                PdfWriter pdfCopy;
                MemoryStream msFinalPdf = new MemoryStream();

                finalPdf = new PdfReader(pdfFiles[0]);
                pdfContainer = new Document();
                pdfCopy = new PdfSmartCopy(pdfContainer, msFinalPdf);

                pdfContainer.Open();

                for (int k = 0; k < pdfFiles.Count; k++)
                {
                    finalPdf = new PdfReader(pdfFiles[k]);
                    for (int i = 1; i < finalPdf.NumberOfPages + 1; i++)
                    {
                        if (k.Equals(pdfFiles.Count - 1) && i.Equals(finalPdf.NumberOfPages)) { }
                        else
                        {
                            ((PdfSmartCopy)pdfCopy).AddPage(pdfCopy.GetImportedPage(finalPdf, i));
                        }
                    }
                    pdfCopy.FreeReader(finalPdf);

                }
                finalPdf.Close();
                pdfCopy.Close();
                pdfContainer.Close();

                return msFinalPdf.ToArray();
            }
            else if (pdfFiles.Count == 1)
            {
                //return new MemoryStream(pdfFiles[0]);
                return pdfFiles[0];
            }
            return null;
        }
    }
}
