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
                Imaging = Image
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

        //==============================боковой слайдер===================================
        public void CreateSliderLeftImg(string Type, byte[] Image)
        {
            SliderLeftImg sl = new SliderLeftImg()
            {
                Id = 0,
                MimeType = Type,
                Imaging = Image
            };
            context.SliderLeftImgs.Add(sl);
            context.SaveChanges();
        }

        public void DellSliderLeftImg(int id)
        {
            context.SliderLeftImgs.Remove(context.SliderLeftImgs.Where(x => x.Id == id).FirstOrDefault());
            context.SaveChanges();

        }

        public IEnumerable<SliderLeftImg> GetSliderLeftImg()
        {
            return context.SliderLeftImgs;
        }
        //для МК дверей
        //главный слайдер
        public void CreateSliderMainImgMk(string Type, byte[] Image)
        {
            SliderMainImgMk sl = new SliderMainImgMk()
            {
                Id = 0,
                MimeType = Type,
                Imaging = Image
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

        //==============================боковой слайдер===================================
        public void CreateSliderLeftImgMk(string Type, byte[] Image)
        {
            SliderLeftImgMk sl = new SliderLeftImgMk()
            {
                Id = 0,
                MimeType = Type,
                Imaging = Image
            };
            context.SliderLeftImgMks.Add(sl);
            context.SaveChanges();
        }

        public void DellSliderLeftImgMk(int id)
        {
            context.SliderLeftImgMks.Remove(context.SliderLeftImgMks.Where(x => x.Id == id).FirstOrDefault());
            context.SaveChanges();

        }

        public IEnumerable<SliderLeftImgMk> GetSliderLeftImgMk()
        {
            return context.SliderLeftImgMks;
        }





    }
}
