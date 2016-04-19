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
        private IContactRepository contactRepository;
  
        public DataManager(IVhodnyeDvRepository vhodnDvRepository, ISliderRepository sliderRepository, IContactRepository contactRepository)
        {
            this.vhodnDvRepository = vhodnDvRepository;
            this.sliderRepository = sliderRepository;
           this.contactRepository = contactRepository;
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
        public IContactRepository ContactRepository
        {
            get { return contactRepository; }
        }

    }
}
