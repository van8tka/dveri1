using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DALdv1;

namespace dveri1.Models
{
    public class SliderModel
    {
        public IEnumerable<SliderMainImg> SliderMI { get; set; }
        public IEnumerable<SliderLeftImg> SliderLI { get; set; }
        public IEnumerable<SliderMainImgMk> SliderMImk { get; set; }
        public IEnumerable<SliderLeftImgMk> SliderLImk { get; set; }
        public string MimeTypeSlider { get; set; }
        public byte[] ImgDataSlider { get; set; }
        public int CountSlide { get; set; }
    }
}