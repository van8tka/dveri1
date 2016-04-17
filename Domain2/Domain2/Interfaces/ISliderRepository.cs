using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;

namespace Domain2.Interfaces
{
   public interface ISliderRepository
    {
        IEnumerable<SliderMainImg> GetSliderMainImg();
        void CreateSliderMainImg(string Type, byte[] Image);
        void DellSliderMainImg(int id);
        IEnumerable<SliderLeftImg> GetSliderLeftImg();
        void CreateSliderLeftImg(string Type, byte[] Image);
        void DellSliderLeftImg(int id);
    }
}
