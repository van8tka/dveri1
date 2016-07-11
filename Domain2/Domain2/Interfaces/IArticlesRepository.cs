using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;

namespace Domain2.Interfaces
{
    public interface IArticlesRepository
    {
        IEnumerable<TableArticle> GetArticles();
        TableArticle GetArticle(int id);
        //дату не вносим . а прописываем в автомате
        void CreateArticle(int id, string name, string descrip, string art);
        void CreateSeoArticle(int id, string title, string key, string desc);
        TableSeoArticle GetSeoArticle(int id);
        void DelArticle(int id);
    }
}
