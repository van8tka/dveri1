using System.Collections.Generic;
using DALdv1;

namespace Domain2.Interfaces
{
    public interface IArticlesRepository
    {
        //для входных дверей
        IEnumerable<TableArticle> GetArticles();
        TableArticle GetArticle(int id);
        //дату не вносим . а прописываем в автомате
        void CreateArticle(int id, string name, string descrip, string art);
        void CreateSeoArticle(int id, string title, string key, string desc);
        TableSeoArticle GetSeoArticle(int id);
        void DelArticle(int id);
        //для мк дверей
        IEnumerable<TableArticlesMk> GetArticlesMk();
        TableArticlesMk GetArticleMk(int id);
        //дату не вносим . а прописываем в автомате
        void CreateArticleMk(int id, string name, string descrip, string art);
        void CreateSeoArticleMk(int id, string title, string key, string desc);
        TableSeoArticlesMk GetSeoArticleMk(int id);
        void DelArticleMk(int id);
    }
}
