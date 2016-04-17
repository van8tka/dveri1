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






    }
}
