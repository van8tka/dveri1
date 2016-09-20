using Domain2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;

namespace Domain2.Implementations
{
    public class EfSliderRepository : ISliderRepository
    {
        private dbdveri1Entities1 context;
        public EfSliderRepository(dbdveri1Entities1 context)
        {
            this.context = context;
        }
        //для ВХ дверей
        //главный слайдер
        public void CreateSliderMainImg(string Type, byte[] Image)
        {
            SliderMainImg sl = new SliderMainImg()
            {
                Id = 0,
                MimeType = Type,
                Imaging = Image,
                LinkImage = null
            };
            context.SliderMainImgs.Add(sl);
            context.SaveChanges();
        }

        public void DellSliderMainImg(int id)
        {
            context.SliderMainImgs.Remove(context.SliderMainImgs.Where(x=>x.Id == id).FirstOrDefault());
            context.SaveChanges();

        }

        public IEnumerable<SliderMainImg> GetSliderMainImg()
        {
            return context.SliderMainImgs;
        }

        public void AddLink(int id, string link)
        {
            if(id!=0)
            {
                SliderMainImg sl = context.SliderMainImgs.Find(id);
                sl.LinkImage = link;
                context.Entry(sl).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
    
        //=======================================================================для МК дверей
        //главный слайдер
        public void CreateSliderMainImgMk(string Type, byte[] Image)
        {
            SliderMainImgMk sl = new SliderMainImgMk()
            {
                Id = 0,
                MimeType = Type,
                Imaging = Image,
                LinkImage = null
            };
            context.SliderMainImgMks.Add(sl);
            context.SaveChanges();
        }

        public void DellSliderMainImgMk(int id)
        {
            context.SliderMainImgMks.Remove(context.SliderMainImgMks.Where(x => x.Id == id).FirstOrDefault());
            context.SaveChanges();

        }

        public IEnumerable<SliderMainImgMk> GetSliderMainImgMk()
        {
            return context.SliderMainImgMks;
        }

        public void AddLinkMk(int id, string link)
        {
            if (id != 0)
            {
                SliderMainImgMk sl = context.SliderMainImgMks.Find(id);
                sl.LinkImage = link;
                context.Entry(sl).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
