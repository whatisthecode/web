using LaptopWebsite.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class InvoiceDAOImpl: BaseImpl<Invoice, Int16>, InvoiceDAO, IDisposable
    {
        public InvoiceDAOImpl() : base()
        {

        }

        public bool check(string column)
        {
            return base.checkColumnExists(column);
        }

        public Invoice checkExist(Invoice invoice)
        {
            Invoice invoice1 = null;
            using (DBContext context = new DBContext())
            {
                invoice1 = context.invoices.Where(c => c.code == invoice.code).FirstOrDefault();
            }
            if (invoice1 != null)
            {
                return invoice1;
            }
            return null;
        }

        public void deleteInvoice(Int16 InvoiceId)
        {
            base.delete(InvoiceId);
        }

        public void dispose()
        {
            base.Dispose();
        }

        public IEnumerable<Invoice> getInvoice()
        {
            return base.get();
        }

        public Invoice getInvoiceById(Int16 invoiceId)
        {
            return base.getById(invoiceId);
        }

        public void insertInvoice(Invoice invoice)
        {
            base.insert(invoice);
        }

        public PagedResult<Invoice> PageView(short buyerId, short salerId, int pageIndex, int pageSize, bool descending = false)
        {
            using (DBContext context = new DBContext())
            {
                PagedResult<Invoice> pv = new PagedResult<Invoice>();
                var query = from c in context.invoices select c;
                if (buyerId == 0 && salerId == 0)
                {
                    if (descending)
                        query = query.OrderByDescending(n => n.createdAt);
                    else
                        query = query.OrderBy(n => n.createdAt);

                    pv = base.PageView(query, pageIndex, pageSize);
                }

                if (buyerId == 0 && salerId != 0)
                {
                    query = query.Where(inv => inv.salerId == salerId);
                    if (descending)
                        query = query.OrderByDescending(n => n.createdAt);
                    else
                        query = query.OrderBy(n => n.createdAt);

                    pv = base.PageView(query, pageIndex, pageSize);
                }

                if (buyerId != 0 && salerId == 0)
                {
                    query = query.Where(inv => inv.buyerId == buyerId);
                    if (descending)
                        query = query.OrderByDescending(n => n.createdAt);
                    else
                        query = query.OrderBy(n => n.createdAt);

                    pv = base.PageView(query, pageIndex, pageSize);
                }

                if (salerId != 0 && salerId != 0)
                {
                    query = query.Where(inv => inv.buyerId == buyerId || inv.salerId == salerId);
                    if (descending)
                        query = query.OrderByDescending(n => n.createdAt);
                    else
                        query = query.OrderBy(n => n.createdAt);

                    pv = base.PageView(query, pageIndex, pageSize);
                }
                return pv;
            }
        }
        public void updateInvoice(Invoice invoice)
        {
            invoice.updatedAt = DateTime.Now;
            base.update(invoice);
        }
    }
}