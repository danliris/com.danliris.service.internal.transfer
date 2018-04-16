using Com.Danliris.Service.Internal.Transfer.Lib.ViewModels.TransferRequestViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Com.Danliris.Service.Internal.Transfer.Lib.PDFTemplates
{
    public class TransferRequestPDFTemplate
    {
        public MemoryStream GeneratePdfTemplate(TransferRequestViewModel viewModel)
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            BaseFont bf_bold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            var normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
            var bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);

            //Creating page.

            Document document = new Document(PageSize.A5);
            MemoryStream stream = new MemoryStream();



            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            writer.CloseStream = false;

            document.Open();

            PdfContentByte cb = writer.DirectContent;

            cb.BeginText();
            cb.SetTextMatrix(15, 23);

            cb.SetFontAndSize(bf_bold, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "PT DAN LIRIS", 200, 550, 0);
            cb.SetFontAndSize(bf, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "BANARAN, GROGOL, SUKOHARJO", 200, 540, 0);

            cb.SetFontAndSize(bf_bold, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "PERMOHONAN TRANSFER", 200, 520, 0);

            cb.SetFontAndSize(bf, 9);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Nomor", 20, 500, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ":", 55, 500, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, viewModel.trNo, 65, 500, 0);

            cb.SetFontAndSize(bf, 9);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Sukoharjo, ", 320, 500, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, viewModel.trDate.ToString("dd-MM-yyyy"), 330, 500, 0);

            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Dari", 20, 490, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ":", 55, 490, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, viewModel.unit.name, 65, 490, 0);

            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Kepada", 50, 306, 0);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ":", 110, 306, 0);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Gudang Material", 120, 306, 0);

            cb.EndText();

            #region CreateTable
            PdfPTable table = new PdfPTable(5);
            PdfPCell cell;
            table.TotalWidth = 360f;

            float[] widths = new float[] { 2f, 4f, 10f, 4f, 4f };
            table.SetWidths(widths);

            cell = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            var rightCell = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            var leftCell = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            //Create cells headers.


            cell.Phrase = new Phrase("NO", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("KODE", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("BARANG", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("JUMLAH", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("HARGA", bold_font);
            table.AddCell(cell);

            int index = 1;

            foreach (var detail in viewModel.details)
            {
                cell = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
                cell.Phrase = new Phrase(index.ToString(), normal_font);
                table.AddCell(cell);

                cell.Phrase = new Phrase(detail.product.code , normal_font);
                table.AddCell(cell);

                cell.Phrase = new Phrase(detail.product.name + " GRADE " + detail.grade + " " + detail.productRemark, normal_font);
                table.AddCell(cell);


                cell.Phrase = new Phrase(string.Format("{0:n}", detail.quantity), normal_font);
                table.AddCell(cell);

                cell.Phrase = new Phrase("", normal_font);
                table.AddCell(cell);

                index++;
            }

            var footerCell = new PdfPCell(new Phrase    ("Kategori              : " + viewModel.category.name, normal_font));
            footerCell.Colspan = 5;
            footerCell.Border = Rectangle.NO_BORDER;
            footerCell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(footerCell);

            var footerCell1 = new PdfPCell(new Phrase   ("Diminta Datang   : " + viewModel.requestedArrivalDate.ToString("dd-MM-yyyy"), normal_font));
            footerCell1.Colspan = 5;
            footerCell1.Border = Rectangle.NO_BORDER;
            footerCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(footerCell1);


            var footerCell2 = new PdfPCell(new Phrase("Keterangan         : " + viewModel.remark, normal_font));
            footerCell2.Colspan = 5;
            footerCell2.Border = Rectangle.NO_BORDER;
            footerCell2.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(footerCell2);



            var footerCellspace = new PdfPCell(new Phrase(""));
            footerCellspace.MinimumHeight = 15;
            footerCellspace.Colspan = 5;
            footerCellspace.Border = Rectangle.NO_BORDER;
            footerCellspace.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(footerCellspace);

            #endregion

            #region CreateTable2
            PdfPTable table2 = new PdfPTable(5);
            PdfPCell cell2;
            table2.TotalWidth = 360f;

            float[] widths2 = new float[] { 5f,5f,5f,5f,5f };
            table2.SetWidths(widths2);

            cell2 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            var rightCell2 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            var leftCell2 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            //Create cells headers.


            cell2.Phrase = new Phrase("ACC MENGETAHUI", bold_font);
            table2.AddCell(cell2);

            cell2.Phrase = new Phrase("BAGIAN PEMBELIAN", bold_font);
            table2.AddCell(cell2);

            cell2.Phrase = new Phrase("KEPALA BAGIAN", bold_font);
            table2.AddCell(cell2);

            cell2.Phrase = new Phrase("YANG MEMERLUKAN", bold_font);
            table2.AddCell(cell2);

            cell2.Phrase = new Phrase("PPIC", bold_font);
            table2.AddCell(cell2);

            cell2 = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            cell2.MinimumHeight = 50;
            cell2.Phrase = new Phrase("", normal_font);
            table2.AddCell(cell);

            cell2.Phrase = new Phrase("", normal_font);
            table2.AddCell(cell2);

            cell2.Phrase = new Phrase("" , normal_font);
            table2.AddCell(cell2);


            cell2.Phrase = new Phrase("", normal_font);
            table2.AddCell(cell2);

            cell2.Phrase = new Phrase("", normal_font);
            table2.AddCell(cell2);

            var footerCell3 = new PdfPCell(table2);
            footerCell3.Colspan = 5;
            footerCell3.Border = Rectangle.NO_BORDER;
            footerCell3.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(footerCell3);

            table.WriteSelectedRows(0, -1, 20, 470, cb);
            // table2.WriteSelectedRows(0, -1, 20, 130, cb);
            #endregion

            //cb.BeginText();
            //cb.SetTextMatrix(15, 23);

            //cb.SetFontAndSize(bf, 9);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Kategori", 20, 200, 0);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ":", 70, 200, 0);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, viewModel.category.name, 75, 200, 0);

            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Diminta Datang", 20, 190, 0);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ":", 70, 190, 0);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, viewModel.requestedArrivalDate.ToString("dd-MM-yyyy"), 75, 190, 0);

            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Keterangan", 20, 180, 0);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ":", 70, 180, 0);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, viewModel.remark, 75, 180, 0);


            //cb.EndText();

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }
}
