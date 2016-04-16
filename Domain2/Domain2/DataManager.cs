using Domain2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain2
{
    public class DataManager
    {
        //туту будут описаны связи
        private IVhodnyeDvRepository vhodnDvRepository;
        private ISliderRepository sliderRepository;
  
        public DataManager(IVhodnyeDvRepository vhodnDvRepository, ISliderRepository sliderRepository)
        {
            this.vhodnDvRepository = vhodnDvRepository;
            this.sliderRepository = sliderRepository;
        }
        //св-ва через которые будет происх вызов
        public IVhodnyeDvRepository VhodnyeDvRepository
        {
            get { return vhodnDvRepository; }
        }
        public ISliderRepository SliderRepository
        {
            get { return sliderRepository; }
        }

    }
}
