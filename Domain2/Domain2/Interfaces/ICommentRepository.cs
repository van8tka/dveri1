using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;

namespace Domain2.Interfaces
{
   public interface ICommentRepository
    {
        IEnumerable<CommentCompany> GetCommetCompany();
        IEnumerable<CommentVhDveri> GetCommentVhDv();
        IEnumerable<CommentMkDv> GetCommentMkDv();
        void CreateCommentCompany(int id, string name, string email, string comment, string response, string header, bool publ, int stars, DateTime date);
        void CreateCommentVhDv(int id, int iddv,string name, string email, string comment, string response, string header, bool publ, int stars, DateTime date);
        void CreateCommentMkDv(int id, int iddv, string name, string email, string comment, string response, string header, bool publ, int stars, DateTime date);
        void DellComCop(int id);
        void DellComVhDv(int id);
        void DellComMkDv(int id);
    }
}
