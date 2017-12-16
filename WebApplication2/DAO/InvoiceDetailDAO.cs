using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface InvoiceDetailDAO
    {
        IEnumerable<InvoiceDetail> getInvoiceDetail();
        InvoiceDetail getInvoiceDetailById(Int16 invoiceDetailId);
        InvoiceDetail checkExist(InvoiceDetail invoicedetail);
        void insertInvoiceDetail(InvoiceDetail invoicedetail);

        void deleteInvoiceDetail(Int16 InvoiceDetailId);
        void updateInvoiceDetail(InvoiceDetail invoicedetail);
        void saveInvoiceDetail();
        Boolean check(String column);
        PagedResult<InvoiceDetail> PageView(int pageIndex, int pageSize, string columnName);
        void dispose();
        IEnumerable<InvoiceDetail> getListDetailByInvoiceId(short invoiceId);
    }
}
