using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace EMS.Web.Models
{
    public class PdfCreation
    {
        public static Document ExportToPdf(DataTable dt)
        {

            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            Font font13 = FontFactory.GetFont("ARIAL", 13);
            Font font18 = FontFactory.GetFont("ARIAL", 18);
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();

                if (dt.Rows.Count > 0)
                {
                    PdfPTable PdfTable = new PdfPTable(2);

                    PdfTable.TotalWidth = 200f;
                    PdfTable.LockedWidth = true;


                    PdfPCell pdfPCell = new PdfPCell(new Phrase(new Chunk("Date Of Credit                                                   Batch No ", font13)));
                    pdfPCell.Border = Rectangle.NO_BORDER;

                    // PdfPCell.SpaceCharRatio = 100f;
                    PdfTable.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(new Chunk(Convert.ToDateTime(dt.Rows[0]["batch_number_date"].ToString()).ToString("dd-MMM-yyyy") + "                                                   " + dt.Rows[0]["batch_number"].ToString(), font13)));
                    pdfPCell.Border = Rectangle.NO_BORDER;
                    //PdfPCell.SpaceCharRatio = 10f;

                    PdfTable.AddCell(pdfPCell);

                    // DrawLine(writer, 25f, pdfDoc.Top - 30f, pdfDoc.PageSize.Width - 25f, pdfDoc.Top - 30f, new BaseColor(System.Drawing.Color.Black));

                    pdfDoc.Add(PdfTable);
                    PdfTable = new PdfPTable(1);





                    pdfPCell = new PdfPCell(new Phrase(new Chunk("Employee Wise Salary", font13)));
                    pdfPCell.Border = Rectangle.NO_BORDER;
                    PdfTable.AddCell(pdfPCell);
                    //DrawLine(writer, 25f, pdfDoc.Top - 30f, pdfDoc.PageSize.Width - 25f, pdfDoc.Top - 30f, new BaseColor(System.Drawing.Color.Black));
                    pdfDoc.Add(PdfTable);

                    PdfTable = new PdfPTable(4);
                    PdfTable.SpacingBefore = 10f;
                    float[] widths = new float[] { 15f, 30f, 90f, 50f };
                    PdfTable.SetWidths(widths);
                    //for (int columns = 0; columns <= dt.Columns.Count - 1; columns++)
                    //{
                    //    PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Columns[columns].ColumnName, font18)));
                    //    PdfTable.AddCell(PdfPCell);
                    //}

                    pdfPCell = new PdfPCell(new Phrase(new Chunk("SrNo.", font13)));
                    PdfTable.AddCell(pdfPCell);
                    pdfPCell = new PdfPCell(new Phrase(new Chunk("Empployee ID", font13)));
                    PdfTable.AddCell(pdfPCell);
                    pdfPCell = new PdfPCell(new Phrase(new Chunk("Employee Name", font13)));
                    PdfTable.AddCell(pdfPCell);
                    pdfPCell = new PdfPCell(new Phrase(new Chunk("Amount Sanctioned", font13)));
                    PdfTable.AddCell(pdfPCell);

                    for (int rows = 0; rows <= dt.Rows.Count - 1; rows++)
                    {

                        pdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows]["sr_no"].ToString(), font13)));
                        pdfPCell.HorizontalAlignment = 1;
                        PdfTable.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows]["empl_id"].ToString(), font13)));
                        pdfPCell.HorizontalAlignment = 1;
                        PdfTable.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows]["employee_name"].ToString(), font13)));
                        PdfTable.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows]["amount_sactioned"].ToString(), font13)));
                        pdfPCell.HorizontalAlignment = 2;
                        PdfTable.AddCell(pdfPCell);

                    }
                    pdfDoc.Add(PdfTable);
                }
                pdfDoc.Close();

            }
            catch (DocumentException de)
            {
            }
            // System.Web.HttpContext.Current.Response.Write(de.Message)
            catch (IOException ioEx)
            {
            }
            // System.Web.HttpContext.Current.Response.Write(ioEx.Message)
            catch (Exception ex)
            {
            }
            return pdfDoc;
        }
        private static void DrawLine(PdfWriter writer, float x1, float y1, float x2, float y2, BaseColor color)
        {
            PdfContentByte contentByte = writer.DirectContent;
            contentByte.SetColorStroke(color);
            contentByte.MoveTo(x1, y1);
            contentByte.LineTo(x2, y2);
            contentByte.Stroke();
        }
    }
}