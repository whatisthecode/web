using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public interface InvoiceDAO
    {
        IEnumerable<Invoice> getInvoice();
        Invoice getInvoiceById(Int16 invoiceId);
        Invoice checkExist(Invoice invoice);
        void insertInvoice(Invoice invoice);

        void deleteInvoice(Int16 InvoiceId);
        void updateInvoice(Invoice invoice);
        void saveInvoice();
        Boolean check(String column);
        PagedResult<Invoice> PageView(int pageIndex, int pageSize, string columnName);
        void dispose();
    }
}
