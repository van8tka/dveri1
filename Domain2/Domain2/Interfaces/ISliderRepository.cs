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
        void AddLink(int id, string link);
        IEnumerable<SliderMainImgMk> GetSliderMainImgMk();
        void CreateSliderMainImgMk(string Type, byte[] Image);
        void DellSliderMainImgMk(int id);
        void AddLinkMk(int id, string link);
    }
}
